using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.UtilityServices
{
    public class UtilityClassesService
    {
        public FileService _fileService = new FileService();
        public void CreateAutoMapperUtility()
        {
            var text = $@"
using AutoMapper;
using {FolderNames.Utilities}.{FolderNames.Interfaces};

namespace {FolderNames.Utilities}.{FolderNames.Classes}
{{
    public class AutoMapperUtility : IAutoMapperUtility
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
            _fileService.Create(FolderPaths.UtilitiesClassesFolder, "AutoMapperUtility.cs", text);
        }

        public void CreateDataTableUtility()
        {
            var text = $@"
using System.Data;
using System.Reflection;
using {FolderNames.Utilities}.{FolderNames.Interfaces};

namespace {FolderNames.Utilities}.{FolderNames.Classes}
{{
    public class DataTableUtility : IDataTableUtility
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
            _fileService.Create(FolderPaths.UtilitiesClassesFolder, "DataTableUtility.cs", text);
        }

        public void CreateAppUtility()
        {
            var text = $@"
using {FolderNames.Utilities}.{FolderNames.Interfaces};

namespace {FolderNames.Utilities}.{FolderNames.Classes}
{{
    public class AppUtility : IAppUtility
    {{ 
        public IConfigurationRoot GetConfiguration() {{  
            return new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(""appsettings.json"", optional: false, reloadOnChange: true)
           .Build();
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesClassesFolder, "AppUtility.cs", text);
        }
    }
}
