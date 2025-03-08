using api.Core.Entities;

namespace api.Core.Interfaces.IComment;

public interface ICommentWriteRepository
{
    Task CreateAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(Comment comment);
}