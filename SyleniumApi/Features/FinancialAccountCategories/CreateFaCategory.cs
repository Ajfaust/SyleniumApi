using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record CreateFaCategoryCommand(string Name, FinancialCategoryType Type)
    : IRequest<Result<CreateFaCategoryResponse>>;

public record CreateFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class CreateFaCategoryValidator : AbstractValidator<CreateFaCategoryCommand>
{
    public CreateFaCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class CreateFaCategoryHandler(SyleniumDbContext context, IValidator<CreateFaCategoryCommand> validator)
    : IRequestHandler<CreateFaCategoryCommand, Result<CreateFaCategoryResponse>>
{
    public async Task<Result<CreateFaCategoryResponse>> Handle(CreateFaCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage));

        var entity = new FinancialAccountCategory
        {
            FinancialAccountCategoryName = request.Name,
            FinancialCategoryType = request.Type
        };

        context.FinancialAccountCategories.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateFaCategoryResponse(
            entity.FinancialAccountCategoryId,
            entity.FinancialAccountCategoryName,
            entity.FinancialCategoryType
        );

        return Result.Ok(response);
    }
}

public class CreateFaCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/fa-categories", async (CreateFaCategoryCommand req, ISender sender) =>
        {
            var result = await sender.Send(req);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}