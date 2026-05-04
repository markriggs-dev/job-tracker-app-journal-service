using JournalService.Core.Enums;

namespace JournalService.Core.DTOs;

public record CreateJournalEntryRequest(
    InteractionType InteractionType,
    string? Notes,
    DateTimeOffset EntryDate
);

public record UpdateJournalEntryRequest(
    InteractionType InteractionType,
    string? Notes,
    DateTimeOffset EntryDate
);

public record JournalEntryResponse(
    Guid Id,
    Guid JobRequisitionId,
    InteractionType InteractionType,
    string InteractionTypeDisplay,
    string? Notes,
    DateTimeOffset EntryDate,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
