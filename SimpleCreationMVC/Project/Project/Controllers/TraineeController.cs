using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Pagination; // Import the new pagination namespace
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/trainee")]
    [ApiController]
    public class TraineeController : ControllerBase
    {
        private readonly ITraineeService _traineeService;

        public TraineeController(ITraineeService traineeService)
        {
            _traineeService = traineeService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] Trainee? filter = null)
        {
            try
            {
                // Expect and return a PagedResult instead of an IEnumerable
                PagedResult<Trainee> data = await _traineeService.GetAllAsync(pageNumber, pageSize, filter);
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
                Trainee? data = await _traineeService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]Trainee trainee)
        {
            try
            {
                Trainee? data = await _traineeService.InsertAsync(trainee);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]Trainee trainee)
        {
            try
            {
                if(id != trainee.Id) return BadRequest("Id mismatched.");

                Trainee? data = await _traineeService.GetByIdAsync(id);
                if (data == null) return NotFound();

                Trainee? updatedData = await _traineeService.UpdateAsync(trainee); 
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
                Trainee? data = await _traineeService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _traineeService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<Trainee> listData)
        {
            try
            {
                IEnumerable<Trainee> data = await _traineeService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<Trainee> listData)
        {
            try
            {
                IEnumerable<Trainee> data = await _traineeService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<Trainee> listData)
        {
            try
            {
                IEnumerable<Trainee> data = await _traineeService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<Trainee> listData)
        {
            try
            {
                IEnumerable<Trainee> data = await _traineeService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}