using Models;
using Models.Pagination;

namespace Services.Interfaces
{
    public interface ITraineeService
    {
        Task<Trainee?> InsertAsync(Trainee data);
        Task<Trainee?> UpdateAsync(Trainee data);
        Task<PagedResult<Trainee>> GetAllAsync(int pageNumber = 1, int pageSize = 10, Trainee? filter = null);
        Task<Trainee?> GetByIdAsync(int id);
        Task<Trainee?> DeleteByIdAsync(int id);
        Task<IEnumerable<Trainee>> BulkInsertAsync(List<Trainee> data);
        Task<IEnumerable<Trainee>> BulkUpdateAsync(List<Trainee> data);
        Task<IEnumerable<Trainee>> BulkUpsertAsync(List<Trainee> data);
        Task<IEnumerable<Trainee>> BulkMergeAsync(List<Trainee> data);
    }
}
