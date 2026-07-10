
using Models;
using Repositories.Interfaces;
using ApplicationContexts;

namespace Repositories.Classes
{
    public class TouringDeductionRepository : GenericRepository<TouringDeduction>, ITouringDeductionRepository
    {
        public TouringDeductionRepository(ApplicationContext context):base(context)
        {
        }
    }
}
