using System.Collections;
using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Transactions;

namespace SyleniumApi.Tests.Transactions;

public class UpdateTransactionTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Exists_And_Valid_Should_Update_Transaction()
    {
        // Arrange
        var transaction = _fixture.Build<Transaction>()
            .Without(x => x.Id)
            .With(x => x.FinancialAccountId, DefaultTestValues.Id)
            .With(x => x.TransactionCategoryId, DefaultTestValues.Id)
            .With(x => x.VendorId, DefaultTestValues.Id)
            .With(x => x.Date, DateTime.UtcNow)
            .Create();
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync(true);

        var transactionDto = _fixture.Build<TransactionDto>()
            .With(x => x.Id, transaction.Id)
            .With(x => x.AccountId, transaction.FinancialAccountId)
            .With(x => x.CategoryId, transaction.TransactionCategoryId)
            .With(x => x.VendorId, transaction.VendorId)
            .With(x => x.Date, DateTime.UtcNow.AddDays(-1))
            .Create();

        // Act
        var command = new UpdateTransactionCommand(transactionDto);
        var response = await _client.PutAsJsonAsync($"/api/transactions/{command.Dto.Id}", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedTransaction = await _context.Transactions.FindAsync(command.Dto.Id);
        updatedTransaction.Should().NotBeNull();
        await _context.Entry(updatedTransaction!).ReloadAsync();

        // Trim the dto date to microseconds to match SQL date precision
        var trimmedDate = transactionDto.Date.Trim(TimeSpan.TicksPerMicrosecond);

        updatedTransaction!.Date.Should().Be(trimmedDate);
        updatedTransaction.Description.Should().Be(transactionDto.Description);
        updatedTransaction.Inflow.Should().Be(transactionDto.Inflow);
        updatedTransaction.Outflow.Should().Be(transactionDto.Outflow);
        updatedTransaction.Cleared.Should().Be(transactionDto.Cleared);
    }

    [Theory]
    [ClassData(typeof(InvalidData))]
    public async Task When_Exists_And_Invalid_Command_Should_Return_BadRequest(DateTime date, string desc,
        int accountId, int categoryId, int vendorId)
    {
        // Arrange
        var transaction = _fixture.Build<Transaction>()
            .Without(x => x.Id)
            .With(x => x.FinancialAccountId, DefaultTestValues.Id)
            .With(x => x.TransactionCategoryId, DefaultTestValues.Id)
            .With(x => x.VendorId, DefaultTestValues.Id)
            .With(x => x.Date, DateTime.UtcNow)
            .Create();
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync(true);

        var dto = _fixture.Build<TransactionDto>()
            .With(x => x.Id, transaction.Id)
            .With(x => x.AccountId, accountId)
            .With(x => x.CategoryId, categoryId)
            .With(x => x.VendorId, vendorId)
            .With(x => x.Date, date)
            .With(x => x.Description, desc)
            .Create();
        var command = new UpdateTransactionCommand(dto);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/transactions/{transaction.Id}", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_Not_Exists_Should_Return_Not_Found()
    {
        // Arrange
        const int id = 100;
        var dto = _fixture.Build<TransactionDto>()
            .With(x => x.Id, id)
            .With(x => x.AccountId, DefaultTestValues.Id)
            .With(x => x.CategoryId, DefaultTestValues.Id)
            .With(x => x.VendorId, DefaultTestValues.Id)
            .Create();
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/transactions/{id}", new UpdateTransactionCommand(dto));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private class InvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var id = DefaultTestValues.Id;
            yield return [DateTime.UtcNow.AddDays(1), "Valid description", id, id, id];
            yield return [DateTime.UtcNow, new string('a', 600), id, id, id];
            yield return [DateTime.UtcNow, "Valid Description", 100, id, id];
            yield return [DateTime.UtcNow, "Valid Description", id, 100, id];
            yield return [DateTime.UtcNow, "Valid Description", id, id, 100];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}