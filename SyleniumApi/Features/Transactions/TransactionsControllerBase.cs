using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public partial class TransactionsController(ILogger logger) : ControllerBase
{
    private readonly ILogger _logger = logger;
}