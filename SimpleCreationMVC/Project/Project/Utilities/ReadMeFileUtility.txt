
/// <summary>
/// Example usage.
/// </summary>

        public class Example
        {
            private readonly IFileUtility _imageUtility;
            private readonly IFileUtility _profileUtility;
        
            public Example()
            {
                _imageUtility = new FileUtility("src/images");
                _profileUtility = new FileUtility(@"C:\src\images\profile", isAbsolutePath: true);
            }
        }

/// <summary>
/// Example controller.
/// </summary>

        [HttpGet("file/{fileName}")]
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

        import axios from "axios";
        
        export default async function GetFile(fileName: string) {
          const response = await axios.get<Blob>(`api/files/file/${fileName}`, {
            responseType: "blob",
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
                width="100%"
                height="600px"
                style={{ border: "none" }}
                title="Vehicle OR/CR PDF"
            />
        )}

/// <summary>
/// Example axios (upload).
/// </summary>

        uploadFile: async (values: Data): Promise<any> => {
            const formData = new FormData();
          // append text fields
              formData.append("data",JSON.stringify(values));
        
              // append file
              if (values.file && values.file[0]?.originFileObj) {
                formData.append("file", values.file[0].originFileObj);
              }
        
            const response = await axios.post("https://localhost:5001/api/files/upload", formData);
        
            return response.data;
          }
___________________________________________________
         const [file, setFile] = useState<File | null>(null);
          const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
            if (e.target.files && e.target.files.length > 0) {
              setFile(e.target.files[0]);
            }
          };
         <input type="file" onChange={handleFileChange} />

