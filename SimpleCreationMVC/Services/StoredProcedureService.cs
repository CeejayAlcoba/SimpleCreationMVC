using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SimpleCreation.Services
{
    public class StoredProcedureService
    {
        private readonly SqlService SqlService;
        private readonly FileService fileService = new FileService();
        public StoredProcedureService(string connectionString) { 
        
        this.SqlService = new SqlService(connectionString);
        }
        public void CreateStoredProceduresFiles(List<TableSchema> tableSchemas = null)
        {
            if(tableSchemas.IsNullOrEmpty()) tableSchemas = SqlService.GetAllTableSchema();
            var currentProcedures = SqlService.GetAllCurentStoredProcesures();

            foreach (var tableSchema in tableSchemas)
            {
                var tableName = tableSchema.TABLE_NAME;
                if (tableSchema.isGetAllProcedureAllowed == true)
                    CreateProcedureGetAllFile(tableSchema, currentProcedures);
                if (tableSchema.isInsertProcedureAllowed == true)
                    CreateProcedureInsertFile(tableSchema, currentProcedures);
                if(tableSchema.isUpdateProcedureAllowed == true)
                    CreateProcedureUpdateFile(tableSchema, currentProcedures);
                if(tableSchema.isGetByIdProcedureAllowed == true)
                    CreateProcedureGetByIdFile(tableSchema, currentProcedures);
                if (tableSchema.isHardDeleteByIdProcedureAllowed == true)
                    CreateProcedureHardDeleteByIdFile(tableSchema, currentProcedures);
            }
        }
        private void CreateProcedureGetByIdFile(TableSchema tableSchema, List<string> currentProcedures)
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
            fileService.Create(FolderNames.StoredProcedures.ToString(), $"{procedureName}.sql", text.ToString());
        }
        private void CreateProcedureUpdateFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.Update.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            string primaryKey = SqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
            StringBuilder setValues = new StringBuilder();

            for (int i = 0; i < tableSchema.Columns.Count; i++)
            {
                var column = tableSchema.Columns[i];
                if (primaryKey == column.COLUMN_NAME) continue;

                setValues.Append($"{column.COLUMN_NAME}=@{column.COLUMN_NAME}");
                if (i < tableSchema.Columns.Count - 1)
                {
                    setValues.Append($",");
                }
            }
            string parameters = SqlService.GetColumnParameterSP(tableSchema.Columns);
            StringBuilder text = new StringBuilder($@"
             {alterOrCreate} PROCEDURE {procedureName}
             {parameters}
             AS
                UPDATE {tableSchema.TABLE_NAME}
                SET {setValues}
                WHERE {primaryKey} = @{primaryKey}
                SELECT * FROM {tableSchema.TABLE_NAME} WHERE {primaryKey} = @Id
             GO
            ");
            fileService.Create(FolderNames.StoredProcedures.ToString(), $"{procedureName}.sql", text.ToString());
        }
        private void CreateProcedureGetAllFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.GetAll.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);

            StringBuilder text = new StringBuilder($@"
             {alterOrCreate} PROCEDURE {procedureName}
             AS
                SELECT * FROM {tableSchema.TABLE_NAME}
             GO
            ");

            fileService.Create(FolderNames.StoredProcedures.ToString(), $"{procedureName}.sql", text.ToString());

        }
        private void CreateProcedureHardDeleteByIdFile(TableSchema tableSchema, List<string> currentProcedures)
        {
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.HardDeleteById.ToString()}";
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

            fileService.Create(FolderNames.StoredProcedures.ToString(), $"{procedureName}.sql", text.ToString());
        }
        private void CreateProcedureInsertFile(TableSchema tableSchema,List<string> currentProcedures )
        {
            var primaryKey = SqlService.GetTablePrimaryKey(tableSchema.TABLE_NAME).COLUMN_NAME;
            string procedureName = $"{tableSchema.TABLE_NAME}_{ProcedureTypes.Insert.ToString()}";
            string alterOrCreate = AlterOrCreate(currentProcedures, procedureName);
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            for (int i = 0; i < tableSchema.Columns.Count; i++)
            {
                var column = tableSchema.Columns[i];
                if (primaryKey == column.COLUMN_NAME) continue;

                columns.Append($"{column.COLUMN_NAME}");
                if (i < tableSchema.Columns.Count - 1)
                {
                    columns.Append($",");
                }
            }

            for (int i = 0; i < tableSchema.Columns.Count; i++)
            {
                var column = tableSchema.Columns[i];
                if (primaryKey == column.COLUMN_NAME) continue;

                values.Append($"@{column.COLUMN_NAME}");
                if (i < tableSchema.Columns.Count - 1)
                {
                    values.Append($",");
                }
            }

            string parameters = SqlService.GetColumnParameterSP(tableSchema.Columns);
            StringBuilder text = new StringBuilder($@"
             {alterOrCreate} PROCEDURE {procedureName}
             {parameters}
             AS
                INSERT INTO {tableSchema.TABLE_NAME}({columns})
                VALUES ({values})
                SELECT * FROM {tableSchema.TABLE_NAME} WHERE {primaryKey} = SCOPE_IDENTITY()
             GO
            ");

            fileService.Create(FolderNames.StoredProcedures.ToString(), $"{procedureName}.sql", text.ToString());
        }
        public void CreateEnumProcedureFile()
        {
            var procedures = SqlService.GetAllCurentStoredProcesures();
            StringBuilder text = new StringBuilder();
            StringBuilder procedureText = new StringBuilder();
            var tables = new List<string>();
            foreach (var procedure in procedures)
            {
                var tableName = procedure.Split("_")[0];
                if (!tables.Contains(tableName))
                {
                    procedureText.AppendLine($"\n\t\t//---{tableName}---//");
                }
                procedureText.AppendLine($"\t\t{procedure},");

                tables.Add(tableName);
            }

       
            text.AppendLine($"namespace Project.{FolderNames.Enums.ToString()}");
            text.AppendLine("{");
            text.AppendLine($"    public enum StoredProcedures");
            text.AppendLine("     {");
            text.AppendLine($"{procedureText}");
            text.AppendLine("     }");
            text.AppendLine("}");

            fileService.Create(FolderNames.Enums.ToString(), "StoredProcedures.cs", text.ToString());
        }
        private string AlterOrCreate(List<string> currentProcedures, string procedureName)
        {
            return currentProcedures.Contains(procedureName) ? "ALTER" : "CREATE";
        }
    }
}
