using Microsoft.EntityFrameworkCore;
using RunningTracker.Domain.Models;
using RunningTracker.Infrastructure.Data;
using RunningTracker.Infrastructure.Interfaces;
using Serilog;
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
            try
            {
                return await _context.RunningActivities
                    .Include(u => u.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all running activities");
                throw; 
            }
        }

        public async Task AddRunningActivityAsync(RunningActivity runningActivity)
        {
            try
            {
                await _context.RunningActivities.AddAsync(runningActivity);
                await _context.SaveChangesAsync();
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
                return await _context.RunningActivities
                    .Include(u => u.User)
                    .FirstOrDefaultAsync(u => u.Id == runningActivityId);
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
                return await _context.RunningActivities
                    .Where(ra => ra.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving running activities for user with ID {UserId}", userId);
                throw; 
            }
        }

        public async Task UpdateRunningActivityAsync(RunningActivity runningActivity)
        {
            try
            {
                _context.RunningActivities.Update(runningActivity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating running activity with ID {RunningActivityId}", runningActivity.Id);
                throw; 
            }
        }

        public async Task<bool> DeleteRunningActivityAsync(RunningActivity runningActivity)
        {
            try
            {
                _context.RunningActivities.Remove(runningActivity);
                int changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting running activity with ID {RunningActivityId}", runningActivity.Id);
                throw; 
            }
        }
    }
}
