using api.Application.Dtos.Comment;
using api.Core.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class CommentMappers : Profile
{
    public CommentMappers()
    {
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Comment, CreateCommentRequestDto>().ReverseMap();
        CreateMap<Comment, UpdateCommentRequestDto>().ReverseMap();
    }
}
