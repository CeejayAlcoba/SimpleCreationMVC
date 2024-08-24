using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreation.Services;
using System.IO;

namespace SimpleCreation.Controllers
{
    [Route("api/dapper-stored-procedure")]
    public class DapperStoredProcedureController : Controller
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
                StoredProcedureService _storedProcedureService = new StoredProcedureService(connectionString);
                ServiceService _serviceService = new ServiceService();
                ControllerService _controllerService = new ControllerService(connectionString);

                _fileService.Delete();
                _genericService.CreateProcedureGeneric();
                _modelService.CreateModelClassesFiles(tableSchemas);
                _repositoryService.CreateRepositoriesFiles(tableSchemas);
                _storedProcedureService.CreateStoredProceduresFiles(tableSchemas);
                _storedProcedureService.CreateEnumProcedureFile();
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
