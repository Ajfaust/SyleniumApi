using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.Features.FinancialAccountCategories;

namespace SyleniumApi.Tests.FinancialAccountCategories;

public class CreateFaCategoryTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Should_Create_FaCategory()
    {
        var command = _fixture.Create<CreateFaCategoryCommand>();
        var response = await _client.PostAsJsonAsync("api/facategories", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task When_Invalid_Should_Return_Bad_Request()
    {
        _fixture.Register(() => new string('a', 300));
        var command = _fixture.Create<CreateFaCategoryCommand>();

        var response = await _client.PostAsJsonAsync("api/facategories", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}