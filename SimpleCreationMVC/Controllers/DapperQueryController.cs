
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreation.Controllers
{
    [Route("api/dapper-query")]
    public class DapperQueryController : Controller
    {

        [HttpGet("download-all")]
        public IActionResult DownloadAll([FromQuery] string connectionString)
        {
            try
            {
                FileService fileService = new FileService();
                GenericService genericService = new GenericService(connectionString);
                ModelService modelService = new ModelService(connectionString);
                RepositoryService repositoryService = new RepositoryService(connectionString);
                ServiceService serviceService = new ServiceService(connectionString);
                ControllerService controllerService = new ControllerService(connectionString);

                fileService.Delete();

                genericService.CreateDapperQueryGeneric();
                modelService.CreateModelClassesFiles();
                repositoryService.CreateRepositoriesFiles();
                serviceService.CreateServicesFiles();
                controllerService.CreateWebApisControllerFiles();

                string projectZip = fileService.ZipProjectFile();
                return File(System.IO.File.OpenRead(projectZip), "application/octet-stream", Path.GetFileName(projectZip));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("download-custom")]
        public IActionResult DownloadCustom([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FileService fileService = new FileService();
                GenericService genericService = new GenericService(connectionString);
                ModelService modelService = new ModelService(connectionString);
                RepositoryService repositoryService = new RepositoryService(connectionString);
                ServiceService serviceService = new ServiceService(connectionString);
                ControllerService controllerService = new ControllerService(connectionString);

                fileService.Delete();

                genericService.CreateDapperQueryGeneric();
                modelService.CreateModelClassesFiles(tableSchemas);
                repositoryService.CreateRepositoriesFiles();
                serviceService.CreateServicesFiles(tableSchemas);
                controllerService.CreateWebApisControllerFiles(tableSchemas);

                string projectZip = fileService.ZipProjectFile();
                return File(System.IO.File.OpenRead(projectZip), "application/octet-stream", Path.GetFileName(projectZip));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
