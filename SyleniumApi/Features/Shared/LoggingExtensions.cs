using FluentValidation.Results;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Shared;

public static class LoggingExtensions
{
    public static void LogValidationErrors(this ILogger logger, string endpointName, List<ValidationFailure> failures)
    {
        logger.Error($"Validation failed for {endpointName}");
        foreach (var f in failures)
            logger.Error("{0}: {1}", f.PropertyName, f.ErrorMessage);
    }
}