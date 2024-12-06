using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;
using SimpleCreation.Services;
using SimpleCreationMVC.Services;

namespace SimpleCreationMVC.ApiControllers
{
    [Route("api/front-end")]
    [ApiController]
    public class FrontEndController : ControllerBase
    {
        [HttpPost("js-classes/create")]
        public IActionResult GetJsClasses([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FronEndService _fronEndService = new FronEndService(connectionString);
                FileService _fileService = new FileService();
                SqlService _sqlService = new SqlService(connectionString);

                if (tableSchemas.IsNullOrEmpty())
                {
                    tableSchemas = _sqlService.GetAllTableSchema();
                }
                _fileService.Delete();
                _fronEndService.CreateJsClasses(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ts-types/create")]
        public IActionResult GetTsTypes([FromQuery] string connectionString, [FromBody] List<TableSchema> tableSchemas)
        {
            try
            {
                FronEndService _fronEndService = new FronEndService(connectionString);
                FileService _fileService = new FileService();

                _fileService.Delete();
                _fronEndService.CreateTsTypes(tableSchemas);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
