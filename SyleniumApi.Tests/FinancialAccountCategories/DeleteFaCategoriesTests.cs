using System.Net;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.FinancialAccountCategories;

public class DeleteFaCategoriesTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Exists_Should_Delete_FaCategory()
    {
        var response = await _client.DeleteAsync($"api/fa-categories/{DefaultTestValues.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var faCategory = await _context.FinancialAccountCategories.FindAsync(DefaultTestValues.Id);
        faCategory.Should().BeNull();
    }
}