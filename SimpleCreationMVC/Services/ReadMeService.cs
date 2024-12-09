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

PM> Install-Package AutoMapper
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

PM> Install-Package AutoMapper
"
            ;

            _fileService.Create("", "ReadMe.txt", text);
        }
    }
}
