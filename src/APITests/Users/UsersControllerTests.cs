using API.Controllers;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace APITests.Users
{
    public class UsersControllerTests
    {
        private IUsersService _usersService = null!;
        private ILogger<UsersController> _logger = null!;
        private UsersController _usersController = null!;

        public AuthenticateRequest validModel = new AuthenticateRequest
        {
            Username = "username1",
            Password = "password1",
            Role = "Librarian",
        };

        [SetUp]
        public void Setup()
        {
            _usersService = Substitute.For<IUsersService>();
            _logger = Substitute.For<ILogger<UsersController>>();
            _usersController = new UsersController(_usersService, _logger);

        }

        [Test]
        public async Task Authenticate_Ok()
        {
            _usersService.Authenticate(validModel).Returns("MyValidToken");
            var result = await _usersController.Authenticate(validModel);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task Authenticate_BadRequest()
        {
            string fakeToken = null;
            _usersService.Authenticate(validModel).Returns(fakeToken);
            var result = await _usersController.Authenticate(validModel);

            var responseResult = result as ObjectResult;

            Assert.IsNotNull(responseResult);
            Assert.That(responseResult.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));

        }
    }
}
