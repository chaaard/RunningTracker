using Microsoft.AspNetCore.Mvc;
using Moq;
using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Controllers;
using RunningTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                new User { Id = 1, Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) },
                new User { Id = 2, Name = "Jonel San Diego", Weight = 60, Height = 165, BirthDate = new DateTime(2000, 11, 1) }
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
        public async Task GetUserById_NonExistingId_ReturnsNotFound()
        {
            int userId = 99;
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _controller.GetUserById(userId);

            Assert.IsType<NotFoundResult>(result.Result);
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

        [Fact]
        public async Task UpdateUser_ExistingId_ReturnsNoContent()
        {
            var updateUserDto = new UserUpdateDto { Name = "Updated Richard Baluyut", Weight = 78, Height = 175, BirthDate = new DateTime(1997, 10, 21) };
            var updatedUser = new User { Id = 1, Name = "Updated Richard Baluyut", Weight = 78, Height = 175, BirthDate = new DateTime(1997, 10, 21) };
            _mockUserService.Setup(service => service.UpdateUserAsync(1, updateUserDto)).ReturnsAsync(updatedUser);

            var result = await _controller.UpdateUser(1, updateUserDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_NonExistingId_ReturnsNotFound()
        {
            var updateUserDto = new UserUpdateDto { Name = "Non-existing User", Weight = 60, Height = 165, BirthDate = new DateTime(2000, 11, 1) };
            _mockUserService.Setup(service => service.UpdateUserAsync(99, updateUserDto)).ReturnsAsync((User)null);

            var result = await _controller.UpdateUser(99, updateUserDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ExistingId_ReturnsNoContent()
        {
            int userId = 1;
            _mockUserService.Setup(service => service.DeleteUserAsync(userId)).ReturnsAsync(true);

            var result = await _controller.DeleteUser(userId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_NonExistingId_ReturnsNotFound()
        {
            int userId = 99;
            _mockUserService.Setup(service => service.DeleteUserAsync(userId)).ReturnsAsync(false);

            var result = await _controller.DeleteUser(userId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
