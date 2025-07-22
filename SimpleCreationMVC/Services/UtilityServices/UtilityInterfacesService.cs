using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.UtilityServices
{
    public class UtilityInterfacesService
    {
        public FileService _fileService = new FileService();
        public  void CreateAppUtility()
        {
            string text = $@"
namespace {FolderNames.Utilities}.{FolderNames.Interfaces}
{{
    public interface IAppUtility
    {{
        IConfigurationRoot GetConfiguration();
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesInterfacesFolder, "IAppUtility.cs", text);
        }

        public void CreateAutoMapperUtility()
        {
            string text = $@"
namespace {FolderNames.Utilities}.{FolderNames.Interfaces}
{{
    public interface IAutoMapperUtility
    {{
        TDestination Map<TSource,TDestination>(TSource source);
        List<TDestination> MapList<TSource,TDestination>(IEnumerable<TSource> source);
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesInterfacesFolder, "IAutoMapperUtility.cs", text);
        }

        public void CreateDataTableUtility()
        {
            string text = $@"
using System.Data;

namespace {FolderNames.Utilities}.{FolderNames.Interfaces}
{{
    public interface IDataTableUtility
    {{
        DataTable Convert<T>(IEnumerable<T> lists) where T : class;
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesInterfacesFolder, "IDataTableUtility.cs", text);
        }
    }
}
