using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.ServiceServices
{
    public class ServiceClassesService
    {
        private readonly FileService _fileService = new FileService();
        private readonly TextService _textService = new TextService();
        public void Create(string tableName)
        {
            string repository = $"{tableName}Repository";
            string repositoryName = $"_{_textService.ToCamelCase(repository)}";
            string repositoryCamel = $"{_textService.ToCamelCase(repository)}";
            string serviceName = $"{tableName}Service";

            string text = $@"
using {FolderNames.Models};
using {FolderNames.Repositories}.{FolderNames.Interfaces};
using {FolderNames.Services}.{FolderNames.Interfaces};

namespace {FolderNames.Services}.{FolderNames.Classes}
{{
    public class {serviceName} : I{serviceName}
    {{
        private readonly I{repository} {repositoryName};

        public TasktblService(I{repository} {repositoryCamel})
        {{
            {repositoryName} = {repositoryCamel};
        }}

        public async Task<{tableName}?> InsertAsync({tableName} data)
        {{
            return await {repositoryName}.InsertAsync(data);
        }}

        public async Task<{tableName}?> UpdateAsync({tableName} data)
        {{
            return await {repositoryName}.UpdateAsync(data);
        }}

        public async Task<IEnumerable<{tableName}>> GetAllAsync({tableName}? filter)
        {{
            return await {repositoryName}.GetAllAsync(filter);
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
            _fileService.Create(FolderPaths.ServicesClassesFolder, $"{tableName}Service.cs", text);
        }
    }
}
