using Microsoft.AspNetCore.Mvc;
using Moq;
using RunningTracker.Application.DTOs;
using RunningTracker.Application.Interfaces;
using RunningTracker.Controllers;
using RunningTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.UnitTesting
{
    public class RunningActivityControllerTests
    {
        private readonly Mock<IRunningActivityService> _mockRunningActivityService;
        private readonly RunningActivityController _controller;

        public RunningActivityControllerTests()
        {
            _mockRunningActivityService = new Mock<IRunningActivityService>();
            _controller = new RunningActivityController(_mockRunningActivityService.Object);
        }

        [Fact]
        public async Task GetAllRunningActivities_ReturnsOk()
        {
            var runningActivities = new List<RunningActivity>
            {
                new RunningActivity
                {
                    Id = 1,
                    Location = "S&R BGC",
                    StartDateTime = new DateTime(2024, 7, 15, 6, 0, 0),
                    EndDateTime = new DateTime(2024, 7, 15, 7, 0, 0),
                    Distance = 10.0,
                    UserId = 1,
                    User = new User { Id = 1, Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) }
                }
            };
            _mockRunningActivityService.Setup(service => service.GetAllRunningActivitiesAsync()).ReturnsAsync(runningActivities);

            var result = await _controller.GetAllRunningActivitiesAsync();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedActivities = Assert.IsAssignableFrom<IEnumerable<RunningActivity>>(okResult.Value);
            Assert.Equal(runningActivities.Count, returnedActivities.Count());
        }

        [Fact]
        public async Task GetRunningActivityById_ExistingId_ReturnsOk()
        {
            int activityId = 1;
            var runningActivity = new RunningActivity
            {
                Id = activityId,
                Location = "S&R BGC",
                StartDateTime = new DateTime(2024, 7, 15, 6, 0, 0),
                EndDateTime = new DateTime(2024, 7, 15, 7, 0, 0),
                Distance = 10.0,
                UserId = 1,
                User = new User { Id = 1, Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) }
            };
            _mockRunningActivityService.Setup(service => service.GetRunningActivityByIdAsync(activityId)).ReturnsAsync(runningActivity);

            var result = await _controller.GetRunningActivityById(activityId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedActivity = Assert.IsType<RunningActivity>(okResult.Value);
            Assert.Equal(activityId, returnedActivity.Id);
            Assert.Equal(runningActivity.Location, returnedActivity.Location);
            Assert.Equal(runningActivity.StartDateTime, returnedActivity.StartDateTime);
            Assert.Equal(runningActivity.EndDateTime, returnedActivity.EndDateTime);
            Assert.Equal(runningActivity.Distance, returnedActivity.Distance);
            Assert.Equal(runningActivity.UserId, returnedActivity.UserId);
            Assert.Equal(runningActivity.User.Name, returnedActivity.User.Name);
        }

        [Fact]
        public async Task CreateRunningActivity_ValidInput_ReturnsCreatedAtAction()
        {
            var newRunningActivityDto = new RunningActivityDto
            {
                Location = "Landmark BGC",
                StartDateTime = new DateTime(2024, 7, 16, 6, 0, 0),
                EndDateTime = new DateTime(2024, 7, 16, 6, 30, 0),
                Distance = 5.0,
                UserId = 2
            };
            var createdRunningActivity = new RunningActivity
            {
                Id = 2,
                Location = "Landmark BGC",
                StartDateTime = new DateTime(2024, 7, 16, 6, 0, 0),
                EndDateTime = new DateTime(2024, 7, 16, 6, 30, 0),
                Distance = 5.0,
                UserId = 2,
                User = new User { Id = 2, Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) }
            };
            _mockRunningActivityService.Setup(service => service.AddRunningActivityAsync(newRunningActivityDto)).ReturnsAsync(createdRunningActivity);

            var result = await _controller.AddRunningActivity(newRunningActivityDto);

            var actionResult = Assert.IsType<ActionResult<RunningActivity>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("GetRunningActivityById", createdAtActionResult.ActionName);
            Assert.Equal(createdRunningActivity.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdateRunningActivity_ExistingId_ReturnsNoContent()
        {
            var updatedRunningActivityDto = new RunningActivityDto
            {
                Location = "Uptown Mall BGC",
                StartDateTime = new DateTime(2024, 7, 15, 6, 0, 0),
                EndDateTime = new DateTime(2024, 7, 15, 7, 0, 0),
                Distance = 8.0,
                UserId = 1
            };
            var updatedRunningActivity = new RunningActivity
            {
                Id = 1,
                Location = "Uptown Mall BGC",
                StartDateTime = new DateTime(2024, 7, 15, 6, 0, 0),
                EndDateTime = new DateTime(2024, 7, 15, 7, 0, 0),
                Distance = 8.0,
                UserId = 1,
                User = new User { Id = 1, Name = "Richard Baluyut", Weight = 77, Height = 175, BirthDate = new DateTime(1997, 10, 21) }
            };
            _mockRunningActivityService.Setup(service => service.UpdateRunningActivityAsync(updatedRunningActivity.Id, updatedRunningActivityDto));

            var result = await _controller.UpdateRunningActivity(updatedRunningActivity.Id, updatedRunningActivityDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRunningActivity_ExistingId_ReturnsNoContent()
        {
            int activityId = 1;
            _mockRunningActivityService.Setup(service => service.DeleteRunningActivityAsync(activityId)).ReturnsAsync(true);

            var result = await _controller.DeleteRunningActivity(activityId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRunningActivity_NonExistingId_ReturnsNotFound()
        {
            int activityId = 1;
            _mockRunningActivityService.Setup(service => service.DeleteRunningActivityAsync(activityId)).ReturnsAsync(false);

            var result = await _controller.DeleteRunningActivity(activityId);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
