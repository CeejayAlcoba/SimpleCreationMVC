// In Program.cs (Main project) add this: 
// builder.Services.AddUnitOfWork();

using Microsoft.Extensions.DependencyInjection;
using Repositories.Interfaces;
using Repositories.Classes;

namespace Repositories
{
    public static class UnitOfWorkRegistration
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}