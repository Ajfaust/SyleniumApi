namespace BudgetUpServer.Test
{
    public class LedgersControllerTests : IClassFixture<BudgetUpTestFactory>
    {
        private readonly BudgetUpTestFactory _factory;

        public LedgersControllerTests(BudgetUpTestFactory factory)
        {
            _factory = factory;
        }
    }
}