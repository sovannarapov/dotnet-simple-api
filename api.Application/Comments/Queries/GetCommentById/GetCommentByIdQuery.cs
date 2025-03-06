using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Comments.Queries.GetCommentById;

public record GetCommentByIdQuery(int Id) : IRequest<CommentDto>;
