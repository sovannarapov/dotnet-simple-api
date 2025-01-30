using api.Application.Dtos.Comment;
using api.Application.Mappers;
using api.Common.Extensions;
using api.Core.Entities;
using api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route("api/v{apiVersion:apiVersion}/comment")]
[Authorize]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IStockService _stockService;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(
        ICommentService commentService, 
        IStockService stockService, 
        UserManager<AppUser> userManager
    )
    {
        _commentService = commentService;
        _stockService = stockService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentService.GetAllAsync();
        var commentDto = comments.Select(cmt => cmt.ToCommentDto());

        return Ok(commentDto);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var comment = await _commentService.GetByIdAsync(id);

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
        
        if (!await _stockService.StockExists(stockId))
        {
            return BadRequest("Stock does not exist");
        }

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        
        var comment = createCommentRequestDto.ToCommentFromCreateDto(stockId);

        comment.AppUserId = appUser.Id;
        await _commentService.CreateAsync(comment);

        return CreatedAtAction(nameof(GetById), new { Id = comment.Id }, comment.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comment = await _commentService.UpdateAsync(id, updateCommentRequestDto.ToCommentFromUpdateDto());

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
        var comment = await _commentService.DeleteAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
