using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Domain.Models;
using RunningTracker.Infrastructure.Interfaces;
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
            return await _runningActivityRepository.GetAllRunningActivitiesAsync();
        }

        public async Task<RunningActivity> AddRunningActivityAsync(RunningActivityDto runningActivityDto)
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

            return runningActivity;
        }

        public async Task<RunningActivity> GetRunningActivityByIdAsync(int runningActivityId)
        {
            return await _runningActivityRepository.GetRunningActivityByIdAsync(runningActivityId);
        }

        public async Task<IEnumerable<RunningActivity>> GetRunningActivitiesByUserIdAsync(int userId)
        {
            return await _runningActivityRepository.GetRunningActivitiesByUserIdAsync(userId);
        }

        public async Task UpdateRunningActivityAsync(int runningActivityId, RunningActivityDto runningActivityDto)
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

            double duration = runningActivity.Duration;
            double averagePace = runningActivity.AveragePace;
        }

        public async Task<bool> DeleteRunningActivityAsync(int runningActivityId)
        {
            var existingActivity = await _runningActivityRepository.GetRunningActivityByIdAsync(runningActivityId);
            if (existingActivity == null)
            {
                throw new ArgumentException($"Running activity with ID {runningActivityId} not found.");
            }

            return await _runningActivityRepository.DeleteRunningActivityAsync(existingActivity);
        }
    }
}
