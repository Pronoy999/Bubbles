using System.IO;
using System.Linq;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
using BubblesEngine.Helpers;
using BubblesEngine.Models;

namespace BubblesEngine.Controllers.Implementation
{
    public class DatabaseController : IDatabaseController
    {
        private readonly IFileWrapper _fileWrapper;

        public DatabaseController(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public bool CreateDatabase(string databaseName, string userId)
        {
            var databasePath = Utils.GetDatabaseLocation(databaseName, userId);
            return _fileWrapper.CreateFolder(databasePath);
        }

        public Database GetDatabase(string databaseName, string userId)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new BubblesException(new DatabaseNotFoundException());
            var dbPath = Utils.GetDatabaseLocation(databaseName, userId);
            if (!_fileWrapper.IsDirectoryExists(dbPath)) throw new BubblesException(new DatabaseNotFoundException());
            var graphPath = dbPath + Path.DirectorySeparatorChar + Constants.GraphFolderName;
            var graphNames = _fileWrapper.GetDirectories(graphPath);

            var graphs = graphNames.Select(oneGraph => new Graph { GraphName = oneGraph }).ToList();
            return new Database
            {
                DatabaseName = databaseName,
                Graphs = graphs
            };
        }
    }
}