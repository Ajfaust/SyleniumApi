using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccounts;

public record CreateFinancialAccountCommand(string Name, int LedgerId, int FaCategoryId)
    : IRequest<Result<CreateFinancialAccountResponse>>;

public record CreateFinancialAccountResponse(int Id, string Name, int LedgerId, int FaCategoryId);

public class CreateFinancialAccountValidator : AbstractValidator<CreateFinancialAccountCommand>
{
    public CreateFinancialAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LedgerId).NotEmpty();
        RuleFor(x => x.FaCategoryId).NotEmpty();
    }
}

public class
    CreateFinancialAccountHandler(SyleniumDbContext context, IValidator<CreateFinancialAccountCommand> validator)
    : IRequestHandler<CreateFinancialAccountCommand,
        Result<CreateFinancialAccountResponse>>
{
    public async Task<Result<CreateFinancialAccountResponse>> Handle(CreateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));

        var entity = new FinancialAccount
        {
            FinancialAccountName = request.Name,
            LedgerId = request.LedgerId,
            FinancialAccountCategoryId = request.FaCategoryId
        };

        context.FinancialAccounts.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateFinancialAccountResponse(
            entity.FinancialAccountId,
            entity.FinancialAccountName,
            entity.LedgerId,
            entity.FinancialAccountCategoryId
        );

        return Result.Ok(response);
    }
}

public class CreateFinancialAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/financial-accounts", async (CreateFinancialAccountCommand req, ISender sender) =>
        {
            var result = await sender.Send(req);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}