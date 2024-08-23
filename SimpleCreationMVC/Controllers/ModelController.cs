using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Controllers
{
    [Route("api/model")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        [HttpGet("download-all")]
        public IActionResult DownloadAllModel([FromQuery] string connectionString)
        {
            FileService fileService = new FileService();
            ModelService modelService = new ModelService(connectionString);

            fileService.Delete();
            modelService.CreateModelClassesFiles();

            string projectZip = fileService.ZipProjectFile();
            return File(System.IO.File.OpenRead(projectZip), "application/octet-stream", Path.GetFileName(projectZip));
        }
    }
}
