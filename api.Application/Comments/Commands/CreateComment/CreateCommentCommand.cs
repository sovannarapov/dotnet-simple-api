using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Comments.Commands.CreateComment;

public record CreateCommentCommand(int StockId, CreateCommentRequestDto CreateCommentRequestDto, string Username) : IRequest<CommentDto>;
