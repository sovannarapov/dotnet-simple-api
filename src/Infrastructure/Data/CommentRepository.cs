using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        /*
         * FirstOrDefaultAsync is used to retrieve the first entity that matches a specified condition
         * returns null if no matching entity is found
         */
        var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

        if (comment == null)
        {
            return null;
        }

        _context.Comments.Remove(comment);

        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context
            .Comments
            .Include(comment => comment.AppUser)
            .Include(comment => comment.Stock)
            .ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context
            .Comments
            .Include(comment => comment.AppUser)
            .FirstOrDefaultAsync(comment => comment.Id == id);
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        /*
         * FindAsync() is specifically designed to look up an entity by its primary key
         * returning null if no entity is found with that key
         */
        var existingComment = await _context.Comments
            .Include(cmt => cmt.AppUser)
            .FirstOrDefaultAsync(cmt => cmt.Id == id);

        if (existingComment == null)
        {
            return null;
        }

        existingComment.Title = comment.Title;
        existingComment.Content = comment.Content;

        await _context.SaveChangesAsync();

        return existingComment;
    }
}
