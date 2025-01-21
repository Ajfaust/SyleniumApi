using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccounts;

namespace SyleniumApi.Tests.FinancialAccounts;

public class CreateFinancialAccountTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly SyleniumDbContext _context = factory.DbContext;
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Should_Create_FinancialAccount()
    {
        var command = _fixture.Build<CreateFinancialAccountCommand>()
            .With(fa => fa.LedgerId, DefaultTestValues.Id)
            .With(fa => fa.FaCategoryId, DefaultTestValues.Id)
            .Create();
        var postResponse = await _client.PostAsJsonAsync("api/financial-accounts", command);

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await postResponse.Content.ReadAsStringAsync();
        var id = JsonConvert.DeserializeObject<CreateFinancialAccountResponse>(content)?.Id;
        id.Should().NotBeNull();

        var financialAccount = await _context.FinancialAccounts.FindAsync(id);

        financialAccount.Should().NotBeNull();
        financialAccount!.Id.Should().Be(id);
    }

    [Fact]
    public async Task When_Command_Invalid_Should_Return_BadRequest()
    {
        var command = _fixture.Build<CreateFinancialAccountCommand>()
            .With(fa => fa.LedgerId, DefaultTestValues.Id)
            .With(fa => fa.FaCategoryId, DefaultTestValues.Id)
            .With(fa => fa.Name, new string('a', 201))
            .Create();

        var postResponse = await _client.PostAsJsonAsync("api/financial-accounts", command);

        postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}