using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers;

namespace SyleniumApi.Tests.Ledgers;

public class UpdateActiveLedgerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Ledger_Exists_Should_Set_Active()
    {
        // Arrange
        var activeLedger = _fixture.Build<Ledger>()
            .Without(l => l.Id)
            .With(l => l.Name, "Name")
            .With(l => l.IsActive, true)
            .Create();

        _context.Add(activeLedger);
        await _context.SaveChangesAsync();

        var ledger = _fixture.Build<Ledger>()
            .Without(l => l.Id)
            .With(l => l.Name, "Name2")
            .Create();

        _context.Add(ledger);
        await _context.SaveChangesAsync();

        var command = new UpdateActiveLedgerCommand(ledger.Id);

        // Act
        var response = await _client.PutAsJsonAsync("/api/ledgers/active", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var newActiveLedger = await _context.Ledgers.AsNoTracking().SingleOrDefaultAsync(l => l.IsActive);
        newActiveLedger.Should().NotBeNull();
        newActiveLedger.Id.Should().Be(ledger.Id);

        var oldActiveLedger = await _context.Ledgers.AsNoTracking().SingleOrDefaultAsync(l => l.Id == activeLedger.Id);
        oldActiveLedger.Should().NotBeNull();
        oldActiveLedger.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task When_Ledger_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100;
        var command = new UpdateActiveLedgerCommand(id);
        
        // Act
        var response = await _client.PutAsJsonAsync("/api/ledgers/active", command);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}