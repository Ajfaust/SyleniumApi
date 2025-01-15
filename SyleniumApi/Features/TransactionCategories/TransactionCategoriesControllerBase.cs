using Microsoft.AspNetCore.Mvc;

namespace SyleniumApi.Features.TransactionCategories;

[Produces("application/json")]
[ApiController]
[Route("api/transaction-categories")]
public partial class TransactionCategoriesController : ControllerBase;