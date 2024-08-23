using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Services;
using System.IO.Compression;

namespace SimpleCreation.Controllers
{
    [Route("api/system")]
    [ApiController]
    public class SystemController : Controller
    {
      
        [HttpGet("download")]
        public IActionResult DownloadAll()
        {
            try
            {
                FileService _fileService = new FileService();
                var publishedZip = _fileService.ZipPublishedFile();
                return File(System.IO.File.OpenRead(publishedZip), "application/octet-stream", Path.GetFileName(publishedZip));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
