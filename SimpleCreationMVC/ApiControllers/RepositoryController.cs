using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;

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
                RepositoryService _repositoryService = new RepositoryService(connectionString);

                _fileService.Delete();
                _repositoryService.CreateRepositoriesFiles(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
