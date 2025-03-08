using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Features.Comments.Commands.CreateComment;

public record CreateCommentCommand(int StockId, CreateCommentRequest CreateCommentRequest, string Username)
    : IRequest<CommentDto>;