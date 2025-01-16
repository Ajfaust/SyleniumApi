using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccounts;

[Route("api/financial-accounts")]
[ApiController]
[Produces("application/json")]
public partial class FinancialAccountsController(ILogger logger) : ControllerBase;