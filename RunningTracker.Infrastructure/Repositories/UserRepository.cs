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
    public class UserRepository : IUserRepository
    {
        private readonly RunningTrackerContext _context;

        public UserRepository(RunningTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.RunningActivities)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.RunningActivities)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
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

        private Task<bool> UserExistsAsync(int id)
        {
            return _context.Users.AnyAsync(e => e.Id == id);
        }
    }
}
