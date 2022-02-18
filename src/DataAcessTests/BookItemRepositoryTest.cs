using BusinessLayer.Interfaces.BookItems;
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
    public class BookItemRepositoryTest
    {
        public IBookItemsRepository _bookItemsRepository;
        public ILogger<BookItemsRepository> _logger;
        public DataContext _context;

        BookItemEntity bookItemsDataEntity = new BookItemEntity
        {
            BookItemId = Guid.Parse("dff22997-43ec-44dc-8856-b528e046f4fe"),
            Barcode = "1234567",
            BorrowedDate = new DateTime(2015 - 06 - 12),
            ReturnDate = new DateTime(2015 - 07 - 12),
            BookStatus = BusinessLayer.Enumerations.BookItemStatusEnumeration.Available,
            BookId = Guid.Parse("4b156c4e-dada-43d9-828e-714ae132033b")
        };


        BookItem bookItemsData = new BookItem
        {
            BookItemId = Guid.Parse("dff22997-43ec-44dc-8856-b528e046f4fe"),
            Barcode = "1234567",
            BorrowedDate = new DateTime(2015 - 06 - 12),
            ReturnDate = new DateTime(2015 - 07 - 12),
            BookStatus = BusinessLayer.Enumerations.BookItemStatusEnumeration.Available,
            BookId = Guid.Parse("4b156c4e-dada-43d9-828e-714ae132033b")
        };

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<BookItemsRepository>>();
            _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();

            if (_context != null)
            {
                _bookItemsRepository = new BookItemsRepository(_context, _logger);
            }
        }

        [Test]
        public async Task GetAsync()
        {
            _context.BookItems.Add(bookItemsDataEntity);
            _context.SaveChanges();

            var testr = _context.BookItems.FirstOrDefault();

            var resultG = await _bookItemsRepository.Get(testr.BookItemId);
            var test = _context.BookItems.FirstOrDefault();

            Assert.That(resultG, Is.Not.Null);

            Assert.That(resultG.Barcode, Is.EqualTo(test.Barcode));
            Assert.That(resultG.BorrowedDate, Is.EqualTo(test.BorrowedDate));
            Assert.That(resultG.ReturnDate, Is.EqualTo(test.ReturnDate));
            Assert.That(resultG.BookStatus, Is.EqualTo(test.BookStatus));
            Assert.That(resultG.BookId, Is.EqualTo(test.BookId));
        }

        [Test]
        public async Task CreateAsync()
        {
            var resultC = await _bookItemsRepository.Create(bookItemsData);

            var testC = _context.BookItems.FirstOrDefault();

            Assert.That(resultC, Is.Not.Null);

            Assert.That(resultC.BookItemId, Is.EqualTo(testC.BookItemId));
        }

        [Test]
        public async Task UpdateAsync()
        {
            _context.BookItems.Add(bookItemsDataEntity);
            _context.SaveChanges();

            var testr = _context.BookItems.FirstOrDefault();

            var resultU = await _bookItemsRepository.Update(testr.BookItemId, bookItemsData);

            var testU = _context.BookItems.FirstOrDefault();

            Assert.That(resultU, Is.Not.Null);

            Assert.That(resultU.BookItemId, Is.EqualTo(testU.BookItemId));
            Assert.That(resultU.Barcode, Is.EqualTo(testU.Barcode));
            Assert.That(resultU.BorrowedDate, Is.EqualTo(testU.BorrowedDate));
            Assert.That(resultU.ReturnDate, Is.EqualTo(testU.ReturnDate));
            Assert.That(resultU.BookStatus, Is.EqualTo(testU.BookStatus));
            Assert.That(resultU.BookId, Is.EqualTo(testU.BookId));
        }

        [Test]
        public async Task DeleteAsync()
        {
            _context.BookItems.Add(bookItemsDataEntity);
            _context.SaveChanges();

            var testr = _context.BookItems.FirstOrDefault();

            await _bookItemsRepository.Delete(testr.BookItemId);

            var testD = _context.BookItems.FirstOrDefault();

            Assert.That(testD, Is.Null);

        }

        [Test]
        public async Task Contains()
        {
            _context.BookItems.Add(bookItemsDataEntity);
            _context.SaveChanges();

            var testr = _context.BookItems.FirstOrDefault();

            var resultok = await _bookItemsRepository.Contains(testr.BookItemId);

            Assert.That(resultok, Is.True);
        }
    }
}
