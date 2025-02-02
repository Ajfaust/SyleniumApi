using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Ledgers.Get;

namespace SyleniumApi.Tests.Ledgers;

public class GetLedgerTransactionCategoriesTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Theory]
    [InlineData(3)]
    [InlineData(0)]
    public async Task When_Ledger_Exists_Should_Return_Categories(int numCategories)
    {
        // Arrange
        if (numCategories > 0)
        {
            var categories = _fixture.Build<TransactionCategory>()
                .Without(c => c.Id)
                .Without(c => c.ParentCategoryId)
                .With(c => c.Name, "Test")
                .With(c => c.LedgerId, DefaultTestValues.Id)
                .CreateMany(numCategories);
            await _context.TransactionCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        // Act
        var request = new GetLedgerTransactionCategoriesRequest(DefaultTestValues.Id);
        var response = await _client.GetAsync($"/api/ledgers/{DefaultTestValues.Id}/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var obtainedCategories = JsonConvert.DeserializeObject<GetLedgerTransactionCategoriesResponse>(content);

        obtainedCategories.Should().NotBeNull();
        obtainedCategories.Categories.Count.Should().Be(numCategories + 1);
    }

    [Fact]
    public async Task When_Ledger_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int ledgerId = 100;

        // Act
        var response = await _client.GetAsync($"/api/ledgers/{ledgerId}/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}