using System.Net;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.FinancialAccountCategories;

public class DeleteFaCategoriesTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly Fixture _fixture = new();
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    
    [Fact]
    public async Task When_Exists_Should_Delete_FaCategory()
    {
        const int id = 1;
        var existingFaCategory = _fixture.MakeFaCategory(id);
        await _context.AddAsync(existingFaCategory);
        await _context.SaveChangesAsync();
        
        var response = await _client.DeleteAsync($"api/facategories/{id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var faCategoryResponse = await _client.GetAsync($"api/facategories/{id}");
        
        faCategoryResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}