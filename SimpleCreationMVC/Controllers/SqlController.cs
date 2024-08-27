using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCreation.Models;
using SimpleCreation.Services;

namespace SimpleCreation.Controllers
{
    [Route("api/sql")]
    [ApiController]
    public class SqlController : Controller
    {
        [HttpGet("table-schema")]
        public IActionResult GetTableSchema([FromQuery] string connectionString)
        {
            try
            {
                SqlService sqlService = new SqlService(connectionString);

                var tables =  sqlService.GetAllTableSchema();

                return Ok(tables);
             
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("stored-procedure/create")]
        public IActionResult CreateStoredProcedure([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FileService _fileService = new FileService();
                StoredProcedureService _storedProcedureService = new StoredProcedureService(connectionString);

                _fileService.Delete();
                _storedProcedureService.CreateStoredProceduresFiles(tableSchemas);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
