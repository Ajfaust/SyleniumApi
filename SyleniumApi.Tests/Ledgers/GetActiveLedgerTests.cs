using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers.Get;

namespace SyleniumApi.Tests.Ledgers;

public class GetActiveLedgerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Active_Ledger_Should_Return_Active_Ledger()
    {
        // Arrange
        var activeLedger = _fixture.Build<Ledger>()
            .Without(l => l.Id)
            .With(l => l.IsActive, true)
            .With(l => l.Name, "Test")
            .Create();
        await _context.AddAsync(activeLedger);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/ledgers/active");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var active = JsonConvert.DeserializeObject<GetActiveLedgerIdResponse>(content);

        active.Should().NotBeNull();
        active.Id.Should().Be(activeLedger.Id);
    }

    [Fact]
    public async Task When_Active_Ledger_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange

        //Act
        var response = await _client.GetAsync("/ledgers/active");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}