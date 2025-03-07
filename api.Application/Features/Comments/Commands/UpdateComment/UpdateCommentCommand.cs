using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Features.Comments.Commands.UpdateComment;

public record UpdateCommentCommand(int Id, UpdateCommentRequestDto UpdateCommentRequestDto) : IRequest<CommentDto>;
