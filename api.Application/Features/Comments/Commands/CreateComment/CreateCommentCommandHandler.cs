using api.Application.Dtos.Comment;
using api.Core.Entities;
using api.Core.Interfaces;
using api.Core.Interfaces.IComment;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace api.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly ICommentWriteRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;

    public CreateCommentCommandHandler(
        ICommentWriteRepository commentRepository,
        IStockRepository stockRepository,
        UserManager<AppUser> userManager,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByIdAsync(request.StockId);

        if (stock is null) throw new KeyNotFoundException("Stock does not exist");

        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null) throw new UnauthorizedAccessException("User does not exist");

        var comment = _mapper.Map<Comment>(request.CreateCommentRequestDto);

        comment.AppUserId = user.Id;
        comment.StockId = request.StockId;

        await _commentRepository.CreateAsync(comment);

        var commentDto = _mapper.Map<CommentDto>(comment);

        commentDto.CreatedBy = user.UserName;

        return commentDto;
    }
}
