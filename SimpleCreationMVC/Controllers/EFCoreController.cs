using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreation.Controllers
{
    [Route("api/ef-core")]
    public class EFCoreController : Controller
    {
        [HttpPost("create")]
        public IActionResult DownloadCustom([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FileService _fileService = new FileService();
                GenericService _genericService = new GenericService(connectionString);
                ModelService _modelService = new ModelService(connectionString);
                RepositoryService _repositoryService = new RepositoryService();
                ServiceService _serviceService = new ServiceService();
                ControllerService _controllerService = new ControllerService(connectionString);

                _fileService.Delete();
                _genericService.CreateEFCoreGeneric();
                _modelService.CreateModelClassesFiles(tableSchemas);
                _repositoryService.CreateRepositoriesFiles(tableSchemas);
                _serviceService.CreateServicesFiles(tableSchemas);
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
