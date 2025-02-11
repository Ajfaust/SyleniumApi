using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.TransactionCategories;

namespace SyleniumApi.Tests.TransactionCategories;

public class CreateTransactionCategoryTests : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client;
    private readonly SyleniumDbContext _context;
    private readonly Fixture _fixture;

    public CreateTransactionCategoryTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.DbContext;
        _fixture = new Fixture { OmitAutoProperties = true };

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Theory]
    [InlineData(null)]
    [InlineData(DefaultTestValues.Id)]
    public async Task When_Valid_Should_Create_Transaction_Category(int? parentId)
    {
        var command = _fixture.Build<CreateTransactionCategoryCommand>()
            .With(c => c.LedgerId, DefaultTestValues.Id)
            .With(c => c.ParentId, parentId)
            .Create();

        var response = await _client.PostAsJsonAsync("/api/transaction-categories", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<CreateTransactionCategoryResponse>(content)?.Id;
        id.Should().NotBeNull();

        var category = await _context.TransactionCategories.FindAsync(id);
        category.Should().NotBeNull();
        category!.Id.Should().Be(id);
        category!.ParentCategoryId.Should().Be(parentId);
    }

    [Fact]
    public async Task When_Has_Subcategories_Should_Create_All_Categories()
    {
        // Arrange
        const int numSubCategories = 3;
        var subcategories = _fixture.Build<CreateTransactionCategoryCommand>()
            .With(c => c.LedgerId, DefaultTestValues.Id)
            .With(c => c.ParentId, (int?)null)
            .CreateMany(numSubCategories);

        var command = _fixture.Build<CreateTransactionCategoryCommand>()
            .With(c => c.LedgerId, DefaultTestValues.Id)
            .With(c => c.ParentId, (int?)null)
            .With(c => c.Subcategories, subcategories.ToList())
            .Create();

        // Act
        var response = await _client.PostAsJsonAsync("/api/transaction-categories", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<CreateTransactionCategoryResponse>(content)?.Id;
        id.Should().NotBeNull();

        var category = await _context.TransactionCategories.Include(t => t.SubCategories)
            .SingleOrDefaultAsync(t => t.Id == id);
        category.Should().NotBeNull();
        category!.Id.Should().Be(id);
        category!.SubCategories.Count.Should().Be(numSubCategories);
    }
}