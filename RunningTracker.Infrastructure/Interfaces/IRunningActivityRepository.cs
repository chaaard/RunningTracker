using RunningTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Infrastructure.Interfaces
{
    public interface IRunningActivityRepository
    {
        Task<IEnumerable<RunningActivity>> GetAllRunningActivitiesAsync();
        Task AddRunningActivityAsync(RunningActivity runningActivity);
        Task<RunningActivity> GetRunningActivityByIdAsync(int runningActivityId);
        Task<IEnumerable<RunningActivity>> GetRunningActivitiesByUserIdAsync(int userId);
        Task UpdateRunningActivityAsync(RunningActivity runningActivity);
        Task<bool> DeleteRunningActivityAsync(RunningActivity runningActivity);
    }
}
