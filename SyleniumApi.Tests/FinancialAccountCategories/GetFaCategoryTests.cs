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
        var response = await _client.GetAsync($"api/fa-categories/{DefaultTestValues.Id}");

        var content = await response.Content.ReadAsStringAsync();
        var faCategory = JsonConvert.DeserializeObject<GetFaCategoryResponse>(content);

        faCategory.Should().NotBeNull();
        faCategory!.Id.Should().Be(DefaultTestValues.Id);
    }

    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        var response = await _client.GetAsync("api/fa-categories/2");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}