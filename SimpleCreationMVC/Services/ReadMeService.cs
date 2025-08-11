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

Sample GetConnectionString using App Utility
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config.GetConnectionString(""DefaultConnection"");

Sample get item in appsetting.json
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config[""JWT:Secret""];


Implementing JWT in appsetting.json
 ""Jwt"": {
    ""Key"": ""Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hxx"",
    ""Issuer"": ""JWTAuthenticationServer""
  }

Implementing JWT in Program.cs
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc(""v1"", new OpenApiInfo { Title = ""WMS V2 API"", Version = ""v1"" });
    opt.AddSecurityDefinition(""Bearer"", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = ""Please enter token"",
        Name = ""Authorization"",
        Type = SecuritySchemeType.Http,
        BearerFormat = ""JWT"",
        Scheme = ""bearer""
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=""Bearer""
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        /* ValidIssuer = builder.Configuration[""Jwt:Issuer""],*/
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[""Jwt:Key""]))
    };
});
app.UseAuthentication();
app.UseAuthorization();



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

Sample GetConnectionString using App Utility
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config.GetConnectionString(""DefaultConnection"");

Sample get item in appsetting.json
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config[""JWT:Secret""];


Implementing JWT in appsetting.json
 ""Jwt"": {
    ""Key"": ""Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hxx"",
    ""Issuer"": ""JWTAuthenticationServer""
  }

Implementing JWT in Program.cs
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc(""v1"", new OpenApiInfo { Title = ""WMS V2 API"", Version = ""v1"" });
    opt.AddSecurityDefinition(""Bearer"", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = ""Please enter token"",
        Name = ""Authorization"",
        Type = SecuritySchemeType.Http,
        BearerFormat = ""JWT"",
        Scheme = ""bearer""
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=""Bearer""
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        /* ValidIssuer = builder.Configuration[""Jwt:Issuer""],*/
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[""Jwt:Key""]))
    };
});
app.UseAuthentication();
app.UseAuthorization();
"
            ;

            _fileService.Create("", "ReadMe.txt", text);
        }
    }
}
