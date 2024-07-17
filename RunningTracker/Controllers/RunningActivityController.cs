using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Application.Services;
using RunningTracker.Domain.Models;

namespace RunningTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunningActivityController : ControllerBase
    {
        private readonly IRunningActivityService _runningActivityService;

        public RunningActivityController(IRunningActivityService runningActivityService)
        {
            _runningActivityService = runningActivityService;
        }

        [HttpGet("GetAllRunningActivities")]
        public async Task<ActionResult<IEnumerable<RunningActivity>>> GetAllRunningActivitiesAsync()
        {
            return Ok(await _runningActivityService.GetAllRunningActivitiesAsync());
        }

        [HttpGet("GetRunningActivityById{id}")]
        public async Task<ActionResult<RunningActivity>> GetRunningActivity(int id)
        {
            var runningActivity = await _runningActivityService.GetRunningActivityByIdAsync(id);
            if (runningActivity == null)
            {
                return NotFound();
            }
            return Ok(runningActivity);
        }

        [HttpPost("AddRunningActivity")]
        public async Task<ActionResult<RunningActivity>> AddRunningActivity(RunningActivityDto runningActivityDto)
        {
            try
            {
                var addedRunningActivity = await _runningActivityService.AddRunningActivityAsync(runningActivityDto);
                return CreatedAtAction(nameof(GetRunningActivity), new { id = addedRunningActivity.Id }, addedRunningActivity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateRunningActivity{id}")]
        public async Task<IActionResult> UpdateRunningActivity(int id, RunningActivityDto runningActivityDto)
        {
            try
            {
                await _runningActivityService.UpdateRunningActivityAsync(id, runningActivityDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteRunningActivity{id}")]
        public async Task<IActionResult> DeleteRunningActivity(int id)
        {
            await _runningActivityService.DeleteRunningActivityAsync(id);
            return NoContent();
        }
    }
}
