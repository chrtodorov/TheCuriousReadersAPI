using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAccess
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration configuration;
        private readonly ILogger<UsersRepository> _logger;
        private Dictionary<string, string> users = new Dictionary<string, string> { { "username1", "password1" }, { "username2", "password2" } };
        private List<string> roles = new List<string> { Roles.Administrator, Roles.Customer, Roles.Librarian };

        public UsersRepository(DataContext dbContext, IConfiguration configuration, ILogger<UsersRepository> logger)
        {
            this._dbContext = dbContext;
            this.configuration = configuration;
            this._logger = logger;
        }

        public string Authenticate(AuthenticateRequest authenticateRequest)
        {
            if (!users.Any(u => u.Key == authenticateRequest.Username && u.Value == authenticateRequest.Password) 
                || !roles.Contains(authenticateRequest.Role))
            {
                return null;
            }

            var key = configuration.GetSection("JwtSecret").Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, authenticateRequest.Username),
                    new Claim(ClaimTypes.Role, authenticateRequest.Role),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            _logger.LogInformation("Created JWT token for user: {@Username}", authenticateRequest.Username);
            return tokenHandler.WriteToken(token);
        }
    }
}
