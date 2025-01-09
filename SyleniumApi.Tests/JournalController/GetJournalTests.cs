using System.Net.Http.Json;
using AutoFixture;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Tests.JournalController;

public class GetJournalTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private readonly Fixture _fixture = new();
    private readonly SyleniumContext _context = factory.Context;
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task When_Journals_Exist_Should_Return_All_Journals()
    {
        const int numJournals = 3;
        var existingJournals = _fixture.CreateMany<Journal>(numJournals);
        await _context.AddRangeAsync(existingJournals);
        await _context.SaveChangesAsync();
        
        var journalDtos = await _client.GetFromJsonAsync<IEnumerable<JournalDto>>("api/journals");

        Assert.NotNull(journalDtos);
        Assert.Equal(numJournals, journalDtos.Count());
    }

    [Fact]
    public async Task When_Journal_Exists_Should_Return_Journal()
    {
        var existingJournal = _fixture.Create<Journal>();
        await _context.AddAsync(existingJournal);
        await _context.SaveChangesAsync();

        var journalDto = await _client.GetFromJsonAsync<JournalDto>($"api/journals/{existingJournal.JournalId}");

        Assert.NotNull(journalDto);
        Assert.Equal(existingJournal.JournalId, journalDto.JournalId);
    }
}