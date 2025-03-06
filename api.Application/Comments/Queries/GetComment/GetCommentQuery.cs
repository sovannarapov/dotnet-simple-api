using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Comments.Queries.GetComment;

public record GetCommentQuery : IRequest<List<CommentDto>>;
