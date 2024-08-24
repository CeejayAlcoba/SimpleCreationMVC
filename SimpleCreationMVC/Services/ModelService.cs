using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleCreation.Services
{
    public class ModelService
    {
        private readonly SqlService SqlService;
        private readonly DataType DataType = new DataType();
        private readonly FileService fileService = new FileService();
        public ModelService(string connectionString)
        {
            this.SqlService = new SqlService(connectionString);
        }
        public Model GetModelByTable(TableSchema table)
        {
            var columns = ConvertColumnsDataTypeToDotNet(table.Columns);

            return new Model
            {
                Name = table.TABLE_NAME,
                Columns = columns,
            };
        }
        public List<Column> ConvertColumnsDataTypeToDotNet(List<Column> columns)
        {
            List<Column> newColumns = new List<Column>();
            foreach (var column in columns)
            {
                newColumns.Add(new Column
                {
                    COLUMN_NAME = column.COLUMN_NAME,
                    IS_NULLABLE = column.IS_NULLABLE,
                    DATA_TYPE = DataType.ConvertToDotNet(column.DATA_TYPE)
                });
            };
            return newColumns;
        }
        public void CreateModelClassesFiles(List<TableSchema> tableSchemas)
        {
            foreach (var tableSchema in tableSchemas)
            {
                var model = GetModelByTable(tableSchema);
                CreateModelClassFile(model);
            }
            
        }
        public void CreateModelClassFile(Model model)
        {
            StringBuilder columns = new StringBuilder();
            foreach (var column in model.Columns)
            {
                var primaryKey = SqlService.GetTablePrimaryKey(model.Name).COLUMN_NAME;
                var isNullable = (column.IS_NULLABLE == "YES" || primaryKey == column.COLUMN_NAME) ? "?" : "";
                var isPrimaryKey = primaryKey == column.COLUMN_NAME ? "[Key]\n" : "";

                var columnText = isPrimaryKey + "\t\tpublic " + column.DATA_TYPE + isNullable + " " + column.COLUMN_NAME + " {get;set;}";

                columns.AppendLine(columnText);
            }
            string text = @"
using System.ComponentModel.DataAnnotations;

namespace Project."+FolderNames.Models.ToString()+@"
{
    public class "+model.Name+@"
     {
		"+columns+@"
     }
}
";
            fileService.Create(FolderNames.Models.ToString(), $"{model.Name}.cs", text);
        }
    }
}
