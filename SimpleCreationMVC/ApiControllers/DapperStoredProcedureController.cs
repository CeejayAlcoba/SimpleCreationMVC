using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreation.Services;
using SimpleCreationMVC.Services;
using SimpleCreationMVC.Services.GenericServices;
using SimpleCreationMVC.Services.RepositoryServices;
using SimpleCreationMVC.Services.ServiceServices;
using SimpleCreationMVC.Services.UtilityServices;
using System.IO;

namespace SimpleCreation.ApiControllers
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
                GenericMainService _genericMainService = new GenericMainService(connectionString);
                ModelService _modelService = new ModelService(connectionString);
                RepositoryMainService _repositoryMainService = new RepositoryMainService(connectionString);
                StoredProcedureService _storedProcedureService = new StoredProcedureService(connectionString);
                ServiceMainService _serviceMainService = new ServiceMainService();
                ControllerService _controllerService = new ControllerService(connectionString);
                ReadMeService _readMeService = new ReadMeService();
                UtilityMainService _utilityMainService = new UtilityMainService();
                    
                _fileService.Delete();
                _genericMainService.CreateStoredProcedure();
                _modelService.CreateModelClassesFiles(tableSchemas);
                _repositoryMainService.CreateStoredProcedure(tableSchemas);
                _storedProcedureService.CreateStoredProceduresFiles(tableSchemas);
                _storedProcedureService.CreateEnumProceduresFile();
                _serviceMainService.CreateCommon(tableSchemas);
                _controllerService.CreateWebApisControllerFiles(tableSchemas);
                _utilityMainService.Create();
                _readMeService.CreateDapperNote();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




    }
}
