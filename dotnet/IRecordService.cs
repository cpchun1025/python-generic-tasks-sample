using MyApi.Models;

namespace MyApi.Services;

public interface IRecordService
{
    Task<IEnumerable<Record>> GetRecordsAsync();
    Task<Record?> GetRecordByIdAsync(int id);
    Task<Record> AddRecordAsync(Record record);
    Task<Record?> UpdateRecordAsync(int id, Record record);
    Task<bool> DeleteRecordAsync(int id);
}