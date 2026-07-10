using Repositories.Interfaces;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorityRepository Authoritys { get; }
        IControlNumberRepository ControlNumbers { get; }
        IDemeritRecordRepository DemeritRecords { get; }
        ITestTblRepository TestTbls { get; }
        ITouringDeductionRepository TouringDeductions { get; }
        ITraineeRepository Trainees { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}