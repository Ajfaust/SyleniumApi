using System.Net;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccounts;

namespace SyleniumApi.Tests.FinancialAccounts;

public class GetFinancialAccountTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Exists_Should_Return_FinancialAccount()
    {
        var response = await _client.GetAsync($"api/financial-accounts/{DefaultTestValues.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var financialAccountResponse = JsonConvert.DeserializeObject<GetFinancialAccountResponse>(content);

        financialAccountResponse.Should().NotBeNull();
        financialAccountResponse!.Id.Should().Be(DefaultTestValues.Id);
    }

    [Fact]
    public async Task When_NotExists_Should_Return_NotFound()
    {
        var response = await _client.GetAsync("api/financial-accounts/2");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}