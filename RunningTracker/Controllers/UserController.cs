using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Domain.Models;

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
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("GetUserById{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> CreateUser(UserCreateDto userProfileCreateDto)
        {
            var user = await _userService.CreateUserAsync(userProfileCreateDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("UpdateUser{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userProfileUpdateDto)
        {
            var user = await _userService.UpdateUserAsync(id, userProfileUpdateDto);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("DeleteUser{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
