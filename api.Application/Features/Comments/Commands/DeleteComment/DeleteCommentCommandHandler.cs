using api.Core.Interfaces.IComment;
using MediatR;

namespace api.Application.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, string>
{
    private readonly ICommentReadRepository _commentReadRepository;
    private readonly ICommentWriteRepository _commentWriteRepository;

    public DeleteCommentCommandHandler(ICommentWriteRepository commentWriteRepository,
        ICommentReadRepository commentReadRepository)
    {
        _commentWriteRepository = commentWriteRepository;
        _commentReadRepository = commentReadRepository;
    }

    public async Task<string> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentReadRepository.GetByIdAsync(request.Id) ??
                      throw new KeyNotFoundException("Comment not found");
        await _commentWriteRepository.DeleteAsync(comment);

        return "Comment deleted successfully.";
    }
}