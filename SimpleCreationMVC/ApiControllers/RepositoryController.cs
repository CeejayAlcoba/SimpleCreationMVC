using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;
using SimpleCreationMVC.Services.RepositoryServices;

namespace SimpleCreationMVC.ApiControllers
{
    [Route("api/repository")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateRepository([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FileService _fileService = new FileService();
                RepositoryMainService _repositoryMainService = new RepositoryMainService(connectionString);

                _fileService.Delete();
                _repositoryMainService.CreateCommon(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
