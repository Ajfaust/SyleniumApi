using FluentResults;
using FluentValidation.Results;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Shared;

public class EntityNotFoundError(string message) : Error(message);

public class ValidationError : Error
{
    public ValidationError(string? message = null, IEnumerable<ValidationFailure>? errors = null) : base(message)
    {
        if (errors != null)
            foreach (var error in errors)
                Message += $"\n{error.PropertyName}: {error.ErrorMessage}";

        Message = Message.Trim();
    }
}

public static class LogMessages
{
    public static void LogNotFoundError(this ILogger logger, Result result)
    {
        logger.Error("Not Found Error: {Message}",
            result.Reasons.Where(r => r is EntityNotFoundError).Select(r => r.Message));
    }

    public static void LogNotFoundError<T>(this ILogger logger, Result<T> result)
    {
        logger.Error("Not Found Error: {Message}",
            result.Reasons.Where(r => r is EntityNotFoundError).Select(r => r.Message));
    }

    public static void LogValidationError<T>(this ILogger logger, Result<T> result)
    {
        logger.Error("Validation Error: {Message}",
            result.Reasons.Where(r => r is ValidationError).Select(r => r.Message));
    }
}