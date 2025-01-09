using System.Net.Http.Json;
using AutoFixture;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Tests.JournalController;

public class UpdateJournalTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly Fixture _fixture = new();
    private readonly SyleniumContext _context = factory.Context;
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task When_Journal_Exists_Should_Update_Journal()
    {
        var existingJournal = _fixture.Create<Journal>();
        await _context.AddAsync(existingJournal);
        await _context.SaveChangesAsync();

        const string expectedName = "Updated Journal Name";
        var journalDto = _fixture.Create<JournalDto>();
        journalDto.JournalId = existingJournal.JournalId;
        journalDto.JournalName = expectedName;
        var response = await _client.PutAsJsonAsync($"api/journals/{existingJournal.JournalId}", journalDto);
        
        response.EnsureSuccessStatusCode();
        
        var updatedJournal = await _client.GetFromJsonAsync<JournalDto>($"api/journals/{existingJournal.JournalId}");
        
        Assert.NotNull(updatedJournal);
        Assert.Equal(expectedName, updatedJournal.JournalName);
    }
}