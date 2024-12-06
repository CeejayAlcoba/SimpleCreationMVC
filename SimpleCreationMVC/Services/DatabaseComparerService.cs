using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace SimpleCreationMVC.Services
{
    public class DatabaseComparerService
    {
        public async Task<IEnumerable<TableInfo>> GetTablesAsync(IDbConnection connection)
        {
            string query = @"
    SELECT 
        c.TABLE_NAME AS TableName, 
        c.COLUMN_NAME AS ColumnName, 
        c.DATA_TYPE AS DataType,
        CASE 
            WHEN kcu.COLUMN_NAME IS NOT NULL THEN 1 
            ELSE 0 
        END AS IsPrimaryKey
    FROM INFORMATION_SCHEMA.COLUMNS c
    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
        ON c.TABLE_NAME = kcu.TABLE_NAME
        AND c.COLUMN_NAME = kcu.COLUMN_NAME
        AND kcu.CONSTRAINT_NAME LIKE 'PK_%'";
            return await connection.QueryAsync<TableInfo>(query);
        }

        public async Task<Dictionary<string, string>> GetStoredProceduresWithContentAsync(IDbConnection connection)
        {
            string query = @"
        SELECT 
            ROUTINE_NAME AS Name, 
            OBJECT_DEFINITION(OBJECT_ID(ROUTINE_NAME)) AS Content
        FROM INFORMATION_SCHEMA.ROUTINES
        WHERE ROUTINE_TYPE = 'PROCEDURE'";
            var result = await connection.QueryAsync<ProcedureInfo>(query);
            return result.ToDictionary(p => p.Name, p => p.Content);
        }

        public IEnumerable<string> CompareTables(IEnumerable<TableInfo> first, IEnumerable<TableInfo> second)
        {
            var queries = new List<string>();
            var secondTableDict = second.GroupBy(t => t.TableName).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var table in first.GroupBy(t => t.TableName))
            {
                if (!secondTableDict.ContainsKey(table.Key))
                {
                    var columns = table.Select(c =>
                        c.IsPrimaryKey
                            ? $"{c.ColumnName} {MapDataType(c.DataType)} PRIMARY KEY IDENTITY(1,1)"
                            : $"{c.ColumnName} {MapDataType(c.DataType)} NULL").ToList();

                    var columnDefinitions = string.Join(", ", columns);
                    queries.Add($"CREATE TABLE {table.Key} ({columnDefinitions});");
                    continue;
                }

                var firstColumns = table.ToDictionary(t => t.ColumnName);
                var secondColumns = secondTableDict[table.Key].ToDictionary(t => t.ColumnName);

                foreach (var column in firstColumns)
                {
                    if (!secondColumns.ContainsKey(column.Key))
                    {
                        queries.Add(column.Value.IsPrimaryKey
                            ? $"ALTER TABLE {table.Key} ADD {column.Key} {MapDataType(column.Value.DataType)} PRIMARY KEY IDENTITY(1,1);"
                            : $"ALTER TABLE {table.Key} ADD {column.Key} {MapDataType(column.Value.DataType)} NULL;");
                    }
                }

                foreach (var column in secondColumns)
                {
                    if (!firstColumns.ContainsKey(column.Key))
                    {
                        queries.Add($"ALTER TABLE {table.Key} DROP COLUMN {column.Key};");
                    }
                }
            }

            foreach (var table in secondTableDict)
            {
                if (!first.Any(t => t.TableName == table.Key))
                {
                    queries.Add($"DROP TABLE {table.Key};");
                }
            }

            return queries;
        }

        public IEnumerable<string> CompareStoredProcedures(
       Dictionary<string, string> firstProcedures,
       Dictionary<string, string> secondProcedures)
        {
            var queries = new List<string>();

            // Procedures missing in the second database (CREATE PROCEDURE)
            var missingProcedures = firstProcedures.Keys.Except(secondProcedures.Keys);
            foreach (var procedure in missingProcedures)
            {
                queries.Add($"{firstProcedures[procedure]} GO");
            }

            // Procedures with the same name but different content (ALTER PROCEDURE)
            var commonProcedures = firstProcedures.Keys.Intersect(secondProcedures.Keys);
            foreach (var procedure in commonProcedures)
            {
                if (!string.Equals(firstProcedures[procedure], secondProcedures[procedure], StringComparison.OrdinalIgnoreCase))
                {
                    var newProcedure = Regex.Replace(firstProcedures[procedure], @"\bCREATE \b", "ALTER ", RegexOptions.IgnoreCase);
                    queries.Add($"{newProcedure} GO");
                }
            }

            // Procedures missing in the first database (DROP PROCEDURE)
            var extraProcedures = secondProcedures.Keys.Except(firstProcedures.Keys);
            foreach (var procedure in extraProcedures)
            {
                queries.Add($"DROP PROCEDURE {procedure};");
            }

            return queries;
        }



        private static string MapDataType(string dataType)
        {
            return dataType switch
            {
                "nvarchar" => "NVARCHAR(MAX)",
                "varchar" => "VARCHAR(MAX)",
                "int" => "INT",
                "bit" => "BIT",
                "datetime" => "DATETIME",
                _ => dataType
            };
        }

        public class TableInfo
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public bool IsPrimaryKey { get; set; }
        }
        public class ProcedureInfo
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }
    }
}
