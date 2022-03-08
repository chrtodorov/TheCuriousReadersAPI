using API.Controllers;
using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITests.Publishers
{
    public class PublishersControllerTest
    {
        private IPublishersService _publishersService;
        private ILogger<PublishersController> _logger;

        private PublishersController _publishersController;

        Publisher publisherData = new Publisher
        {
            PublisherId = Guid.NewGuid(),
            Name = "Chavdar"
        };

        PublisherRequest publisherRequest = new PublisherRequest
        {
            Name = "Chavdar"
        };

        [SetUp]
        public void Setup()
        {
            _publishersService = Substitute.For<IPublishersService>();
            _logger = Substitute.For<ILogger<PublishersController>>();

            _publishersController = new PublishersController(_publishersService, _logger);
        }

        [Test]
        public async Task GetAsync_Ok()
        {
            _publishersService.Get(Arg.Any<Guid>()).Returns(publisherData);
            var result = await _publishersController.Get(publisherData.PublisherId);
            await _publishersService.Received(1).Get(Arg.Any<Guid>());

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(publisherRequest);
            Assert.AreEqual(200, okResult.StatusCode);

            var publisherResult = okResult.Value as Publisher;

            Assert.IsNotNull(publisherResult);
            Assert.That(publisherResult.PublisherId, Is.EqualTo(publisherData.PublisherId));
            Assert.That(publisherResult.Name, Is.EqualTo(publisherData.Name));
        }

        [Test]
        public async Task GetAsync_NotFound()
        {
            var fakeId = Guid.NewGuid();
            Publisher error = null;

            _publishersService.Get(fakeId).Returns(error);
            var result = await _publishersController.Get(fakeId);
            await _publishersService.Received(1).Get(fakeId);

            var okResult = result as ObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreateAsync()
        {
            _publishersService.Create(Arg.Any<Publisher>()).Returns(publisherData);

            var resultC = await _publishersController.Create(publisherRequest);

            await _publishersService.Received(1).Create(Arg.Is<Publisher>(p =>
                p.Name == publisherData.Name));

            var okResult = resultC as ObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var publisherResult = okResult.Value as Publisher;

            Assert.IsNotNull(publisherResult);

            Assert.That(publisherResult.Name, Is.EqualTo(publisherData.Name));
        }

        [Test]
        public async Task UpdateAsync()
        {
            _publishersService.Update(Arg.Any<Guid>(), Arg.Any<Publisher>()).Returns(publisherData);

            _publishersService.Contains(Arg.Any<Guid>()).Returns(true);

            var resultU = await _publishersController.Update(publisherData.PublisherId, publisherRequest);

            await _publishersService.Received(1).Update(Arg.Any<Guid>(), Arg.Is<Publisher>(p =>
                p.Name == publisherData.Name 
            ));

            var statusResult = resultU as ObjectResult;

            Assert.IsNotNull(statusResult);
            Assert.AreEqual(200, statusResult.StatusCode);

            var valueResult = statusResult.Value as Publisher;

            Assert.AreEqual(valueResult.Name, publisherData.Name);
        }

        [Test]
        public async Task DeleteAsync()
        {
            await _publishersService.Delete(Arg.Any<Guid>());
            _publishersService.Contains(Arg.Any<Guid>()).Returns(true);

            var resultD = await _publishersController.Delete(publisherData.PublisherId);
            await _publishersService.Received(1).Delete(Arg.Any<Guid>());

            Assert.That(resultD, Is.Not.Null);
        }
    }
}
