using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Authors;
using BusinessLayer.Models;
using BusinessLayer.Services;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLayerTests.Authors;

public class AuthorServiceTest
{
    private IAuthorsService _authorsService;
    private IAuthorsRepository _authorsRepository;

    Author authorData = new Author
    {
        AuthorId = Guid.Parse("112233AC-5566-7788-99AA-BBCCDDEEFF00"),
        Name = "Nick",
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
        Assert.AreEqual(authorC.Name, authorData.Name);
        Assert.AreEqual(authorC.Bio, authorData.Bio);
    }


    [Test]
    public async Task UpdateAsync()
    {
        _authorsRepository.Update(authorData.AuthorId,authorData).Returns(authorData);
        var authorU = await _authorsService.Update(authorData.AuthorId, authorData);
        await _authorsRepository.Received(1).Update(authorData.AuthorId, authorData);


        Assert.AreEqual(authorU.AuthorId, authorData.AuthorId );
        Assert.AreEqual(authorU.Name, authorData.Name);
        Assert.AreEqual(authorU.Bio, authorData.Bio);

    }
    [Test]
    public async Task UpdateAsyncFail()
    {
        _authorsRepository.Update(authorData.AuthorId , authorData).Returns(authorData);
        var authorU = await _authorsService.Update(authorData.AuthorId , authorData);
        await _authorsRepository.Received(1).Update(authorData.AuthorId , authorData);

        var fakeAuthorId = Guid.NewGuid();
        var fakeName = "John";
        var fakeBio = "Wait for me";

        Assert.AreNotEqual(authorU.AuthorId, fakeAuthorId);
        Assert.AreNotEqual(authorU.Name, fakeName);
        Assert.AreNotEqual(authorU.Bio, fakeBio);

    }

    [Test]
    public async Task DeleteAsync()
    {
        _authorsRepository.Contains(authorData.AuthorId).Returns(true);
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

    [Test]
    public async Task IsAuthorNameExisting()
    {
        _authorsRepository.IsAuthorNameExisting(authorData.Name).Returns(true);

        var result = await _authorsService.IsAuthorNameExisting(authorData.Name);

        await _authorsRepository.Received(1).IsAuthorNameExisting(authorData.Name);

        Assert.IsTrue(result);
    }
}