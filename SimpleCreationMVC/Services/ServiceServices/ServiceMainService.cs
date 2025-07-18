using System.Text;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.ServiceServices
{
    public class ServiceMainService
    {
        private readonly ServiceClassesService _serviceClassesService = new ServiceClassesService();
        private readonly ServiceInterfacesService _serviceInterfacesService = new ServiceInterfacesService();
        private readonly FileService _fileService = new FileService();
        public void CreateCommon(List<TableSchema> tableSchemas)
        {
            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isServiceFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                _serviceInterfacesService.Create(tableName);
                _serviceClassesService.Create(tableName);
            }
            CreateRegistration(tableSchemas);
        }

        public void CreateRegistration(List<TableSchema> tableSchemas)
        {
            StringBuilder scopesText = new StringBuilder();
            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isServiceFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                scopesText.AppendLine($"services.AddScoped<I{tableName}Service, {tableName}Service>();");
            }

            string text = $@"
//In Program.cs (Main project) add this 
//builder.Services.AddServices();

using {FolderNames.Services}.{FolderNames.Interfaces};
using {FolderNames.Services}.{FolderNames.Classes};

namespace {FolderNames.Services}
{{
    public static class ServiceRegistration
    {{
        public static void AddServices(this IServiceCollection services)
        {{
            {scopesText}
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.ServicesFolder, "ServiceRegistration.cs", text);
        }
    }
}
