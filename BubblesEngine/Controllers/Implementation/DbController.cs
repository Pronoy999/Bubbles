using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
using BubblesEngine.Helpers;
using BubblesEngine.Models;
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


        public bool CreateDatabase(string databaseName,string userId)
        {
            var databasePath = Utils.GetDatabaseLocation(databaseName,userId);
            return _fileWrapper.CreateFolder(databasePath);
        }

        public bool CreateGraph(string graphName, string databaseName,string userId)
        {
            var graphPath = Utils.GetGraphLocation(databaseName, graphName,userId);
            return _fileWrapper.CreateFolder(graphPath);
        }

        public async Task<string> CreateNode(string databaseName, string graphName, string type, dynamic data,string userId)
        {
            var graphLocation = Utils.GetGraphLocation(databaseName, graphName,userId);
            if (!_fileWrapper.IsExists(graphLocation))
                throw new BubblesException(new GraphNotFoundException());

            var typesFolderPath = Utils.GetTypeLocation(databaseName, graphName, type,userId);
            var nodeId = Utils.GenerateNodeId(graphName);
            await CheckAndCreateTypeFile(typesFolderPath, type, nodeId);
            var nodesFilePath = Utils.GetNodeLocation(databaseName, graphName, nodeId,userId) + "." + Constants.FileExtension;

            var node = new Node
            {
                Data = data,
                Id = nodeId,
                Type = type
            };
            await _fileWrapper.CreateFile(nodesFilePath, node.ToString());
            return nodeId;
        }

        public Database GetDatabase(string databaseName,string userId)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new BubblesException(new DatabaseNotFoundException());
            var dbPath = Utils.GetDatabaseLocation(databaseName,userId);
            if (!_fileWrapper.IsExists(dbPath)) throw new BubblesException(new DatabaseNotFoundException());
            var graphNames = _fileWrapper.GetDirectories(dbPath);

            var graphs = graphNames.Select(oneGraph => new Graph { GraphName = oneGraph }).ToList();
            return new Database
            {
                DatabaseName = databaseName,
                Graphs = graphs
            };
        }

        public Graph GetGraph(string databaseName, string graphName,string userId)
        {
            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(graphName))
                throw new BubblesException(new GraphNotFoundException());
            var location = Utils.GetGraphLocation(databaseName, graphName,userId);
            if (!_fileWrapper.IsExists(location))
                throw new BubblesException(new GraphNotFoundException());
            var nodesIds = _fileWrapper.GetAllFilesNames(location);
            var nodes = nodesIds.Select(oneNodeFile => new Node { Id = oneNodeFile.Split(".")[0] }).ToList();
            return new Graph
            {
                GraphName = graphName,
                Nodes = nodes
            };
        }

        public async Task<Node> GetNode(string database, string graphName, string nodeId,string userId)
        {
            if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(graphName) || string.IsNullOrEmpty(nodeId)){
                throw new BubblesException(new NodeNotFoundException());
            }

            var location = Utils.GetNodeLocation(database, graphName, nodeId,userId);
            if (!_fileWrapper.IsExists(location)) throw new BubblesException(new NodeNotFoundException());
            var nodeData = await _fileWrapper.GetFileContents(location);
            if (nodeData == null){
                throw new BubblesException(new NodeNotFoundException());
            }

            return JsonConvert.DeserializeObject<Node>(nodeData)!;
        }

        public async Task<Relationship> ConnectNode(string database, string leftNodeId, string rightNodeId,
            string relationshipType,
            dynamic data,string userId)
        {
            var dbLocation = Utils.GetDatabaseLocation(database,userId);
            if (!_fileWrapper.IsExists(dbLocation))
                throw new BubblesException(new DatabaseNotFoundException());
            var leftNodeLocation = _fileWrapper.SearchFiles(dbLocation, leftNodeId + "." + Constants.FileExtension);
            var rightNodeLocation = _fileWrapper.SearchFiles(dbLocation, rightNodeId + "." + Constants.FileExtension);
            if (string.IsNullOrEmpty(leftNodeLocation) || string.IsNullOrEmpty(rightNodeLocation))
                throw new BubblesException(new NodeNotFoundException());
            var relationshipId = Utils.GenerateRelationshipId();
            var relationshipLocation = Utils.GetRelationshipLocation(database,userId);
            Relationship relationship = new Relationship
            {
                Data = data,
                Id = relationshipId,
                LeftNodeId = leftNodeId,
                RightNodeId = rightNodeId,
                Type = relationshipType
            };
            await _fileWrapper.CreateFile(
                relationshipLocation + Path.DirectorySeparatorChar + relationshipId + "." + Constants.FileExtension,
                relationship.ToString());
            var relationshipTypeFolderLocation = Utils.GetRelationshipLocation(database,userId) + Path.DirectorySeparatorChar +
                                                 Constants.TypesFolderName;
            if (!_fileWrapper.IsExists(relationshipTypeFolderLocation))
                _fileWrapper.CreateFolder(relationshipTypeFolderLocation);
            var relationshipTypeFileLocation = relationshipTypeFolderLocation + Path.DirectorySeparatorChar +
                                               relationshipType + "." + Constants.FileExtension;
            RelationshipType relationshipTypeData;
            if (!_fileWrapper.IsExists(relationshipTypeFileLocation)){
                relationshipTypeData = new RelationshipType
                {
                    RelationshipIds = new List<string>()
                };
            }
            else{
                relationshipTypeData =
                    JsonConvert.DeserializeObject<RelationshipType>(
                        await _fileWrapper.GetFileContents(relationshipTypeFileLocation))!;
            }

            relationshipTypeData.RelationshipIds.Add(relationshipId);
            await _fileWrapper.CreateFile(relationshipTypeFileLocation, relationshipTypeData.ToString());
            return relationship;
        }

        public async Task<Relationship> GetRelationship(string database, string relationshipId,string userId)
        {
            var dbLocation = Utils.GetDatabaseLocation(database,userId);
            if (!_fileWrapper.IsExists(dbLocation))
                throw new BubblesException(new DatabaseNotFoundException());
            var relationshipFileLocation = Utils.GetRelationshipLocation(database,userId) + Path.DirectorySeparatorChar +
                                           relationshipId + "." + Constants.FileExtension;
            if (!_fileWrapper.IsExists(relationshipFileLocation))
                throw new BubblesException(new RelationshipNotFoundException());
            return JsonConvert.DeserializeObject<Relationship>(
                await _fileWrapper.GetFileContents(relationshipFileLocation))!;
        }

        public async Task<Node> SearchNodeById(string databaseName, string nodeId,string userId)
        {
            var databaseLocation = Utils.GetDatabaseLocation(databaseName,userId);
            var nodeFileName = nodeId + "." + Constants.FileExtension;
            var result = _fileWrapper.SearchFiles(databaseLocation, nodeFileName);
            if (string.IsNullOrEmpty(result)){
                throw new BubblesException(new NodeNotFoundException());
            }

            var nodeData = await _fileWrapper.GetFileContents(result);
            return JsonConvert.DeserializeObject<Node>(nodeData)!;
        }

        public async Task<Node> SearchNodeByData(string databaseName, string data,string userId)
        {
            var searchLocation = Utils.GetDatabaseLocation(databaseName,userId) + Path.DirectorySeparatorChar +
                                 Constants.GraphFolderName;
            var files = _fileWrapper.GetAllFiles(searchLocation);
            foreach (var oneFilePath in files){
                var oneFileData = await _fileWrapper.GetFileContents(oneFilePath);
                if (oneFileData.Contains(data))
                    return JsonConvert.DeserializeObject<Node>(oneFileData)!;
            }

            return null;
        }
    }
}