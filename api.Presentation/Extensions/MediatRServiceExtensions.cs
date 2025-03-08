using System.Reflection;
using api.Application.Features.Comments.Commands.CreateComment;
using api.Application.Features.Comments.Commands.DeleteComment;
using api.Application.Features.Comments.Commands.UpdateComment;
using api.Application.Features.Comments.Queries.GetComment;
using api.Application.Features.Comments.Queries.GetCommentById;
using api.Application.Features.Stocks.Commands.CreateStock;
using api.Application.Features.Stocks.Commands.UpdateStock;
using api.Application.Features.Stocks.Queries.GetStock;
using api.Application.Features.Stocks.Queries.GetStockById;

namespace api.Presentation.Extensions;

public static class MediatRServiceExtensions
{
    private static Assembly[] GetHandlerAssemblies()
    {
        return
        [
            // Stock related handlers
            typeof(CreateStockCommandHandler).Assembly,
            typeof(UpdateStockCommandHandler).Assembly,
            typeof(GetStockQueryHandler).Assembly,
            typeof(GetStockByIdQueryHandler).Assembly,

            // Comment related handlers
            typeof(CreateCommentCommandHandler).Assembly,
            typeof(UpdateCommentCommandHandler).Assembly,
            typeof(DeleteCommentCommandHandler).Assembly,
            typeof(GetCommentQueryHandler).Assembly,
            typeof(GetCommentByIdQueryHandler).Assembly
        ];
    }

    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(CreateStockCommandHandler).Assembly); });

        return services;
    }
}