using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Domain.Models;
using Serilog;

namespace RunningTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                Log.Information("Retrieved all users");
                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all users");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    Log.Warning("User with ID {UserId} not found", id);
                    return NotFound();
                }

                Log.Information("Retrieved user with ID {UserId}", id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving user with ID {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> CreateUser(UserCreateDto userProfileCreateDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(userProfileCreateDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userProfileUpdateDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, userProfileUpdateDto);
                if (user == null)
                {
                    Log.Warning("User with ID {UserId} not found", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating user with ID {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    Log.Warning("User with ID {UserId} not found", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
