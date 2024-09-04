using Microsoft.IdentityModel.Tokens;
using SimpleCreation.Models;

namespace SimpleCreation.Services
{
    public class ControllerService
    {
        private readonly TextService textService = new TextService();
        private readonly FileService fileService = new FileService();
        private readonly SqlService sqlService;
        public ControllerService(string connectionString) { 
            this.sqlService = new SqlService(connectionString);
        }
        public void CreateWebApisControllerFiles(List<TableSchema> tableSchemas)
        {
            foreach (var tableSchema in tableSchemas)
            {
                CreateWebApiControllerFile(tableSchema.TABLE_NAME);
            }
        }
        public void CreateWebApiControllerFile(string tableName)
        {
            string primaryKey = sqlService.GetTablePrimaryKey(tableName).COLUMN_NAME;
            string table = tableName;
            string variableTableName = textService.ToCamelCase(table);
            string service = $"{tableName}Service";
            string serviceName = $"_{textService.ToCamelCase(service)}";
            string text = @"
using Microsoft.AspNetCore.Mvc;
using Project."+FolderNames.Models.ToString()+ @";
using Project."+FolderNames.Services.ToString()+@";

namespace Project.Controllers
{
    [Route(""api/" + textService.ToKebabCase(table) + @""")]
    [ApiController]
    public class " + table + @"Controller : ControllerBase
    {
        private readonly " + service + " " + serviceName + " = new " + service + @"();

        [HttpGet(""list"")]
        public async Task<IActionResult> GetAll" + table + @"()
        {
            try
            {
                var data = await " + serviceName + @".GetAll();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet(""{id}"")]
        public async Task<IActionResult> GetById" + table + @"(int id)
        {
            try
            {
                var data = await  " + serviceName + @".GetById(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Insert" + table + @"([FromBody]"+table+" "+variableTableName+@")
        {
            try
            {
                var data = await " + serviceName + @".Insert("+ variableTableName + @");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(""{id}"")]
        public async Task<IActionResult> Update" + table + @"(int id,[FromBody]"+table+" "+variableTableName+ @")
        {
            try
            {
                if(id != "+ $"{variableTableName}.{primaryKey}" + @") return BadRequest(""Id mismatched."");

                var data = await " + serviceName + @".GetById(id);
                if (data == null) return NotFound();

                var updatedData = await " + serviceName + @".Update("+ variableTableName + @"); 
                return Ok(updatedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(""{id}"")]
        public async Task<IActionResult> DeleteById" + table + @"(int id)
        {
            try
            {
                var data = await " + serviceName + @".GetById(id);
                if (data == null) return NotFound();

                var deletedData = await " + serviceName + @".DeleteById(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
";
            fileService.Create(FolderNames.Controllers.ToString(), $"{table}Controller.cs", text);
        }
    }
}
