using JournalService.Core.DTOs;
using JournalService.Core.Enums;
using JournalService.Core.Interfaces;
using JournalService.Core.Models;

namespace JournalService.Core.Services;

public class JournalEntryService
{
    private readonly IJournalEntryRepository _repository;

    public JournalEntryService(IJournalEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<JournalEntryResponse>> GetEntriesForJobAsync(
        Guid jobRequisitionId, string userId)
    {
        var entries = await _repository.GetByJobRequisitionIdAsync(jobRequisitionId, userId);
        return entries.Select(MapToResponse);
    }

    public async Task<JournalEntryResponse?> GetByIdAsync(Guid id, string userId)
    {
        var entry = await _repository.GetByIdAsync(id, userId);
        return entry is null ? null : MapToResponse(entry);
    }

    public async Task<JournalEntryResponse> CreateEntryAsync(
        Guid jobRequisitionId, string userId, CreateJournalEntryRequest request)
    {
        var entry = new JournalEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            JobRequisitionId = jobRequisitionId,
            InteractionType = request.InteractionType,
            Notes = request.Notes,
            EntryDate = new DateTimeOffset(request.EntryDate.Date, TimeSpan.Zero),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var created = await _repository.CreateAsync(entry);
        return MapToResponse(created);
    }

    public async Task<JournalEntryResponse?> UpdateEntryAsync(
        Guid id, string userId, UpdateJournalEntryRequest request)
    {
        var existing = await _repository.GetByIdAsync(id, userId);
        if (existing is null) return null;

        existing.InteractionType = request.InteractionType;
        existing.Notes = request.Notes;
        existing.EntryDate = new DateTimeOffset(request.EntryDate.Date, TimeSpan.Zero);
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        var updated = await _repository.UpdateAsync(existing);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteEntryAsync(Guid id, string userId)
    {
        return await _repository.DeleteAsync(id, userId);
    }

    private static string GetInteractionTypeDisplay(InteractionType type) => type switch
    {
        InteractionType.PhoneScreen => "Phone Screen",
        InteractionType.Email => "Email",
        InteractionType.Interview => "Interview",
        InteractionType.OfferDiscussion => "Offer Discussion",
        InteractionType.Rejection => "Rejection",
        InteractionType.FollowUp => "Follow-Up",
        InteractionType.ApplicationSubmitted => "Application Submitted",
        InteractionType.RecruiterOutreach => "Recruiter Outreach",
        InteractionType.Networking => "Networking",
        InteractionType.Note => "Note",
        _ => type.ToString()
    };

    private static JournalEntryResponse MapToResponse(JournalEntry entry) => new(
        entry.Id,
        entry.JobRequisitionId,
        entry.InteractionType,
        GetInteractionTypeDisplay(entry.InteractionType),
        entry.Notes,
        entry.EntryDate,
        entry.CreatedAt,
        entry.UpdatedAt
    );
}
