using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.Ledgers;

public record CreateLedgerCommand(string Name) : IRequest<Result<CreateLedgerResponse>>;

public record CreateLedgerResponse(int Id, string Name);

public class CreateLedgerValidator : AbstractValidator<CreateLedgerCommand>
{
    public CreateLedgerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateLedgerHandler(SyleniumDbContext context, IValidator<CreateLedgerCommand> validator) : IRequestHandler<CreateLedgerCommand, Result<CreateLedgerResponse>>
{
    public async Task<Result<CreateLedgerResponse>> Handle(CreateLedgerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage));
        }
        
        var entity = new Ledger
        {
            LedgerName = request.Name,
            CreatedDate = DateTime.UtcNow,
        };

        context.Ledgers.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateLedgerResponse(Id: entity.LedgerId, Name: entity.LedgerName);

        return Result.Ok(response);
    }
}

public class CreateLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ledgers", async (CreateLedgerCommand req, ISender sender) =>
        {
            var result = await sender.Send(req);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}