using System.Text.Json;
using api.Application.Dtos.Comment;
using api.Core.Interfaces.IComment;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDto>
{
    private readonly ICommentWriteRepository _commentWriteRepository;
    private readonly ICommentReadRepository _commentReadRepository;
    private readonly IMapper _mapper;

    public UpdateCommentCommandHandler(ICommentWriteRepository commentWriteRepository, ICommentReadRepository commentReadRepository, IMapper mapper)
    {
        _commentWriteRepository = commentWriteRepository;
        _commentReadRepository = commentReadRepository;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var existingComment = await _commentReadRepository.GetByIdAsync(request.Id);

        if (existingComment is null) throw new KeyNotFoundException("Comment not found");

        Console.WriteLine("existingComment >>> " + JsonSerializer.Serialize(existingComment));

        _mapper.Map(request.UpdateCommentRequestDto, existingComment);

        await _commentWriteRepository.UpdateAsync(existingComment);

        var commentDto = _mapper.Map<CommentDto>(existingComment);

        commentDto.CreatedBy = existingComment.AppUser.UserName;

        return commentDto;
    }
}
