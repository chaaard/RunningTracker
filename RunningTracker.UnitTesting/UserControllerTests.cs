using Microsoft.AspNetCore.Mvc;
using Moq;
using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Controllers;
using RunningTracker.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RunningTracker.UnitTesting
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOk()
        {
            var users = new List<User>
            {
                new User { Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) },
                new User { Name = "Jonel San Diego", Weight = 60, Height = 165, BirthDate = new DateTime(2000, 11, 1) }
            };

            _mockUserService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);

            var result = await _controller.GetUsers();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(users.Count, returnedUsers.Count());
        }

        [Fact]
        public async Task GetUserById_ExistingId_ReturnsOk()
        {
            int userId = 1;
            var user = new User { Id = userId, Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) };
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var result = await _controller.GetUserById(userId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task CreateUser_ValidInput_ReturnsCreatedAtAction()
        {
            var newUserDto = new UserCreateDto { Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) };
            var createdUser = new User { Id = 1, Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) };
            _mockUserService.Setup(service => service.CreateUserAsync(newUserDto)).ReturnsAsync(createdUser);

            var result = await _controller.CreateUser(newUserDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetUserById", createdAtActionResult.ActionName);
            Assert.Equal(createdUser.Id, createdAtActionResult.RouteValues["id"]);
        }
    }
}
