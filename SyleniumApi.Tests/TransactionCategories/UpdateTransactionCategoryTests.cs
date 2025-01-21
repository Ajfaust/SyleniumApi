using System.Collections;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.TransactionCategories;

namespace SyleniumApi.Tests.TransactionCategories;

public class UpdateTransactionCategoryTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;

    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    public async Task When_Exists_And_Valid_Should_Update_TransactionCategory(int? parentId)
    {
        // Arrange
        const string name = "Updated Name";
        var newCategory = new TransactionCategory
        {
            LedgerId = DefaultTestValues.Id,
            ParentCategoryId = null,
            Name = "Test Subcategory"
        };
        _context.TransactionCategories.Add(newCategory);
        await _context.SaveChangesAsync(true);

        var id = newCategory.Id;
        var command = new UpdateTransactionCategoryCommand(id, DefaultTestValues.Id, parentId, name);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/transaction-categories/{id}", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var update = await _context.TransactionCategories.FindAsync(id);
        update.Should().NotBeNull();

        await _context.Entry(update!).ReloadAsync();
        update!.Name.Should().Be(name);
        update.ParentCategoryId.Should().Be(parentId);
    }

    [Theory]
    [ClassData(typeof(InvalidData))]
    public async Task When_Exists_And_Invalid_Should_Return_Error_Response(int? parentId, string name)
    {
        // Arrange
        var newCategory = new TransactionCategory
        {
            LedgerId = DefaultTestValues.Id,
            ParentCategoryId = null,
            Name = "Test Subcategory"
        };
        _context.TransactionCategories.Add(newCategory);
        await _context.SaveChangesAsync(true);
        var command =
            new UpdateTransactionCategoryCommand(newCategory.Id, DefaultTestValues.Id, parentId,
                name);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/transaction-categories/{newCategory.Id}",
            command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/api/transaction-categories/100");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private class InvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [DefaultTestValues.Id, new string('a', 300)];
            yield return [100, "New Name"];
            yield return [null!, string.Empty];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}