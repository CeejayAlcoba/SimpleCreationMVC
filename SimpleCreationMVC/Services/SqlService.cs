using Dapper;
using Microsoft.Data.SqlClient;
using SimpleCreation.Models;
using System.Text;

namespace SimpleCreation.Services
{
    public class SqlService
    {
        private readonly string connectionString = "";
        private readonly SqlConnection connection;
        public SqlService(string connectionString) {
            this.connectionString = connectionString;
            this.connection = new SqlConnection(connectionString);
        }

        public List<TableSchema> GetAllTableSchema()
        {
            List<TableSchema> tableSchemas = new List<TableSchema>();
            string tablesQuery = $@"SELECT TABLE_NAME
                             FROM INFORMATION_SCHEMA.TABLES
                             ORDER BY TABLE_NAME";
          
            var tables =  connection.Query<string>(tablesQuery);


            foreach (var table in tables)
            {
                string columnsQuery = $@"select *
                             from INFORMATION_SCHEMA.COLUMNS
                            where TABLE_NAME='{table}'";
                tableSchemas.Add(new TableSchema
                {
                    TABLE_NAME = table,
                    Columns =  connection.Query<Column>(columnsQuery).ToList(),
                });
            }

            return tableSchemas;
        }

        public List<string> GetAllCurentStoredProcesures()
        {
            string query = $@"SELECT SPECIFIC_NAME 
                             FROM INFORMATION_SCHEMA.ROUTINES
                            WHERE ROUTINE_TYPE = 'PROCEDURE'";
            return connection.Query<string>(query).OrderBy(_ => _).ToList();
        }
        public List<string> GetStoredProceduresByTable(string tableName)
        {
            string query = @"
        SELECT SPECIFIC_NAME
        FROM INFORMATION_SCHEMA.ROUTINES
        WHERE ROUTINE_TYPE = 'PROCEDURE'
        AND SPECIFIC_NAME LIKE @TableName + '%';"; 

            var storedProcedures = connection.Query<string>(query, new { TableName = tableName }).ToList();

            return storedProcedures.OrderBy(sp => sp).ToList();
        }
        public string GetColumnParameterSP(List<Column> columns)
        {
            StringBuilder columnParameter = new StringBuilder();
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var stringLenghtText = "";
                if (column.CHARACTER_MAXIMUM_LENGTH == -1 && column.DATA_TYPE.Contains("char")) stringLenghtText = "(MAX)";
                if (column.CHARACTER_MAXIMUM_LENGTH > 0 && column.DATA_TYPE.Contains("char")) stringLenghtText = $"({column.CHARACTER_MAXIMUM_LENGTH})";

                columnParameter.Append($"\t@{column.COLUMN_NAME} {column.DATA_TYPE}{stringLenghtText} ");
                if(column.IS_NULLABLE == "YES")
                {
                    columnParameter.Append(" = NULL");
                }
                if (i < columns.Count-1)
                {
                    columnParameter.Append($",\n");
                }
            }
            return columnParameter.ToString();
        }
        public Column GetTablePrimaryKey(string tableName)
        {
            string query = $@"SELECT DISTINCT CCU.COLUMN_NAME, C.DATA_TYPE, C.IS_NULLABLE FROM
                               INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                               JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU
                               ON CCU.CONSTRAINT_NAME=TC.CONSTRAINT_NAME
                               JOIN INFORMATION_SCHEMA.COLUMNS C
                               ON C.COLUMN_NAME = CCU.COLUMN_NAME
                               WHERE
                               CCU.TABLE_NAME='{tableName}'
                               AND TC.CONSTRAINT_TYPE='PRIMARY KEY'";

            return connection.QueryFirstOrDefault<Column>(query)?? new Column();
        }

    }
}
