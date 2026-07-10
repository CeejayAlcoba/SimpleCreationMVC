using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Pagination; // Import the new pagination namespace
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/control-number")]
    [ApiController]
    public class ControlNumberController : ControllerBase
    {
        private readonly IControlNumberService _controlNumberService;

        public ControlNumberController(IControlNumberService controlNumberService)
        {
            _controlNumberService = controlNumberService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] ControlNumber? filter = null)
        {
            try
            {
                // Expect and return a PagedResult instead of an IEnumerable
                PagedResult<ControlNumber> data = await _controlNumberService.GetAllAsync(pageNumber, pageSize, filter);
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
                ControlNumber? data = await _controlNumberService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]ControlNumber controlNumber)
        {
            try
            {
                ControlNumber? data = await _controlNumberService.InsertAsync(controlNumber);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]ControlNumber controlNumber)
        {
            try
            {
                if(id != controlNumber.Id) return BadRequest("Id mismatched.");

                ControlNumber? data = await _controlNumberService.GetByIdAsync(id);
                if (data == null) return NotFound();

                ControlNumber? updatedData = await _controlNumberService.UpdateAsync(controlNumber); 
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
                ControlNumber? data = await _controlNumberService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _controlNumberService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<ControlNumber> listData)
        {
            try
            {
                IEnumerable<ControlNumber> data = await _controlNumberService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<ControlNumber> listData)
        {
            try
            {
                IEnumerable<ControlNumber> data = await _controlNumberService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<ControlNumber> listData)
        {
            try
            {
                IEnumerable<ControlNumber> data = await _controlNumberService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<ControlNumber> listData)
        {
            try
            {
                IEnumerable<ControlNumber> data = await _controlNumberService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}