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
         public TDestination Map<TSource,TDestination>(TSource source)
        {{
            var config = new MapperConfiguration(cfg =>
            {{
                cfg.CreateMap(typeof(TSource), typeof(TDestination));
            }});
            var mapper = new Mapper(config);
            return mapper.Map<TSource,TDestination>(source);
        }}

        public List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        {{
            if (source == null || !source.Any())
            {{
                return new List<TDestination>();
            }}

            var config = new MapperConfiguration(cfg =>
            {{
                cfg.CreateMap(typeof(TSource), typeof(TDestination));
            }});
            var mapper = new Mapper(config);

            return mapper.Map<IEnumerable<TSource>,List<TDestination>>(source);
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
        public DataTable Convert<T>(IEnumerable<T>? lists) where T : class
        {{
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add columns (even if list is null or empty)
            foreach (PropertyInfo property in properties)
            {{
                Type columnType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                dt.Columns.Add(property.Name, columnType);
            }}

            // Only add rows if the list is not null or empty
            if (lists != null)
            {{
                foreach (T item in lists)
                {{
                    DataRow row = dt.NewRow();
                    foreach (PropertyInfo property in properties)
                    {{
                        row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                    }}
                    dt.Rows.Add(row);
                }}
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
        public void CreateClaimsHelperUtility()
        {
            var text = $@"
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using {FolderNames.Utilities}.{FolderNames.Interfaces};

namespace {FolderNames.Utilities}.{FolderNames.Classes}
{{
    public class ClaimsHelperUtility : IClaimsHelperUtility
    {{
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsHelperUtility(IHttpContextAccessor httpContextAccessor)
        {{
            _httpContextAccessor = httpContextAccessor;
        }}

        public int? GetUserId()
        {{
            string? strUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(""UserId"")?.Value;
            if (string.IsNullOrEmpty(strUserId)) return null;
            return int.Parse(strUserId);
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesClassesFolder, "ClaimsHelperUtility.cs", text);
        }

        public void CreateEncryptUtility()
        {
            var text = $@"
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using {FolderNames.Utilities}.{FolderNames.Interfaces};

namespace {FolderNames.Utilities}.{FolderNames.Classes}
{{
    public class EncryptUtility : IEncryptUtility
    {{
        public string GenerateRandomSalt()
        {{
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            return Convert.ToBase64String(salt);
        }}

        public string GenerateHashedPassword(string password, string salt)
        {{
            byte[] saltBytes = Convert.FromBase64String(salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));
            return hashed;
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesClassesFolder, "EncryptUtility.cs", text);
        }

        public void CreateJwtUtility()
        {
            var text = $@"
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using {FolderNames.Models};
using {FolderNames.Utilities}.{FolderNames.Interfaces};

namespace {FolderNames.Utilities}.{FolderNames.Classes}
{{
    public class JwtUtility : IJwtUtility
    {{
        private readonly IConfigurationRoot _config;

        public JwtUtility(IAppUtility appUtility)
        {{
            _config = appUtility.GetConfiguration();
        }}

        public string GenerateToken(int userId)
        {{
            var claims = new List<Claim>
            {{
                new Claim(""UserId"", userId.ToString() ?? """")
            }};

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[""Jwt:Key""])),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesClassesFolder, "JwtUtility.cs", text);
        }
        public void CreateFileUtility()
        {
            var text = $@"
using {FolderNames.Utilities}.{FolderNames.Interfaces};

namespace {FolderNames.Utilities}.{FolderNames.Classes}
{{
     public class FileUtility : IFileUtility
    {{
        private readonly string _baseDirectory;

        public FileUtility(string baseDirectory = ""src/images"", bool isAbsolutePath = false)
        {{
            _baseDirectory = isAbsolutePath ? baseDirectory : Path.Combine(Directory.GetCurrentDirectory(), baseDirectory);

            if (!Directory.Exists(_baseDirectory))
                Directory.CreateDirectory(_baseDirectory);
        }}

        public async Task<string> CreateAsync(string fileName, Stream fileStream)
        {{
            var filePath = GetFullPath(fileName);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {{
                await fileStream.CopyToAsync(stream);
            }}

            return filePath;
        }}

        public async Task<string> UpdateAsync(string oldFileName, string newFileName, Stream newFileStream)
        {{
            Delete(oldFileName); 
            return await CreateAsync(newFileName, newFileStream);
        }}

        public void Delete(string fileName)
        {{
            var filePath = GetFullPath(fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }}

        public bool Exists(string fileName)
        {{
            var filePath = GetFullPath(fileName);
            return File.Exists(filePath);
        }}

        public async Task<byte[]> GetAsync(string fileName)
        {{
            var filePath = GetFullPath(fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(""File not found"", fileName);

            return await File.ReadAllBytesAsync(filePath);
        }}

        public string GetContentType(string fileName)
        {{
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
        
            return extension switch
            {{
                "".jpg"" or "".jpeg"" => ""image/jpeg"",
                "".png"" => ""image/png"",
                "".gif"" => ""image/gif"",
                "".pdf"" => ""application/pdf"",
                "".txt"" => ""text/plain"",
                "".mp4"" => ""video/mp4"",
                _ => ""application/octet-stream"" // default
            }};
        }}

        private string GetFullPath(string fileName)
        {{
            return Path.Combine(_baseDirectory, fileName);
        }}
    }}
}}
";
            _fileService.Create(FolderPaths.UtilitiesClassesFolder, "FileUtility.cs", text);
        }
    }
}
