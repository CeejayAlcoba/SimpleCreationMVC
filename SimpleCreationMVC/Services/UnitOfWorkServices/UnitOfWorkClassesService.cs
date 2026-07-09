using SimpleCreation.Models;
using SimpleCreation.Services;
using System.Text;

namespace SimpleCreationMVC.Services.UnitOfWorkServices
{
    public class UnitOfWorkClassesService
    {
        private readonly FileService _fileService = new FileService();

        public void Create(List<TableSchema> tableSchemas)
        {
            StringBuilder propertiesText = new StringBuilder();
            StringBuilder instantiationsText = new StringBuilder();

            foreach (var tableSchema in tableSchemas)
            {
                if (tableSchema.isServiceFileAllowed == false) continue;

                string tableName = tableSchema.TABLE_NAME;

                // e.g., public IDeveloperRepository Developers { get; private set; }
                propertiesText.AppendLine($"        public I{tableName}Repository {tableName}s {{ get; private set; }}");

                // e.g., Developers = new DeveloperRepository(_context);
                instantiationsText.AppendLine($"            {tableName}s = new {tableName}Repository(_context);");
            }

            string text = $@"using Microsoft.EntityFrameworkCore;
using {FolderNames.ApplicationContexts};
using {FolderNames.Repositories}.{FolderNames.Interfaces};
using {FolderNames.Repositories}.{FolderNames.Classes};

namespace {FolderNames.Repositories}.{FolderNames.Classes}
{{
    public class UnitOfWork : IUnitOfWork
    {{
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context)
        {{
            _context = context;
{instantiationsText.ToString().TrimEnd()}
        }}

{propertiesText.ToString().TrimEnd()}

        public int Complete()
        {{
            return _context.SaveChanges();
        }}

        public async Task<int> CompleteAsync()
        {{
            return await _context.SaveChangesAsync();
        }}

        public void Dispose()
        {{
            _context.Dispose();
        }}
    }}
}}";

            _fileService.Create(FolderPaths.RepositoriesClassesFolder, "UnitOfWork.cs", text);
        }
    }
}