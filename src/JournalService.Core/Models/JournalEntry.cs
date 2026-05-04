using JournalService.Core.Enums;

namespace JournalService.Core.Models;

public class JournalEntry
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid JobRequisitionId { get; set; }
    public InteractionType InteractionType { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset EntryDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
