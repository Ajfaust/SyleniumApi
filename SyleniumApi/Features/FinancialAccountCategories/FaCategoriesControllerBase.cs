using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

[Produces("application/json")]
[ApiController]
[Route("/api/fa-categories")]
public partial class FaCategoriesController(ILogger logger) : ControllerBase;