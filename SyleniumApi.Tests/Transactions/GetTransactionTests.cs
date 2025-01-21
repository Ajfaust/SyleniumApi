using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Transactions;

namespace SyleniumApi.Tests.Transactions;

public class GetTransactionTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Exists_Should_Return_Transaction()
    {
        // Arrange
        var transaction = _fixture.Build<Transaction>()
            .With(t => t.FinancialAccountId, DefaultTestValues.Id)
            .With(t => t.TransactionCategoryId, DefaultTestValues.Id)
            .With(t => t.VendorId, DefaultTestValues.Id)
            .Create();
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(true);

        // Act
        var response = await _client.GetAsync($"/api/transactions/{transaction.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<GetTransactionResponse>(content)?.Dto.Id;
        id.Should().NotBeNull();
        id!.Should().Be(transaction.Id);
    }

    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100;

        // Act
        var response = await _client.GetAsync($"/api/Transactions/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}