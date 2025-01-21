using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                CreatedBy = comment.AppUser.UserName,
                StockId = comment.StockId
            };
        }

        public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto createCommentRequestDto, int stockId)
        {
            return new Comment
            {
                Title = createCommentRequestDto.Title,
                Content = createCommentRequestDto.Content,
                StockId = stockId
            };
        }

        public static Comment ToCommentFromUpdateDto(this UpdateCommentRequestDto updateCommentRequestDto)
        {
            return new Comment
            {
                Title = updateCommentRequestDto.Title,
                Content = updateCommentRequestDto.Content
            };
        }
    }
}
