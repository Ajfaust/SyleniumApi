using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Models.Dtos;

namespace SyleniumApi.Interfaces;

public interface IJournalService
{
    Task<List<JournalDto>> GetJournals();
    Task<JournalDto?> GetJournal(int id);
    Task<JournalDto> CreateJournal(JournalDto journalDto);
    Task UpdateJournal(int id, JournalDto journalDto);
    Task DeleteJournal(int id);
}