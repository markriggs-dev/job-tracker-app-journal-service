using JournalService.Core.Interfaces;
using JournalService.Core.Models;
using JournalService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JournalService.Infrastructure.Repositories;

public class JournalEntryRepository : IJournalEntryRepository
{
    private readonly JournalServiceDbContext _context;

    public JournalEntryRepository(JournalServiceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JournalEntry>> GetByJobRequisitionIdAsync(
        Guid jobRequisitionId, string userId)
    {
        return await _context.JournalEntries
            .Where(e => e.JobRequisitionId == jobRequisitionId && e.UserId == userId)
            .OrderByDescending(e => e.EntryDate)
            .ThenByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<JournalEntry?> GetByIdAsync(Guid id, string userId)
    {
        return await _context.JournalEntries
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
    }

    public async Task<JournalEntry> CreateAsync(JournalEntry entry)
    {
        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task<JournalEntry?> UpdateAsync(JournalEntry entry)
    {
        _context.JournalEntries.Update(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var existing = await _context.JournalEntries
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

        if (existing is null) return false;

        _context.JournalEntries.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
