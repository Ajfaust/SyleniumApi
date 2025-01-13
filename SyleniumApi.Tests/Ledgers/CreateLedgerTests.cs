using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoFixture;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers;
using Xunit.Abstractions;
using FluentAssertions;

namespace SyleniumApi.Tests.Ledgers;

public class CreateLedgerTests(IntegrationTestFactory factory)
    : IClassFixture<IntegrationTestFactory>
{
    private readonly Fixture _fixture = new();
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Should_Create_Ledger()
    {
        var command = _fixture.Create<CreateLedgerCommand>();
        var response = await _client.PostAsJsonAsync("api/ledgers", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
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