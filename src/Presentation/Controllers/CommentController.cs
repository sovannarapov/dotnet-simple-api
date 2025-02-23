using Common;
using Common.Extensions;
using Application.Dtos.Comment;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route(RouteConstants.CommentRoutePrefix)]
[Authorize]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommentService _commentService;
    private readonly IStockService _stockService;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(
        ICommentService commentService, 
        IStockService stockService, 
        UserManager<AppUser> userManager,
        IMapper mapper
    )
    {
        _commentService = commentService;
        _stockService = stockService;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentService.GetAllAsync();
        var commentDto = comments.Select(cmt => _mapper.Map<Comment>(comments)).ToList();

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

        return Ok(_mapper.Map<Comment>(comment));
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
        var appUser = await _userManager.FindByNameAsync(username!);

        var comment = _mapper.Map<Comment>(createCommentRequestDto);

            comment.AppUserId = appUser!.Id;
        await _commentService.CreateAsync(comment);
        
        return CreatedAtAction(nameof(GetById), new { comment.Id }, _mapper.Map<Comment>(comment));
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var comment = await _commentService.UpdateAsync(id, _mapper.Map<Comment>(updateCommentRequestDto));

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(_mapper.Map<Comment>(comment));
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
