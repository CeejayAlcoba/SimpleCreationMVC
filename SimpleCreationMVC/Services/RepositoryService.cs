
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;

namespace SimpleCreation.Services
{
    public class RepositoryService
    {
        private readonly FileService fileService = new FileService();
        private readonly StoredProcedureService _storedProcedureService;
        private readonly SqlService _sqlService;
        public RepositoryService(string connectionString)
        {
            _storedProcedureService = new StoredProcedureService(connectionString);
            _sqlService = new SqlService(connectionString);
         
        }
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
                var storedProcedures = _sqlService.GetStoredProceduresByTable(tableName);
                var keyValueList = new List<string>();

                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.GetAll}"))
                {
                    keyValueList.Add($"{ProcedureTypes.GetAll} = {tableName}Procedures.{tableName}_{ProcedureTypes.GetAll}");
                }
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.GetById}"))
                {
                    keyValueList.Add($"{ProcedureTypes.GetById} = {tableName}Procedures.{tableName}_{ProcedureTypes.GetById}");
                }
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.Insert}"))
                {
                    keyValueList.Add($"{ProcedureTypes.Insert} = {tableName}Procedures.{tableName}_{ProcedureTypes.Insert}");
                }
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.Update}"))
                {
                    keyValueList.Add($"{ProcedureTypes.Update} = {tableName}Procedures.{tableName}_{ProcedureTypes.Update}");
                }
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.BulkInsert}"))
                {
                    keyValueList.Add($"{ProcedureTypes.BulkInsert} = {tableName}Procedures.{tableName}_{ProcedureTypes.BulkInsert}");
                }
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.BulkUpdate}"))
                {
                    keyValueList.Add($"{ProcedureTypes.BulkUpdate} = {tableName}Procedures.{tableName}_{ProcedureTypes.BulkUpdate}");
                }
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.BulkUpsert}"))
                {
                    keyValueList.Add($"{ProcedureTypes.BulkUpsert} = {tableName}Procedures.{tableName}_{ProcedureTypes.BulkUpsert}");
                }

                string text = $@"
using Project.{FolderNames.Models};
using Project.{FolderNames.ProcedureEnums};

namespace Project.{FolderNames.Repositories}
{{
    public class {tableName}Repository : GenericRepository<{tableName}, {tableName}Procedures>
    {{
        private static GenericProcedure<{tableName}Procedures> _procedures = new GenericProcedure<{tableName}Procedures>
        {{
            {string.Join(",\n",keyValueList)}
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

