using System.Data;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Interfaces;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Services;

public sealed class JournalService(SyleniumDbContext dbContext) : IJournalService
{
    public async Task<List<JournalDto>> GetJournals()
    {
        return await dbContext
            .Journals
            .Select(j => new JournalDto
            {
                JournalId = j.JournalId,
                JournalName = j.JournalName,
            })
            .ToListAsync();
    }
    
    public async Task<JournalDto?> GetJournal(int id)
    {
        var journal = await dbContext
            .Journals
            .FirstOrDefaultAsync(j => j.JournalId == id);

        if (journal == null)
        {
            return null;
        }

        return new JournalDto()
        {
            JournalId = journal.JournalId,
            JournalName = journal.JournalName,
        };
    }

    public async Task<JournalDto> CreateJournal(JournalDto journalDto)
    {
        var journal = new Journal()
        {
            JournalName = journalDto.JournalName,
        };
        
        dbContext.Journals.Add(journal);
        await dbContext.SaveChangesAsync();

        return new JournalDto()
        {
            JournalId = journal.JournalId,
            JournalName = journal.JournalName,
        };
    }
    
    public async Task UpdateJournal(int id, JournalDto journalDto)
    {
        var journal = await dbContext.Journals.FindAsync(id);

        if (journal == null)
        {
            throw new DataException("Journal not found");
        }

        journal.JournalName = journalDto.JournalName;
        dbContext.Entry(journal).State = EntityState.Modified;
        
        await dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteJournal(int id)
    {
        var journal = await dbContext.Journals.FindAsync(id);

        if (journal != null)
        {
            dbContext.Journals.Remove(journal);
            await dbContext.SaveChangesAsync();
        }
    }
}