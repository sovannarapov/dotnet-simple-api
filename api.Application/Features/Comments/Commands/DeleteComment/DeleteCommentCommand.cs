using MediatR;

namespace api.Application.Features.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(int Id) : IRequest<string>;
