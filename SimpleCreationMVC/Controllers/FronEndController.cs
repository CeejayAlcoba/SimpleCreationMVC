using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Services;
using SimpleCreationMVC.Services;

namespace SimpleCreationMVC.Controllers
{
    [Route("api/front-end")]
    [ApiController]
    public class FrontEndController : ControllerBase
    {
        [HttpGet("js-classes-download")]
        public IActionResult GetJsClasses([FromQuery] string connectionString)
        {
            try
            {
                FronEndService _fronEndService = new FronEndService(connectionString);
                FileService _fileService = new FileService();
                _fileService.Delete();
                _fronEndService.CreateJsClasses();

                string projectZip = _fileService.ZipProjectFile();
                return File(System.IO.File.OpenRead(projectZip), "application/octet-stream", Path.GetFileName(projectZip));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ts-types-download")]
        public IActionResult GetTsTypes([FromQuery] string connectionString)
        {
            try
            {
                FronEndService _fronEndService = new FronEndService(connectionString);
                FileService _fileService = new FileService();
                _fileService.Delete();
                _fronEndService.CreateTsTypes();

                string projectZip = _fileService.ZipProjectFile();
                return File(System.IO.File.OpenRead(projectZip), "application/octet-stream", Path.GetFileName(projectZip));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
