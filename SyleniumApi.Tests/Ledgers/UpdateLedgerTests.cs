using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.Data.Entities;
using SyleniumApi.Features.Ledgers;

namespace SyleniumApi.Tests.Ledgers;

public class UpdateLedgerTests : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client;
    private const int LedgerId = 1;
    
    public UpdateLedgerTests(IntegrationTestFactory factory)
    {
        Fixture fixture = new();
        _client = factory.CreateClient();
        var context = factory.DbContext;

        var existingLedger = fixture.Build<Ledger>()
            .Without(l => l.FinancialAccounts)
            .With(l => l.CreatedDate, DateTime.UtcNow)
            .With(l => l.LedgerId, LedgerId)
            .Create();
        context.Add(existingLedger);
        context.SaveChanges();
    }
    
    [Fact]
    public async Task When_Ledger_Exists_Should_Update_Ledger()
    {
        var command = new UpdateLedgerCommand(LedgerId, "New Name");
        
        var response = await _client.PutAsJsonAsync($"api/ledgers/{LedgerId}", command);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task When_Update_Invalid_Should_Return_Bad_Request()
    {
        var command = new UpdateLedgerCommand(LedgerId, new string('a', 300));
        
        var response = await _client.PutAsJsonAsync($"api/ledgers/{LedgerId}", command);
        
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