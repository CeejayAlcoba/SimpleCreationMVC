using SimpleCreation.Models;
using SimpleCreation.Services;
using System.Collections.Generic;
using System.Text;

namespace SimpleCreationMVC.Services.RepositoryServices
{
    public class RepositoryMainService
    {
        private readonly RepositoryClassesService _repositoryClassesService;
        private readonly RepositoryInterfacesService _repositoryInterfacesService = new RepositoryInterfacesService();
        private readonly FileService _fileService = new FileService();
        public RepositoryMainService(string connectionString)
        {
            _repositoryClassesService = new RepositoryClassesService(connectionString);
        }
        public void CreateStoredProcedure(List<TableSchema> tableSchemas)
        {
            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isRepositoryFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                _repositoryClassesService.CreateStoredProcedure(tableName);
                _repositoryInterfacesService.CreateStoredProcedure(tableName);
            }
            CreateStoredProcedureRegistration(tableSchemas);

        }
        public void CreateCommon(List<TableSchema> tableSchemas)
        {
            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isRepositoryFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                _repositoryClassesService.CreateCommon(tableName);
                _repositoryInterfacesService.CreateCommon(tableSchemas);
            }
            CreateCommonRegistration(tableSchemas);

        }

        public void CreateStoredProcedureRegistration(List<TableSchema> tableSchemas)
        {
            StringBuilder scopesText = new StringBuilder();
            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isServiceFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                scopesText.AppendLine($"services.AddScoped<I{tableName}Repository, {tableName}Repository>();");
            }

            string text = $@"
//In Program.cs (Main project) add this 
//builder.Services.AddRepositories();

using {FolderNames.Repositories}.{FolderNames.Interfaces};
using {FolderNames.Repositories}.{FolderNames.Classes};

namespace {FolderNames.Repositories}
{{
    public static class RepositoryRegistration
    {{
        public static void AddRepositories(this IServiceCollection services)
        {{
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            {scopesText}
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.RepositoriesFolder, "RepositoryRegistration.cs", text);
        }
        public void CreateCommonRegistration(List<TableSchema> tableSchemas)
        {
            StringBuilder scopesText = new StringBuilder();
            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isServiceFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                scopesText.AppendLine($"services.AddScoped<I{tableName}Repository, {tableName}Repository>();");
            }

            string text = $@"
//In Program.cs (Main project) add this 
//builder.Services.AddRepositories();

using {FolderNames.Repositories}.{FolderNames.Interfaces};
using {FolderNames.Repositories}.{FolderNames.Classes};

namespace {FolderNames.Repositories}
{{
    public static class RepositoryRegistration
    {{
        public static void AddRepositories(this IServiceCollection services)
        {{
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            {scopesText}
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.RepositoriesFolder, "RepositoryRegistration.cs", text);
        }
    }
    
}
