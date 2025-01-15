using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Vendors;

namespace SyleniumApi.Tests.Vendors;

public class GetVendorTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _dbContext = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Vendor_Exists_Should_Return_Vendor()
    {
        // Arrange
        var id = DefaultTestValues.Id;

        // Act
        var response = await _client.GetAsync($"/api/vendors/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GetVendorResponse>();
        content.Should().NotBeNull();
        content!.Id.Should().Be(id);
    }
}