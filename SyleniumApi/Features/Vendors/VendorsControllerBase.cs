using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

[Produces("application/json")]
[ApiController]
[Route("/api/[controller]")]
public partial class VendorsController(ILogger logger) : ControllerBase;