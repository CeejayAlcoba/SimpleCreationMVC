using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreationMVC.Services;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleCreation.Services
{
    public class StoredProcedureService
    {
        private readonly SqlService _sqlService;
        private readonly FileService _fileService = new FileService();
        private readonly TableValuedParameterService _tableValuedParameterService = new TableValuedParameterService();
        public StoredProcedureService(string connectionString) { 
          _sqlService = new SqlService(connectionString);
        }
        public void CreateStoredProceduresFiles(List<TableSchema> tableSchemas)
        {
            var currentProcedures = _sqlService.GetAllCurentStoredProcesures();
            StringBuilder queries = new StringBuilder();
            foreach (var tableSchema in tableSchemas)
            {
                var tableName = tableSchema.TABLE_NAME;

                string strGetAll = CreateProcedureGetAllFile(tableSchema, currentProcedures);
                queries.AppendLine(strGetAll);
               
                string strInsert = CreateProcedureInsertFile(tableSchema, currentProcedures);
                queries.AppendLine(strInsert);
               
                string strUpdate = CreateProcedureUpdateFile(tableSchema, currentProcedures);
                queries.AppendLine(strUpdate);
               
                string strGetById = CreateProcedureGetByIdFile(tableSchema, currentProcedures);
                queries.AppendLine(strGetById);
               
                string strDeleteById = CreateProcedureDeleteByIdFile(tableSchema, currentProcedures);
                queries.AppendLine(strDeleteById);

                string strTVP = _tableValuedParameterService.CreateTVPFile(tableSchema);
                queries.AppendLine(strTVP);

                string strInsertMany = CreateProcedureInsertManyFile(tableSchema, currentProcedures);
                queries.AppendLine(strInsertMany);

                string strUpdateMany = CreateProcedureUpdateManyFile(tableSchema, currentProcedures);
                queries.AppendLine(strUpdateMany);

            }
            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"All.sql", queries.ToString());
        }
        private string CreateProcedureGetByIdFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.GetById.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            string primaryKey = _sqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
            StringBuilder text = new StringBuilder($@"
{alterOrCreate} PROCEDURE {procedureName}
@Id INT
AS
   SELECT * FROM {tableSchema.TABLE_NAME}
   WHERE {primaryKey} = @Id
GO
            ");
            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        private string CreateProcedureUpdateFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.Update.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            string primaryKey = _sqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
            StringBuilder setValues = new StringBuilder();

            for (int i = 0; i < tableSchema.Columns.Count; i++)
            {
                var column = tableSchema.Columns[i];
                if (primaryKey == column.COLUMN_NAME) continue;

                setValues.Append($"\t\t{column.COLUMN_NAME}=@{column.COLUMN_NAME}");
                if (i < tableSchema.Columns.Count - 1)
                {
                    setValues.Append($",\n");
                }
            }
            string parameters = _sqlService.GetColumnParameterSP(tableSchema.Columns);
            StringBuilder text = new StringBuilder($@"
{alterOrCreate} PROCEDURE {procedureName}
{parameters}
AS
   UPDATE {tableSchema.TABLE_NAME}
   SET 
{setValues}
    WHERE {primaryKey} = @{primaryKey}
    SELECT * FROM {tableSchema.TABLE_NAME} WHERE {primaryKey} = @Id
 GO
            ");
            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        private string CreateProcedureGetAllFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.GetAll.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);

            StringBuilder text = new StringBuilder($@"
{alterOrCreate} PROCEDURE {procedureName}
AS
    SELECT * FROM {tableSchema.TABLE_NAME}
GO
");

            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        private string CreateProcedureDeleteByIdFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.DeleteById.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);

            var primaryKey = _sqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
            StringBuilder text = new StringBuilder($@"
{alterOrCreate} PROCEDURE {procedureName}
    @Id INT
AS
    
    DELETE FROM {tableSchema.TABLE_NAME}
    WHERE {primaryKey} =  @Id
GO
            ");

            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        private string CreateProcedureInsertFile(TableSchema tableSchema,List<string> currentProcedures )
        {
            var primaryKey = _sqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME);
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.Insert.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            for (int i = 0; i < tableSchema.Columns.Count; i++)
            {
                var column = tableSchema.Columns[i];
                if (primaryKey.COLUMN_NAME == column.COLUMN_NAME) continue;

                columns.Append($"\t\t{column.COLUMN_NAME}");
                if (i < tableSchema.Columns.Count - 1)
                {
                    columns.Append($",\n");
                }
            }

            for (int i = 0; i < tableSchema.Columns.Count; i++)
            {
                var column = tableSchema.Columns[i];
                if (primaryKey.COLUMN_NAME == column.COLUMN_NAME) continue;

                values.Append($"\t\t@{column.COLUMN_NAME}");
                if (i < tableSchema.Columns.Count - 1)
                {
                    values.Append($",\n");
                }
            }
            var filteredColumns = tableSchema.Columns.Where(c => c.COLUMN_NAME != primaryKey.COLUMN_NAME).ToList();
            string parameters = _sqlService.GetColumnParameterSP(filteredColumns);
            StringBuilder text = new StringBuilder($@"
{alterOrCreate} PROCEDURE {procedureName}
    @{primaryKey.COLUMN_NAME} {primaryKey.DATA_TYPE} = NULL,
{parameters}
AS
   INSERT INTO {tableSchema.TABLE_NAME}(
{columns}
        )
   VALUES (
{values}
        )
   SELECT * FROM {tableSchema.TABLE_NAME} WHERE {primaryKey.COLUMN_NAME} = SCOPE_IDENTITY()
GO
            ");

            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        public void CreateEnumProceduresFile()
        {
            var tables = _sqlService.GetAllTableSchema();
            foreach(var table  in tables)
            {
                var tableName = table.TABLE_NAME;
                var procedures = _sqlService.GetStoredProceduresByTable(tableName);

               
                    StringBuilder text = new StringBuilder();

                    text.AppendLine($@"
namespace Project.{FolderNames.ProcedureEnums}
{{
    // Procedures for the {tableName} table
    public enum {tableName}Procedures
    {{
");

                    foreach (var procedure in procedures)
                    {
                        text.AppendLine($"        {procedure},");
                    }

                    text.AppendLine($@"
    }}
}}");
                    _fileService.Create(FolderNames.ProcedureEnums.ToString(), $"{tableName}Procedures.cs", text.ToString());
            }
        }

        public string CreateProcedureInsertManyFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string tvpName = _tableValuedParameterService.GetTVPName(tableSchema.TABLE_NAME);
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.InsertMany}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            Column primaryKey = _sqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME);
            string[] tableColumns = _sqlService.GetTableColumns(tableSchema.TABLE_NAME, false).Select(c=> $"\t\t{c.COLUMN_NAME}").ToArray();
            string[] tvpColumns = _sqlService.GetTableColumns(tableSchema.TABLE_NAME, false).Select(c => $"\t\ttvp.{c.COLUMN_NAME}").ToArray();

            string content = $@"
{alterOrCreate} PROCEDURE {procedureName}
    @TVP {tvpName} READONLY
AS
   INSERT INTO {tableSchema.TABLE_NAME}(
{string.Join($",\n", tableColumns)}
        )
   OUTPUT INSERTED.*
   SELECT 
{string.Join($",\n", tvpColumns)}
    FROM @TVP AS tvp
GO
";
            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", content);
            return content;
        }

        public string CreateProcedureUpdateManyFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string tvpName = _tableValuedParameterService.GetTVPName(tableSchema.TABLE_NAME);
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.UpdateMany}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            Column primaryKey = _sqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME);
            string[] setQueries = _sqlService.GetTableColumns(tableSchema.TABLE_NAME, false).Select(c => $"\t\ttbl.{c.COLUMN_NAME} = tvp.{c.COLUMN_NAME}").ToArray();

            string content = $@"
{alterOrCreate} PROCEDURE {procedureName}
    @TVP {tvpName} READONLY
AS
   UPDATE tbl
   SET
{string.Join($",\n", setQueries)}
    FROM {tableSchema.TABLE_NAME} AS tbl
    JOIN @TVP AS tvp ON tvp.{primaryKey.COLUMN_NAME} = tbl.{primaryKey.COLUMN_NAME}
    
    SELECT * FROM @TVP 
GO
";
            _fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", content);
            return content;
        }

        private string AlterOrCreate(List<string> currentProcedures, string procedureName)
        {
            return currentProcedures.Contains(procedureName) ? "ALTER" : "CREATE";
        }
    }
}
