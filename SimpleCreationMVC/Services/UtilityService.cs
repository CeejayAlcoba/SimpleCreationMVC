using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services
{
    public class UtilityService
    {
        public FileService _fileService = new FileService();
        public void CreateAutoMapperConfigFile()
        {
            var text = $@"
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.{FolderNames.Utilities}
{{
    public class AutoMapperConfig
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
            _fileService.Create(FolderNames.Utilities.ToString(), "AutoMapperConfig.cs", text);
        }
    }
}
