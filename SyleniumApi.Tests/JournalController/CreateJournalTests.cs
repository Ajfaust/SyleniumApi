using System.Net.Http.Json;
using AutoFixture;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;

namespace SyleniumApi.Tests.JournalController;

public class CreateJournalTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly Fixture _fixture = new();
    private readonly SyleniumDbContext _dbContext = factory.DbContext;
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task When_No_Journal_Should_Create_Journal()
    {
        var journalDto = _fixture.Create<JournalDto>();
        var response = await _client.PostAsJsonAsync("api/journals", journalDto);

        response.EnsureSuccessStatusCode();
    }
}