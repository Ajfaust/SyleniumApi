using Microsoft.AspNetCore.Mvc;

namespace SyleniumApi.Features.Ledgers;

[Route("/api/[controller]")]
[ApiController]
[Produces("application/json")]
public partial class LedgersController : ControllerBase;