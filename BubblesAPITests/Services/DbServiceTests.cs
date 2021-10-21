using AutoMapper;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesAPI.Services.Implementation;
using BubblesEngine.Controllers;
using BubblesEngine.Exceptions;
using BubblesEngine.Models;
using Moq;
using Xunit;

namespace BubblesAPITests.Services
{
    public class DbServiceTests
    {
        private readonly Mock<IDbController> _dbController;
        private readonly IDbService _dbService;
        private readonly Mock<IMapper> _mapper;

        public DbServiceTests()
        {
            _dbController = new Mock<IDbController>();
            _mapper = new Mock<IMapper>();
            _dbService = new DbService(_dbController.Object, _mapper.Object);
        }

        [Fact]
        public void ShouldCreateDbWithValidData()
        {
            _dbController.Setup(db => db.CreateDatabase(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var result = _dbService.CreateDb("some-db-name", "some-user-id");
            Assert.True(result);
        }

        [Fact]
        public void ShouldReturnDbWhenDbExists()
        {
            var response = new Database()
            {
                DatabaseName = "some-db-name"
            };
            _dbController.Setup(db => db.GetDatabase(It.IsAny<string>(), It.IsAny<string>())).Returns(response);
            _mapper.Setup(m => m.Map<DatabaseResponse>(It.IsAny<object>())).Returns(new DatabaseResponse()
                { DatabaseName = response.DatabaseName });

            var result = _dbService.GetDb("some-db-name", "some-user-id");

            Assert.Equal(response.DatabaseName, result.DatabaseName);
        }

        [Fact]
        public void ShouldThrowErrorWhenDatabaseDoesNotExists()
        {
            _dbController.Setup(db => db.GetDatabase(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<DatabaseNotFoundException>();
            Assert.Throws<DatabaseNotFoundException>(() => _dbService.GetDb(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}