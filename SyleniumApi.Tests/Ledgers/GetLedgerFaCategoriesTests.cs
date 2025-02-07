using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers.Get;

namespace SyleniumApi.Tests.Ledgers;

public class GetLedgerFaCategoriesTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Exists_Should_Return_All_Categories()
    {
        // Arrange
        const int count = 3;
        var categories = _fixture.Build<FinancialAccountCategory>()
            .With(fa => fa.LedgerId, DefaultTestValues.Id)
            .Without(fa => fa.Id)
            .With(fa => fa.Name, "Test")
            .With(fa => fa.Type, FinancialCategoryType.Asset)
            .CreateMany(3);
        await _context.AddRangeAsync(categories);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/ledgers/{DefaultTestValues.Id}/fa-categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetLedgerFaCategoriesResponse>(content);
        result.Should().NotBeNull();
        result.Categories.Should().HaveCount(count + 1);
    }

    [Fact]
    public async Task When_No_Categories_Should_Return_Empty_List()
    {
        // Arrange
        var categories = _context.FinancialAccountCategories.ToList();
        _context.RemoveRange(categories);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/ledgers/{DefaultTestValues.Id}/fa-categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetLedgerFaCategoriesResponse>(content);
        result.Should().NotBeNull();
        result.Categories.Should().BeEmpty();
    }

    [Fact]
    public async Task When_Ledger_Not_Exist_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100;

        // Act
        var response = await _client.GetAsync($"/api/ledgers/{id}/fa-categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}