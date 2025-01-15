using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers;

namespace SyleniumApi.Tests.Ledgers;

public class GetLedgerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Exists_Should_Get_Ledger()
    {
        var response = await _client.GetAsync($"api/ledgers/{DefaultTestValues.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<GetLedgerResponse>(content)?.Id;
        id.Should().NotBeNull();
        id.Should().Be(DefaultTestValues.Id);
    }

    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        var response = await _client.GetAsync("api/ledgers/2");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}