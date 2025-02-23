using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        var comments = await _commentRepository.GetAllAsync();

        return comments;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        return comment;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        var createdComment = await _commentRepository.CreateAsync(comment);

        return createdComment;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var updatedComment = await _commentRepository.UpdateAsync(id, comment);

        return updatedComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var deletedComment = await _commentRepository.DeleteAsync(id);

        return deletedComment;
    }
}
