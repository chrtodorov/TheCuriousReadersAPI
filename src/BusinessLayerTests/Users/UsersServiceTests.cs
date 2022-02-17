using BusinessLayer;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models.Requests;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLayerTests.Users
{
    public class UsersServiceTests
    {
        private IUsersRepository _usersRepository = null!;
        private IUsersService _usersService = null!;


        public AuthenticateRequest validModel = new AuthenticateRequest
        {
            Username = "username1",
            Password = "password1",
            Role = "Librarian",
        };

        [SetUp]
        public void Setup()
        {
            _usersRepository = Substitute.For<IUsersRepository>();
            _usersService = new UsersService(_usersRepository);
        }

        [Test]
        public void Authenticate_Success()
        {
            var validToken = "MyValidToken";
            _usersRepository.Authenticate(validModel).Returns(validToken);
            var token = _usersService.Authenticate(validModel);

            Assert.That(token, Is.EqualTo(validToken));
        }
    }
}
