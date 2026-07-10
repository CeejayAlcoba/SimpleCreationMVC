
//In Program.cs (Main project) add this 
//builder.Services.AddServices();

using Services.Interfaces;
using Services.Classes;

namespace Services
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorityService, AuthorityService>();
services.AddScoped<IControlNumberService, ControlNumberService>();
services.AddScoped<IDemeritRecordService, DemeritRecordService>();
services.AddScoped<ITestTblService, TestTblService>();
services.AddScoped<ITouringDeductionService, TouringDeductionService>();
services.AddScoped<ITraineeService, TraineeService>();

        }
    }
}
