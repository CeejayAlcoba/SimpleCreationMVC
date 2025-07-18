using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;
using SimpleCreationMVC.Services.ServiceServices;

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
                ServiceMainService _serviceMainService = new ServiceMainService();

                _fileService.Delete();
                _serviceMainService.CreateCommon(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
