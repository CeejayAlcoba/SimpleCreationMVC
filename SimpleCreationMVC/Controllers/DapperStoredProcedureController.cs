using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;
using System.IO;

namespace SimpleCreation.Controllers
{
    [Route("api/dapper-stored-procedure")]
    public class DapperStoredProcedureController : Controller
    {

        [HttpGet("download-all")]
        public IActionResult DownloadAll([FromQuery]string connectionString)
        {
            try
            {
                FileService fileService = new FileService();
                GenericService genericService = new GenericService(connectionString);
                ModelService modelService = new ModelService(connectionString);
                RepositoryService repositoryService = new RepositoryService(connectionString);
                StoredProcedureService storedProcedureService = new StoredProcedureService(connectionString);
                ServiceService serviceService = new ServiceService(connectionString);
                ControllerService controllerService = new ControllerService(connectionString);
                
                fileService.Delete();

                genericService.CreateProcedureGeneric();
                modelService.CreateModelClassesFiles();
                repositoryService.CreateRepositoriesFiles();
                storedProcedureService.CreateStoredProceduresFiles();
                storedProcedureService.CreateEnumProcedureFile();
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
                StoredProcedureService storedProcedureService = new StoredProcedureService(connectionString);
                ServiceService serviceService = new ServiceService(connectionString);
                ControllerService controllerService = new ControllerService(connectionString);

                fileService.Delete();

                genericService.CreateProcedureGeneric();
                modelService.CreateModelClassesFiles(tableSchemas);
                repositoryService.CreateRepositoriesFiles();
                storedProcedureService.CreateStoredProceduresFiles(tableSchemas);
                //storedProcedureService.CreateEnumProcedureFile(tableSchemas);
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
