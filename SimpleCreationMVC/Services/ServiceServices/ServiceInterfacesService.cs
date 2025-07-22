using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.ServiceServices
{
    public class ServiceInterfacesService
    {
        private readonly FileService _fileService = new FileService();
        public void Create(string tableName)
        {
            string text = $@"
using Models;

namespace {FolderNames.Services}.{FolderNames.Interfaces}
{{
    public interface I{tableName}Service
    {{
        Task<{tableName}?> InsertAsync({tableName} data);
        Task<{tableName}?> UpdateAsync({tableName} data);
        Task<IEnumerable<{tableName}>> GetAllAsync({tableName}? filter);
        Task<{tableName}?> GetByIdAsync(int id);
        Task<{tableName}?> DeleteByIdAsync(int id);
        Task<IEnumerable<{tableName}>> BulkInsertAsync(List<{tableName}> data);
        Task<IEnumerable<{tableName}>> BulkUpdateAsync(List<{tableName}> data);
        Task<IEnumerable<{tableName}>> BulkUpsertAsync(List<{tableName}> data);
        Task<IEnumerable<{tableName}>> BulkMergeAsync(List<{tableName}> data);
    }}
}}
";
            _fileService.Create(FolderPaths.ServicesInterfacesFolder, $"I{tableName}Service.cs", text);
        }
    }
}
