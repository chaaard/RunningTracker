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
    public class UserRepository : IUserRepository
    {
        private readonly RunningTrackerContext _context;

        public UserRepository(RunningTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users
                    .Include(u => u.RunningActivities)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all users");
                throw; 
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.RunningActivities)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving user with ID {UserId}", id);
                throw; 
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating user");
                throw; 
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating user with ID {UserId}", user.Id);
                throw; 
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return false;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting user with ID {UserId}", id);
                throw; 
            }
        }
    }
}
