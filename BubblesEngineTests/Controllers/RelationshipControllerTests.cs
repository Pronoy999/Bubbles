using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BubblesEngine.Controllers.Implementation;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
using BubblesEngine.Helpers;
using BubblesEngine.Models;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BubblesEngine.Tests.Controllers
{
    public class RelationshipControllerTests
    {
        private readonly Mock<IFileWrapper> _fileWrapper;
        private readonly RelationshipController _relationshipController;
        private readonly string someUserId = "some-user-id";

        public RelationshipControllerTests()
        {
            _fileWrapper = new Mock<IFileWrapper>();
            _relationshipController = new RelationshipController(_fileWrapper.Object);
            Environment.SetEnvironmentVariable(Constants.DbRootFolderKey, "/some-folder");
            Environment.SetEnvironmentVariable(Constants.TypesFolderName, "types");
        }

        #region Relationship

        [Fact]
        public async void ShouldReturnRelationshipDataWhenValidIdIsPassed()
        {
            const string relationshipId = "guid-1";
            var relationship = new Relationship
            {
                Id = relationshipId,
                Data = "{}",
                LeftNodeId = "guid-node-1",
                RightNodeId = "guid-right-1",
                Type = "Is_Brother_of"
            };
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>()))
                .ReturnsAsync(JsonConvert.SerializeObject(relationship));

            var result = await _relationshipController.GetRelationship("some-db", relationshipId, someUserId);
            var expected = JsonConvert.SerializeObject(relationship);

            var actual = JsonConvert.SerializeObject(result);
            Assert.Equal(expected, actual);
            _fileWrapper.Verify(fs => fs.IsExists(It.IsAny<string>()), Times.Exactly(1));
            _fileWrapper.Verify(fs => fs.IsDirectoryExists(It.IsAny<string>()), Times.Exactly(1));
            _fileWrapper.Verify(fs => fs.GetFileContents(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowErrorWhenRelationshipDoesNotExists()
        {
            const string dbLocation = "/some-folder/some-db";
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == dbLocation))).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            await Assert.ThrowsAsync<BubblesException>(() => _relationshipController.GetRelationship("some-db",
                "guid-1", someUserId));
        }

        [Fact]
        public void ShouldGetAllRelationships()
        {
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.IsAny<string>())).Returns(true);
            var listOfFiles = new List<string>
            {
                "some-location/guid-1",
                "some-location/guid-2"
            };
            var listOfRelationships = new List<string>()
            {
                "guid-1",
                "guid-2"
            };
            _fileWrapper.Setup(fs => fs.GetAllFiles(It.IsAny<string>())).Returns(listOfFiles);

            var result = _relationshipController.GetAllRelationships("some-db", "some-user-id");

            Assert.NotEmpty(result);
            Assert.Equal(listOfRelationships.Count, result.Count);
            Assert.Equal(listOfRelationships[0], result[0]);
        }

        [Fact]
        public void ShouldThrowExceptionWhenDatabaseNotExists()
        {
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.IsAny<string>())).Returns(false);
            Assert.Throws<BubblesException>(
                () => _relationshipController.GetAllRelationships("some-db", "some-user-id"));
        }

        #endregion
    }
}