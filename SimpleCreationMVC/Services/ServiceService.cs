using SimpleCreation.Models;

namespace SimpleCreation.Services
{
    public class ServiceService
    {
        private readonly FileService fileService = new FileService();
        private readonly TextService textService = new TextService();
        public void CreateServicesFiles(List<TableSchema> tableSchemas)
        {
            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isServiceFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;
                string text = GenerateServiceText(tableName);
                fileService.Create(FolderNames.Services.ToString(), $"{tableName}Service.cs", text);
            }
        }
        public string GenerateServiceText(string tableName)
        {
            string repository = $"{tableName}Repository";
            string repositoryName = $"_{textService.ToCamelCase(repository)}";
            string serviceName = $"{tableName}Service";

            string text = $@"
using Project.{FolderNames.Models};
using Project.{FolderNames.Repositories};

namespace Project.{FolderNames.Services}
{{
    public class {serviceName}
    {{
        private readonly {repository} {repositoryName} = new {repository}();

        public async Task<{tableName}?> InsertAsync({tableName} data)
        {{
            return await {repositoryName}.InsertAsync(data);
        }}

        public async Task<{tableName}?> UpdateAsync({tableName} data)
        {{
            return await {repositoryName}.UpdateAsync(data);
        }}

        public async Task<IEnumerable<{tableName}>> GetAllAsync()
        {{
            return await {repositoryName}.GetAllAsync();
        }}

        public async Task<{tableName}?> GetByIdAsync(int id)
        {{
            return await {repositoryName}.GetByIdAsync(id);
        }}

        public async Task<{tableName}?> DeleteByIdAsync(int id)
        {{
            return await {repositoryName}.DeleteByIdAsync(id);
        }}
        public async Task<IEnumerable<{tableName}>> BulkInsertAsync(List<{tableName}> data)
        {{
            return await {repositoryName}.BulkInsertAsync(data);
        }}
        public async Task<IEnumerable<{tableName}>> BulkUpdateAsync(List<{tableName}> data)
        {{
            return await {repositoryName}.BulkUpdateAsync(data);
        }}
        public async Task<IEnumerable<{tableName}>> BulkUpsertAsync(List<{tableName}> data)
        {{
            return await {repositoryName}.BulkUpsertAsync(data);
        }}
        public async Task<IEnumerable<{tableName}>> BulkMergeAsync(List<{tableName}> data)
        {{
            return await {repositoryName}.BulkMergeAsync(data);
        }}
    }}
}}";

            return text;
        }
    }
}
