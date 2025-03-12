using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject query)
        {
            var comments = _dbContext.Comments.Include(comment => comment.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                comments = comments.Where(comment => comment.Stock.Symbol.Contains(query.Symbol));
            }
            comments = query.IsDecsending
                ? comments.OrderByDescending(comment => comment.CreatedOn)
                : comments.OrderBy(comment => comment.CreatedOn);

            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext
                .Comments.Include(comment => comment.AppUser)
                .FirstOrDefaultAsync(comment => comment.Id == id);
        }

        public async Task<List<Comment>> GetAllByStockIdAsync(int stockId)
        {
            return await _dbContext
                .Comments.Where(comment => comment.StockId == stockId)
                .ToListAsync();
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentDto)
        {
            var existingComment = await _dbContext.Comments.FindAsync(id);

            if (existingComment == null)
            {
                return null;
            }

            existingComment.Title = commentDto.Title;
            existingComment.Content = commentDto.Content;

            await _dbContext.SaveChangesAsync();

            return existingComment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = _dbContext.Comments.FirstOrDefault(comment => comment.Id == id);

            if (comment == null)
            {
                return null;
            }

            _dbContext.Remove(comment);
            await _dbContext.SaveChangesAsync();

            return comment;
        }
    }
}
