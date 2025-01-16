using System.Collections;
using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Transactions;

namespace SyleniumApi.Tests.Transactions;

public class CreateTransactionsTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new() { OmitAutoProperties = true };

    [Fact]
    public async Task When_Valid_Should_Create_Transaction()
    {
        // Arrange
        var command = _fixture.Build<CreateTransactionCommand>()
            .With(x => x.AccountId, DefaultTestValues.Id)
            .With(x => x.TransactionCategoryId, DefaultTestValues.Id)
            .With(x => x.VendorId, DefaultTestValues.Id)
            .With(x => x.Date, DateTime.UtcNow)
            .Create();

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactions", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<CreateTransactionResponse>(content)?.Id;

        var nt = await _context.Transactions.FindAsync(id);
        nt.Should().NotBeNull();
    }

    [Theory]
    [ClassData(typeof(InvalidData))]
    public async Task When_Invalid_Should_Return_Bad_Request(DateTime date, string description)
    {
        // Arrange
        var command = _fixture.Build<CreateTransactionCommand>()
            .With(x => x.AccountId, DefaultTestValues.Id)
            .With(x => x.TransactionCategoryId, DefaultTestValues.Id)
            .With(x => x.VendorId, DefaultTestValues.Id)
            .With(x => x.Date, date)
            .With(x => x.Description, description)
            .Create();

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactions", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private class InvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [DateTime.UtcNow, new string('a', 700)];
            yield return [new DateTime(2026, 1, 1, 1, 1, 1), "Valid description string"];
            yield return [new DateTime(2026, 1, 1, 1, 1, 1), new string('a', 700)];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}