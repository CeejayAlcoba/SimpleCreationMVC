using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleCreation.Services
{
    public class StoredProcedureService
    {
        private readonly SqlService SqlService;
        private readonly FileService fileService = new FileService();
        public StoredProcedureService(string connectionString) { 
        
        this.SqlService = new SqlService(connectionString);
        }
        public void CreateStoredProceduresFiles(List<TableSchema> tableSchemas)
        {
            var currentProcedures = SqlService.GetAllCurentStoredProcesures();
            StringBuilder allSp = new StringBuilder();
            foreach (var tableSchema in tableSchemas)
            {
                var tableName = tableSchema.TABLE_NAME;

                if (tableSchema.isGetAllProcedureAllowed == true)
                {
                    string text = CreateProcedureGetAllFile(tableSchema, currentProcedures);
                    allSp.AppendLine(text);
                }
                if (tableSchema.isInsertProcedureAllowed == true)
                {
                    string text = CreateProcedureInsertFile(tableSchema, currentProcedures);
                    allSp.AppendLine(text);
                }
                if(tableSchema.isUpdateProcedureAllowed == true)
                {
                    string text = CreateProcedureUpdateFile(tableSchema, currentProcedures);
                    allSp.AppendLine(text);
                }
                if(tableSchema.isGetByIdProcedureAllowed == true)
                {
                    string text = CreateProcedureGetByIdFile(tableSchema, currentProcedures);
                    allSp.AppendLine(text);
                }
                if (tableSchema.isDeleteByIdProcedureAllowed == true)
                {
                    string text = CreateProcedureDeleteByIdFile(tableSchema, currentProcedures);
                    allSp.AppendLine(text);
                }
            }
            fileService.Create(FolderNames.ProcedureQueries.ToString(), $"All.sql", allSp.ToString());
        }
        private string CreateProcedureGetByIdFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.GetById.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            string primaryKey = SqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
            StringBuilder text = new StringBuilder($@"
{alterOrCreate} PROCEDURE {procedureName}
@Id INT
AS
   SELECT * FROM {tableSchema.TABLE_NAME}
   WHERE {primaryKey} = @Id
GO
            ");
            fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        private string CreateProcedureUpdateFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.Update.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            string primaryKey = SqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
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
            string parameters = SqlService.GetColumnParameterSP(tableSchema.Columns);
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
            fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

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

            fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        private string CreateProcedureDeleteByIdFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.DeleteById.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);

            var primaryKey = SqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
            StringBuilder text = new StringBuilder($@"
{alterOrCreate} PROCEDURE {procedureName}
    @Id INT
AS
    
    DELETE FROM {tableSchema.TABLE_NAME}
    WHERE {primaryKey} =  @Id
GO
            ");

            fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        private string CreateProcedureInsertFile(TableSchema tableSchema,List<string> currentProcedures )
        {
            var primaryKey = SqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME);
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
            string parameters = SqlService.GetColumnParameterSP(filteredColumns);
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

            fileService.Create(FolderNames.ProcedureQueries.ToString(), $"{procedureName}.sql", text.ToString());

            return text.ToString();
        }
        public void CreateEnumProceduresFile()
        {
            var tables = SqlService.GetAllTableSchema();
            foreach(var table  in tables)
            {
                var tableName = table.TABLE_NAME;
                var procedures = SqlService.GetStoredProceduresByTable(tableName);

               
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
                    fileService.Create(FolderNames.ProcedureEnums.ToString(), $"{tableName}Procedures.cs", text.ToString());
            }
        }


        private string AlterOrCreate(List<string> currentProcedures, string procedureName)
        {
            return currentProcedures.Contains(procedureName) ? "ALTER" : "CREATE";
        }
    }
}
