using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Vendors;

namespace SyleniumApi.Tests.Vendors;

public class CreateVendorTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _dbContext = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Data_Valid_Should_Create_Vendor()
    {
        // Arrange
        var command = _fixture.Build<CreateVendorCommand>()
            .With(vc => vc.LedgerId, DefaultTestValues.Id)
            .Create();

        // Act
        var response = await _client.PostAsJsonAsync("/api/vendors", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<CreateVendorResponse>();
        var vendor = await _dbContext.Vendors.FindAsync(content?.Id);
        vendor.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0, "")]
    [InlineData(1, "")]
    [InlineData(0, "Vendor Name")]
    public async Task When_Data_Invalid_Should_Return_Bad_Request(int ledgerId, string name)
    {
        // Arrange
        var command = new CreateVendorCommand(ledgerId, name);

        // Act
        var response = await _client.PostAsJsonAsync("/api/vendors", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}