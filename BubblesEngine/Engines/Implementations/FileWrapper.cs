using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (var oneDir in directories){
                var names = oneDir.Split(Path.DirectorySeparatorChar);
                directoryNames.Add(names[^1]);
            }

            return directoryNames;
        }
    }
}