using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers.Get;

namespace SyleniumApi.Tests.Ledgers;

public class GetLedgerVendorsTest(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Ledger_Exists_Should_Return_All_Vendors()
    {
        // Arrange
        const int numVendors = 3;
        var vendors = _fixture.Build<Vendor>()
            .Without(v => v.Id)
            .With(v => v.Name, "Test")
            .With(v => v.LedgerId, DefaultTestValues.Id)
            .CreateMany(numVendors);
        await _context.AddRangeAsync(vendors);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/ledgers/{DefaultTestValues.Id}/vendors");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var getLedgerVendorsResponse = JsonConvert.DeserializeObject<GetLedgerVendorsResponse>(content);

        getLedgerVendorsResponse.Should().NotBeNull();
        getLedgerVendorsResponse.Vendors.Count.Should().Be(numVendors + 1);
    }

    [Fact]
    public async Task When_Ledger_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100;

        // Act
        var response = await _client.GetAsync($"/api/ledgers/{id}/vendors");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}