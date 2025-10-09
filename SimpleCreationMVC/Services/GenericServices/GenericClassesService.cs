using System.Text;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.GenericServices
{
    public class GenericClassesService
    {
        private readonly FileService _fileService = new FileService();
        private readonly SqlService sqlService;
        private readonly string connectionString;
        private readonly string modifiedConnectionString;
        public GenericClassesService(string connectionString)
        {
            this.sqlService = new SqlService(connectionString);
            this.connectionString = connectionString;
            this.modifiedConnectionString = connectionString.Replace("\\", "\\\\");
        }
        public void CreateStoredProcedure()
        {
            string text = $@"
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using {FolderNames.Utilities}.{FolderNames.Classes};
using {FolderNames.Repositories}.{FolderNames.Interfaces};

namespace {FolderNames.Repositories}.{FolderNames.Classes}
{{
    public class GenericProcedure<TProcedures>
    {{
        public TProcedures? {ProcedureTypes.GetAllByFilters} {{ get; set; }} 
        public TProcedures? {ProcedureTypes.GetById} {{ get; set; }}
        public TProcedures? {ProcedureTypes.Insert} {{ get; set; }}
        public TProcedures? {ProcedureTypes.Update} {{ get; set; }}
        public TProcedures? {ProcedureTypes.DeleteById} {{ get; set; }}
        public TProcedures? {ProcedureTypes.BulkInsert} {{ get; set; }}
        public TProcedures? {ProcedureTypes.BulkUpdate} {{ get; set; }}
        public TProcedures? {ProcedureTypes.BulkDeleteNotInTVP} {{ get; set; }}
    }}

    public class GenericRepository<T, TProcedures> : IGenericRepository<T, TProcedures>
        where T : class
        where TProcedures : struct, Enum
    {{
        public readonly IDbConnection _connection;
        private readonly GenericProcedure<TProcedures> _procedures;
        public readonly int _commandTimeout = 120;
        private readonly DataTableUtility _dataTableUtility = new DataTableUtility();

        public GenericRepository(GenericProcedure<TProcedures> procedures)
        {{
            _procedures = procedures;
            _connection = new SqlConnection(""{modifiedConnectionString}"");
        }}

        private string EnsureProcedureName(TProcedures? procedure)
        {{
            if (procedure.ToString() == null || procedure?.ToString() == ""0""){{
                string name = typeof(T).Name;
                throw new InvalidOperationException($""Stored procedure for '{{name}}' is not defined."");
            }}
            return procedure.ToString() ?? """";
        }}

        public async Task<IEnumerable<T>> GetAllAsync(T? filter = null)
        {{
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.GetAllByFilters});
            return await _connection.QueryAsync<T>(proc, filter, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> GetByIdAsync(int id)
        {{
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.GetById});
            return await _connection.QueryFirstOrDefaultAsync<T>(proc, new {{ Id = id }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> InsertAsync(T entity)
        {{
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.Insert});
            return await _connection.QueryFirstOrDefaultAsync<T>(proc, entity, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> UpdateAsync(T entity)
        {{
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.Update});
            return await _connection.QueryFirstOrDefaultAsync<T>(proc, entity, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> DeleteByIdAsync(int id)
        {{
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.DeleteById});
            return await _connection.QueryFirstOrDefaultAsync<T>(proc, new {{ Id = id }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<IEnumerable<T>> BulkInsertAsync(List<T> data)
        {{
            if (data == null || data.Count == 0) throw new ArgumentException(""Data list cannot be null or empty."", nameof(data));
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.BulkInsert});
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(proc, new {{ TVP = dt.AsTableValuedParameter($""TVP_{{tableName}}"") }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<IEnumerable<T>> BulkUpdateAsync(List<T> data)
        {{
            if (data == null || data.Count == 0) throw new ArgumentException(""Data list cannot be null or empty."", nameof(data));
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.BulkUpdate});
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(proc, new {{ TVP = dt.AsTableValuedParameter($""TVP_{{tableName}}"") }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}


        public async Task<IEnumerable<T>> BulkUpsertAsync(List<T> items)
        {{
            var keyProperty = GetKeyProperty<T>();
        
            var insertList = items
                .Where(x => IsKeyDefaultValue(keyProperty.GetValue(x)))
                .ToList();
        
            var updateList = items
                .Where(x => !IsKeyDefaultValue(keyProperty.GetValue(x)))
                .ToList();
        
            var inserted = new List<T>();
            var updated = new List<T>();
        
            if (insertList.Any())
            {{
                var newInserted = await BulkInsertAsync(insertList);
                inserted = newInserted.ToList();
            }}
        
            if (updateList.Any())
            {{
                var newUpdated = await BulkUpdateAsync(updateList);
                updated = newUpdated.ToList();
            }}
        
            return inserted.Concat(updated);
        }}

        public async Task<IEnumerable<T>> BulkMergeAsync(List<T> data, object? filtersParams = null)
        {{
            if (data == null || data.Count == 0) throw new ArgumentException(""Data list cannot be null or empty."", nameof(data));
            DynamicParameters deleteParameters = BuildParametersWithTVP(data, additionalParams);
            await BulkDeleteNotInTVPAsync(data,filtersParams);
            await BulkUpsertAsync(data);
            var tableName = typeof(T).Name;
            return data;
        }}

        private async Task<IEnumerable<T>> BulkDeleteNotInTVPAsync(List<T> data, object? filtersParams = null)
        {{
            if (data == null || data.Count == 0) throw new ArgumentException(""Data list cannot be null or empty."", nameof(data));
            var proc = EnsureProcedureName(_procedures.{ProcedureTypes.BulkDeleteNotInTVP});
            DynamicParameters parameters = BuildParametersWithTVP(data, filtersParams);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(proc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        private DynamicParameters BuildParametersWithTVP<T>(List<T> data, object? additionalParams = null)
        {{
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
        
            var parameters = new DynamicParameters();
            parameters.Add(""TVP"", dt.AsTableValuedParameter($""TVP_{{tableName}}""));
        
            if (additionalParams != null)
            {{
                foreach (var prop in additionalParams.GetType().GetProperties())
                {{
                    parameters.Add(prop.Name, prop.GetValue(additionalParams));
                }}
            }}
        
            return parameters;
        }} 

        private static PropertyInfo GetKeyProperty<T>()
        {{
            var keyProperty = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
        
            if (keyProperty == null)
                throw new InvalidOperationException(
                    $""Type '{{typeof(T).Name}}' does not contain a property marked with [Key].""
                );
        
            return keyProperty;
        }}
    }}
}}";
            _fileService.Create(FolderPaths.RepositoriesClassesFolder, $"GenericRepository.cs", text);
        }
        public void CreateDapperQuery()
        {
            string text = $@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using {FolderNames.Utilities}.{FolderNames.Classes};
using {FolderNames.Repositories}.{FolderNames.Interfaces};

namespace {FolderNames.Repositories.ToString()}
{{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {{
        public IDbConnection _connection;

        private readonly string connectionString = ""{modifiedConnectionString}"";

        public GenericRepository()
        {{
            _connection = new SqlConnection(connectionString);
        }}

        public virtual async Task<T?> InsertAsync(T entity)
        {{
            string keyColumn = GetKeyColumnName();
            string tableName = GetTableName();
            string columns = GetColumns(excludeKey: true);
            string properties = GetPropertyNames(excludeKey: true);
            string query = $@""INSERT INTO {{tableName}} ({{columns}}) VALUES ({{properties}})
                              SELECT * FROM {{tableName}} WHERE {{keyColumn}} = SCOPE_IDENTITY()"";
            return await _connection.QueryFirstOrDefaultAsync<T>(query, entity);

        }}

        public virtual async Task<T?> DeleteByIdAsync(int id)
        {{
            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            var deletedData = await GetByIdAsync(id);
            string query = $""DELETE FROM {{tableName}} WHERE {{keyColumn}} = @Id;"";
        
            _connection.Execute(query, new {{ Id = id }});
            return deletedData;
        }}

        public virtual async Task<IEnumerable<T>> GetAllAsync(T? filter = null)
        {{
            string tableName = GetTableName();
            var properties = typeof(T).GetProperties();
        
            var whereConditions = new List<string>();
            var parameters = new DynamicParameters();
        
            foreach (var prop in properties)
            {{
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                var columnName = columnAttr != null ? columnAttr.Name : prop.Name;
                var value = filter == null ? null : prop.GetValue(filter);
        
                // Add filter only if value is not null
                if (value != null)
                {{
                    whereConditions.Add($""(@{{columnName}} IS NULL OR {{columnName}} = @{{columnName}})"");
                    parameters.Add($""@{{columnName}}"", value);
                }}
                else
                {{
                    // Still include it to match the stored procedure behavior
                    whereConditions.Add($""(@{{columnName}} IS NULL OR {{columnName}} = @{{columnName}})"");
                    parameters.Add($""@{{columnName}}"", null);
                }}
            }}
        
            string whereClause = whereConditions.Count > 0
                ? ""WHERE "" + string.Join("" AND "", whereConditions)
                : """";
        
            string query = $@""
        SELECT * 
        FROM {{tableName}}
        {{whereClause}}
        "";
        
            return await _connection.QueryAsync<T>(query, parameters);
        }}


        public virtual async Task<T?> GetByIdAsync(int Id)
        {{
            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string query = $""SELECT * FROM {{tableName}} WHERE {{keyColumn}} = '{{Id}}'"";
            return await _connection.QueryFirstOrDefaultAsync<T>(query);
        }}

        public virtual async Task<T?> UpdateAsync(T entity)
        {{

            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string keyProperty = GetKeyPropertyName();

            StringBuilder query = new StringBuilder();
            query.AppendLine($""UPDATE {{tableName}} SET "");

            var properties = GetProperties(true);
            for (int a = 0; a < properties.Count(); a++) {{
                string propertyName = properties[a].Name;
                string comma = a < properties.Count() - 1 ? "","" : """";
                query.AppendLine($""{{propertyName}} = @{{propertyName}} {{comma}} ""); 
            }}
            query.Remove(query.Length - 1, 1);

            query.AppendLine($""WHERE {{keyColumn}} = @{{keyProperty}} SELECT * FROM {{tableName}} WHERE {{keyColumn}} = @{{keyProperty}}"");

            return await _connection.QueryFirstOrDefaultAsync<T>(query.ToString(), entity);
        }}

        public virtual async Task<IEnumerable<T>> BulkInsertAsync(List<T> list)
        {{
            List<T> result = new List<T>();
            foreach (T item in list)
            {{
               T addedItem =  await InsertAsync(item);
               result.Add(addedItem);
            }}
            return result;
        }}

        public virtual async Task<IEnumerable<T>> BulkUpdateAsync(List<T> list)
        {{
            List<T> result = new List<T>();
            foreach (T item in list)
            {{
                T updatedItem = await UpdateAsync(item);
                result.Add(updatedItem);
            }}
            return result;
        }}

        public virtual async Task<IEnumerable<T>> BulkUpsertAsync(List<T> entities)
        {{
            List<T> result = new List<T>();
            foreach (var entity in entities)
            {{
                string keyProperty = GetKeyPropertyName();
                var keyValue = typeof(T).GetProperty(keyProperty)?.GetValue(entity);

                // If key is default or null, insert new record, otherwise update existing
                if (keyValue == null || Convert.ToInt32(keyValue) == 0)
                {{
                    T insertedItem = await InsertAsync(entity);
                    result.Add(insertedItem);
                }}
                else
                {{
                    T updatedItem = await UpdateAsync(entity);
                    result.Add(updatedItem);
                }}
            }}
            return result;
        }}

         public virtual async Task<IEnumerable<T>> BulkMergeAsync(List<T> entities  ,string? filterColumnName = null, object? filterValue = null)
        {{
            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string keyProperty = GetKeyPropertyName();

            var keepIds = entities
                            .Select(x => typeof(T).GetProperty(keyProperty)?.GetValue(x))
                            .Where(id => id != null && Convert.ToInt32(id) > 0)
                            .Cast<int>()
                            .ToList();

            //Bulk Delete
            await BulkDeleteAsync(entities, keepIds, filterColumnName, filterValue);

            //Bulk Insert and Update
            await BulkUpsertAsync(entities);

            return entities;
        }}

        public async Task<IEnumerable<T>> BulkDeleteAsync(IEnumerable<T> entities,List<int>? keepIds, string? filterColumnName = null, object? filterValue = null)
        {{

            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string keyProperty = GetKeyPropertyName();

            var deleteSql = new StringBuilder();
            deleteSql.AppendLine($""DELETE FROM {{tableName}}"");
            deleteSql.AppendLine($""WHERE {{keyColumn}} NOT IN @Ids"");
            
            if (!string.IsNullOrWhiteSpace(filterColumnName) && filterValue != null)
            {{
                deleteSql.AppendLine($""AND {{filterColumnName}} = @FilterValue"");
            }}

            await _connection.ExecuteAsync(deleteSql.ToString(), new
            {{
                Ids = keepIds,
                FilterValue = filterValue
            }});
            
            return entities;
        }}
        private string GetTableName()
        {{
            string tableName = """";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {{
                tableName = tableAttr.Name;
                return tableName;
            }}

            return type.Name;
        }}

        public static string GetKeyColumnName()
        {{
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {{
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {{
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {{
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }}
                    else
                    {{
                        return property.Name;
                    }}
                }}
            }}

            return null;
        }}


        private string GetColumns(bool excludeKey = false)
        {{
            var type = typeof(T);
            var columns = string.Join("", "", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {{
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }}));

            return columns;
        }}

        protected string GetPropertyNames(bool excludeKey = false)
        {{
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join("", "", properties.Select(p =>
            {{
                return $""@{{p.Name}}"";
            }}));

            return values;
        }}

        protected List<PropertyInfo> GetProperties(bool excludeKey = false)
        {{
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties.ToList();
        }}

        protected string GetKeyPropertyName()
        {{
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {{
                return properties.FirstOrDefault().Name;
            }}

            return null;
        }}
    }}
}}";
            _fileService.Create(FolderNames.Repositories.ToString(), "GenericRepository.cs", text);
        }
        public void CreateEFCoreContext()
        {
            StringBuilder dbSetText = new StringBuilder();
            var tables = sqlService.GetAllTableSchema();
            foreach (var table in tables)
            {
                dbSetText.AppendLine("\n\t\tpublic DbSet<" + table.TABLE_NAME + "> " + table.TABLE_NAME + " { get; set; }");
            }

            string text = $@"
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using {FolderNames.Models.ToString()};

namespace {FolderNames.ApplicationContexts.ToString()}
{{
    public class ApplicationContext : DbContext
    {{
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {{
            string connectionString = ""{modifiedConnectionString}"";
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }}
{dbSetText}
    }}
}}
";
            _fileService.Create(FolderNames.ApplicationContexts.ToString(), "ApplicationContext.cs", text);
        }
        public void CreateEFCore()
        {
            string text = $@"
using Microsoft.EntityFrameworkCore;
using {FolderNames.ApplicationContexts.ToString()};
using System.ComponentModel.DataAnnotations;
using EFCore.BulkExtensions;
using System.Reflection;
using System.Linq.Expressions;
using {FolderNames.Utilities}.{FolderNames.Classes};
using {FolderNames.Repositories}.{FolderNames.Interfaces};

namespace {FolderNames.Repositories.ToString()}
{{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {{
        private readonly ApplicationContext _context = new ApplicationContext();

        public virtual async Task<T?> InsertAsync(T entity)
        {{
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }}

        public virtual async Task<IEnumerable<T>> GetAllAsync(T? filter = null)
        {{
            IQueryable<T> query = _context.Set<T>();
        
            if (filter != null)
            {{
                var parameter = Expression.Parameter(typeof(T), ""x"");
                Expression? combined = null;
        
                foreach (var property in typeof(T).GetProperties())
                {{
                    var value = property.GetValue(filter);
                    var member = Expression.Property(parameter, property);
                    var constant = Expression.Constant(value, property.PropertyType);
        
                    // Build: (value == null || x.Property == value)
                    var isNullCheck = Expression.Equal(constant, Expression.Constant(null, property.PropertyType));
                    var equalsCheck = Expression.Equal(member, constant);
                    var condition = Expression.OrElse(isNullCheck, equalsCheck);
        
                    combined = combined == null ? condition : Expression.AndAlso(combined, condition);
                }}
        
                if (combined != null)
                {{
                    var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                    query = query.Where(lambda);
                }}
            }}
        
            return await query.ToListAsync();
        }}

        public virtual async Task<T?> GetByIdAsync(int id)
        {{
            return await _context.Set<T>().FindAsync(id);
        }}

        public virtual async Task<T?> UpdateAsync(T entity)
        {{
            var keyValue = GetKeyValueAsInt(entity);
            var retrievedEntity = await GetByIdAsync(keyValue.Value);
            var updateData = UpdateEntityProperties(retrievedEntity, entity);
            await _context.SaveChangesAsync();
            return updateData;
        }}

        public virtual async Task<T?> DeleteByIdAsync(int id)
        {{
            T? deletedData = await GetByIdAsync(id);
            _context.Set<T>().Remove(deletedData);
            await _context.SaveChangesAsync();
            return deletedData;
        }}

        public virtual async Task<IEnumerable<T>> BulkUpdateAsync(List<T> list)
        {{
            await _context.BulkUpdateAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }}

        public virtual async Task<IEnumerable<T>> BulkInsertAsync(List<T> list)
        {{
            await _context.BulkInsertAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }}

        public virtual async Task<IEnumerable<T>> BulkUpsertAsync(List<T> entities)
        {{
            var (entitiesToInsert, entitiesToUpdate) = SeparateEntities(entities);

            // Bulk Insert
            if (entitiesToInsert.Any())
            {{
                await _context.BulkInsertAsync(entitiesToInsert);
            }}

            // Bulk Update
            if (entitiesToUpdate.Any())
            {{
                await _context.BulkUpdateAsync(entitiesToUpdate);
            }}

            await _context.SaveChangesAsync();

            return entities;
        }}

        public virtual async Task<IEnumerable<T>> BulkMergeAsync(List<T> entities, Expression<Func<T, bool>>? deleteFilter = null)
        {{
            var keyProperty = GetKeyProperty();

            var keepIds = entities
                            .Select(x => keyProperty.GetValue(x))
                            .Where(id => id != null && Convert.ToInt32(id) > 0)
                            .Cast<int>()
                            .ToList();

            IQueryable<T> query = _context.Set<T>();

            if (deleteFilter != null)
            {{
                query = query.Where(deleteFilter);
            }}

            if (keepIds.Any())
            {{
                var entitiesToDelete = await query
                    .Where(x => !keepIds.Contains(EF.Property<int>(x, keyProperty.Name)))
                    .ToListAsync();

                if (entitiesToDelete.Any())
                {{
                    await _context.BulkDeleteAsync(entitiesToDelete);
                }}
            }}

            await BulkUpsertAsync(entities);

            return entities;
        }}

        private PropertyInfo GetKeyProperty()
        {{
            var keyProperty = typeof(T).GetProperties()
                                .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any());

            if (keyProperty == null)
                throw new InvalidOperationException($""No key property found for entity type {{typeof(T).Name}}"");

            return keyProperty;
        }}

        private (List<T> entitiesToInsert, List<T> entitiesToUpdate) SeparateEntities(List<T> entities)
        {{
            var entitiesToInsert = new List<T>();
            var entitiesToUpdate = new List<T>();

            var keyProperty = GetKeyProperty();

            foreach (var entity in entities)
            {{
                var keyValue = keyProperty.GetValue(entity);
                bool isDefaultKey = keyValue == null ||
                                   (keyValue is int intValue && intValue == 0) ||
                                   (keyValue is long longValue && longValue == 0);

                if (isDefaultKey)
                {{
                    entitiesToInsert.Add(entity);
                }}
                else
                {{
                    entitiesToUpdate.Add(entity);
                }}
            }}
            return (entitiesToInsert, entitiesToUpdate);
        }}

        private int? GetKeyValueAsInt(T entity)
        {{
            var entityType = typeof(T);
            var keyProperty = GetKeyProperty();

            if (keyProperty == null)
            {{
                throw new InvalidOperationException(""No Key attribute found on properties."");
            }}

            var keyValue = keyProperty.GetValue(entity);

            if (keyValue is int intValue)
            {{
                return intValue;
            }}

            throw new InvalidOperationException(""Key value is not of type int."");
        }}


        private T UpdateEntityProperties(T oldEntity, T newEntity)
        {{
            var entityType = typeof(T);

            foreach (var property in entityType.GetProperties())
            {{
                if (property.CanWrite && !Attribute.IsDefined(property, typeof(KeyAttribute)))
                {{
                    var newValue = property.GetValue(newEntity);
                    property.SetValue(oldEntity, newValue);
                }}
            }}
            return newEntity;

        }}

    }}
}}";
            _fileService.Create(FolderNames.Repositories.ToString(), "GenericRepository.cs", text);
        }
    }
}
