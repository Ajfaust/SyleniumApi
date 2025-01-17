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
        const string newName = "New Name";
        const FinancialCategoryType newType = FinancialCategoryType.Liability;

        var command = new UpdateFaCategoryCommand(DefaultTestValues.Id, DefaultTestValues.Id, newName, newType);

        var response = await _client.PutAsJsonAsync($"api/fa-categories/{DefaultTestValues.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedFaCategory = await _context.FinancialAccountCategories.FindAsync(DefaultTestValues.Id);

        updatedFaCategory.Should().NotBeNull();

        await _context.Entry(updatedFaCategory!).ReloadAsync(); // Reload the entity to get the updated values
        updatedFaCategory!.FinancialAccountCategoryName.Should().Be(newName);
        updatedFaCategory!.FinancialCategoryType.Should().Be(newType);
    }

    [Fact]
    public async Task When_Invalid_Should_Return_Bad_Request()
    {
        var invalidName = new string('a', 201);

        var command = new UpdateFaCategoryCommand(DefaultTestValues.Id, DefaultTestValues.Id, invalidName, FinancialCategoryType.Asset);

        var response = await _client.PutAsJsonAsync($"api/fa-categories/{DefaultTestValues.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}