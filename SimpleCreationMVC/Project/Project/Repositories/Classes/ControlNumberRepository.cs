
using Models;
using Repositories.Interfaces;
using ApplicationContexts;

namespace Repositories.Classes
{
    public class ControlNumberRepository : GenericRepository<ControlNumber>, IControlNumberRepository
    {
        public ControlNumberRepository(ApplicationContext context):base(context)
        {
        }
    }
}
