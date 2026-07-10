using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Pagination; // Import the new pagination namespace
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/authority")]
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly IAuthorityService _authorityService;

        public AuthorityController(IAuthorityService authorityService)
        {
            _authorityService = authorityService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] Authority? filter = null)
        {
            try
            {
                // Expect and return a PagedResult instead of an IEnumerable
                PagedResult<Authority> data = await _authorityService.GetAllAsync(pageNumber, pageSize, filter);
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
                Authority? data = await _authorityService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]Authority authority)
        {
            try
            {
                Authority? data = await _authorityService.InsertAsync(authority);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]Authority authority)
        {
            try
            {
                if(id != authority.Id) return BadRequest("Id mismatched.");

                Authority? data = await _authorityService.GetByIdAsync(id);
                if (data == null) return NotFound();

                Authority? updatedData = await _authorityService.UpdateAsync(authority); 
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
                Authority? data = await _authorityService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _authorityService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<Authority> listData)
        {
            try
            {
                IEnumerable<Authority> data = await _authorityService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<Authority> listData)
        {
            try
            {
                IEnumerable<Authority> data = await _authorityService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<Authority> listData)
        {
            try
            {
                IEnumerable<Authority> data = await _authorityService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<Authority> listData)
        {
            try
            {
                IEnumerable<Authority> data = await _authorityService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}