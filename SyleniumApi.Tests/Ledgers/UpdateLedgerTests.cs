using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers;

namespace SyleniumApi.Tests.Ledgers;

public class UpdateLedgerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;

    [Fact]
    public async Task When_Ledger_Exists_Should_Update_Ledger()
    {
        var command = new UpdateLedgerCommand(DefaultTestValues.Id, "New Name");

        var response = await _client.PutAsJsonAsync($"api/ledgers/{DefaultTestValues.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ledger = await _context.Ledgers.FindAsync(DefaultTestValues.Id);
        ledger.Should().NotBeNull();

        await _context.Entry(ledger!).ReloadAsync();

        ledger!.Name.Should().Be("New Name");
    }

    [Fact]
    public async Task When_Update_Invalid_Should_Return_Bad_Request()
    {
        var command = new UpdateLedgerCommand(DefaultTestValues.Id, new string('a', 300));

        var response = await _client.PutAsJsonAsync($"api/ledgers/{DefaultTestValues.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_Ledger_Not_Exists_Should_Return_Not_Found()
    {
        var command = new UpdateLedgerCommand(2, "New Name");

        var response = await _client.PutAsJsonAsync("api/ledgers/2", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}