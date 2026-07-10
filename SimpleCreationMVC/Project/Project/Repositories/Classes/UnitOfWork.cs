using Microsoft.EntityFrameworkCore;
using ApplicationContexts;
using Repositories.Interfaces;
using Repositories.Classes;

namespace Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Authoritys = new AuthorityRepository(_context);
            ControlNumbers = new ControlNumberRepository(_context);
            DemeritRecords = new DemeritRecordRepository(_context);
            TestTbls = new TestTblRepository(_context);
            TouringDeductions = new TouringDeductionRepository(_context);
            Trainees = new TraineeRepository(_context);
        }

        public IAuthorityRepository Authoritys { get; private set; }
        public IControlNumberRepository ControlNumbers { get; private set; }
        public IDemeritRecordRepository DemeritRecords { get; private set; }
        public ITestTblRepository TestTbls { get; private set; }
        public ITouringDeductionRepository TouringDeductions { get; private set; }
        public ITraineeRepository Trainees { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}