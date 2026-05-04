using JournalService.Core.DTOs;
using JournalService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JournalService.Api.Controllers;

[ApiController]
[Route("api/jobs/{jobRequisitionId:guid}/journal")]
[Authorize]
public class JournalEntriesController : ControllerBase
{
    private readonly JournalEntryService _service;
    private readonly ILogger<JournalEntriesController> _logger;

    public JournalEntriesController(
        JournalEntryService service,
        ILogger<JournalEntriesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("User ID not found in token");
    }

    // GET /api/jobs/{jobRequisitionId}/journal
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JournalEntryResponse>>> GetAll(
        Guid jobRequisitionId)
    {
        var userId = GetUserId();
        var results = await _service.GetEntriesForJobAsync(jobRequisitionId, userId);
        return Ok(results);
    }

    // GET /api/jobs/{jobRequisitionId}/journal/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JournalEntryResponse>> GetById(
        Guid jobRequisitionId,
        Guid id)
    {
        var userId = GetUserId();
        var result = await _service.GetByIdAsync(id, userId);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    // POST /api/jobs/{jobRequisitionId}/journal
    [HttpPost]
    public async Task<ActionResult<JournalEntryResponse>> Create(
        Guid jobRequisitionId,
        [FromBody] CreateJournalEntryRequest request)
    {
        var userId = GetUserId();
        var result = await _service.CreateEntryAsync(jobRequisitionId, userId, request);

        return CreatedAtAction(
            nameof(GetById),
            new { jobRequisitionId, id = result.Id },
            result);
    }

    // PUT /api/jobs/{jobRequisitionId}/journal/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<JournalEntryResponse>> Update(
        Guid jobRequisitionId,
        Guid id,
        [FromBody] UpdateJournalEntryRequest request)
    {
        var userId = GetUserId();
        var result = await _service.UpdateEntryAsync(id, userId, request);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    // DELETE /api/jobs/{jobRequisitionId}/journal/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid jobRequisitionId,
        Guid id)
    {
        var userId = GetUserId();
        var deleted = await _service.DeleteEntryAsync(id, userId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
