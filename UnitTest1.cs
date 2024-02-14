using EisenringDamianLB_295.Controllers;
using EisenringDamianLB_295.Context;
using EisenringDamianLB_295.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace EisenringDamianLB_295.Tests
{
    [TestClass]
    public class AuthControllerTests
    {
        private AuthController _controller;
        private Mock<IConfiguration> _mockConfiguration;
        private ApplicationDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated(); // Sicherstellen, dass die Datenbank erstellt wurde

            _mockConfiguration = new Mock<IConfiguration>();
            _controller = new AuthController(_mockConfiguration.Object, _dbContext);
        }

        [TestMethod]
        public void Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var newUserDto = new UserDto { Username = "testuser", Password = "password" };

            // Act
            var result = _controller.Register(newUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        
    }
}
