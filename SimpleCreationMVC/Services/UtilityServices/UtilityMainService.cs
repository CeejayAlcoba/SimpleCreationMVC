using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.UtilityServices
{
    public class UtilityMainService
    {

        private readonly UtilityInterfacesService _utilityInterfacesService = new UtilityInterfacesService();
        private readonly UtilityClassesService _utilityClassesService = new UtilityClassesService();
        private readonly UtilityReadMe _utilityReadMe = new UtilityReadMe();
        private readonly FileService _fileService = new FileService();
        public void Create()
        {
            _utilityInterfacesService.CreateAppUtility();
            _utilityInterfacesService.CreateAutoMapperUtility();
            _utilityInterfacesService.CreateDataTableUtility();
            _utilityInterfacesService.CreateClaimsHelperUtility();
            _utilityInterfacesService.CreateJwtUtility();
            _utilityInterfacesService.CreateEncryptUtility();
            _utilityInterfacesService.CreateFileUtility();

            _utilityClassesService.CreateAppUtility();
            _utilityClassesService.CreateAutoMapperUtility();
            _utilityClassesService.CreateDataTableUtility();
            _utilityClassesService.CreateClaimsHelperUtility();
            _utilityClassesService.CreateJwtUtility();
            _utilityClassesService.CreateEncryptUtility();
            _utilityClassesService.CreateFileUtility();

            _utilityReadMe.CreateFileUtility();
            _utilityReadMe.CreateJWTUtility();
            _utilityReadMe.CreateAppUtility();

            CreateRegistration();
        }

        private void CreateRegistration()
        {
            string text = $@"
//In Program.cs (Main project) add this 
//builder.Services.AddUtilities();

using {FolderNames.Utilities}.{FolderNames.Interfaces};
using {FolderNames.Utilities}.{FolderNames.Classes};

namespace {FolderNames.Utilities}
{{
    public static class UtilityRegistration
    {{
        public static void AddUtilities(this IServiceCollection services)
        {{
            services.AddScoped<IAppUtility, AppUtility>();
            services.AddScoped<IAutoMapperUtility, AutoMapperUtility>();
            services.AddScoped<IDataTableUtility, DataTableUtility>();
            services.AddScoped<IClaimsHelperUtility, ClaimsHelperUtility>();
            services.AddScoped<IEncryptUtility, EncryptUtility>();
            services.AddScoped<IJwtUtility, JwtUtility>();
            services.AddScoped<IFileUtility, FileUtility>();
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesFolder, "UtilityRegistration.cs",text);
        }
    }
}
