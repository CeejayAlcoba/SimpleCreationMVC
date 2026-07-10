
using Models;
using Repositories.Interfaces;
using ApplicationContexts;

namespace Repositories.Classes
{
    public class AuthorityRepository : GenericRepository<Authority>, IAuthorityRepository
    {
        public AuthorityRepository(ApplicationContext context):base(context)
        {
        }
    }
}
