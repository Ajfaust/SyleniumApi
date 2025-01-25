using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers;

namespace SyleniumApi.Tests.Ledgers;

public class GetLegerAccountsTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Ledger_Exists_Should_Return_All_Accounts()
    {
        // Arrange
        const int numAccounts = 3;
        var accounts = _fixture.Build<FinancialAccount>()
            .With(a => a.LedgerId, DefaultTestValues.Id)
            .With(a => a.FinancialAccountCategoryId, DefaultTestValues.Id)
            .Without(a => a.Id)
            .Without(a => a.Ledger)
            .Without(a => a.FinancialAccountCategory)
            .CreateMany(numAccounts);
        await _context.AddRangeAsync(accounts);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/ledgers/{DefaultTestValues.Id}/accounts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var getLedgerAccountsResponse = JsonConvert.DeserializeObject<GetLedgerAccountsResponse>(content);

        getLedgerAccountsResponse.Should().NotBeNull();
        getLedgerAccountsResponse.Accounts.Count.Should().Be(numAccounts + 1);
    }

    [Fact]
    public async Task When_Ledger_Not_Exists_Should_Return_Bad_Request()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/api/ledgers/2/accounts");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}