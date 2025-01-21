using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccountCategories;

namespace SyleniumApi.Tests.FinancialAccountCategories;

public class CreateFaCategoryTests(IntegrationTestFactory factory)
    : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Should_Create_FaCategory()
    {
        var command = _fixture.Build<CreateFaCategoryCommand>()
            .With(fac => fac.LedgerId, DefaultTestValues.Id)
            .Create();

        var response = await _client.PostAsJsonAsync("api/fa-categories", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get the id from the response
        var content = await response.Content.ReadAsStringAsync();
        var faCategoryId = JsonConvert.DeserializeObject<GetFaCategoryResponse>(content)?.Id;
        faCategoryId.Should().NotBeNull();

        var faCategory = await _context.FinancialAccountCategories.FindAsync(faCategoryId);
        faCategory.Should().NotBeNull();
        faCategory!.Id.Should().Be(faCategoryId);
    }

    [Fact]
    public async Task When_Invalid_Should_Return_Bad_Request()
    {
        _fixture.Register(() => new string('a', 300));
        var command = _fixture.Create<CreateFaCategoryCommand>();

        var response = await _client.PostAsJsonAsync("api/fa-categories", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}