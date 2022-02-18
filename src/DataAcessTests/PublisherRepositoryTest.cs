using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Models;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessTests
{
    public class PublisherRepositoryTest
    {
        public IPublishersRepository _publishersRepository;
        public ILogger<PublishersRepository> _logger;
        public DataContext _context;

        PublisherEntity publisherDataEntity = new PublisherEntity
        {
            PublisherId = Guid.Parse("dff22997-43ec-44dc-8856-b528e046f4fe"),
            Name = "Chavdar",
        };

        PublisherEntity publisherDataEntity2 = new PublisherEntity
        {
            PublisherId = Guid.Parse("dff22997-43ec-44dc-8856-b528e046f4f5"),
            Name = "Gosho",
        };

        Publisher publisherData = new Publisher
        {
            PublisherId = Guid.Parse("dff22997-43ec-44dc-8856-b528e046f4fe"),
            Name = "Chavdar",
        };

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<PublishersRepository>>();
            _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
            if (_context != null)
            {
                _publishersRepository = new PublishersRepository(_context, _logger);
            }
        }

        [Test]
        public async Task GetAsync()
        {
            _context.Publishers.Add(publisherDataEntity);
            _context.SaveChanges();

            var testr = _context.Publishers.FirstOrDefault();

            var resultG = await _publishersRepository.Get(testr.PublisherId);
            var test = _context.Publishers.FirstOrDefault();

            Assert.That(resultG, Is.Not.Null);

            Assert.That(resultG.Name, Is.EqualTo(test.Name));
            Assert.That(resultG.PublisherId, Is.EqualTo(test.PublisherId));
        }

        [Test]
        public async Task GetAllAsync()
        {
            _context.Publishers.Add(publisherDataEntity);
            await _context.SaveChangesAsync();

            var testr = _context.Publishers.Select(p => p.PublisherId).ToList();
            var resultG = await _publishersRepository.GetAll();
            var test = _context.Publishers.Select(p => p.PublisherId).ToList();

            Assert.IsNotNull(resultG);

            Assert.AreEqual(resultG.Count, test.Count);
        }

        [Test]
        public async Task CreateAsync()
        {
            var resultC = await _publishersRepository.Create(publisherData);

            var testC = _context.Publishers.FirstOrDefault();

            Assert.That(resultC, Is.Not.Null);

            Assert.That(resultC.PublisherId, Is.EqualTo(testC.PublisherId));
        }

        [Test]
        public async Task UpdateAsync()
        {
            _context.Publishers.Add(publisherDataEntity);
            _context.SaveChanges();

            var testr = _context.Publishers.FirstOrDefault();

            var resultU = await _publishersRepository.Update(testr.PublisherId, publisherData);

            var testU = _context.Publishers.FirstOrDefault();

            Assert.That(resultU, Is.Not.Null);

            Assert.That(resultU.PublisherId, Is.EqualTo(testU.PublisherId));
            Assert.That(resultU.Name, Is.EqualTo(testU.Name));
        }

        [Test]
        public async Task DeleteAsync()
        {
            _context.Publishers.Add(publisherDataEntity);
            _context.SaveChanges();

            var testr = _context.Publishers.FirstOrDefault();

            await _publishersRepository.Delete(testr.PublisherId);

            var testD = _context.Publishers.FirstOrDefault();

            Assert.That(testD, Is.Null);

        }

        [Test]
        public async Task Contains()
        {
            _context.Publishers.Add(publisherDataEntity);
            _context.SaveChanges();

            var testr = _context.Publishers.FirstOrDefault();

            var resultok = await _publishersRepository.Contains(testr.PublisherId);

            Assert.That(resultok, Is.True);
        }
    }
}
