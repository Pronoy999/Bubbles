using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Engines.Implementations;
using Moq;
using Xunit;

namespace BubblesEngine.Tests.Engines
{
    public class FileWrapperTests
    {
        private readonly Mock<IDomainFs> _domainFs;
        private readonly FileWrapper _fileWrapper;

        public FileWrapperTests()
        {
            _domainFs = new Mock<IDomainFs>();
            _fileWrapper = new FileWrapper(_domainFs.Object);
        }

        [Fact]
        public async void ShouldCreateFileWithValidPath()
        {
            _domainFs.Setup(fs => fs.WriteFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var result = await _fileWrapper.CreateFile("some-path", "some-content");
            Assert.True(result);
        }
    }
}