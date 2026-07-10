using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Models.Pagination;

namespace Services.Classes
{
    public class TraineeService : ITraineeService
    {
        private readonly ITraineeRepository _traineeRepository;

        public TraineeService(ITraineeRepository traineeRepository)
        {
            _traineeRepository = traineeRepository;
        }

        public async Task<Trainee?> InsertAsync(Trainee data)
        {
            return await _traineeRepository.InsertAsync(data);
        }

        public async Task<Trainee?> UpdateAsync(Trainee data)
        {
            return await _traineeRepository.UpdateAsync(data);
        }
        public async Task<PagedResult<Trainee>> GetAllAsync(int pageNumber = 1, int pageSize = 10, Trainee? filter = null)
        {
            return await _traineeRepository.GetAllAsync(pageNumber, pageSize, filter);
        }

        public async Task<Trainee?> GetByIdAsync(int id)
        {
            return await _traineeRepository.GetByIdAsync(id);
        }

        public async Task<Trainee?> DeleteByIdAsync(int id)
        {
            return await _traineeRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Trainee>> BulkInsertAsync(List<Trainee> data)
        {
            return await _traineeRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Trainee>> BulkUpdateAsync(List<Trainee> data)
        {
            return await _traineeRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Trainee>> BulkUpsertAsync(List<Trainee> data)
        {
            return await _traineeRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Trainee>> BulkMergeAsync(List<Trainee> data)
        {
            return await _traineeRepository.BulkMergeAsync(data);
        }
    }
}