using api.Application.Dtos.Comment;
using api.Core.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class CommentMappers : Profile
{
    public CommentMappers()
    {
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Comment, CreateCommentRequest>().ReverseMap();
        CreateMap<Comment, UpdateCommentRequest>().ReverseMap();
    }
}