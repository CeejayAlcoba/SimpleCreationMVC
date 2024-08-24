using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
