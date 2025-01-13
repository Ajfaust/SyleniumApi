using System.Net;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.Ledgers;

public class GetLedgerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly Fixture _fixture = new();
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;

    [Fact]
    public async Task When_Exists_Should_Get_Ledger()
    {
        var existingLedger = _fixture.Build<Ledger>()
            .Without(l => l.FinancialAccounts)
            .With(l => l.CreatedDate, DateTime.UtcNow)
            .Create();
        await _context.AddAsync(existingLedger);
        await _context.SaveChangesAsync();
        
        var response = await _client.GetAsync($"api/ledgers/{existingLedger.LedgerId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        var response = await _client.GetAsync("api/ledgers/1");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}