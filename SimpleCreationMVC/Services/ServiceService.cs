using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
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

            string text = @"
using Project."+FolderNames.Models+@";
using Project."+FolderNames.Repositories+@";

namespace Project."+FolderNames.Services+@"
{
    public class "+ serviceName + @"
    {
        private readonly "+ repository + @" "+ repositoryName + @" = new "+ repository+@"();

        public async Task<" + tableName + @"> Insert("+ tableName + @" data)
        {
           return await " + repositoryName + @".Insert(data);
        }

        public async Task<"+ tableName + @"> Update("+ tableName + @" data)
        {
            return await " + repositoryName + @".Update(data);
        }

        public async Task<IEnumerable<"+ tableName + @">> GetAll()
        {
            return await "+ repositoryName + @".GetAll();
        }

        public async Task<"+ tableName + @"> GetById(int id)
        {
            return await "+ repositoryName + @".GetById(id);
        }
        public async Task<"+ tableName + @"> DeleteById(int id)
        {
              return await  " + repositoryName + @".DeleteById(id);
        }
    }
}";
            return text;
        }
    }
}
