
using Models;
using Repositories.Interfaces;
using ApplicationContexts;

namespace Repositories.Classes
{
    public class TestTblRepository : GenericRepository<TestTbl>, ITestTblRepository
    {
        public TestTblRepository(ApplicationContext context):base(context)
        {
        }
    }
}
