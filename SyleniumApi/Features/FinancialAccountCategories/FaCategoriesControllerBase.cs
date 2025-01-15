using Microsoft.AspNetCore.Mvc;

namespace SyleniumApi.Features.FinancialAccountCategories;

[Produces("application/json")]
[ApiController]
[Route("/api/fa-categories")]
public partial class FaCategoriesController : ControllerBase;