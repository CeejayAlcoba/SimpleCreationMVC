
//In Program.cs (Main project) add this 
//builder.Services.AddRepositories();

using Repositories.Interfaces;
using Repositories.Classes;

namespace Repositories
{
    public static class RepositoryRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthorityRepository, AuthorityRepository>();
services.AddScoped<IControlNumberRepository, ControlNumberRepository>();
services.AddScoped<IDemeritRecordRepository, DemeritRecordRepository>();
services.AddScoped<ITestTblRepository, TestTblRepository>();
services.AddScoped<ITouringDeductionRepository, TouringDeductionRepository>();
services.AddScoped<ITraineeRepository, TraineeRepository>();

        }
    }
}
