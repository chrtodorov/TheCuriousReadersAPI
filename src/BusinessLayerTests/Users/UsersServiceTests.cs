using BusinessLayer;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BusinessLayerTests.Users
{
    public class UsersServiceTests
    {
        private IUsersRepository _usersRepository = null!;
        private IUsersService _usersService = null!;

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
                ApartmentNumber = 14,
                BuildingNumber = 1,
                StreetNumber = 15,
                AdditionalInfo = "test info"
            }
        };

        [SetUp]
        public void Setup()
        {
            _usersRepository = Substitute.For<IUsersRepository>();
            _usersService = new UsersService(_usersRepository);
        }

        [Test]
        public async Task Authenticate()
        {
            var validToken = "validToken";

            var validAuthenticatedUser = new AuthenticatedUser(
                $"{validCustomer.FirstName} {validCustomer.LastName}", 
                validCustomer.EmailAddress, 
                validCustomer.RoleName,
                validToken, 
                validToken);

            _usersRepository.Authenticate(validCustomer.EmailAddress, validCustomer.Password).Returns(validAuthenticatedUser);
            var authenticatedUser = await _usersService.Authenticate(validCustomer.EmailAddress, validCustomer.Password);

            Assert.That(authenticatedUser.JwtToken, Is.EqualTo(validToken));
            Assert.That(authenticatedUser.RefreshToken, Is.EqualTo(validToken));
        }

        [Test]
        public async Task Register()
        {
            var validToken = "validToken";

            var validAuthenticatedUser = new AuthenticatedUser(
                $"{validCustomer.FirstName} {validCustomer.LastName}",
                validCustomer.EmailAddress,
                validCustomer.RoleName,
                validToken,
                validToken);

            _usersRepository.Register(validCustomer).Returns(validAuthenticatedUser);
            var authenticatedUser = await _usersService.Register(validCustomer);

            Assert.That(authenticatedUser.JwtToken, Is.EqualTo(validToken));
            Assert.That(authenticatedUser.RefreshToken, Is.EqualTo(validToken));
        }
    }
}
