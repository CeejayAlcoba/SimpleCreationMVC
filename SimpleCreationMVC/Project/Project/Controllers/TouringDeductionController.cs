using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Pagination; // Import the new pagination namespace
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/touring-deduction")]
    [ApiController]
    public class TouringDeductionController : ControllerBase
    {
        private readonly ITouringDeductionService _touringDeductionService;

        public TouringDeductionController(ITouringDeductionService touringDeductionService)
        {
            _touringDeductionService = touringDeductionService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] TouringDeduction? filter = null)
        {
            try
            {
                // Expect and return a PagedResult instead of an IEnumerable
                PagedResult<TouringDeduction> data = await _touringDeductionService.GetAllAsync(pageNumber, pageSize, filter);
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
                TouringDeduction? data = await _touringDeductionService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]TouringDeduction touringDeduction)
        {
            try
            {
                TouringDeduction? data = await _touringDeductionService.InsertAsync(touringDeduction);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]TouringDeduction touringDeduction)
        {
            try
            {
                if(id != touringDeduction.Id) return BadRequest("Id mismatched.");

                TouringDeduction? data = await _touringDeductionService.GetByIdAsync(id);
                if (data == null) return NotFound();

                TouringDeduction? updatedData = await _touringDeductionService.UpdateAsync(touringDeduction); 
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
                TouringDeduction? data = await _touringDeductionService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _touringDeductionService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<TouringDeduction> listData)
        {
            try
            {
                IEnumerable<TouringDeduction> data = await _touringDeductionService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<TouringDeduction> listData)
        {
            try
            {
                IEnumerable<TouringDeduction> data = await _touringDeductionService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<TouringDeduction> listData)
        {
            try
            {
                IEnumerable<TouringDeduction> data = await _touringDeductionService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<TouringDeduction> listData)
        {
            try
            {
                IEnumerable<TouringDeduction> data = await _touringDeductionService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}