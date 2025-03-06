using api.Core.Entities;

namespace api.Core.Interfaces.IComment;
public interface ICommentReadRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int id);
}
