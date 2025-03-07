using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Features.Comments.Commands.CreateComment;

public record CreateCommentCommand(int StockId, CreateCommentRequestDto CreateCommentRequestDto, string Username) : IRequest<CommentDto>;
