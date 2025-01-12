using FluentResults;

namespace SyleniumApi.Shared;

public class EntityNotFoundError(string message) : Error(message);

public class ValidationError(string message) : Error(message);