using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.RepositoryServices
{
    public class RepositoryClassesService
    {
        private readonly FileService fileService = new FileService();
        private readonly SqlService _sqlService;
        public RepositoryClassesService(string connectionString)
        {
            _sqlService = new SqlService(connectionString);
        }
        public void CreateStoredProcedure(string tableName)
        {
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
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.DeleteById}"))
                {
                    keyValueList.Add($"{ProcedureTypes.DeleteById} = {tableName}Procedures.{tableName}_{ProcedureTypes.DeleteById}");
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
                if (storedProcedures.Contains($"{tableName}_{ProcedureTypes.BulkMerge}"))
                {
                    keyValueList.Add($"{ProcedureTypes.BulkMerge} = {tableName}Procedures.{tableName}_{ProcedureTypes.BulkMerge}");
                }

                string text = $@"
using {FolderNames.Models};
using {FolderNames.ProcedureEnums};
using {FolderNames.Repositories}.{FolderNames.Interfaces};

namespace {FolderNames.Repositories}.{FolderNames.Classes}
{{
    public class {tableName}Repository : GenericRepository<{tableName}, {tableName}Procedures>, I{tableName}Repository
    {{
        private static GenericProcedure<{tableName}Procedures> _procedures = new GenericProcedure<{tableName}Procedures>
        {{
            {string.Join(",\n\t\t\t", keyValueList)}
        }};
        public {tableName}Repository() : base(_procedures)
        {{
        }}
    }}
}}";
                fileService.Create(FolderPaths.RepositoriesClassesFolder, $"{tableName}Repository.cs", text);
            }
        

        public void CreateCommon(string tableName)
        {
            string text = $@"
using {FolderNames.Models};
using {FolderNames.Repositories}.{FolderNames.Interfaces};

namespace {FolderNames.Repositories}.{FolderNames.Classes}
{{
    public class {tableName}Repository : GenericRepository<{tableName}>, I{tableName}Repository
    {{
    }}
}}
";
            fileService.Create(FolderPaths.RepositoriesClassesFolder, $"{tableName}Repository.cs", text);
        }
    }
}
