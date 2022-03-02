using System;
using System.Net;
using System.Threading.Tasks;
using API.Controllers;
using BusinessLayer.Interfaces.Authors;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace APITests.Authors;

public class AuthorsControllerTest
{
    private IAuthorsService _authorsService;
    private ILogger<AuthorsController> _logger;

    private AuthorsController _authorsController;

    Author authorData = new Author
    {
        AuthorId = Guid.NewGuid(),
        Name = "Nick",
        Bio = "Whats up guys!"
    };

    AuthorsRequest authorsRequest = new AuthorsRequest
    {
        Name = "Nick",
        Bio = "Whats up guys!"
    };

    [SetUp]
    public void Setup()
    {
        _authorsService= Substitute.For<IAuthorsService>();
        _logger = Substitute.For<ILogger<AuthorsController>>();

        _authorsController = new AuthorsController(_authorsService, _logger);

    }

    [Test]
    public async Task GetAsync_Ok()
    {
        _authorsService.Get(Arg.Any<Guid>()).Returns(authorData);
        var result = await _authorsController.Get(authorData.AuthorId);
        await _authorsService.Received(1).Get(Arg.Any<Guid>());

        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var authorResult = okResult.Value as Author;

        Assert.IsNotNull(authorResult);
        Assert.That(authorResult.AuthorId, Is.EqualTo(authorData.AuthorId));
        Assert.That(authorResult.Name, Is.EqualTo(authorData.Name));
        Assert.That(authorResult.Bio, Is.EqualTo(authorData.Bio));
    }

    [Test]
    public async Task GetAsync_NotFound()
    {
        var fakeId = Guid.NewGuid();
        Author error = null;

        _authorsService.Get(fakeId).Returns(error);
        var result = await _authorsController.Get(fakeId);
        await _authorsService.Received(1).Get(fakeId);

        var okResult = result as ObjectResult;

        Assert.IsNotNull(okResult);
        Assert.That(okResult.StatusCode,Is.EqualTo((int)HttpStatusCode.NotFound));
    }

    [Test]
    public async Task CreateAsync()
    {

        _authorsService.Create(Arg.Any<Author>()).Returns(authorData);

        var resultC = await _authorsController.Create(authorsRequest);

        await _authorsService.Received(1).Create(Arg.Is<Author>(a =>
            a.Name == authorData.Name &&
            a.Bio == authorData.Bio
        ));

        var okResult = resultC as ObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var authorResult = okResult.Value as Author;

        Assert.IsNotNull(authorResult);

        Assert.That(authorResult.Name, Is.EqualTo(authorData.Name));
        Assert.That(authorResult.Bio, Is.EqualTo(authorData.Bio));
    }

    [Test]
    public async Task UpdateAsync()
    {
        _authorsService.Update(Arg.Any<Guid>(), Arg.Any<Author>()).Returns(authorData);

        _authorsService.Contains(Arg.Any<Guid>()).Returns(true);

        var resultU = await _authorsController.Update(authorData.AuthorId, authorsRequest);

        await _authorsService.Received(1).Update(Arg.Any<Guid>(), Arg.Is<Author>(a =>
            a.Name == authorData.Name &&
            a.Bio == authorData.Bio
        ));

        var statusResult = resultU as ObjectResult;

        Assert.IsNotNull(statusResult);
        Assert.AreEqual(200, statusResult.StatusCode);

        var valueResult = statusResult.Value as Author;

        Assert.AreEqual(valueResult.Name, authorData.Name);
    }

    [Test]
    public async Task DeleteAsync()
    {
        await _authorsService.Delete(Arg.Any<Guid>());
        _authorsService.Contains(Arg.Any<Guid>()).Returns(true);

        var resultD = await _authorsController.Delete(authorData.AuthorId);
        await _authorsService.Received(1).Delete(Arg.Any<Guid>());

        Assert.That(resultD, Is.Not.Null);
    }
}