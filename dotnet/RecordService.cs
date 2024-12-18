using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Services;

public class RecordService : IRecordService
{
    private readonly MyDbContext _context;

    public RecordService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Record>> GetRecordsAsync()
    {
        return await _context.Records.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<Record?> GetRecordByIdAsync(int id)
    {
        return await _context.Records.FindAsync(id);
    }

    public async Task<Record> AddRecordAsync(Record record)
    {
        record.CreatedAt = DateTime.UtcNow;
        _context.Records.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<Record?> UpdateRecordAsync(int id, Record record)
    {
        var existingRecord = await _context.Records.FindAsync(id);
        if (existingRecord == null)
        {
            return null;
        }

        existingRecord.Name = record.Name;
        existingRecord.Description = record.Description;

        _context.Entry(existingRecord).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return existingRecord;
    }

    public async Task<bool> DeleteRecordAsync(int id)
    {
        var record = await _context.Records.FindAsync(id);
        if (record == null)
        {
            return false;
        }

        _context.Records.Remove(record);
        await _context.SaveChangesAsync();

        return true;
    }
}