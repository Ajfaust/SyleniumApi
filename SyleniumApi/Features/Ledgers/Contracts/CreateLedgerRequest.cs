using MediatR;
using FluentResults;

namespace SyleniumApi.Contracts;

public class CreateLedgerRequest
{
    public string LedgerName { get; set; } = string.Empty;
}