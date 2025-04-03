using SimpleCreation.Services;

namespace SimpleCreationMVC.Services
{
    public class ReadMeService
    {
        public FileService _fileService = new FileService();
        public void CreateDapperNote()
        {
            string text = @"NuGet Packages Required

The project should download the following NuGet packages:

PM> Install-Package Dapper
PM> Install-Package Microsoft.Data.SqlClient

For Auto Mapper Utility
PM> Install-Package AutoMapper

For App Utility
PM> Install-Package Microsoft.Extensions.Configuration
PM> Install-Package Microsoft.Extensions.Configuration.Json
PM> Install-Package Microsoft.Extensions.Configuration.Binder




Sample GetConnectionString using App Utility
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config.GetConnectionString(""DefaultConnection"");

Sample get item in appsetting.json
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config[""JWT:Secret""];
"
            ;

            _fileService.Create("", "ReadMe.txt", text);
        }
        public void CreateEFCoreNote()
        {
            string text = @"NuGet Packages Required

The project should download the following NuGet packages:

PM> Install-Package Microsoft.EntityFrameworkCore
PM> Install-Package Microsoft.EntityFrameworkCore.Tools
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer
PM> Install-Package EFCore.BulkExtensions

For Auto Mapper Utility
PM> Install-Package AutoMapper

For App Utility
PM> Install-Package Microsoft.Extensions.Configuration
PM> Install-Package Microsoft.Extensions.Configuration.Json
PM> Install-Package Microsoft.Extensions.Configuration.Binder




Sample GetConnectionString using App Utility
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config.GetConnectionString(""DefaultConnection"");

Sample get item in appsetting.json
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config[""JWT:Secret""];
"
            ;

            _fileService.Create("", "ReadMe.txt", text);
        }
    }
}
