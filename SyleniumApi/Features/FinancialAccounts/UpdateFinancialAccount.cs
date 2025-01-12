using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccounts;

public record UpdateFinancialAccountRequest(int Id, string Name)
    : IRequest<Result<UpdateFinancialAccountResponse>>;

public record UpdateFinancialAccountResponse(int Id, string Name);

public class UpdateFinancialAccountValidator : AbstractValidator<UpdateFinancialAccountRequest>
{
    public UpdateFinancialAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateFinancialAccountHandler(
    SyleniumDbContext context,
    IValidator<UpdateFinancialAccountRequest> validator)
    : IRequestHandler<UpdateFinancialAccountRequest, Result<UpdateFinancialAccountResponse>>
{
    public async Task<Result<UpdateFinancialAccountResponse>> Handle(UpdateFinancialAccountRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));

        var entity = await context.FinancialAccounts.FindAsync(request.Id);
        if (entity == null)
            return Result.Fail<UpdateFinancialAccountResponse>("Financial account not found");

        entity.FinancialAccountName = request.Name;

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateFinancialAccountResponse(entity.FinancialAccountId, entity.FinancialAccountName);

        return Result.Ok(response);
    }
}

public class UpdateFinancialAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/financial-accounts/{id:int}", async (UpdateFinancialAccountRequest req, ISender sender) =>
        {
            var result = await sender.Send(req);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}