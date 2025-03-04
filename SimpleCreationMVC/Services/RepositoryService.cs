
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
using Project." + FolderNames.Models.ToString() + @";

namespace Project." + FolderNames.Repositories.ToString() + @"
{
    public class " + tableName + "Repository : GenericRepository<" + tableName + @">
    {

    }
}
";
            fileService.Create(FolderNames.Repositories.ToString(), $"{tableName}Repository.cs", text);
        }

        public void CreateRepositoryStoredProcedureFile(List<TableSchema> tableSchemas)
        {
            foreach (var table in tableSchemas)
            {
                var tableName = table.TABLE_NAME;
                string text = $@"
using Project.{FolderNames.Models};
using Project.{FolderNames.ProcedureEnums};

namespace Project.{FolderNames.Repositories}
{{
    public class {tableName}Repository : GenericRepository<{tableName}, {tableName}Procedures>
    {{
        private static GenericProcedure<{tableName}Procedures> _procedures = new GenericProcedure<{tableName}Procedures>
        {{
            GetAll = {tableName}Procedures.{tableName}_GetAll,
            GetById = {tableName}Procedures.{tableName}_GetById,
            DeleteById = {tableName}Procedures.{tableName}_DeleteById,
            Insert = {tableName}Procedures.{tableName}_Insert,
            Update = {tableName}Procedures.{tableName}_Update,
            InsertMany = {tableName}Procedures.{tableName}_InsertMany,
            UpdateMany = {tableName}Procedures.{tableName}_UpdateMany,
        }};
        public {tableName}Repository() : base(_procedures)
        {{
        }}
    }}
}}";

                // Create the repository file for the table
                fileService.Create(FolderNames.Repositories.ToString(), $"{tableName}Repository.cs", text);
            }
        }

    }
}
