using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.RepositoryServices
{
    public class RepositoryInterfacesService
    {
        private readonly FileService _fileService = new FileService();
        public void CreateStoredProcedure(string tableName)
        {
            string text = $@"
using {FolderNames.Models};
using {FolderNames.ProcedureEnums};

namespace {FolderNames.Repositories}.{FolderNames.Interfaces}
{{
    public interface I{tableName}Repository : IGenericRepository<{tableName}, {tableName}Procedures>
    {{
       
    }}
}}
";
            _fileService.Create(FolderPaths.RepositoriesInterfacesFolder, $"I{tableName}Repository.cs", text);

        }
        public void CreateCommon(List<TableSchema> tableSchemas)
        {
            foreach (var tableSchema in tableSchemas)
            {
                string tableName = tableSchema.TABLE_NAME;

                string text = $@"
using {FolderNames.Models};

namespace {FolderNames.Repositories}.{FolderNames.Interfaces}
{{
    public interface I{tableName}Repository : IGenericRepository<{tableName}>
    {{
       
    }}
}}
";
                _fileService.Create(FolderPaths.RepositoriesInterfacesFolder, $"I{tableName}Repository.cs", text);
            }

        }
    }
}
