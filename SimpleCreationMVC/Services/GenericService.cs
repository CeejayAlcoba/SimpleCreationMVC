using SimpleCreation.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Reflection;
using System.Text;
using static Dapper.SqlMapper;

namespace SimpleCreation.Services
{
    public class GenericService
    {
        private readonly FileService fileService = new FileService();
        private readonly SqlService sqlService;
        private readonly string connectionString;
        private readonly string modifiedConnectionString;
        public GenericService(string connectionString)
        {
            this.sqlService = new SqlService(connectionString);
            this.connectionString = connectionString;
            this.modifiedConnectionString = connectionString.Replace("\\", "\\\\"); ;
        }
        public void CreateProcedureGeneric()
        {

            string text = $@"
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Project.{FolderNames.Utilities};

namespace Project.{FolderNames.Repositories}
{{
    public class GenericProcedure<TProcedures>
    {{
        public TProcedures? {ProcedureTypes.GetAll} {{ get; set; }} 
        public TProcedures? {ProcedureTypes.GetById} {{ get; set; }}
        public TProcedures? {ProcedureTypes.Insert} {{ get; set; }}
        public TProcedures? {ProcedureTypes.Update} {{ get; set; }}
        public TProcedures? {ProcedureTypes.DeleteById} {{ get; set; }}
        public TProcedures? {ProcedureTypes.BulkInsert} {{ get; set; }}
        public TProcedures? {ProcedureTypes.BulkUpdate} {{ get; set; }}
        public TProcedures? {ProcedureTypes.BulkUpsert} {{ get; set; }}
    }}

     public class GenericRepository<T, TProcedures>
        where T : class
        where TProcedures : struct, Enum
    {{
        public readonly IDbConnection _connection;
        private readonly GenericProcedure<TProcedures> _procedures;
        public readonly int _commandTimeout = 120;
        public readonly DataTableUtility _dataTableUtility = new DataTableUtility();

        public GenericRepository(GenericProcedure<TProcedures> procedures)
        {{
            _procedures = procedures;
            _connection = new SqlConnection(""{modifiedConnectionString}"");
        }}

        public async Task<IEnumerable<T>> GetAllAsync()
        {{
            return await _connection.QueryAsync<T>(_procedures.{ProcedureTypes.GetAll}.ToString(), commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> GetByIdAsync(int id)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.{ProcedureTypes.GetById}.ToString(), new {{ Id = id }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> InsertAsync(T entity)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.{ProcedureTypes.Insert}.ToString(), entity, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> UpdateAsync(T entity)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.{ProcedureTypes.Update}.ToString(), entity, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<T?> DeleteByIdAsync(int id)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.{ProcedureTypes.DeleteById}.ToString(), new {{ Id = id }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<IEnumerable<T>> BulkInsertAsync( List<T> data)
        {{
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(_procedures.{ProcedureTypes.BulkInsert}.ToString(), new {{ TVP = dt.AsTableValuedParameter($""TVP_{{tableName}}"") }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<IEnumerable<T>> BulkUpdateAsync( List<T> data)
        {{
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(_procedures.{ProcedureTypes.BulkUpdate}.ToString(), new {{ TVP = dt.AsTableValuedParameter($""TVP_{{tableName}}"") }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        public async Task<IEnumerable<T>> BulkUpsertAsync( List<T> data)
        {{
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(_procedures.{ProcedureTypes.BulkUpsert}.ToString(), new {{ TVP = dt.AsTableValuedParameter($""TVP_{{tableName}}"") }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}
    }}
}}";
            fileService.Create(FolderNames.Repositories.ToString(), $"GenericRepository.cs", text);
            
        }
        public void CreateDapperQueryGeneric()
        {
            string text = @"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Project." + FolderNames.Repositories.ToString() + @"
{
    public class GenericRepository<T> where T : class
    {
        public IDbConnection _connection;

        private readonly string connectionString = """ + modifiedConnectionString + @""";

        public GenericRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            string keyColumn = GetKeyColumnName();
            string tableName = GetTableName();
            string columns = GetColumns(excludeKey: true);
            string properties = GetPropertyNames(excludeKey: true);
            string query = $@""INSERT INTO {tableName} ({columns}) VALUES ({properties})
                              SELECT * FROM {tableName} WHERE {keyColumn} = SCOPE_IDENTITY()"";
            return await _connection.QueryFirstOrDefaultAsync<T>(query, entity);

        }

        public virtual async Task<T> DeleteByIdAsync(int id)
        {
            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            var deletedData = await GetByIdAsync(id);
            string query = $@""DELETE FROM {tableName} WHERE {keyColumn} = @Id;"";
        
            _connection.Execute(query, new { Id = id });
            return deletedData;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            string tableName = GetTableName();
            string query = $""SELECT * FROM {tableName}"";
            return await _connection.QueryAsync<T>(query);
        }

        public virtual async Task<T> GetByIdAsync(int Id)
        {
            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string query = $""SELECT * FROM {tableName} WHERE {keyColumn} = '{Id}'"";
            return await _connection.QueryFirstOrDefaultAsync<T>(query);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {

            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string keyProperty = GetKeyPropertyName();

            StringBuilder query = new StringBuilder();
            query.AppendLine($""UPDATE {tableName} SET "");

            var properties = GetProperties(true);
            for (int a = 0; a < properties.Count(); a++) {
                string propertyName = properties[a].Name;
                string comma = a < properties.Count() - 1 ? "","" : """";
                query.AppendLine($""{propertyName} = @{propertyName} {comma} ""); 
            }
            query.Remove(query.Length - 1, 1);

            query.AppendLine($""WHERE {keyColumn} = @{keyProperty} SELECT * FROM {tableName} WHERE {keyColumn} = @{keyProperty}"");

            return await _connection.QueryFirstOrDefaultAsync<T>(query.ToString(), entity);
        }

        public virtual async Task<IEnumerable<T>> BulkInsertAsync(List<T> list)
        {
            List<T> result = new List<T>();
            foreach (T item in list)
            {
               T addedItem =  await InsertAsync(item);
               result.Add(addedItem);
            }
            return result;
        }
        public virtual async Task<IEnumerable<T>> BulkUpdateAsync(List<T> list)
        {
            List<T> result = new List<T>();
            foreach (T item in list)
            {
                T updatedItem = await UpdateAsync(item);
                result.Add(updatedItem);
            }
            return result;
        }
        public virtual async Task<IEnumerable<T>> BulkUpsertAsync(IEnumerable<T> entities)
        {
            List<T> result = new List<T>();
            foreach (var entity in entities)
            {
                string keyProperty = GetKeyPropertyName();
                var keyValue = typeof(T).GetProperty(keyProperty)?.GetValue(entity);

                // If key is default or null, insert new record, otherwise update existing
                if (keyValue == null || Convert.ToInt32(keyValue) == 0)
                {
                    T insertedItem = await InsertAsync(entity);
                    result.Add(insertedItem);
                }
                else
                {
                    T updatedItem = await UpdateAsync(entity);
                    result.Add(updatedItem);
                }
            }
            return result;
        }
        private string GetTableName()
        {
            string tableName = """";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name;
        }

        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }


        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join("", "", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join("", "", properties.Select(p =>
            {
                return $""@{p.Name}"";
            }));

            return values;
        }

        protected List<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties.ToList();
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }
    }
}";
            fileService.Create(FolderNames.Repositories.ToString(), "GenericRepository.cs", text);
        }
        public void CreateEFCoreContext()
        {
            StringBuilder dbSetText = new StringBuilder();
            var tables = sqlService.GetAllTableSchema();
            foreach (var table in tables)
            {
                dbSetText.AppendLine("\t\tpublic DbSet<" + table.TABLE_NAME + "> " + table.TABLE_NAME + " { get; set; }");
            }

            string text = @"
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Project." + FolderNames.Models.ToString() + @";

namespace Project." + FolderNames.ApplicationContexts.ToString() + @"
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = """ + modifiedConnectionString + @""";
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
" + dbSetText + @"
    }
}
";
            fileService.Create(FolderNames.ApplicationContexts.ToString(), "ApplicationContext.cs", text);
        }
        public void CreateEFCoreGeneric()
        {
            string text = @"
using Microsoft.EntityFrameworkCore;
using Project." + FolderNames.ApplicationContexts.ToString() + @";
using System.ComponentModel.DataAnnotations;
using EFCore.BulkExtensions;

namespace Project." + FolderNames.Repositories.ToString() + @"
{
    public class GenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context = new ApplicationContext();

        public virtual async Task<T> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            var keyValue = GetKeyValueAsInt(entity);
            var retrievedEntity = await GetByIdAsync(keyValue.Value);
            var updateData = UpdateEntityProperties(retrievedEntity, entity);
            await _context.SaveChangesAsync();
            return updateData;
        }
        public virtual async Task<T> DeleteByIdAsync(int id)
        {
            T? deletedData = await GetByIdAsync(id);
            _context.Set<T>().Remove(deletedData);
            await _context.SaveChangesAsync();
            return deletedData;
        }
        public virtual async Task<IEnumerable<T>> BulkUpdateAsync(List<T> list)
        {
            await _context.BulkUpdateAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }
        public virtual async Task<IEnumerable<T>> BulkInsertAsync(List<T> list)
        {
            await _context.BulkInsertAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }
        public virtual async Task<IEnumerable<T>> BulkUpsertAsync(List<T> entities)
        {
            // Get the primary key property info
            var keyProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any());

            if (keyProperty == null)
                throw new InvalidOperationException($""No key property found for entity type {typeof(T).Name}"");

            // Separate entities into insert and update lists
            var toInsert = new List<T>();
            var toUpdate = new List<T>();

            foreach (var entity in entities)
            {
                var keyValue = keyProperty.GetValue(entity);
                bool isDefaultKey = keyValue == null ||
                                   (keyValue is int intValue && intValue == 0) ||
                                   (keyValue is long longValue && longValue == 0);

                if (isDefaultKey)
                {
                    toInsert.Add(entity);
                }
                else
                {
                    toUpdate.Add(entity);
                }
            }

            // Perform insert operations
            if (toInsert.Any())
            {
                await _context.BulkInsertAsync(toInsert);
            }

            // Perform update operations
            if (toUpdate.Any())
            {
                await _context.BulkUpdateAsync(toUpdate);
            }

            await _context.SaveChangesAsync();

            // Return all processed entities
            return entities;
        }
        private int? GetKeyValueAsInt(T entity)
        {
            var entityType = typeof(T);
            var keyProperty = entityType.GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));

            if (keyProperty == null)
            {
                throw new InvalidOperationException(""No Key attribute found on properties."");
            }

            var keyValue = keyProperty.GetValue(entity);

            if (keyValue is int intValue)
            {
                return intValue;
            }

            throw new InvalidOperationException(""Key value is not of type int."");
        }


        private T UpdateEntityProperties(T oldEntity, T newEntity)
        {
            var entityType = typeof(T);

            foreach (var property in entityType.GetProperties())
            {
                if (property.CanWrite && !Attribute.IsDefined(property, typeof(KeyAttribute)))
                {
                    var newValue = property.GetValue(newEntity);
                    property.SetValue(oldEntity, newValue);
                }
            }
            return newEntity;

        }

    }
}";
            fileService.Create(FolderNames.Repositories.ToString(), "GenericRepository.cs", text);
        }
    }
}
