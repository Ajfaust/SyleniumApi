using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.TransactionCategories;

namespace SyleniumApi.Tests.TransactionCategories;

public class GetTransactionCategoryTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;

    [Fact]
    public async Task When_Exists_Should_Return_Transaction_Category()
    {
        // Arrange
        const int id = DefaultTestValues.Id;
        var request = new GetTransactionCategoryRequest(id);

        // Act
        var response = await _client.GetAsync($"/api/transaction-categories/{id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var resultId = JsonConvert.DeserializeObject<GetTransactionCategoryResponse>(content)?.Id;
        resultId.Should().NotBeNull();
        resultId.Should().Be(id);
    }
    
    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100500;
        var request = new GetTransactionCategoryRequest(id);
        
        // Act
        var response = await _client.GetAsync($"/api/transaction-categories/{id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}