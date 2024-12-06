using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Services;

namespace SimpleCreationMVC.ApiControllers
{
    [Route("api/download-project")]
    [ApiController]
    public class DownloadProjectController : ControllerBase
    {
        [HttpGet]
        public IActionResult DownloadProject()
        {
            FileService _fileService = new FileService();
            string projectZip = _fileService.ZipProjectFile();
            _fileService.Delete(isProjectZip: false);
            return File(System.IO.File.OpenRead(projectZip), "application/octet-stream", Path.GetFileName(projectZip));
        }
    }
}
