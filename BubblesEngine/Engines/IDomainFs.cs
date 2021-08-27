using System.Collections.Generic;
using System.Threading.Tasks;

namespace BubblesEngine.Engines
{
    public interface IDomainFs
    {
        public Task<bool> WriteFile(string path, string content);
        public Task<string> ReadFile(string path);
        public bool IsExists(string path);
        public bool CreateDirectory(string path);
        public List<string> ListDirectories(string path);
    }
}