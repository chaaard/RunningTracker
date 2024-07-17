using Microsoft.EntityFrameworkCore;
using RunningTracker.Domain.Models;
using RunningTracker.Infrastructure.Data;
using RunningTracker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Infrastructure.Repositories
{
    public class RunningActivityRepository : IRunningActivityRepository
    {
        private readonly RunningTrackerContext _context;

        public RunningActivityRepository(RunningTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RunningActivity>> GetAllRunningActivitiesAsync()
        {
            return await _context.RunningActivities
                .Include(u => u.User)
                .ToListAsync();
        }

        public async Task AddRunningActivityAsync(RunningActivity runningActivity)
        {
            await _context.RunningActivities.AddAsync(runningActivity);
            await _context.SaveChangesAsync();
        }

        public async Task<RunningActivity> GetRunningActivityByIdAsync(int runningActivityId)
        {
            return await _context.RunningActivities
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == runningActivityId);
        }

        public async Task<IEnumerable<RunningActivity>> GetRunningActivitiesByUserIdAsync(int userId)
        {
            return await _context.RunningActivities.Where(ra => ra.UserId == userId).ToListAsync();
        }

        public async Task UpdateRunningActivityAsync(RunningActivity runningActivity)
        {
            _context.RunningActivities.Update(runningActivity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRunningActivityAsync(RunningActivity runningActivity)
        {
            _context.RunningActivities.Remove(runningActivity);
            int changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}
