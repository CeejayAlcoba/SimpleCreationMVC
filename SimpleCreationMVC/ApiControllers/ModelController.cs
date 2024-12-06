using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreationMVC.ApiControllers
{
    [Route("api/model")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult DownloadAllModel([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FileService _fileService = new FileService();
                ModelService _modelService = new ModelService(connectionString);

                _fileService.Delete();
                _modelService.CreateModelClassesFiles(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
    }
}
