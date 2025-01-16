using System.Net;
using FluentAssertions;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.TransactionCategories;

public class DeleteTransactionCategoryTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;

    [Fact]
    public async Task When_Exists_Should_Delete_Transaction_Category()
    {
        // Arrange
        const int id = DefaultTestValues.Id;

        // Act
        var response = await _client.DeleteAsync($"api/transaction-categories/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var tc = await _context.TransactionCategories.FindAsync(id);
        tc.Should().BeNull();
    }

    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 10;

        // Act
        var response = await _client.DeleteAsync($"api/transaction-categories/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}