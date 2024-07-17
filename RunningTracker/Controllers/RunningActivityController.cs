using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Application.Services;
using RunningTracker.Domain.Models;
using Serilog;

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
            try
            {
                var runningActivities = await _runningActivityService.GetAllRunningActivitiesAsync();
                Log.Information("Retrieved all running activities");
                return Ok(runningActivities);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all running activities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetRunningActivityById/{id}")]
        public async Task<ActionResult<RunningActivity>> GetRunningActivity(int id)
        {
            try
            {
                var runningActivity = await _runningActivityService.GetRunningActivityByIdAsync(id);

                if (runningActivity == null)
                {
                    Log.Warning("Running activity with ID {RunningActivityId} not found", id);
                    return NotFound();
                }

                Log.Information("Retrieved running activity with ID {RunningActivityId}", id);
                return Ok(runningActivity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving running activity with ID {RunningActivityId}", id);
                return StatusCode(500, "Internal server error");
            }
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
                Log.Warning("Error adding running activity: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding running activity");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateRunningActivity/{id}")]
        public async Task<IActionResult> UpdateRunningActivity(int id, RunningActivityDto runningActivityDto)
        {
            try
            {
                await _runningActivityService.UpdateRunningActivityAsync(id, runningActivityDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Log.Warning("Error updating running activity: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating running activity with ID {RunningActivityId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteRunningActivity/{id}")]
        public async Task<IActionResult> DeleteRunningActivity(int id)
        {
            try
            {
                await _runningActivityService.DeleteRunningActivityAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting running activity with ID {RunningActivityId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
