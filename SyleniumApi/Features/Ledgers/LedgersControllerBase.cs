using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

[Route("/api/[controller]")]
[ApiController]
[Produces("application/json")]
public partial class LedgersController(ILogger logger) : ControllerBase;