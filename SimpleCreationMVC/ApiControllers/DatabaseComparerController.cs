using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SimpleCreationMVC.Models;
using SimpleCreationMVC.Services;

namespace SimpleCreationMVC.ApiControllers
{
    [Route("api/database-comparer")]
    [ApiController]
    public class DatabaseComparerController : ControllerBase
    {
        private readonly DatabaseComparerService _databaseCompererService = new DatabaseComparerService();
        [HttpGet]
        public async Task<IActionResult> CompareDatabases([FromQuery] DatabaseComparerParam data)
        {

            using var firstConnection = new SqlConnection(data.BaseConnectionString);
            using var secondConnection = new SqlConnection(data.ComparisonConnectionString);

            await firstConnection.OpenAsync();
            await secondConnection.OpenAsync();

            // Get table and column information
            var firstTables = await _databaseCompererService.GetTablesAsync(firstConnection);
            var secondTables = await _databaseCompererService.GetTablesAsync(secondConnection);

            // Compare tables and columns
            var tableDifferences = _databaseCompererService.CompareTables(firstTables, secondTables);

            // Get stored procedures
            var firstProcedures = await _databaseCompererService.GetStoredProceduresWithContentAsync(firstConnection);
            var secondProcedures = await _databaseCompererService.GetStoredProceduresWithContentAsync(secondConnection);

            // Compare stored procedures
            var procedureDifferences = _databaseCompererService.CompareStoredProcedures(firstProcedures, secondProcedures);

            // Combine results
            var differences = tableDifferences.Concat(procedureDifferences).ToList();

            return Ok(differences);
        }
    }
}
