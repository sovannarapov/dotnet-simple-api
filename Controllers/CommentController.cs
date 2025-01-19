using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDto = comments.Select(cmt => cmt.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto createCommentRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var comment = createCommentRequestDto.ToCommentFromCreateDto(stockId);

            await _commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { Id = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var comment = await _commentRepository.UpdateAsync(id, updateCommentRequestDto.ToCommentFromUpdateDto());

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepository.DeleteAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
