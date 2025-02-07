using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
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
    public async Task When_Ledger_Exists_Should_Update_Active_Ledger()
    {
        // Arrange
        var activeLedger = _fixture.Build<Ledger>()
            .Without(l => l.Id)
            .With(l => l.IsActive, true)
            .With(l => l.Name, "Test")
            .Create();
        await _context.Ledgers.AddAsync(activeLedger);
        await _context.SaveChangesAsync();

        var cmd = new UpdateActiveLedgerCommand(DefaultTestValues.Id);

        // Act
        var response = await _client.PutAsJsonAsync("/api/ledgers/active", cmd);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var active = _context.Ledgers.SingleOrDefault(l => l.IsActive);
        active.Should().NotBeNull();
        active.Id.Should().Be(DefaultTestValues.Id);
    }

    [Fact]
    public async Task When_Ledger_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100;
        var cmd = new UpdateActiveLedgerCommand(id);

        // Act
        var response = await _client.PutAsJsonAsync("/api/ledgers/active", cmd);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}