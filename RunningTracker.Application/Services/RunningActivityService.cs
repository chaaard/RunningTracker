using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Domain.Models;
using RunningTracker.Infrastructure.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Application.Services
{
    public class RunningActivityService : IRunningActivityService
    {
        private readonly IRunningActivityRepository _runningActivityRepository;

        public RunningActivityService(IRunningActivityRepository runningActivityRepository)
        {
            _runningActivityRepository = runningActivityRepository;
        }

        public async Task<IEnumerable<RunningActivity>> GetAllRunningActivitiesAsync()
        {
            try
            {
                return await _runningActivityRepository.GetAllRunningActivitiesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all running activities");
                throw;
            }
        }

        public async Task<RunningActivity> AddRunningActivityAsync(RunningActivityDto runningActivityDto)
        {
            try
            {
                // Handle null EndDateTime scenario
                if (runningActivityDto.EndDateTime == null)
                {
                    runningActivityDto.EndDateTime = runningActivityDto.StartDateTime;
                }

                var runningActivity = new RunningActivity
                {
                    StartDateTime = runningActivityDto.StartDateTime,
                    EndDateTime = runningActivityDto.EndDateTime.Value,
                    Distance = runningActivityDto.Distance,
                    Location = runningActivityDto.Location,
                    UserId = runningActivityDto.UserId
                };

                await _runningActivityRepository.AddRunningActivityAsync(runningActivity);

                Log.Information("Added running activity with ID {RunningActivityId}", runningActivity.Id);

                return runningActivity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding running activity");
                throw;
            }
        }

        public async Task<RunningActivity> GetRunningActivityByIdAsync(int runningActivityId)
        {
            try
            {
                return await _runningActivityRepository.GetRunningActivityByIdAsync(runningActivityId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving running activity with ID {RunningActivityId}", runningActivityId);
                throw;
            }
        }

        public async Task<IEnumerable<RunningActivity>> GetRunningActivitiesByUserIdAsync(int userId)
        {
            try
            {
                return await _runningActivityRepository.GetRunningActivitiesByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving running activities for user with ID {UserId}", userId);
                throw;
            }
        }

        public async Task UpdateRunningActivityAsync(int runningActivityId, RunningActivityDto runningActivityDto)
        {
            try
            {
                var runningActivity = await _runningActivityRepository.GetRunningActivityByIdAsync(runningActivityId);
                if (runningActivity == null)
                {
                    throw new ArgumentException($"Running activity with ID {runningActivityId} not found.");
                }

                runningActivity.StartDateTime = runningActivityDto.StartDateTime;
                runningActivity.EndDateTime = runningActivityDto.EndDateTime.Value;
                runningActivity.Distance = runningActivityDto.Distance;
                runningActivity.Location = runningActivityDto.Location;

                await _runningActivityRepository.UpdateRunningActivityAsync(runningActivity);

                Log.Information("Updated running activity with ID {RunningActivityId}", runningActivityId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating running activity with ID {RunningActivityId}", runningActivityId);
                throw;
            }
        }

        public async Task<bool> DeleteRunningActivityAsync(int runningActivityId)
        {
            try
            {
                var existingActivity = await _runningActivityRepository.GetRunningActivityByIdAsync(runningActivityId);
                if (existingActivity == null)
                {
                    throw new ArgumentException($"Running activity with ID {runningActivityId} not found.");
                }

                var deleted = await _runningActivityRepository.DeleteRunningActivityAsync(existingActivity);

                Log.Information("Deleted running activity with ID {RunningActivityId}", runningActivityId);

                return deleted;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting running activity with ID {RunningActivityId}", runningActivityId);
                throw;
            }
        }
    }
}
