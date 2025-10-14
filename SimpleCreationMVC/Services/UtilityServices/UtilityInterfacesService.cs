using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.UtilityServices
{
    public class UtilityInterfacesService
    {
        public FileService _fileService = new FileService();

        public void CreateAppUtility()
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

        public void CreateClaimsHelperUtility()
        {
            string text = $@"
namespace {FolderNames.Utilities}.{FolderNames.Interfaces}
{{
    public interface IClaimsHelperUtility
    {{
        int? GetUserId();
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesInterfacesFolder, "IClaimsHelperUtility.cs", text);
        }

        public void CreateEncryptUtility()
        {
            string text = $@"
namespace {FolderNames.Utilities}.{FolderNames.Interfaces}
{{
    public interface IEncryptUtility
    {{
        string GenerateRandomSalt();
        string GenerateHashedPassword(string password, string salt);
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesInterfacesFolder, "IEncryptUtility.cs", text);
        }

        public void CreateJwtUtility()
        {
            string text = $@"
using {FolderNames.Models};

namespace {FolderNames.Utilities}.{FolderNames.Interfaces}
{{
    public interface IJwtUtility
    {{
        string GenerateToken(int userId);
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesInterfacesFolder, "IJwtUtility.cs", text);
        }
        public void CreateFileUtility()
        {
            string text = $@"
using {FolderNames.Models};

namespace {FolderNames.Utilities}.{FolderNames.Interfaces}
{{
    public interface IFileUtility
    {{
        Task<string> CreateAsync(string fileName, Stream fileStream);
        Task<string> UpdateAsync(string oldFileName, string newFileName, Stream newFileStream);
        void Delete(string fileName);
        bool Exists(string fileName);
        Task<byte[]> GetAsync(string fileName);
        string GetContentType(string fileName);
    }}
}}


";
            _fileService.Create(FolderPaths.UtilitiesInterfacesFolder, "IFileUtility.cs", text);
        }
    }
}
