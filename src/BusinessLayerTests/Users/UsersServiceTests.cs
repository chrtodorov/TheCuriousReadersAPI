using BusinessLayer;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLayerTests.Users
{
    public class UsersServiceTests
    {
        private IUsersRepository _usersRepository = null!;
        private IUsersService _usersService = null!;
        private IConfiguration _configuration = null!;

        private User validCustomer = new User
        {
            FirstName = "Valid",
            LastName = "Customer",
            EmailAddress = "customer@abv.bg",
            Password = "1234",
            RoleName = "customer",
            PhoneNumber = "123456789",
            Address = new Address
            {
                Country = "Bulgaria",
                City = "Plovdiv",
                Street = "main street",
                ApartmentNumber = "14",
                BuildingNumber = "1",
                StreetNumber = "15",
                AdditionalInfo = "test info"
            }
        };

        [SetUp]
        public void Setup()
        {
            _usersRepository = Substitute.For<IUsersRepository>();
            _configuration = Substitute.For<IConfiguration>();
            _configuration.GetSection("JwtSecret").Value.Returns("acf484d7-49a9-4e9b-b7eb-9731cabca6c5");
            _usersService = new UsersService(_usersRepository, _configuration);
        }

        [Test]
        public async Task Authenticate()
        {
            _usersRepository.GetUser(validCustomer.EmailAddress, validCustomer.Password, false).Returns(validCustomer);
            var authenticatedUser = await _usersService.Authenticate(validCustomer.EmailAddress, validCustomer.Password);

            Assert.That(authenticatedUser.Name, Is.EqualTo($"{validCustomer.FirstName} {validCustomer.LastName}"));
            Assert.That(authenticatedUser.Email, Is.EqualTo(validCustomer.EmailAddress));
            Assert.That(authenticatedUser.Role, Is.EqualTo(validCustomer.RoleName));
            Assert.That(authenticatedUser.JwtToken, Is.Not.Null.Or.Empty);
            Assert.That(authenticatedUser.RefreshToken, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task Authenticate_HashedPassword()
        {
            validCustomer.Password = BCrypt.Net.BCrypt.HashPassword(validCustomer.Password);
            _usersRepository.GetUser(validCustomer.EmailAddress, validCustomer.Password, true).Returns(validCustomer);
            var authenticatedUser = await _usersService.Authenticate(validCustomer.EmailAddress, validCustomer.Password, true);

            Assert.That(authenticatedUser.Name, Is.EqualTo($"{validCustomer.FirstName} {validCustomer.LastName}"));
            Assert.That(authenticatedUser.Email, Is.EqualTo(validCustomer.EmailAddress));
            Assert.That(authenticatedUser.Role, Is.EqualTo(validCustomer.RoleName));
            Assert.That(authenticatedUser.JwtToken, Is.Not.Null.Or.Empty);
            Assert.That(authenticatedUser.RefreshToken, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task RefreshToken()
        {
            _usersRepository.GetUser(validCustomer.EmailAddress).Returns(validCustomer);
            _usersRepository.GetUser(validCustomer.EmailAddress, validCustomer.Password, true)
                .Returns(validCustomer);

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims
                .Returns(new Claim[] 
                { 
                    new Claim(ClaimTypes.Email, validCustomer.EmailAddress) 
                });

            var authenticatedUser = await _usersService.RefreshToken(claimsPrincipal);

            Assert.That(authenticatedUser.Name, Is.EqualTo($"{validCustomer.FirstName} {validCustomer.LastName}"));
            Assert.That(authenticatedUser.Email, Is.EqualTo(validCustomer.EmailAddress));
            Assert.That(authenticatedUser.Role, Is.EqualTo(validCustomer.RoleName));
            Assert.That(authenticatedUser.JwtToken, Is.Not.Null.Or.Empty);
            Assert.That(authenticatedUser.RefreshToken, Is.Not.Null.Or.Empty);
        }
    }
}
