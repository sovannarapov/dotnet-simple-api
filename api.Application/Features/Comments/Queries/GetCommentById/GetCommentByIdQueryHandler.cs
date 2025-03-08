using api.Application.Dtos.Comment;
using api.Core.Interfaces.IComment;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Comments.Queries.GetCommentById;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentDto>
{
    private readonly ICommentReadRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetCommentByIdQueryHandler(ICommentReadRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.Id);

        if (comment is null) throw new Exception("Comment not found");

        return _mapper.Map<CommentDto>(comment);
    }
}