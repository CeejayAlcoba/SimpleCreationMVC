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

In Program.cs (Main project) add this 
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddUtilities();
builder.Services.AddHttpContextAccessor(); 

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

In Program.cs (Main project) add this 
    builder.Services.AddRepositories();
    builder.Services.AddServices();
    builder.Services.AddUtilities();
    builder.Services.AddHttpContextAccessor(); 
    builder.Services.AddUnitOfWork();
    var connectionString = builder.Configuration.GetConnectionString(""DefaultConnection"");
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(
            connectionString,
            b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)
        ));
    //This is to avoid circular reference cycles
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

In appsettings.json
    ""ConnectionStrings"": {
        ""DefaultConnection"": ""<your connection string>"",
    },

Command
    add-migration Initial
    update-database
"
            ;

            _fileService.Create("", "ReadMe.txt", text);
        }
    }
}
