using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models;
using DataAccess.Entities;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(DataContext dbContext, IConfiguration configuration, ILogger<UsersRepository> logger)
        {
            this._dbContext = dbContext;
            this._configuration = configuration;
            this._logger = logger;
        }
        public async Task<AuthenticatedUser> Authenticate(string email, string password)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.EmailAddress == email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                _logger.LogInformation("Invalid credentials were provided from user: {@userEmail}", email);
                throw new ArgumentException("Invalid credentials were provided!");
            }

            var key = _configuration.GetSection("JwtSecret").Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);
            var refreshToken = GenerateRefreshToken();

            var currentUserToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == user.UserId);

            if (currentUserToken is null)
            {
                refreshToken.User = user;
                await _dbContext.AddAsync(refreshToken);
            }
            else
            {
                currentUserToken.Token = refreshToken.Token;
                currentUserToken.ExpiresOn = refreshToken.ExpiresOn;
                currentUserToken.User = user;
                _dbContext.Update(currentUserToken);
            }

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Authenticated user: {@userEmail}", email);

            return new AuthenticatedUser($"{user.FirstName} {user.LastName}", user.EmailAddress, user.Role.Name, jwtTokenString, refreshToken.Token);
        }

        public async Task<AuthenticatedUser> Register(User user)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == user.RoleName.ToLower());

            if (role is null)
            {
                throw new ArgumentException($"Role with name {user.RoleName} does not exist!");
            }
            var userEntity = user.ToUserEntity(role);

            await _dbContext.Users.AddAsync(userEntity);

            switch (role.Name)
            {
                case Roles.Administrator:
                    await _dbContext.Administrators.AddAsync(user.ToAdministartorEntity(userEntity));
                    break;

                case Roles.Librarian:
                    await _dbContext.Librarians.AddAsync(user.ToLibrarianEntity(userEntity));
                    break;

                case Roles.Customer:
                    await _dbContext.Customers.AddAsync(user.ToCustomerEntity(userEntity));
                    break;
            }

            await _dbContext.SaveChangesAsync();
            return await Authenticate(user.EmailAddress, user.Password);
        }

        private RefreshTokenEntity GenerateRefreshToken()
        {
            var refreshToken = new RefreshTokenEntity
            {
                Token = GetUniqueToken(),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
            };

            return refreshToken;
        }

        private string GetUniqueToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var tokenIsUnique = !_dbContext.Users.Any(u => u.RefreshToken.Token == token);

            if (!tokenIsUnique)
                return GetUniqueToken();

            return token;
        }
    }
}
