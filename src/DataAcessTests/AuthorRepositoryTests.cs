using BusinessLayer.Interfaces.Authors;
using BusinessLayer.Models;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Mappers;
using DataAccess.Repositories;
using DataAccessTests;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorRepositoryTests
{
    public class AuthorRepositoryTests
    {
        public IAuthorsRepository _authorsRepository;
        public ILogger<AuthorsRepository> _logger;
        public DataContext _context;

        AuthorEntity authorDataEntity = new AuthorEntity
        {
            AuthorId = Guid.Parse("112233AC-5566-7788-99AA-BBCCDDEEFF00"),
            FirstName = "Nic",
            LastName = "Lend",
            Bio = "Whats up guys!"
        };


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
            _logger = Substitute.For<ILogger<AuthorsRepository>>();
            _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
            if (_context != null)
            {
                _authorsRepository = new AuthorsRepository(_context, _logger);
            }
        }

        [Test]
        public async Task GetAsync()
        {
            _context.Authors.Add(authorDataEntity);
            _context.SaveChanges();

            var testr = _context.Authors.FirstOrDefault();

            var resultG = await _authorsRepository.Get(testr.AuthorId);
            var test = _context.Authors.FirstOrDefault();

            Assert.That(resultG, Is.Not.Null);

            Assert.That(resultG.FirstName, Is.EqualTo(test.FirstName));
            Assert.That(resultG.AuthorId, Is.EqualTo(test.AuthorId));
        }

        [Test]
        public async Task CreateAsync()
        {
            var resultC = await _authorsRepository.Create(authorData);

            var testC = _context.Authors.FirstOrDefault();

            Assert.That(resultC, Is.Not.Null);

            Assert.That(resultC.AuthorId,Is.EqualTo(testC.AuthorId));
        }

        [Test]
        public async Task UpdateAsync()
        {
            _context.Authors.Add(authorDataEntity);
            _context.SaveChanges();

            var testr = _context.Authors.FirstOrDefault();

            var resultU = await _authorsRepository.Update(testr.AuthorId, authorData);

            var testU = _context.Authors.FirstOrDefault();

            Assert.That(resultU, Is.Not.Null);

            Assert.That(resultU.AuthorId, Is.EqualTo(testU.AuthorId));
            Assert.That(resultU.FirstName, Is.EqualTo(testU.FirstName));
            Assert.That(resultU.LastName, Is.EqualTo(testU.LastName));
            Assert.That(resultU.Bio, Is.EqualTo(testU.Bio));
        }

        [Test]
        public async Task DeleteAsync()
        {
            _context.Authors.Add(authorDataEntity);
            _context.SaveChanges();

            var testr = _context.Authors.FirstOrDefault();

            await _authorsRepository.Delete(testr.AuthorId);

            var testD = _context.Authors.FirstOrDefault();

            Assert.That(testD, Is.Null);

        }

        [Test]
        public async Task Contains()
        {
            _context.Authors.Add(authorDataEntity);
            _context.SaveChanges();

            var testr = _context.Authors.FirstOrDefault();

            var resultok = await _authorsRepository.Contains(testr.AuthorId);

            Assert.That(resultok, Is.True);
        }
    }
}