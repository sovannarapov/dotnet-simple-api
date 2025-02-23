using Application.Dtos.Comment;
using Core.Entities;
using AutoMapper;

namespace Application.Mappers;

public class CommentMappers : Profile
{
    public CommentMappers()
    {
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Comment, CreateCommentRequestDto>().ReverseMap();
        CreateMap<Comment, UpdateCommentRequestDto>().ReverseMap();
    }
}
