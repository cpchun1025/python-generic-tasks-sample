using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecordsController : ControllerBase
{
    private readonly IRecordService _recordService;

    public RecordsController(IRecordService recordService)
    {
        _recordService = recordService;
    }

    // GET: api/Records
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Record>>> GetRecords()
    {
        var records = await _recordService.GetRecordsAsync();
        return Ok(records);
    }

    // GET: api/Records/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Record>> GetRecord(int id)
    {
        var record = await _recordService.GetRecordByIdAsync(id);

        if (record == null)
        {
            return NotFound();
        }

        return Ok(record);
    }

    // POST: api/Records
    [HttpPost]
    public async Task<ActionResult<Record>> PostRecord(Record record)
    {
        var createdRecord = await _recordService.AddRecordAsync(record);
        return CreatedAtAction(nameof(GetRecord), new { id = createdRecord.Id }, createdRecord);
    }

    // PUT: api/Records/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRecord(int id, Record record)
    {
        var updatedRecord = await _recordService.UpdateRecordAsync(id, record);

        if (updatedRecord == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Records/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecord(int id)
    {
        var success = await _recordService.DeleteRecordAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}