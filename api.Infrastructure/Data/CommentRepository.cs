using api.Core.Entities;
using api.Core.Interfaces.IComment;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Data;

public class CommentRepository : ICommentReadRepository, ICommentWriteRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments
            .AsNoTracking()
            .Include(cmt => cmt.AppUser)
            .Include(cmt => cmt.Stock)
            .ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments
            .AsNoTracking()
            .Include(cmt => cmt.AppUser)
            .FirstOrDefaultAsync(comment => comment.Id == id);
    }

    public async Task CreateAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Comment comment)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Comment comment)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
