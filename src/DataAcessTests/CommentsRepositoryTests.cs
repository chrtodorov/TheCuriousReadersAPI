using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Comments;
using BusinessLayer.Models;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Mappers;
using DataAccess.Repositories;
using DataAccess.Seeders;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessTests
{
    public class CommentsRepositoryTests
    {
        private ICommentsRepository _commentsRepository = null!;
        private DataContext _context = null!;
        private PagingParameters pagingParameters = new PagingParameters
        {
            PageNumber = 1,
            PageSize = 10,
        };

        private static BookEntity book1 = new BookEntity
        {
            BookId = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
            Isbn = "1234567890",
            Title = "Harry Potter",
            Description = "Harry Potter Book",
            Genre = "Fantasy",
            CoverUrl = "http://coverurl",
            PublisherId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2"),
        };

        private static BookEntity book2 = new BookEntity
        {
            BookId = Guid.Parse("9fc0ae59-15cb-1111-9916-4c431383fab5"),
            Isbn = "1234567890",
            Title = "Harry Potter",
            Description = "Harry Potter Book",
            Genre = "Fantasy",
            CoverUrl = "http://coverurl",
            PublisherId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2"),
        };

        private CommentEntity pendingComment1 = new CommentEntity
        {
            Status = CommentStatus.Pending,
            Content = "This is test comment 1",
            Book = book1
        };

        private CommentEntity pendingComment2 = new CommentEntity
        {
            Status = CommentStatus.Pending,
            Content = "This is test comment 2",
            Book = book2
        };


        [SetUp]
        public async Task Setup()
        {
            _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
            if (_context != null)
            {
                _commentsRepository = new CommentsRepository(_context);
            }

            await RoleSeeder.SeedRolesAsync(_context);
            await UserSeeder.SeedUsersAsync(_context);
            pendingComment1.User = _context.Users.First();
            pendingComment2.User = _context.Users.First();
            await _context.Comments.AddRangeAsync(pendingComment1, pendingComment2);
            await _context.SaveChangesAsync();
        }

        [Test]
        public void GetPendingComments()
        {
            pendingComment1.Status = CommentStatus.Pending;
            pendingComment2.Status = CommentStatus.Pending;
            _context.UpdateRange(pendingComment1, pendingComment2);
            _context.SaveChanges();
            var commentsList = _commentsRepository.GetPendingComments(pagingParameters);
            Assert.That(commentsList, Is.Not.Null.Or.Empty);
            Assert.That(commentsList.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetCommentsByBookId()
        {
            var commentsList = _commentsRepository.GetCommentsByBookId(book1.BookId, pagingParameters);
            Assert.That(commentsList, Is.Not.Null.Or.Empty);
            Assert.That(commentsList.Data.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetCommentsByUserId()
        {
            var commentsList = _commentsRepository.GetCommentsByUserId(_context.Users.First().UserId, pagingParameters);
            Assert.That(commentsList, Is.Not.Null.Or.Empty);
            Assert.That(commentsList.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ApproveComment()
        {
            var approvedComment = await _commentsRepository.ApproveComment(pendingComment1.CommentId);
            Assert.That(approvedComment.Status, Is.EqualTo(CommentStatus.Approved));
        }

        [Test]
        public void ApproveComment_Fails()
        {
            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _commentsRepository.ApproveComment(new Guid());
            });
        }

        [Test]
        public async Task RejectComment()
        {
            var approvedComment = await _commentsRepository.RejectComment(pendingComment1.CommentId);
            Assert.That(approvedComment.Status, Is.EqualTo(CommentStatus.Rejected));
        }

        [Test]
        public void RejectComment_Fails()
        {
            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _commentsRepository.RejectComment(new Guid());
            });
        }

        [Test]
        public async Task AddComment()
        {
            var commentToAdd = new Comment
            {
                Status = CommentStatus.Approved,
                Content = "This is test comment 2",
                BookId = book2.BookId,
                UserId = _context.Users.Last().UserId,
            };

            _context.UserBooks.Add(new UserBooks
            {
                UserId = commentToAdd.UserId,
                BookId = commentToAdd.BookId,
            });
            await _context.SaveChangesAsync();

            var result = await _commentsRepository.AddComment(commentToAdd);
            Assert.That(result, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void AddComment_Fails_UserHasNotReadTheBook()
        {
            var commentToAdd = new Comment
            {
                Status = CommentStatus.Pending,
                Content = "This is test comment 2",
                BookId = book1.BookId,
                UserId = _context.Users.First().UserId,
            };

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _commentsRepository.AddComment(commentToAdd);
            });
        }

        [Test]
        public async Task AddComment_Fails_UserDoesNotExist()
        {
            var commentToAdd = new Comment
            {
                Status = CommentStatus.Pending,
                Content = "This is test comment 2",
                BookId = book1.BookId,
                UserId = new Guid(),
            };

            _context.UserBooks.Add(new UserBooks
            {
                UserId = commentToAdd.UserId,
                BookId = commentToAdd.BookId,
            });
            await _context.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _commentsRepository.AddComment(commentToAdd);
            });
        }

        [Test]
        public async Task AddComment_Fails_BookDoesNotExist()
        {
            var commentToAdd = new Comment
            {
                Status = CommentStatus.Pending,
                Content = "This is test comment 2",
                BookId = new Guid(),
                UserId = _context.Users.First().UserId,
            };

            _context.UserBooks.Add(new UserBooks
            {
                UserId = commentToAdd.UserId,
                BookId = commentToAdd.BookId,
            });
            await _context.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _commentsRepository.AddComment(commentToAdd);
            });
        }

    }
}
