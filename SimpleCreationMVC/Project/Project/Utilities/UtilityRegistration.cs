
//In Program.cs (Main project) add this 
//builder.Services.AddUtilities();

using Utilities.Interfaces;
using Utilities.Classes;

namespace Utilities
{
    public static class UtilityRegistration
    {
        public static void AddUtilities(this IServiceCollection services)
        {
            services.AddScoped<IAppUtility, AppUtility>();
            services.AddScoped<IAutoMapperUtility, AutoMapperUtility>();
            services.AddScoped<IDataTableUtility, DataTableUtility>();
            services.AddScoped<IClaimsHelperUtility, ClaimsHelperUtility>();
            services.AddScoped<IEncryptUtility, EncryptUtility>();
            services.AddScoped<IJwtUtility, JwtUtility>();
            services.AddScoped<IFileUtility, FileUtility>();
        }
    }
}
