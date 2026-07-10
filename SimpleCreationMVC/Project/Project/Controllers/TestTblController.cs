using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Pagination; // Import the new pagination namespace
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/test-tbl")]
    [ApiController]
    public class TestTblController : ControllerBase
    {
        private readonly ITestTblService _testTblService;

        public TestTblController(ITestTblService testTblService)
        {
            _testTblService = testTblService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] TestTbl? filter = null)
        {
            try
            {
                // Expect and return a PagedResult instead of an IEnumerable
                PagedResult<TestTbl> data = await _testTblService.GetAllAsync(pageNumber, pageSize, filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                TestTbl? data = await _testTblService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]TestTbl testTbl)
        {
            try
            {
                TestTbl? data = await _testTblService.InsertAsync(testTbl);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]TestTbl testTbl)
        {
            try
            {
                if(id != testTbl.Id) return BadRequest("Id mismatched.");

                TestTbl? data = await _testTblService.GetByIdAsync(id);
                if (data == null) return NotFound();

                TestTbl? updatedData = await _testTblService.UpdateAsync(testTbl); 
                return Ok(updatedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                TestTbl? data = await _testTblService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _testTblService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<TestTbl> listData)
        {
            try
            {
                IEnumerable<TestTbl> data = await _testTblService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<TestTbl> listData)
        {
            try
            {
                IEnumerable<TestTbl> data = await _testTblService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<TestTbl> listData)
        {
            try
            {
                IEnumerable<TestTbl> data = await _testTblService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<TestTbl> listData)
        {
            try
            {
                IEnumerable<TestTbl> data = await _testTblService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}