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
        public TProcedures? GetAll {{ get; set; }} 
        public TProcedures? GetById {{ get; set; }}
        public TProcedures? Insert {{ get; set; }}
        public TProcedures? Update {{ get; set; }}
        public TProcedures? DeleteById {{ get; set; }}
        public TProcedures? InsertMany {{ get; set; }}
        public TProcedures? UpdateMany {{ get; set; }}
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

        // Get All
        public async Task<IEnumerable<T>> GetAll()
        {{
            return await _connection.QueryAsync<T>(_procedures.GetAll.ToString(), commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        // Get By ID
        public async Task<T?> GetById(int id)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.GetById.ToString(), new {{ Id = id }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        // Insert
        public async Task<T?> Insert(T entity)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.Insert.ToString(), entity, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        // Update
        public async Task<T?> Update(T entity)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.Update.ToString(), entity, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        // Delete By ID
        public async Task<T?> DeleteById(int id)
        {{
            return await _connection.QueryFirstOrDefaultAsync<T>(_procedures.DeleteById.ToString(), new {{ Id = id }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        // Insert Many
        public async Task<IEnumerable<T>> InsertMany( List<T> data)
        {{
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(_procedures.InsertMany.ToString(), new {{ TVP = dt.AsTableValuedParameter($""TVP_{{tableName}}"") }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }}

        // Update Many
        public async Task<IEnumerable<T>> UpdateMany( List<T> data)
        {{
            var dt = _dataTableUtility.Convert<T>(data);
            var tableName = typeof(T).Name;
            return await _connection.QueryAsync<T>(_procedures.UpdateMany.ToString(), new {{ TVP = dt.AsTableValuedParameter($""TVP_{{tableName}}"") }}, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
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

        public virtual async Task<T> Insert(T entity)
        {
            string keyColumn = GetKeyColumnName();
            string tableName = GetTableName();
            string columns = GetColumns(excludeKey: true);
            string properties = GetPropertyNames(excludeKey: true);
            string query = $@""INSERT INTO {tableName} ({columns}) VALUES ({properties})
                              SELECT * FROM {tableName} WHERE {keyColumn} = SCOPE_IDENTITY()"";
            return await _connection.QueryFirstOrDefaultAsync<T>(query, entity);

        }

        public virtual async Task<T> DeleteById(int id)
        {
            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            var deletedData = await GetById(id);
            string query = $@""DELETE FROM {tableName} WHERE {keyColumn} = @Id;"";
            
            _connection.Execute(query, new { Id = id });
            return deletedData;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            string tableName = GetTableName();
            string query = $""SELECT * FROM {tableName}"";
            return await _connection.QueryAsync<T>(query);
        }

        public virtual async Task<T> GetById(int Id)
        {
            string tableName = GetTableName();
            string keyColumn = GetKeyColumnName();
            string query = $""SELECT * FROM {tableName} WHERE {keyColumn} = '{Id}'"";
            return await _connection.QueryFirstOrDefaultAsync<T>(query);
        }

        public virtual async Task<T> Update(T entity)
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

            query.AppendLine(""WHERE {keyColumn} = @{keyProperty} SELECT * FROM {tableName} WHERE {keyColumn} = @{keyProperty}"");

            return await _connection.QueryFirstOrDefaultAsync<T>(query.ToString(), entity);
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

namespace Project." + FolderNames.Repositories.ToString() + @"
{
    public class GenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context = new ApplicationContext();

        public virtual async Task<T> Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> Update(T entity)
        {
            var keyValue = GetKeyValueAsInt(entity);
            var retrievedEntity = await GetById(keyValue.Value);
            var updateData = UpdateEntityProperties(retrievedEntity, entity);
            await _context.SaveChangesAsync();
            return updateData;
        }
        public virtual async Task<T> DeleteById(int id)
        {
            var deletedData = await GetById(id);
            _context.Set<T>().Remove(deletedData);
            await _context.SaveChangesAsync();
            return deletedData;
        }
        public virtual async Task<IEnumerable<T>> UpdateMany(List<T> list)
        {
            foreach (var item in list)
            {
                await Update(item);
            }
            await _context.SaveChangesAsync();
            return list;
        }
        public virtual async Task<IEnumerable<T>> InsertMany(List<T> list)
        {
            await _context.AddRangeAsync(list);
            await _context.SaveChangesAsync();
            return list;
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
