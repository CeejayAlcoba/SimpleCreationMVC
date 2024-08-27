using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class ControllerController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateController([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FileService _fileService = new FileService();
                ControllerService _controllerService = new ControllerService(connectionString);

                _fileService.Delete();
               _controllerService.CreateWebApisControllerFiles(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
