using SimpleCreation.Models;
using SimpleCreation.Services;
using System.Text;

namespace SimpleCreationMVC.Services.UnitOfWorkServices
{
    public class UnitOfWorkInterfacesService
    {
        private readonly FileService _fileService = new FileService();

        public void Create(List<TableSchema> tableSchemas)
        {
            StringBuilder propertiesText = new StringBuilder();

            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isServiceFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                propertiesText.AppendLine($"        I{tableName}Repository {tableName}s {{ get; }}");
            }

            string text = $@"using {FolderNames.Repositories}.{FolderNames.Interfaces};

namespace {FolderNames.Repositories}.{FolderNames.Interfaces}
{{
    public interface IUnitOfWork : IDisposable
    {{
{propertiesText.ToString().TrimEnd()}
        int Complete();
        Task<int> CompleteAsync();
    }}
}}";

            _fileService.Create(FolderPaths.RepositoriesInterfacesFolder, "IUnitOfWork.cs", text);
        }
    }
}