using JournalService.Core.Models;

namespace JournalService.Core.Interfaces;

public interface IJournalEntryRepository
{
    Task<IEnumerable<JournalEntry>> GetByJobRequisitionIdAsync(Guid jobRequisitionId, string userId);
    Task<JournalEntry?> GetByIdAsync(Guid id, string userId);
    Task<JournalEntry> CreateAsync(JournalEntry entry);
    Task<JournalEntry?> UpdateAsync(JournalEntry entry);
    Task<bool> DeleteAsync(Guid id, string userId);
}
