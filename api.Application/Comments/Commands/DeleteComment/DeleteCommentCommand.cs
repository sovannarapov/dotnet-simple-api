using MediatR;

namespace api.Application.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(int Id) : IRequest<string>;
