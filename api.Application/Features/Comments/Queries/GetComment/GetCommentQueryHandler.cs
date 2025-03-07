using api.Application.Dtos.Comment;
using api.Core.Interfaces.IComment;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Comments.Queries.GetComment;

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, List<CommentDto>>
{
    private readonly ICommentReadRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetCommentQueryHandler(ICommentReadRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetAllAsync();

        if (comments is null) throw new Exception("Comment not found");

        return _mapper.Map<List<CommentDto>>(comments);
    }
}
