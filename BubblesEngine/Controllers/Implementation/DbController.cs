using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
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
            return GetDatabaseLocation(databaseName) + Path.DirectorySeparatorChar + Constants.GraphFolderName +
                   Path.DirectorySeparatorChar + graphName;
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

        private async Task CheckAndCreateTypeFile(string location, string typeName, string nodeId)
        {
            if (!_fileWrapper.IsExists(location)){
                _fileWrapper.CreateFolder(location);
            }

            var typeFileLocation = location + Path.DirectorySeparatorChar + typeName + "." + Constants.FileExtension;
            if (!_fileWrapper.IsExists(typeFileLocation)){
                var type = new Type
                {
                    NodeIds = new List<string>
                    {
                        nodeId
                    },
                    TypeName = typeName
                };
                var isCreated = await _fileWrapper.CreateFile(location, type.ToString());
                if (!isCreated)
                    throw new BubblesException(
                        new TypesCouldNotBeCreatedException());
                return;
            }

            await AppendNodesToType(typeFileLocation, typeName, nodeId);
        }

        private async Task AppendNodesToType(string location, string typeName, string nodeId)
        {
            var content = await _fileWrapper.GetFileContents(location);
            var type = JsonConvert.DeserializeObject<Type>(content);
            if (type == null){
                type = new Type
                {
                    TypeName = typeName,
                    NodeIds = new List<string>()
                };
            }

            type.NodeIds.Add(nodeId);
            await _fileWrapper.CreateFile(location, type.ToString());
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

        public async Task<bool> CreateNode(string databaseName, string graphName, string type, dynamic data)
        {
            var graphLocation = GetGraphLocation(databaseName, graphName);
            if (!_fileWrapper.IsExists(graphLocation))
                throw new BubblesException(new GraphNotFoundException());

            var typesFolderPath = GetTypeLocation(databaseName, graphName, type);
            var nodeId = Utils.GenerateNodeId(graphName);
            await CheckAndCreateTypeFile(typesFolderPath, type, nodeId);
            var nodesFilePath = GetNodeLocation(databaseName, graphName, nodeId) + "." + Constants.FileExtension;

            var node = new Node
            {
                Data = data,
                Id = nodeId,
                Type = type
            };
            return await _fileWrapper.CreateFile(nodesFilePath, node.ToString());
        }

        public Database GetDatabase(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new BubblesException(new DatabaseNotFoundException());
            var dbPath = GetDatabaseLocation(databaseName);
            if (!_fileWrapper.IsExists(dbPath)) throw new BubblesException(new DatabaseNotFoundException());
            var graphNames = _fileWrapper.GetDirectories(dbPath);

            var graphs = graphNames.Select(oneGraph => new Graph { GraphName = oneGraph }).ToList();
            return new Database
            {
                DatabaseName = databaseName,
                Graphs = graphs
            };
        }

        public Graph GetGraph(string databaseName, string graphName)
        {
            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(graphName))
                throw new BubblesException(new GraphNotFoundException());
            var location = GetGraphLocation(databaseName, graphName);
            if (!_fileWrapper.IsExists(location))
                throw new BubblesException(new GraphNotFoundException());
            var nodesIds = _fileWrapper.GetFiles(location);
            var nodes = nodesIds.Select(oneNodeFile => new Node { Id = oneNodeFile.Split(".")[0] }).ToList();
            return new Graph
            {
                GraphName = graphName,
                Nodes = nodes
            };
        }

        public async Task<Node> GetNode(string database, string graphName, string nodeId)
        {
            if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(graphName) || string.IsNullOrEmpty(nodeId)){
                throw new BubblesException(new NodeNotFoundException());
            }

            var location = GetNodeLocation(database, graphName, nodeId);
            if (!_fileWrapper.IsExists(location)) throw new BubblesException(new NodeNotFoundException());
            var nodeData = await _fileWrapper.GetFileContents(location);
            if (nodeData == null){
                throw new BubblesException(new NodeNotFoundException());
            }

            return JsonConvert.DeserializeObject<Node>(nodeData)!;
        }
    }
}