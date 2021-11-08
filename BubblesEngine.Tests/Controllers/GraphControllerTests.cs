using System;
using System.Collections.Generic;
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
    public class GraphControllerTests
    {
        private readonly Mock<IFileWrapper> _fileWrapper;
        private readonly Mock<IDomainFs> _domainFs;
        private readonly GraphController _controller;
        private readonly string someUserId = "some-user-id";

        public GraphControllerTests()
        {
            _domainFs = new Mock<IDomainFs>();
            _fileWrapper = new Mock<IFileWrapper>();
            _controller = new GraphController(_fileWrapper.Object);
            Environment.SetEnvironmentVariable(Constants.DbRootFolderKey, "/some-folder");
            Environment.SetEnvironmentVariable(Constants.TypesFolderName, "types");
        }

        #region CreateGraph

        [Fact]
        public void ShouldCreateGraphWhenValidDatabaseIsPassed()
        {
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            var result = _controller.CreateGraph("some-graph", "some-database", someUserId);
            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateGraphWhenAlreadyExists()
        {
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            var result = _controller.CreateGraph("some-graph", "some-database", someUserId);
            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
            _domainFs.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region GetGraph

        [Fact]
        public void ShouldReturnGraphWhenValidDatabaseAndGraphNameIsPassed()
        {
            const string graphName = "some-graph";
            const string databaseName = "some-db";

            var listOfNodes = new List<string>
            {
                "guid-1.json",
                "guid-2.json"
            };
            var expectedGraph = new Graph
            {
                GraphName = graphName,
                Nodes = new List<Node>
                {
                    new Node
                    {
                        Id = "guid-1"
                    },
                    new Node
                    {
                        Id = "guid-2"
                    }
                }
            };
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetAllFilesNames(It.IsAny<string>())).Returns(listOfNodes);
            var actualGraph = _controller.GetGraph(databaseName, graphName, someUserId);
            var expected = JsonConvert.SerializeObject(expectedGraph);
            var actual = JsonConvert.SerializeObject(actualGraph);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldThrowExceptionWhenInvalidGraphNameIsPassed()
        {
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            Assert.Throws<BubblesException>(() => _controller.GetGraph(null, null, null));
        }

        #endregion
    }
}