using Microsoft.AspNetCore.Mvc;

namespace SyleniumApi.Features.FinancialAccountCategories;

[Produces("application/json")]
[ApiController]
[Route("/api/[controller]")]
public partial class FaCategoriesController : ControllerBase;