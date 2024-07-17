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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllUsersAsync();
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
                return await _userRepository.GetUserByIdAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving user with ID {UserId}", id);
                throw;
            }
        }

        public async Task<User> CreateUserAsync(UserCreateDto userCreateDto)
        {
            try
            {
                var user = new User
                {
                    Name = userCreateDto.Name,
                    Weight = userCreateDto.Weight,
                    Height = userCreateDto.Height,
                    BirthDate = userCreateDto.BirthDate
                };

                Log.Information("User created with ID {UserId}", user.Id);
                return await _userRepository.CreateUserAsync(user);
               
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating user");
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    throw new ArgumentException($"User with ID {id} not found.");
                }

                if (!string.IsNullOrEmpty(userUpdateDto.Name))
                {
                    user.Name = userUpdateDto.Name;
                }

                if (userUpdateDto.Weight.HasValue)
                {
                    user.Weight = userUpdateDto.Weight.Value;
                }

                if (userUpdateDto.Height.HasValue)
                {
                    user.Height = userUpdateDto.Height.Value;
                }

                if (userUpdateDto.BirthDate.HasValue)
                {
                    user.BirthDate = userUpdateDto.BirthDate.Value;
                }

                Log.Information("User with ID {UserId} updated", id);
                return await _userRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating user with ID {UserId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var result = await _userRepository.DeleteUserAsync(id);
                if (!result)
                {
                    throw new ArgumentException($"User with ID {id} not found.");
                }

                Log.Information("User with ID {UserId} deleted", id);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting user with ID {UserId}", id);
                throw;
            }
        }
    }
}
