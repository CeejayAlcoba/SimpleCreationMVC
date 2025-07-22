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
            string serviceCammel = $"{textService.ToCamelCase(service)}";
            string text = $@"
using Microsoft.AspNetCore.Mvc;
using {FolderNames.Models};
using {FolderNames.Services}.{FolderNames.Interfaces};

namespace ApiControllers
{{
    [Route(""api/{textService.ToKebabCase(table)}"")]
    [ApiController]
    public class {table}Controller : ControllerBase
    {{
        private readonly I{service} {serviceName};

        public {table}Controller(I{service} {serviceCammel})
        {{
            {serviceName} = {serviceCammel};
        }}

        [HttpGet(""list"")]
        public async Task<IActionResult> GetAllAsync([FromQuery]{table} filter)
        {{
            try
            {{
                IEnumerable<{table}> data = await {serviceName}.GetAllAsync(filter);
                return Ok(data);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
           
        }}
        [HttpGet(""{{id}}"")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {{
            try
            {{
                {table}? data = await {serviceName}.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]{table} {variableTableName})
        {{
            try
            {{
                {table}? data = await {serviceName}.InsertAsync({variableTableName});
                return Ok(data);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}

        [HttpPatch(""{{id}}"")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]{table} {variableTableName})
        {{
            try
            {{
                if(id != {variableTableName}.{primaryKey}) return BadRequest(""Id mismatched."");

                {table}? data = await {serviceName}.GetByIdAsync(id);
                if (data == null) return NotFound();

                {table}? updatedData = await {serviceName}.UpdateAsync({variableTableName}); 
                return Ok(updatedData);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}
        [HttpDelete(""{{id}}"")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {{
            try
            {{
                {table}? data = await {serviceName}.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await {serviceName}.DeleteByIdAsync(id);
                return Ok(deletedData);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}
        [HttpPost(""bulk"")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<{table}> listData)
        {{
            try
            {{
                IEnumerable<{table}> data = await {serviceName}.BulkInsertAsync(listData);
                return Ok(data);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}
        [HttpPatch(""bulk"")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<{table}> listData)
        {{
            try
            {{
                IEnumerable<{table}> data = await {serviceName}.BulkUpdateAsync(listData);
                return Ok(data);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}
        [HttpPost(""bulk-upsert"")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<{table}> listData)
        {{
            try
            {{
                IEnumerable<{table}> data = await {serviceName}.BulkUpsertAsync(listData);
                return Ok(data);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}
        [HttpPost(""bulk-merge"")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<{table}> listData)
        {{
            try
            {{
                IEnumerable<{table}> data = await {serviceName}.BulkMergeAsync(listData);
                return Ok(data);
            }}
            catch (Exception ex)
            {{
                return BadRequest(ex.Message);
            }}
        }}
        
    }}
}}
";
            fileService.Create(FolderNames.Controllers.ToString(), $"{table}Controller.cs", text);
        }
    }
}
