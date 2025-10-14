using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Services.UtilityServices
{
    public class UtilityReadMe
    {
        private readonly FileService _fileService = new FileService();
        public void CreateFileUtility()
        {
            string content = @"
/// <summary>
/// Example usage.
/// </summary>

        public class Example
        {
            private readonly IFileUtility _imageUtility;
            private readonly IFileUtility _profileUtility;
        
            public Example()
            {
                _imageUtility = new FileUtility(""src/images"");
                _profileUtility = new FileUtility(@""C:\src\images\profile"", isAbsolutePath: true);
            }
        }

/// <summary>
/// Example controller.
/// </summary>

        [HttpGet(""file/{fileName}"")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var file = await _fileUtility.GetAsync(fileName);
            var contentType = _fileUtility.GetContentType(fileName);

            return new FileContentResult(file, contentType)
            {
                FileDownloadName = fileName
            };
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] string data,  IFormFile? file)
        {
            var result = JsonConvert.DeserializeObject<ParentModel>(data);
            var newFile = await _fileUtility.CreateAsync(file.FileName,file.OpenReadStream());
            return Ok();  
        }

/// <summary>
/// Example axios (download).
/// </summary>

        import axios from ""axios"";
        
        export default async function GetFile(fileName: string) {
          const response = await axios.get<Blob>(`api/files/file/${fileName}`, {
            responseType: ""blob"",
          });
          return response.data;
        }

------------------------------------------------------------
        const blob = await _vehicleRegistrationService.GetFile(fileName);
        const url = URL.createObjectURL(blob);
        setPdfUrl(url);
        
        {pdfUrl && (
            <iframe
                src={pdfUrl}
                width=""100%""
                height=""600px""
                style={{ border: ""none"" }}
                title=""Vehicle OR/CR PDF""
            />
        )}

/// <summary>
/// Example axios (upload).
/// </summary>

        uploadFile: async (values: Data): Promise<any> => {
            const formData = new FormData();
          // append text fields
              formData.append(""data"",JSON.stringify(values));
        
              // append file
              if (values.file && values.file[0]?.originFileObj) {
                formData.append(""file"", values.file[0].originFileObj);
              }
        
            const response = await axios.post(""https://localhost:5001/api/files/upload"", formData);
        
            return response.data;
          }
___________________________________________________
         const [file, setFile] = useState<File | null>(null);
          const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
            if (e.target.files && e.target.files.length > 0) {
              setFile(e.target.files[0]);
            }
          };
         <input type=""file"" onChange={handleFileChange} />

";
            _fileService.Create(FolderNames.Utilities.ToString(), "ReadMeFileUtility.txt", content);
        }

        public void CreateJWTUtility()
        {
            string content = @"
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
app.UseAuthorization();";
            _fileService.Create(FolderNames.Utilities.ToString(), "ReadMeJWTUtility.txt", content);

        }

        public void CreateAppUtility()
        {
            string content = @"
Sample GetConnectionString using App Utility
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config.GetConnectionString(""DefaultConnection"");

Sample get item in appsetting.json
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config[""JWT:Secret""];
";
            _fileService.Create(FolderNames.Utilities.ToString(), "ReadMeAppUtility.txt", content);
        }
    }
}
