
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
using Dapper;
using Microsoft.Data.SqlClient;
using Project.{FolderNames.Models};
using Project.{FolderNames.ProcedureEnums};
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace Project.{FolderNames.Repositories}
{{
    public class {tableName}Repository : GenericRepository<{tableName}, {tableName}Procedures>
    {{
        // New Get All
        public async Task<IEnumerable<{tableName}>> GetAll()
        {{
            return await GetAll({tableName}Procedures.{tableName}_GetAll);
        }}

        // New Get By ID
        public async Task<{tableName}?> GetById(int id)
        {{
            return await GetById({tableName}Procedures.{tableName}_GetById, id);
        }}

        // New Insert
        public async Task<{tableName}?> Insert({tableName} entity)
        {{
            return await Insert({tableName}Procedures.{tableName}_Insert, entity);
        }}

        // New Update
        public async Task<{tableName}?> Update({tableName} entity)
        {{
            return await Update({tableName}Procedures.{tableName}_Update, entity);
        }}

        // New Delete By ID
        public async Task<{tableName}?> DeleteById(int id)
        {{
             var data = await GetById(id);
             await DeleteById({tableName}Procedures.{tableName}_DeleteById, id);
             return data;
        }}
    }}
}}";

                // Create the repository file for the table
                fileService.Create(FolderNames.Repositories.ToString(), $"{tableName}Repository.cs", text);
            }
        }

    }
}
