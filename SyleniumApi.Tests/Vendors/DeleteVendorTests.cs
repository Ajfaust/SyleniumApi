using System.Net;
using FluentAssertions;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.Vendors;

public class DeleteVendorTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;

    [Fact]
    public async Task When_Vendor_Exists_Should_Delete_Vendor()
    {
        // Arrange
        // Act
        var response = await _client.DeleteAsync($"/api/vendors/{DefaultTestValues.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var vendor = await _context.Vendors.FindAsync(DefaultTestValues.Id);
        vendor.Should().BeNull();
    }

    [Fact]
    public async Task When_Vendor_Does_Not_Exist_Should_Return_Not_Found()
    {
        // Arrange
        // Act
        var response = await _client.DeleteAsync("/api/vendors/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}