using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Comments;
using BusinessLayer.Models;
using DataAccess.Entities;
using DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly DataContext _dbContext;

        public CommentsRepository(DataContext dataContext)
        {
            this._dbContext = dataContext;
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserId == comment.UserId);
            if (!userExists)
            {
                throw new ArgumentException($"User with id: {comment.UserId} does not exist");
            }

            var bookExists = await _dbContext.Books.AnyAsync(b => b.BookId == comment.BookId);
            if (!bookExists)
            {
                throw new ArgumentException($"Book with id: {comment.BookId} does not exist");
            }

            var userCommentedBook = await _dbContext.UserBooks.AnyAsync(ub => ub.UserId == comment.UserId && ub.BookId == comment.BookId);
            if (!userCommentedBook)
            {
                throw new ArgumentException($"The current user has not commented on book with id: {comment.BookId}");
            }

            var createdEntity = await _dbContext.Comments.AddAsync(comment.ToCommentEntity());
            await _dbContext.SaveChangesAsync();

            var resultEntity = await _dbContext.Comments
                .Include(c => c.Book)
                .Include(c => c.User)
                .FirstAsync(c => c.CommentId == createdEntity.Entity.CommentId);

            return resultEntity.ToComment();
        }

        public async Task<Comment> ApproveComment(Guid commentId)
        {
            var commentEntity = await _dbContext.Comments
                .Include(c => c.Book)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (commentEntity is null)
            {
                throw new ArgumentException($"Comment with id: {commentId} does not exist.");
            }

            commentEntity.Status = CommentStatus.Approved;
            _dbContext.Update(commentEntity);
            await _dbContext.SaveChangesAsync();

            return commentEntity.ToComment();
        }

        public async Task<Comment> RejectComment(Guid commentId)
        {
            var commentEntity = await _dbContext.Comments
                .Include(c => c.Book)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (commentEntity is null)
            {
                throw new ArgumentException($"Comment with id: {commentId} does not exist.");
            }

            commentEntity.Status = CommentStatus.Rejected;
            _dbContext.Update(commentEntity);
            await _dbContext.SaveChangesAsync();

            return commentEntity.ToComment();
        }

        public PagedList<Comment> GetCommentsByBookId(Guid bookId, PagingParameters pagingParameters)
        {
            var query = GetCommentsQuery(c => c.Book.BookId == bookId);
            return PagedList<Comment>.ToPagedList(query, pagingParameters.PageNumber, pagingParameters.PageSize);
        }

        public PagedList<Comment> GetCommentsByUserId(Guid userId, PagingParameters pagingParameters)
        {
            var query = GetCommentsQuery(c => c.User.UserId == userId);
            return PagedList<Comment>.ToPagedList(query, pagingParameters.PageNumber, pagingParameters.PageSize);
        }

        public PagedList<Comment> GetPendingComments(PagingParameters pagingParameters)
        {
            var query = GetCommentsQuery(c => c.Status == CommentStatus.Pending);
            return PagedList<Comment>.ToPagedList(query, pagingParameters.PageNumber, pagingParameters.PageSize);
        }

        private IQueryable<Comment> GetCommentsQuery(Expression<Func<CommentEntity, bool>> predicate)
        {
            return _dbContext.Comments
                .Include(c => c.Book)
                .Include(c => c.User)
                .AsNoTracking()
                .Where(predicate)
                .Select(c => c.ToComment());
        }
    }
}
