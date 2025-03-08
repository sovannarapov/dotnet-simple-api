using api.Application.Dtos.Comment;
using MediatR;

namespace api.Application.Features.Comments.Queries.GetComment;

public record GetCommentQuery : IRequest<List<CommentDto>>;