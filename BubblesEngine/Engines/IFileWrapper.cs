using System.Collections.Generic;
using System.Threading.Tasks;

namespace BubblesEngine.Engines
{
    public interface IFileWrapper
    {
        public Task<bool> CreateFile(string path, string contents);
        public Task<string> GetFileContents(string path);
        public bool IsExists(string path);
        public bool IsDirectoryExists(string path);
        public bool CreateFolder(string path);
        public List<string> GetDirectories(string path);
        public List<string> GetAllFilesNames(string path);
        public List<string> GetAllFiles(string path);
        public string SearchFiles(string path, string fileName);
    }
}