using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Features.Comments.Queries.GetCommentById;

public record GetCommentByIdQuery(int Id) : IRequest<CommentDto>;
