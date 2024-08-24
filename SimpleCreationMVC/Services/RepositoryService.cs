
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;

namespace SimpleCreation.Services
{
    public class RepositoryService
    {
        private readonly FileService fileService = new FileService();
        public void CreateRepositoriesFiles(List<TableSchema> tableSchemas)
        {

            foreach (var table in tableSchemas)
            {
                string tableName = table.TABLE_NAME;
                CreateRepositoryFile(tableName);
            }

        }
        public void CreateRepositoryFile(string tableName)
        {
            string text = @"
using Project."+FolderNames.Models.ToString() + @";

namespace Project."+FolderNames.Repositories.ToString()+@"
{
    public class "+tableName+"Repository : GenericRepository<"+tableName+@">
    {

    }
}
";
            fileService.Create(FolderNames.Repositories.ToString(), $"{tableName}Repository.cs", text);
        }

    }
}
