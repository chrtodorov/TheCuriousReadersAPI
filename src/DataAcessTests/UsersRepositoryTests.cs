using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models.Requests;
using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessTests
{
    public class UsersRepositoryTests
    {
        public IUsersRepository _usersRepository = null!;
        public ILogger<UsersRepository> _logger = null!;
        private IConfiguration _configuration = null!;
        public DataContext _context = null!;

        public AuthenticateRequest validModel1 = new AuthenticateRequest
        {
            Username = "username1",
            Password = "password1",
            Role = "Librarian",
        };

        public AuthenticateRequest validModel2 = new AuthenticateRequest
        {
            Username = "username2",
            Password = "password2",
            Role = "Customer",
        };

        public AuthenticateRequest validModel3 = new AuthenticateRequest
        {
            Username = "username2",
            Password = "password2",
            Role = "Administrator",
        };

        public AuthenticateRequest invalidModel1 = new AuthenticateRequest
        {
            Username = "test",
            Password = "password2",
            Role = "Administrator",
        };

        public AuthenticateRequest invalidModel2 = new AuthenticateRequest
        {
            Username = "username2",
            Password = "test",
            Role = "Administrator",
        };

        public AuthenticateRequest invalidModel3 = new AuthenticateRequest
        {
            Username = "username2",
            Password = "password2",
            Role = "Test Role",
        };

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<UsersRepository>>();
            _configuration = Substitute.For<IConfiguration>();
            _configuration.GetSection("JwtSecret").Value.Returns("acf484d7-49a9-4e9b-b7eb-9731cabca6c5");
            _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
            if (_context != null)
            {
                _usersRepository = new UsersRepository(_context, _configuration, _logger);
            }
        }

        [Test]
        public void Authenticate()
        {
            var token = _usersRepository.Authenticate(validModel1);
            Assert.That(token, Is.Not.Null.Or.Empty);

            token = _usersRepository.Authenticate(validModel1);
            Assert.That(token, Is.Not.Null.Or.Empty);

            token = _usersRepository.Authenticate(validModel3);
            Assert.That(token, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void Authenticate_Fails()
        {
            var token = _usersRepository.Authenticate(invalidModel1);
            Assert.That(token, Is.Null);

            token = _usersRepository.Authenticate(invalidModel2);
            Assert.That(token, Is.Null);

            token = _usersRepository.Authenticate(invalidModel3);
            Assert.That(token, Is.Null);
        }
    }
}
