using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Helpers;
using BubblesEngine.Models;
using dotenv.net.Utilities;
using Newtonsoft.Json;

namespace BubblesEngine.Controllers.Implementation
{
    public class DbController : IDbController
    {
        private readonly IFileWrapper _fileWrapper;

        public DbController(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        private string GetDatabaseLocation(string databaseName)
        {
            return EnvReader.GetStringValue(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar +
                   databaseName;
        }

        private string GetGraphLocation(string databaseName, string graphName)
        {
            return GetDatabaseLocation(databaseName) + Path.DirectorySeparatorChar + graphName;
        }

        private string GetTypeLocation(string databaseName, string graphName, string type)
        {
            return GetGraphLocation(databaseName, graphName) + Path.DirectorySeparatorChar +
                   EnvReader.GetStringValue(Constants.TypesFolderName) + Path.DirectorySeparatorChar + type;
        }

        private string GetNodeLocation(string database, string graphName, string nodeId)
        {
            return GetGraphLocation(database, graphName) + Path.DirectorySeparatorChar + nodeId;
        }

        public bool CreateDatabase(string databaseName)
        {
            var databasePath = GetDatabaseLocation(databaseName);
            return _fileWrapper.CreateFolder(databasePath);
        }

        public bool CreateGraph(string graphName, string databaseName)
        {
            var graphPath = GetGraphLocation(databaseName, graphName);
            return _fileWrapper.CreateFolder(graphPath);
        }

        public Task<bool> CreateNode(string databaseName, string graphName, string type, dynamic data)
        {
            var typesFolderPath = GetTypeLocation(databaseName, graphName, type);
            if (!_fileWrapper.IsExists(typesFolderPath)){
                _fileWrapper.CreateFolder(typesFolderPath);
            }

            var nodesFilePath = GetNodeLocation(databaseName, graphName, Utils.GenerateNodeId(graphName));
            return _fileWrapper.CreateFile(nodesFilePath, JsonConvert.SerializeObject(data));
        }

        public Database GetDatabase(string databaseName)
        {
            var dbPath = GetDatabaseLocation(databaseName);
            if (!_fileWrapper.IsExists(dbPath)) return null;
            var graphNames = _fileWrapper.GetDirectories(dbPath);
            var graphs = graphNames.Select(oneGraph => new Graph { GraphName = oneGraph }).ToList();
            return new Database
            {
                DatabaseName = databaseName,
                Graphs = graphs
            };
        }

        public Graph GetGraph(string graphName)
        {
            throw new System.NotImplementedException();
        }

        public Node GetNode(string nodeId)
        {
            throw new System.NotImplementedException();
        }
    }
}