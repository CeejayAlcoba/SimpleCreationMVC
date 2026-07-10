
using Models;
using Repositories.Interfaces;
using ApplicationContexts;

namespace Repositories.Classes
{
    public class DemeritRecordRepository : GenericRepository<DemeritRecord>, IDemeritRecordRepository
    {
        public DemeritRecordRepository(ApplicationContext context):base(context)
        {
        }
    }
}
