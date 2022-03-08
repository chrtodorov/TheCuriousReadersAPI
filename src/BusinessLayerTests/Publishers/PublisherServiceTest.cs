using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Models;
using BusinessLayer.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerTests.Publishers
{
    public class PublisherServiceTest
    {
        private IPublishersService _publishersService;
        private IPublishersRepository _publishersRepository;

        Publisher publisherData = new Publisher
        {
            PublisherId = Guid.Parse("e634eab7-253a-442c-b615-299c9e26e5db"),
            Name = "Chavdar"
        };

        [SetUp]
        public void Setup()
        {
            _publishersRepository = Substitute.For<IPublishersRepository>();
            _publishersService = new PublisherService(_publishersRepository);

        }

        [Test]
        public async Task GetAsync_Success()
        {

            _publishersRepository.Get(publisherData.PublisherId).Returns(publisherData);

            var publisher = await _publishersService.Get(publisherData.PublisherId);

            Assert.AreEqual(publisherData.PublisherId, publisher.PublisherId);

        }

        [Test]
        public async Task GetAsync_Fail()
        {

            _publishersRepository.Get(publisherData.PublisherId).Returns(publisherData);
            var fakeId = Guid.NewGuid();

            var publisher = await _publishersService.Get(publisherData.PublisherId);

            Assert.AreNotEqual(fakeId, publisher.PublisherId);
        }

        [Test]
        public async Task CreateAsync()
        {
            _publishersRepository.Create(publisherData).Returns(publisherData);
            var publisherC = await _publishersService.Create(publisherData);
            await _publishersRepository.Received(1).Create(publisherData);

            Assert.AreEqual(publisherC.PublisherId, publisherData.PublisherId);
            Assert.AreEqual(publisherC.Name, publisherData.Name);
        }


        [Test]
        public async Task UpdateAsync()
        {
            _publishersRepository.Contains(publisherData.PublisherId).Returns(true);
            _publishersRepository.Update(publisherData.PublisherId, publisherData).Returns(publisherData);
            var publisherU = await _publishersService.Update(publisherData.PublisherId, publisherData);
            await _publishersRepository.Received(1).Update(publisherData.PublisherId, publisherData);


            Assert.AreEqual(publisherU.PublisherId, publisherData.PublisherId);
            Assert.AreEqual(publisherU.Name, publisherData.Name);
        }

        [Test]
        public async Task UpdateAsyncFail()
        {
            _publishersRepository.Contains(publisherData.PublisherId).Returns(true);
            _publishersRepository.Update(publisherData.PublisherId, publisherData).Returns(publisherData);
            var authorU = await _publishersService.Update(publisherData.PublisherId, publisherData);
            await _publishersRepository.Received(1).Update(publisherData.PublisherId, publisherData);

            var fakeAuthorId = Guid.NewGuid();
            var fakeName = "John";

            Assert.AreNotEqual(authorU.PublisherId, fakeAuthorId);
            Assert.AreNotEqual(authorU.Name, fakeName);

        }

        [Test]
        public async Task DeleteAsync()
        {
            _publishersRepository.Contains(publisherData.PublisherId).Returns(true);
            var deleted = _publishersService.Delete(publisherData.PublisherId);
            await _publishersRepository.Received(1).Delete(publisherData.PublisherId);
        }

        [Test]
        public async Task Contains()
        {
            bool expectedRes = true;
            _publishersRepository.Contains(publisherData.PublisherId).Returns(expectedRes);
            var result = await _publishersService.Contains(publisherData.PublisherId);
            await _publishersRepository.Received(1).Contains(publisherData.PublisherId);
            Assert.That(result, Is.EqualTo(expectedRes));
        }

        [Test]
        public async Task IsPublisherNameExisting()
        {
            _publishersRepository.IsPublisherNameExisting(publisherData.Name).Returns(true);

            var result = await _publishersService.IsPublisherNameExisting(publisherData.Name);

            await _publishersRepository.Received(1).IsPublisherNameExisting(publisherData.Name);

            Assert.IsTrue(result);
        }
    }
}
