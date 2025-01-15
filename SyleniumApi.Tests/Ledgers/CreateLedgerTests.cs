using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers;

namespace SyleniumApi.Tests.Ledgers;

public class CreateLedgerTests(IntegrationTestFactory factory)
    : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Should_Create_Ledger()
    {
        var command = _fixture.Create<CreateLedgerCommand>();
        var response = await _client.PostAsJsonAsync("api/ledgers", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<CreateLedgerResponse>(content)?.Id;
        id.Should().NotBeNull();

        var ledger = await _context.Ledgers.FindAsync(id);
        ledger.Should().NotBeNull();
    }

    [Fact]
    public async Task When_Name_Invalid_Should_Return_Bad_Request()
    {
        _fixture.Register(() => new string('a', 300));
        var command = _fixture.Create<CreateLedgerCommand>();

        var response = await _client.PostAsJsonAsync("api/ledgers", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}