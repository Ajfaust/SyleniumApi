using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccountCategories;

namespace SyleniumApi.Tests.FinancialAccountCategories;

public class UpdateFaCategoryTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Exists_Should_Update_FaCategory()
    {
        const int id = 1;
        const string newName = "New Name";
        const FinancialCategoryType newType = FinancialCategoryType.Liability;

        var existingFaCategory = _fixture.MakeFaCategory();
        await _context.AddAsync(existingFaCategory);
        await _context.SaveChangesAsync();

        var command = new UpdateFaCategoryCommand(id, newName, newType);

        var response = await _client.PutAsJsonAsync($"api/facategories/{id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedFaCategory = await _context.FinancialAccountCategories.FindAsync(id);

        updatedFaCategory.Should().NotBeNull();

        await _context.Entry(updatedFaCategory!).ReloadAsync(); // Reload the entity to get the updated values
        updatedFaCategory!.FinancialAccountCategoryName.Should().Be(newName);
        updatedFaCategory!.FinancialCategoryType.Should().Be(newType);
    }
    
    [Fact]
    public async Task When_Invalid_Should_Return_Bad_Request()
    {
        const int id = 1;
        var invalidName = new string('a', 201);

        var existingFaCategory = _fixture.MakeFaCategory();
        await _context.AddAsync(existingFaCategory);
        await _context.SaveChangesAsync();

        var command = new UpdateFaCategoryCommand(id, invalidName, FinancialCategoryType.Asset);

        var response = await _client.PutAsJsonAsync($"api/facategories/{id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}