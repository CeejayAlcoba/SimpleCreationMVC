using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services
{
    public class UtilityService
    {
        public FileService _fileService = new FileService();
        public void CreateAutoMapperUtilityFile()
        {
            var text = $@"
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.{FolderNames.Utilities}
{{
    public class AutoMapperUtility
    {{
        public TDestination Map<TDestination>(object source)
        {{
            var config = new MapperConfiguration(cfg =>
            {{
                // Dynamically create the map based on the source and destination types
                cfg.CreateMap(source.GetType(), typeof(TDestination));
            }});
            var mapper = new Mapper(config);
            return mapper.Map<TDestination>(source);
        }}

        public List<TDestination> MapList<TDestination>(IEnumerable<object> source)
        {{
            if (source == null || !source.Any())
            {{
                return new List<TDestination>();
            }}
            var sourceType = source.First().GetType();

            var config = new MapperConfiguration(cfg =>
            {{
                // Dynamically create the map based on the source and destination types
                cfg.CreateMap(sourceType, typeof(TDestination));
            }});
            var mapper = new Mapper(config);

            // Perform mapping
            return mapper.Map<List<TDestination>>(source);
        }}
    }}
}}
";
            _fileService.Create(FolderNames.Utilities.ToString(), "AutoMapperUtility.cs", text);
        }

        public void CreateDataTableUtilityFile()
        {
            var text = $@"
using System.Data;
using System.Reflection;

namespace Project.{FolderNames.Utilities}
{{
    public class DataTableUtility
    {{
        public DataTable Convert<T>(IEnumerable<T> lists) where T : class
        {{
            if (lists == null || !lists.Any())
                throw new ArgumentException(""The list cannot be null or empty."");

            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add columns to DataTable
            foreach (PropertyInfo property in properties)
            {{
                Type columnType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                dt.Columns.Add(property.Name, columnType);
            }}

            // Add rows to DataTable
            foreach (T item in lists)
            {{
                DataRow row = dt.NewRow();
                foreach (PropertyInfo property in properties)
                {{
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }}
                dt.Rows.Add(row);
            }}

            return dt;
        }}
    }}
}}

";
            _fileService.Create(FolderNames.Utilities.ToString(), "DataTableUtility.cs", text);
        }
    }
}
