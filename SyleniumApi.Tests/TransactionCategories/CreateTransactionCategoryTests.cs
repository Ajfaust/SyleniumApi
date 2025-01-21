using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.TransactionCategories;

namespace SyleniumApi.Tests.TransactionCategories;

public class CreateTransactionCategoryTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

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
}