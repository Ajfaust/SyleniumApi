using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.TransactionCategories;

[Produces("application/json")]
[ApiController]
[Route("api/transaction-categories")]
public partial class TransactionCategoriesController(ILogger logger) : ControllerBase;