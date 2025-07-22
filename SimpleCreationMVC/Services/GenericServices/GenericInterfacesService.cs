using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.GenericServices
{
    public class GenericInterfacesService
    {
        private readonly FileService _fileService = new FileService();
        public void CreateStoredProcedure()
        {
            string text = $@"
using System.Data;
using System.Threading.Tasks;

namespace {FolderNames.Repositories}.{FolderNames.Interfaces}
{{
    public interface IGenericRepository<T, TProcedures>
        where T : class
        where TProcedures : struct, Enum
    {{
        Task<IEnumerable<T>> GetAllAsync(T? filter);
        Task<T?> GetByIdAsync(int id);
        Task<T?> InsertAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteByIdAsync(int id);
        Task<IEnumerable<T>> BulkInsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpdateAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkMergeAsync(List<T> data);
    }}
}}
";
            _fileService.Create(FolderPaths.RepositoriesInterfacesFolder, "IGenericRepository.cs", text);
        }
        public void CreateDapperQuery()
        {
            string text = $@"
using System.Data;
using System.Threading.Tasks;

namespace {FolderNames.Repositories}.{FolderNames.Interfaces}
{{
    public interface IGenericRepository<T>
        where T : class
    {{
        Task<IEnumerable<T>> GetAllAsync(T? filter);
        Task<T?> GetByIdAsync(int id);
        Task<T?> InsertAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteByIdAsync(int id);
        Task<IEnumerable<T>> BulkInsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpdateAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkMergeAsync(List<T> data, string? filterColumnName = null, object? filterValue = null);
    }}
}}
";
            _fileService.Create(FolderPaths.RepositoriesInterfacesFolder, "IGenericRepository.cs", text);
        }
        public void CreateEFCore()
        {
            string text = $@"
using System.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace {FolderNames.Repositories}.{FolderNames.Interfaces}
{{
    public interface IGenericRepository<T>
        where T : class
    {{
        Task<IEnumerable<T>> GetAllAsync(T? filter);
        Task<T?> GetByIdAsync(int id);
        Task<T?> InsertAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteByIdAsync(int id);
        Task<IEnumerable<T>> BulkInsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpdateAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkMergeAsync(List<T> entities, Expression<Func<T, bool>>? deleteFilter = null);
    }}
}}
";
            _fileService.Create(FolderPaths.RepositoriesInterfacesFolder, "IGenericRepository.cs", text);
        }
    }
}
