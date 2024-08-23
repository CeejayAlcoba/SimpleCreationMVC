using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreation.Services;
using System.Text;

namespace SimpleCreationMVC.Services
{
    public class FronEndService
    {
        private readonly TextService _textService = new TextService();
        private readonly FileService _fileService = new FileService();
        private readonly SqlService _sqlService;

        public FronEndService(string connectionString)
        {
            _sqlService = new SqlService(connectionString);
        }
        public string GenerateJsClassText(TableSchema tableSchema)
        {
            StringBuilder columns = new StringBuilder();
            List<string> columnsParam = new List<string>();
            foreach (var column in tableSchema.Columns)
            {
                var columnCamel = _textService.ToCamelCase(column.COLUMN_NAME);
                columnsParam.Add(columnCamel);
                columns.AppendLine($"\t\tthis.{columnCamel} = {columnCamel};");
            }
            string text = @"
class " + tableSchema.TABLE_NAME + @" {
  constructor({" + String.Join(",", columnsParam) + @"}) {
" + columns + @"
  }
}";

            return text;
        }

        public void CreateJsClasses(List<TableSchema> tableSchemas = null)
        {
            if (tableSchemas.IsNullOrEmpty()) tableSchemas = _sqlService.GetAllTableSchema();

            StringBuilder jsClassesText = new StringBuilder();
            foreach (var tableSchema in tableSchemas)
            {
                string text = GenerateJsClassText(tableSchema);
                jsClassesText.AppendLine(text + "\n");
            }
            _fileService.Create(FolderNames.JsClasses.ToString(), "Classes.js", jsClassesText.ToString());
        }


        public string CreateTsType(TableSchema tableSchema)
        {
            StringBuilder columns = new StringBuilder();
            var primaryKey = _sqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME);
            string primaryKeyName = primaryKey.COLUMN_NAME;
            foreach (var column in tableSchema.Columns)
            {

                var columnCamel = _textService.ToCamelCase(column.COLUMN_NAME);
                string tsDatatype = new DataType().ConvertToTypeScript(column.DATA_TYPE);
                string isNullable = (column.IS_NULLABLE == "YES" || primaryKeyName == column.COLUMN_NAME) ? "?" : "";
                columns.AppendLine($"\t{columnCamel}{isNullable} : {tsDatatype};");
            }
            string text = @"
export type " + tableSchema.TABLE_NAME + @" = {
 " + columns + @"
};";
            return text;

        }
        public void CreateTsTypes()
        {
            string folderName = FolderNames.TsTypes.ToString();
            var tableShemas = _sqlService.GetAllTableSchema();
            foreach (var tableSchema in tableShemas)
            {
                string text = CreateTsType(tableSchema);
                _fileService.Create(FolderNames.TsTypes.ToString(), $"{tableSchema.TABLE_NAME}.ts", text);
            }

        }
    }
}