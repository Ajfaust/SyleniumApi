using Microsoft.AspNetCore.Mvc;

namespace SyleniumApi.Features.Vendors;

[Produces("application/json")]
[ApiController]
[Route("/api/[controller]")]
public partial class VendorsController : ControllerBase;