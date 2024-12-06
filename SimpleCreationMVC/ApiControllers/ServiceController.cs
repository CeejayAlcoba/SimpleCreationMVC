using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.ApiControllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateService([FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FileService _fileService = new FileService();
                ServiceService _serviceService = new ServiceService();

                _fileService.Delete();
                _serviceService.CreateServicesFiles(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
