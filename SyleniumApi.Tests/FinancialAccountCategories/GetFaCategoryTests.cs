using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccountCategories;

namespace SyleniumApi.Tests.FinancialAccountCategories;

public class GetFaCategoryTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Exists_Should_Get_FaCategory()
    {
        const int id = 1;
        var existingFaCategory = _fixture.MakeFaCategory();

        await _context.AddAsync(existingFaCategory);
        await _context.SaveChangesAsync();

        var response = await _client.GetAsync($"api/facategories/{id}");

        var content = await response.Content.ReadAsStringAsync();
        var faCategory = JsonConvert.DeserializeObject<GetFaCategoryResponse>(content);

        faCategory.Should().NotBeNull();
        faCategory!.Id.Should().Be(id);
    }

    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        var response = await _client.GetAsync("api/facategories/1");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}