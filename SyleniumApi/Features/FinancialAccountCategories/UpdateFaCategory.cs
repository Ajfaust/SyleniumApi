using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record UpdateFaCategoryCommand(int Id, string Name, FinancialCategoryType Type)
    : IRequest<Result<UpdateFaCategoryResponse>>;

public record UpdateFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class UpdateFaCategoryValidator : AbstractValidator<UpdateFaCategoryCommand>
{
    public UpdateFaCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class UpdateFaCategoryHandler(SyleniumDbContext context)
    : IRequestHandler<UpdateFaCategoryCommand, Result<UpdateFaCategoryResponse>>
{
    public async Task<Result<UpdateFaCategoryResponse>> Handle(UpdateFaCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.FinancialAccountCategories.FindAsync(request.Id);
        if (entity is null) return Result.Fail($"Financial Account Category {request.Id} not found");

        entity.FinancialAccountCategoryName = request.Name;
        entity.FinancialCategoryType = request.Type;

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateFaCategoryResponse(
            entity.FinancialAccountCategoryId,
            entity.FinancialAccountCategoryName,
            entity.FinancialCategoryType
        );

        return Result.Ok(response);
    }
}

public class UpdateFaCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/fa-categories/{id:int}", async (UpdateFaCategoryCommand req, ISender sender) =>
        {
            var result = await sender.Send(req);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}