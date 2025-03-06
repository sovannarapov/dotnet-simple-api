using api.Application.Comments.Commands.CreateComment;
using api.Application.Comments.Commands.DeleteComment;
using api.Application.Comments.Commands.UpdateComment;
using api.Application.Comments.Queries.GetComment;
using api.Application.Comments.Queries.GetCommentById;
using api.Application.Dtos.Comment;
using api.Common;
using api.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route(RouteConstants.CommentRoutePrefix)]
[Authorize]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetCommentQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var query = new GetCommentByIdQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] int stockId,
        [FromBody] CreateCommentRequestDto createCommentRequestDto)
    {
        try
        {
            var username = User.GetUsername();

            if (username is null) return Unauthorized("User is not authenticated");

            var command = new CreateCommentCommand(stockId, createCommentRequestDto, username);
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
    {
        try
        {
            var command = new UpdateCommentCommand(id, updateCommentRequestDto);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var command = new DeleteCommentCommand(id);
            await _mediator.Send(command);

            return Ok("Comment deleted successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return NotFound(ex.Message);
        }
    }
}
