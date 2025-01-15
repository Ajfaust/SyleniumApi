using System.Net;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.FinancialAccounts;

public class DeleteFinancialAccountTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Exists_Should_Delete_FinancialAccount()
    {
        var response = await _client.DeleteAsync($"api/financial-accounts/{DefaultTestValues.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var financialAccount = await _context.FinancialAccounts.FindAsync(DefaultTestValues.Id);
        financialAccount.Should().BeNull();
    }

    [Fact]
    public async Task When_NotExists_Should_Return_NotFound()
    {
        var response = await _client.DeleteAsync("api/financial-accounts/2");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}