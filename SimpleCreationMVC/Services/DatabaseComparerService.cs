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
            const string query = @"
SELECT 
    c.TABLE_NAME AS TableName,
    c.COLUMN_NAME AS ColumnName,
    c.DATA_TYPE AS DataType,
    c.CHARACTER_MAXIMUM_LENGTH AS CharacterMaximumLength,
    c.COLUMN_DEFAULT AS ColumnDefault,
    CASE WHEN c.IS_NULLABLE = 'YES' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsNullable,
    CASE WHEN pk.COLUMN_NAME IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsPrimaryKey,
    CASE WHEN fk.CONSTRAINT_NAME IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsForeignKey,
    fk.CONSTRAINT_NAME AS ForeignKeyName,
    fk.ReferencedTable,
    fk.ReferencedColumn
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN (
    -- Primary keys
    SELECT ku.TABLE_NAME, ku.COLUMN_NAME
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku
        ON tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
    WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
) pk
    ON c.TABLE_NAME = pk.TABLE_NAME AND c.COLUMN_NAME = pk.COLUMN_NAME
LEFT JOIN (
    -- Foreign keys
    SELECT 
        rc.CONSTRAINT_NAME,
        kcu.TABLE_NAME,
        kcu.COLUMN_NAME,
        kcu2.TABLE_NAME AS ReferencedTable,
        kcu2.COLUMN_NAME AS ReferencedColumn
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
        ON rc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu2
        ON rc.UNIQUE_CONSTRAINT_NAME = kcu2.CONSTRAINT_NAME
) fk
    ON c.TABLE_NAME = fk.TABLE_NAME AND c.COLUMN_NAME = fk.COLUMN_NAME
ORDER BY c.TABLE_NAME, c.ORDINAL_POSITION;
";
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
            var secondTableDict = second
                .GroupBy(t => t.TableName)
                .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

            var firstTableGroups = first.GroupBy(t => t.TableName);

            foreach (var tableGroup in firstTableGroups)
            {
                var tableName = tableGroup.Key;
                var firstColumns = tableGroup.ToDictionary(c => c.ColumnName, StringComparer.OrdinalIgnoreCase);

                if (!secondTableDict.ContainsKey(tableName))
                {
                    queries.AddRange(CreateTableSql(tableName, tableGroup));
                    continue;
                }

                var secondColumns = secondTableDict[tableName]
                    .ToDictionary(c => c.ColumnName, StringComparer.OrdinalIgnoreCase);

                queries.AddRange(AddNewColumnsSql(tableName, firstColumns, secondColumns));
                queries.AddRange(DropRemovedColumnsSql(tableName, firstColumns, secondColumns));
                queries.AddRange(AlterChangedColumnsSql(tableName, firstColumns, secondColumns));
            }

            queries.AddRange(DropRemovedTablesSql(first, secondTableDict));

            return queries;
        }

        #region Private Helpers

        private IEnumerable<string> CreateTableSql(string tableName, IEnumerable<TableInfo> columns)
        {
            var columnDefs = columns.Select(c =>
            {
                var dataType = MapDataType(c.DataType, c.CharacterMaximumLength);
                var nullable = c.IsNullable ? "NULL" : "NOT NULL";
                var def = !string.IsNullOrEmpty(c.ColumnDefault) ? $"DEFAULT {c.ColumnDefault}" : "";
                var pk = c.IsPrimaryKey ? "PRIMARY KEY" : "";
                return $"[{c.ColumnName}] {dataType} {nullable} {def} {pk}".Trim();
            });

            var tableSql = new List<string>
    {
        @$"CREATE TABLE [{tableName}] (
        {string.Join(",\n\t", columnDefs)}
);"
    };

            // Add FK constraints as separate ALTER TABLE statements
            foreach (var col in columns.Where(c => !string.IsNullOrEmpty(c.ReferencedTable) && !string.IsNullOrEmpty(c.ReferencedColumn)))
            {
                tableSql.Add(AddColumnWithForeignKeySql(tableName, col));
            }

            return tableSql;
        }

        private IEnumerable<string> AddNewColumnsSql(string tableName, Dictionary<string, TableInfo> firstColumns, Dictionary<string, TableInfo> secondColumns)
        {
            var queries = new List<string>();

            foreach (var col in firstColumns.Values)
            {
                if (!secondColumns.ContainsKey(col.ColumnName))
                {
                    queries.Add(AddColumnWithForeignKeySql(tableName, col));
                }
            }

            return queries;
        }

        private string AddColumnWithForeignKeySql(string tableName, TableInfo col)
        {
            var dataType = MapDataType(col.DataType, col.CharacterMaximumLength);
            var nullable = col.IsNullable ? "NULL" : "NOT NULL";
            var def = !string.IsNullOrEmpty(col.ColumnDefault) ? $"DEFAULT {col.ColumnDefault}" : "";

            var statements = new List<string>
    {
        // Add column first
        $"ALTER TABLE [{tableName}] ADD [{col.ColumnName}] {dataType} {nullable} {def};"
    };

            // Add foreign key constraint if defined
            if (!string.IsNullOrEmpty(col.ReferencedTable) && !string.IsNullOrEmpty(col.ReferencedColumn))
            {
                var fkName = !string.IsNullOrEmpty(col.ForeignKeyName)
                    ? col.ForeignKeyName
                    : $"FK_{tableName}_{col.ColumnName}";

                statements.Add(
                    $"ALTER TABLE [{tableName}] ADD CONSTRAINT [{fkName}] FOREIGN KEY ([{col.ColumnName}]) REFERENCES [{col.ReferencedTable}]([{col.ReferencedColumn}]);"
                );
            }

            return string.Join("\n", statements);
        }

        private IEnumerable<string> DropRemovedColumnsSql(string tableName, Dictionary<string, TableInfo> firstColumns, Dictionary<string, TableInfo> secondColumns)
        {
            var queries = new List<string>();
            foreach (var col in secondColumns.Values)
            {
                if (!firstColumns.ContainsKey(col.ColumnName))
                {
                    queries.Add($"ALTER TABLE [{tableName}] DROP COLUMN [{col.ColumnName}];");
                }
            }
            return queries;
        }

        private IEnumerable<string> AlterChangedColumnsSql(string tableName, Dictionary<string, TableInfo> firstColumns, Dictionary<string, TableInfo> secondColumns)
        {
            var queries = new List<string>();
            foreach (var col in firstColumns.Values)
            {
                if (secondColumns.TryGetValue(col.ColumnName, out var oldCol))
                {
                    var newType = MapDataType(col.DataType, col.CharacterMaximumLength);
                    var oldType = MapDataType(oldCol.DataType, oldCol.CharacterMaximumLength);

                    var newDefault = col.ColumnDefault?.Trim('(', ')', ' ') ?? "";
                    var oldDefault = oldCol.ColumnDefault?.Trim('(', ')', ' ') ?? "";

                    bool isChanged =
                        !string.Equals(newType, oldType, StringComparison.OrdinalIgnoreCase) ||
                        col.IsNullable != oldCol.IsNullable ||
                        !string.Equals(newDefault, oldDefault, StringComparison.OrdinalIgnoreCase);

                    if (isChanged)
                    {
                        var nullable = col.IsNullable ? "NULL" : "NOT NULL";
                        var def = !string.IsNullOrEmpty(col.ColumnDefault) ? $"DEFAULT {col.ColumnDefault}" : "";
                        queries.Add($"ALTER TABLE [{tableName}] ALTER COLUMN [{col.ColumnName}] {newType} {nullable} {def};");
                    }
                }
            }
            return queries;
        }

        private IEnumerable<string> DropRemovedTablesSql(IEnumerable<TableInfo> first, Dictionary<string, List<TableInfo>> secondTableDict)
        {
            var queries = new List<string>();
            foreach (var tableName in secondTableDict.Keys)
            {
                if (!first.Any(t => string.Equals(t.TableName, tableName, StringComparison.OrdinalIgnoreCase)))
                {
                    queries.Add($"DROP TABLE [{tableName}];");
                }
            }
            return queries;
        }

        private string NormalizeSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return string.Empty;

            sql = Regex.Replace(sql, @"\s+", " ");

            sql = sql.Trim();

            return sql;
        }

        #endregion

        public IEnumerable<string> CompareStoredProcedures(
       Dictionary<string, string> firstProcedures,
       Dictionary<string, string> secondProcedures)
        {
            var queries = new List<string>();

            //CREATE PROCEDURE
            var missingProcedures = firstProcedures.Keys.Except(secondProcedures.Keys);
            foreach (var procedure in missingProcedures)
            {
                queries.Add($"{firstProcedures[procedure]} \nGO");
            }

            //ALTER PROCEDURE
            var commonProcedures = firstProcedures.Keys.Intersect(secondProcedures.Keys);
            foreach (var procedure in commonProcedures)
            {
                var firstNormalized = NormalizeSql(firstProcedures[procedure]);
                var secondNormalized = NormalizeSql(secondProcedures[procedure]);

                if (!string.Equals(firstNormalized, secondNormalized, StringComparison.OrdinalIgnoreCase))
                {
                    var newProcedure = Regex.Replace(firstProcedures[procedure], @"\bCREATE\s+PROCEDURE\b", "ALTER PROCEDURE", RegexOptions.IgnoreCase);
                    queries.Add($"{newProcedure} \nGO");
                }
            }

            //DROP PROCEDURE
            var extraProcedures = secondProcedures.Keys.Except(firstProcedures.Keys);
            foreach (var procedure in extraProcedures)
            {
                queries.Add($"DROP PROCEDURE {procedure};");
            }

            return queries;
        }

        private string MapDataType(string dataType, int? length)
        {
            if (length.HasValue)
            {
                if (dataType.Equals("nvarchar", StringComparison.OrdinalIgnoreCase) ||
                    dataType.Equals("varchar", StringComparison.OrdinalIgnoreCase) ||
                    dataType.Equals("char", StringComparison.OrdinalIgnoreCase) ||
                    dataType.Equals("nchar", StringComparison.OrdinalIgnoreCase))
                {
                    return $"{dataType}({(length == -1 ? "MAX" : length.ToString())})";
                }
            }
            return dataType;
        }
        public class TableInfo
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public int? CharacterMaximumLength { get; set; }
            public string ColumnDefault { get; set; }
            public bool IsNullable { get; set; }
            public bool IsPrimaryKey { get; set; }
            public string ForeignKeyName { get; set; }
            public string ReferencedTable { get; set; }
            public string ReferencedColumn { get; set; }
        }

        public class ProcedureInfo
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }
    }
}
