using System.Threading.Tasks;

namespace BubblesEngine.Engines
{
    public interface IFileWrapper
    {
        public Task<bool> CreateFile(string path, string contents);
        public Task<string> GetFileContents(string path);
        public bool IsExists(string path);
        public bool CreateFolder(string path);
    }
}