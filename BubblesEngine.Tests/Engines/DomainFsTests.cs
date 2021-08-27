using System;
using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Engines.Implementations;
using Moq;
using Xunit;

namespace BubblesEngine.Tests.Engines
{
    public class DomainFsTests
    {
        private readonly Mock<IDomainFs> _domainFs;

        public DomainFsTests()
        {
            _domainFs = new Mock<IDomainFs>();
        }

        [Fact]
        public async Task ShouldCreateAFileWhenValidPathAndContentIsProvided()
        {
            _domainFs.Setup(fs => fs.WriteFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var domainFs = new DomainFs();
            var result = await domainFs.WriteFile("some-path", "some-content");
            Assert.True(result);
        }

        [Fact]
        public async Task ShouldNotCreateAFileWhenInvalidPathIsProvided()
        {
            _domainFs.Setup(fs => fs.WriteFile(null, null)).ReturnsAsync(false);
            var domainFs = new DomainFs();
            var result = await domainFs.WriteFile(null, null);
            Assert.False(result);
        }

        [Fact]
        public async Task ShouldReturnContentsOfAValidFile()
        {
            const string someContent = "some-content";
            _domainFs.Setup(fs => fs.ReadFile(It.IsAny<string>())).ReturnsAsync(someContent);
            var fs = new DomainFs();
            var result = await fs.ReadFile("some-path");
            Assert.Equal(someContent, result);
        }

        [Fact]
        public async Task ShouldReturnNullOfInvalidFilePathToRead()
        {
            _domainFs.Setup(fs => fs.ReadFile(null)).ReturnsAsync(null as Func<string>);
            var fs = new DomainFs();
            var result = await fs.ReadFile(null);
            Assert.Null(result);
        }
    }
}