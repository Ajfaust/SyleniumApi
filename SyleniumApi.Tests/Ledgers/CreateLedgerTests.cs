using System.Net.Http.Json;
using System.Text.Json;
using AutoFixture;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers;
using Xunit.Abstractions;

namespace SyleniumApi.Tests.Ledgers;

public class CreateLedgerTests(IntegrationTestFactory factory, ITestOutputHelper output)
    : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async Task Should_Create_Ledger()
    {
        var command = _fixture.Create<CreateLedgerCommand>();
        var response = await _client.PostAsJsonAsync("/api/ledgers", command);

        _output.WriteLine(JsonSerializer.Serialize(response));
        response.EnsureSuccessStatusCode();
    }
}