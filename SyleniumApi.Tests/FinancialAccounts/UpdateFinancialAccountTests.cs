using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccounts;

namespace SyleniumApi.Tests.FinancialAccounts;

public class UpdateFinancialAccountTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Exists_Should_Update_FinancialAccount()
    {
        var command = _fixture.Build<UpdateFinancialAccountCommand>()
            .With(fa => fa.Id, DefaultTestValues.Id)
            .With(fa => fa.LedgerId, DefaultTestValues.Id)
            .Create();

        var response = await _client.PutAsJsonAsync($"api/financial-accounts/{DefaultTestValues.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var financialAccount = await _context.FinancialAccounts.FindAsync(DefaultTestValues.Id);
        financialAccount.Should().NotBeNull();

        await _context.Entry(financialAccount!).ReloadAsync();
        financialAccount!.FinancialAccountName.Should().Be(command.Name);
    }

    [Fact]
    public async Task When_NotExists_Should_Return_NotFound()
    {
        var command = _fixture.Build<UpdateFinancialAccountCommand>()
            .With(fa => fa.Id, 2)
            .With(fa => fa.LedgerId, DefaultTestValues.Id)
            .Create();

        var response = await _client.PutAsJsonAsync("api/financial-accounts/2", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task When_Command_Invalid_Should_Return_BadRequest()
    {
        var command = _fixture.Build<UpdateFinancialAccountCommand>()
            .With(fa => fa.Id, DefaultTestValues.Id)
            .With(fa => fa.LedgerId, DefaultTestValues.Id)
            .With(fa => fa.Name, new string('a', 201))
            .Create();

        var response = await _client.PutAsJsonAsync($"api/financial-accounts/{DefaultTestValues.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}