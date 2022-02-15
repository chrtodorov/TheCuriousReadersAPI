using NUnit.Framework;
using BusinessLayer.Interfaces.Authors;
using DataAccess;
using Microsoft.Extensions.Logging;
using NSubstitute;
using BusinessLayer;
using System;
using System.Threading.Tasks;
using BusinessLayer.Models;
using BusinessLayer.Services;

namespace AuthorServiceTest
{
    public class AuthorServiceTest
    {
        private IAuthorsService _authorsService;
        private IAuthorsRepository _authorsRepository;

        Author authorData = new Author
        {
            AuthorId = Guid.Parse("112233AC-5566-7788-99AA-BBCCDDEEFF00"),
            FirstName = "Nick",
            LastName = "Lender",
            Bio = "Whats up guys!"
        };

        [SetUp]
        public void Setup()
        {
            _authorsRepository = Substitute.For<IAuthorsRepository>();   
            _authorsService = new AuthorsService(_authorsRepository);

        }

        [Test]
        public async Task GetAsync_Success()
        {

            _authorsRepository.Get(authorData.AuthorId ).Returns(authorData);

            var author = await _authorsService.Get(authorData.AuthorId );

            Assert.AreEqual(authorData.AuthorId, author.AuthorId);

        }

        [Test]
        public async Task GetAsync_Fail()
        {

            _authorsRepository.Get(authorData.AuthorId).Returns(authorData);
            var fakeId = Guid.NewGuid();

            var author = await _authorsService.Get(authorData.AuthorId );

            Assert.AreNotEqual(fakeId, author.AuthorId);

        }

        [Test]
        public async Task CreateAsync()
        {
            _authorsRepository.Create(authorData).Returns(authorData);
            var authorC = await _authorsService.Create(authorData);
            await _authorsRepository.Received(1).Create(authorData);

            Assert.AreEqual(authorC.AuthorId, authorData.AuthorId);
            Assert.AreEqual(authorC.FirstName, authorData.FirstName);
            Assert.AreEqual(authorC.LastName, authorData.LastName);
            Assert.AreEqual(authorC.Bio, authorData.Bio);
        }


        [Test]
        public async Task UpdateAsync()
        {
            _authorsRepository.Update(authorData.AuthorId,authorData).Returns(authorData);
            var authorU = await _authorsService.Update(authorData.AuthorId, authorData);
            await _authorsRepository.Received(1).Update(authorData.AuthorId, authorData);


            Assert.AreEqual(authorU.AuthorId, authorData.AuthorId );
            Assert.AreEqual(authorU.FirstName, authorData.FirstName);
            Assert.AreEqual(authorU.LastName, authorData.LastName);
            Assert.AreEqual(authorU.Bio, authorData.Bio);

        }
        [Test]
        public async Task UpdateAsyncFail()
        {
            _authorsRepository.Update(authorData.AuthorId , authorData).Returns(authorData);
            var authorU = await _authorsService.Update(authorData.AuthorId , authorData);
            await _authorsRepository.Received(1).Update(authorData.AuthorId , authorData);

            var fakeAuthorId = Guid.NewGuid();
            var fakeFirstName = "John";
            var fakeLastName = "Lennon";
            var fakeBio = "Wait for me";

            Assert.AreNotEqual(authorU.AuthorId, fakeAuthorId);
            Assert.AreNotEqual(authorU.FirstName, fakeFirstName);
            Assert.AreNotEqual(authorU.LastName, fakeLastName);
            Assert.AreNotEqual(authorU.Bio, fakeBio);

        }

        [Test]
        public async Task DeleteAsync()
        {
            var deleted = _authorsService.Delete(authorData.AuthorId );
            await _authorsRepository.Received(1).Delete(authorData.AuthorId );
        }

        [Test]
        public async Task Contains()
        {
            bool expectedRes = true;
            _authorsRepository.Contains(authorData.AuthorId ).Returns(expectedRes);
            var result = await _authorsService.Contains(authorData.AuthorId );
            await _authorsRepository.Received(1).Contains(authorData.AuthorId );
            Assert.That(result,Is.EqualTo(expectedRes)); 
        }
    }
}