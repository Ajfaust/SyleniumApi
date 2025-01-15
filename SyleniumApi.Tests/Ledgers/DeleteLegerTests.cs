using System.Net;
using FluentAssertions;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.Ledgers;

public class DeleteLegerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;

    [Fact]
    public async Task When_Ledger_Exists_Should_Delete_Ledger()
    {
        var response = await _client.DeleteAsync($"api/ledgers/{DefaultTestValues.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var ledger = await _context.Ledgers.FindAsync(DefaultTestValues.Id);
        ledger.Should().BeNull();
    }
}