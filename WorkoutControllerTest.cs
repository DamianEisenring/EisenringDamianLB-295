using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EisenringDamianLB_295.Controllers;
using EisenringDamianLB_295.Context;
using EisenringDamianLB_295.Models;

namespace EisenringDamianLB_295.Tests
{
    [TestClass]
    public class WorkoutControllerTests
    {
        private WorkoutController _controller;
        private Mock<ApplicationDbContext> _mockContext;

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new WorkoutController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetWorkoutById_ExistingId_ReturnsWorkout()
        {
            // Arrange
            int existingWorkoutId = 1;
            var existingWorkout = new Workout { Id = existingWorkoutId, NamePerson = "John", Date = DateTime.Now, Duration = 60, CaloriesBurned = 300 };
            _mockContext.Setup(c => c.Workouts.FindAsync(existingWorkoutId)).ReturnsAsync(existingWorkout);

            // Act
            var result = await _controller.GetWorkoutById(existingWorkoutId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(existingWorkout, okResult.Value);
        }

        [TestMethod]
        public async Task GetExerciseById_ExistingId_ReturnsExercise()
        {
            // Arrange
            int existingExerciseId = 1;
            var existingExercise = new Exercise { Id = existingExerciseId, Name = "Push-up", Category = "Strength" };
            _mockContext.Setup(c => c.Exercises.FindAsync(existingExerciseId)).ReturnsAsync(existingExercise);

            // Act
            var result = await _controller.GetExerciseById(existingExerciseId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(existingExercise, okResult.Value);
        }
    }
}
