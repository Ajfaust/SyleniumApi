using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Vendors;

namespace SyleniumApi.Tests.Vendors;

public class UpdateVendorTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Data_Is_Valid_Should_Update_Vendor()
    {
        // Arrange
        const string newName = "New Vendor Name";
        var command = new UpdateVendorCommand(DefaultTestValues.Id, newName);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/vendors/{DefaultTestValues.Id}", command);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vendor = await _context.Vendors.FindAsync(DefaultTestValues.Id);
        vendor.Should().NotBeNull();

        await _context.Entry(vendor!).ReloadAsync();
        vendor!.VendorName.Should().Be(newName);
    }

    [Fact]
    public async Task When_Data_Is_Invalid_Should_Return_Bad_Request()
    {
        // Arrange
        var name = new string('a', 230);
        var command = new UpdateVendorCommand(DefaultTestValues.Id, name);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/vendors/{DefaultTestValues.Id}", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_Vendor_Does_Not_Exist_Should_Return_Not_Found()
    {
        // Arrange
        var command = new UpdateVendorCommand(99, "New Vendor Name");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/vendors/{command.Id}", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}