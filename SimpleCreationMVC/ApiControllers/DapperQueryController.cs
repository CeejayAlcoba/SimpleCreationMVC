
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreation.Services;
using SimpleCreationMVC.Services;
using SimpleCreationMVC.Services.GenericServices;
using SimpleCreationMVC.Services.RepositoryServices;
using SimpleCreationMVC.Services.ServiceServices;
using SimpleCreationMVC.Services.UtilityServices;

namespace SimpleCreation.ApiControllers
{
    [Route("api/dapper-query")]
    public class DapperQueryController : Controller
    {
        [HttpPost("create")]
        public IActionResult CreateDapperQuery([FromQuery]string connectionString,[FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
               
                FileService _fileService = new FileService();
                GenericMainService _genericMainService = new GenericMainService(connectionString);
                ModelService _modelService = new ModelService(connectionString);
                RepositoryMainService _repositoryMainService = new RepositoryMainService(connectionString);
                ServiceMainService _serviceMainService = new ServiceMainService();
                ControllerService _controllerService = new ControllerService(connectionString);
                ReadMeService _readMeService = new ReadMeService();
                UtilityMainService _utilityMainService = new UtilityMainService();

                _fileService.Delete();
                _genericMainService.CreateDapperQuery();
                _modelService.CreateModelClassesFiles(tableSchemas);
                _repositoryMainService.CreateCommon(tableSchemas);
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
