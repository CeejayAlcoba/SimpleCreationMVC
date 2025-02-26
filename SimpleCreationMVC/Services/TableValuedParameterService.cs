using SimpleCreation.Models;
using SimpleCreation.Services;
using System.Text;

namespace SimpleCreationMVC.Services
{
    public class TableValuedParameterService
    {
        public readonly FileService _fileService = new FileService();
        public string CreateTVPFile(TableSchema tableSchema)
        {
            var columnsCount = tableSchema.Columns.Count();
            var tvpName = GetTVPName(tableSchema.TABLE_NAME);
            StringBuilder columnsStr = new StringBuilder();

            for (int i = 0; i < columnsCount; i++)
            {
                var column = tableSchema.Columns[i];

                var charNum = column.CHARACTER_MAXIMUM_LENGTH > 0 ? column.CHARACTER_MAXIMUM_LENGTH.ToString() : "MAX";
                var dataType = column.DATA_TYPE.Contains("char") ? $"{column.DATA_TYPE}({charNum})" : column.DATA_TYPE;
                var query = $"\t{column.COLUMN_NAME} {dataType}";

                columnsStr.AppendLine(i == columnsCount-1 ? query : query + ",");

            }


            var content = $@"
CREATE TYPE {tvpName} AS TABLE
(
{columnsStr}
)
GO
";
            _fileService.Create(FolderNames.TableValuedParameters.ToString(), $"{tvpName}.sql", content);
            return content;
        }

        public string GetTVPName(string tableName)
        {
            return $"TVP_{tableName}";
        }
    }
}
