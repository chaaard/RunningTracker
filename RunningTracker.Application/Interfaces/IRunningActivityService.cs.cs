using RunningTracker.Application.DTOs;
using RunningTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Application.Interfaces
{
    public interface IRunningActivityService
    {
        Task<IEnumerable<RunningActivity>> GetAllRunningActivitiesAsync();
        Task<RunningActivity> AddRunningActivityAsync(RunningActivityDto runningActivityDto);
        Task<RunningActivity> GetRunningActivityByIdAsync(int runningActivityId);
        Task<IEnumerable<RunningActivity>> GetRunningActivitiesByUserIdAsync(int userId);
        Task UpdateRunningActivityAsync(int runningActivityId, RunningActivityDto runningActivityDto);
        Task<bool> DeleteRunningActivityAsync(int runningActivityId);
    }
}
