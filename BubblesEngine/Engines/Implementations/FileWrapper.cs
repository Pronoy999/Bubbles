using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BubblesEngine.Engines.Implementations
{
    public class FileWrapper : IFileWrapper
    {
        private readonly IDomainFs _domainFs;

        public FileWrapper(IDomainFs domainFs)
        {
            _domainFs = domainFs;
        }

        public async Task<bool> CreateFile(string path, string contents)
        {
            return await _domainFs.WriteFile(path, contents);
        }

        public async Task<string> GetFileContents(string path)
        {
            return await _domainFs.ReadFile(path);
        }

        public bool IsExists(string path)
        {
            return _domainFs.IsExists(path);
        }

        public bool CreateFolder(string path)
        {
            return _domainFs.IsExists(path) || _domainFs.CreateDirectory(path);
        }

        public List<string> GetDirectories(string path)
        {
            var directoryNames = new List<string>();
            var directories = _domainFs.ListDirectories(path);
            if (directories != null)
                foreach (var oneDir in directories){
                    var names = oneDir.Split(Path.DirectorySeparatorChar);
                    directoryNames.Add(names[^1]);
                }

            return directoryNames;
        }

        public List<string> GetFiles(string path)
        {
            var filesNames = new List<string>();
            var files = _domainFs.ListFiles(path);
            if (files != null)
                foreach (var oneFile in files){
                    var names = oneFile.Split(Path.DirectorySeparatorChar);
                    filesNames.Add(names[^1]);
                }

            return filesNames;
        }

        public string SearchFiles(string path, string fileName)
        {
            var files = _domainFs.SearchFiles(path, "*.json");
            if (files == null) return string.Empty;
            foreach (var oneFile in files){
                var name = oneFile.Split(Path.DirectorySeparatorChar)[^1];
                if (name.Equals(fileName))
                    return oneFile;
            }

            return string.Empty;
        }
    }
}