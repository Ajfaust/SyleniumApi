using Microsoft.AspNetCore.Mvc;

namespace SyleniumApi.Features.FinancialAccounts;

[Route("api/financial-accounts")]
[ApiController]
[Produces("application/json")]
public partial class FinancialAccountsController : ControllerBase;