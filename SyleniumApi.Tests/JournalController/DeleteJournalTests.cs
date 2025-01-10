using System.Net;
using AutoFixture;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Tests.JournalController;

public class DeleteJournalTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly Fixture _fixture = new();
    private readonly SyleniumDbContext _dbContext = factory.DbContext;
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task When_Journal_Exists_Should_Delete_Journal()
    {
        var existingJournal = _fixture.Create<Journal>();
        await _dbContext.AddAsync(existingJournal);
        await _dbContext.SaveChangesAsync();

        var response = await _client.DeleteAsync($"api/journals/{existingJournal.JournalId}");
        
        response.EnsureSuccessStatusCode();
        
        response = await _client.GetAsync($"api/journals/{existingJournal.JournalId}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}