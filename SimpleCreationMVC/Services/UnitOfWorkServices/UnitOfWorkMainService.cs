using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.UnitOfWorkServices
{
    public class UnitOfWorkMainService
    {
        private readonly UnitOfWorkClassesService _uowClassesService = new UnitOfWorkClassesService();
        private readonly UnitOfWorkInterfacesService _uowInterfacesService = new UnitOfWorkInterfacesService();
        private readonly FileService _fileService = new FileService();

        public void CreateCommon(List<TableSchema> tableSchemas)
        {
            _uowInterfacesService.Create(tableSchemas);
            _uowClassesService.Create(tableSchemas);

            CreateRegistration();
        }

        public void CreateRegistration()
        {
            string text = $@"// In Program.cs (Main project) add this: 
// builder.Services.AddUnitOfWork();

using Microsoft.Extensions.DependencyInjection;
using {FolderNames.Repositories}.{FolderNames.Interfaces};
using {FolderNames.Repositories}.{FolderNames.Classes};

namespace {FolderNames.Repositories}
{{
    public static class UnitOfWorkRegistration
    {{
        public static void AddUnitOfWork(this IServiceCollection services)
        {{
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }}
    }}
}}";

            _fileService.Create(FolderPaths.RepositoriesFolder, "UnitOfWorkRegistration.cs", text);
        }
    }
}