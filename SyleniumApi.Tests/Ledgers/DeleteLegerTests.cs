using System.Net;
using AutoFixture;
using FluentAssertions;

namespace SyleniumApi.Tests.Ledgers;

public class DeleteLegerTests : IClassFixture<IntegrationTestFactory>
{
    private const int LedgerId = 1;
    private readonly HttpClient _client;

    public DeleteLegerTests(IntegrationTestFactory factory)
    {
        Fixture fixture = new();
        _client = factory.CreateClient();
        var context = factory.DbContext;

        var existingLedger = fixture.MakeLedger();
        context.Add(existingLedger);
        context.SaveChanges();
    }

    [Fact]
    public async Task When_Ledger_Exists_Should_Delete_Ledger()
    {
        var response = await _client.DeleteAsync($"api/ledgers/{LedgerId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var ledgerResponse = await _client.GetAsync($"api/ledgers/{LedgerId}");

        ledgerResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}