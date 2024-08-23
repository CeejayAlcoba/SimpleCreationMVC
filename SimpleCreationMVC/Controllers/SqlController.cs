using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Services;

namespace SimpleCreation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SqlController : Controller
    {
        [HttpGet("table-schema")]
        public IActionResult GetTableSchema([FromQuery] string connectionString)
        {
            try
            {
                //FileService fileService = new FileService();
                //InitialFileCreationService initialService = new InitialFileCreationService(connectionString);
                //ModelService modelService = new ModelService(connectionString);
                //RepositoryService repositoryService = new RepositoryService(connectionString);
                //StoredProcedureService storedProcedureService = new StoredProcedureService(connectionString);
                //ServiceService serviceService = new ServiceService(connectionString);

                SqlService sqlService = new SqlService(connectionString);

                var tables =  sqlService.GetAllTableSchema();

                return Ok(tables);
             
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
