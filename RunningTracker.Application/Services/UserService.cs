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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(UserCreateDto userCreateDto)
        {
            var user = new User
            {
                Name = userCreateDto.Name,
                Weight = userCreateDto.Weight,
                Height = userCreateDto.Height,
                BirthDate = userCreateDto.BirthDate
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<User> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

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

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}
