using System.Net;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Tests.Transactions;

public class DeleteTransactionTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Exists_Should_Delete_Transaction()
    {
        // Arrange
        var transaction = _fixture.Build<Transaction>()
            .Without(x => x.TransactionId)
            .With(x => x.FinancialAccountId, DefaultTestValues.Id)
            .With(x => x.TransactionCategoryId, DefaultTestValues.Id)
            .With(x => x.VendorId, DefaultTestValues.Id)
            .Create();
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync(true);

        // Act
        var response = await _client.DeleteAsync($"/api/transactions/{transaction.TransactionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var t = _context.Transactions.Any(t => t.TransactionId == transaction.TransactionId);
        t.Should().BeFalse();
    }
    
    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100;
        
        // Act
        var response = await _client.DeleteAsync($"/api/transactions/{id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}