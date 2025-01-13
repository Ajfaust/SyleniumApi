using AutoFixture;
using SyleniumApi.Data.Entities;

namespace SyleniumApi.Tests.Ledgers;

public static class LedgerTestHelpers
{
    public static Ledger MakeLedger(this Fixture fixture, int id = 1)
    {
        return fixture.Build<Ledger>()
            .Without(l => l.FinancialAccounts)
            .With(l => l.CreatedDate, DateTime.UtcNow)
            .With(l => l.LedgerId, id)
            .Create();
    }
}