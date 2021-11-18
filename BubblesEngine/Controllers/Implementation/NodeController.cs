using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
using BubblesEngine.Helpers;
using BubblesEngine.Models;
using Newtonsoft.Json;

namespace BubblesEngine.Controllers.Implementation
{
    public class NodeController : INodeController
    {
        private readonly IFileWrapper _fileWrapper;

        public NodeController(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        private async Task CheckAndCreateTypeFile(string location, string typeName, string nodeId)
        {
            if (!_fileWrapper.IsDirectoryExists(location)){
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
                var isCreated = await _fileWrapper.CreateFile(typeFileLocation, type.ToString());
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
            var type = JsonConvert.DeserializeObject<Type>(content) ?? new Type
            {
                TypeName = typeName,
                NodeIds = new List<string>()
            };

            type.NodeIds.Add(nodeId);
            await _fileWrapper.CreateFile(location, type.ToString());
        }

        private void CreateFolderIfNotExists(string location)
        {
            if (!_fileWrapper.IsDirectoryExists(location))
                _fileWrapper.CreateFolder(location);
        }


        public async Task<string> CreateNode(string databaseName, string graphName, string type, dynamic data,
            string userId)
        {
            var graphLocation = Utils.GetGraphLocation(databaseName, graphName, userId);
            if (!_fileWrapper.IsDirectoryExists(graphLocation))
                throw new BubblesException(new GraphNotFoundException());

            var typesFolderPath = Utils.GetTypeLocation(databaseName, graphName, type, userId);
            var nodeId = Utils.GenerateNodeId(graphName);
            await CheckAndCreateTypeFile(typesFolderPath, type, nodeId);
            var nodesFilePath = Utils.GetNodeLocation(databaseName, graphName, nodeId, userId) + "." +
                                Constants.FileExtension;

            var node = new Node
            {
                Data = data,
                Id = nodeId,
                Type = type
            };
            await _fileWrapper.CreateFile(nodesFilePath, node.ToString());
            return nodeId;
        }

        public async Task<Node> GetNode(string database, string graphName, string nodeId, string userId)
        {
            if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(graphName) || string.IsNullOrEmpty(nodeId)){
                throw new BubblesException(new NodeNotFoundException());
            }

            var location = Utils.GetNodeLocation(database, graphName, nodeId, userId) + "." + Constants.FileExtension;
            if (!_fileWrapper.IsExists(location)) throw new BubblesException(new NodeNotFoundException());
            var nodeData = await _fileWrapper.GetFileContents(location);
            if (nodeData == null){
                throw new BubblesException(new NodeNotFoundException());
            }

            return JsonConvert.DeserializeObject<Node>(nodeData)!;
        }

        public async Task<Relationship> ConnectNode(string database, string leftNodeId, string rightNodeId,
            string relationshipType,
            dynamic data, string userId)
        {
            var dbLocation = Utils.GetDatabaseLocation(database, userId);
            if (!_fileWrapper.IsDirectoryExists(dbLocation))
                throw new BubblesException(new DatabaseNotFoundException());
            var leftNodeLocation = _fileWrapper.SearchFiles(dbLocation, leftNodeId + "." + Constants.FileExtension);
            var rightNodeLocation = _fileWrapper.SearchFiles(dbLocation, rightNodeId + "." + Constants.FileExtension);
            if (string.IsNullOrEmpty(leftNodeLocation) || string.IsNullOrEmpty(rightNodeLocation))
                throw new BubblesException(new NodeNotFoundException());
            var relationshipId = Utils.GenerateRelationshipId();
            var relationshipFolderLocation = Utils.GetRelationshipLocation(database, userId);

            CreateFolderIfNotExists(relationshipFolderLocation);

            var relationship = new Relationship
            {
                Data = data,
                Id = relationshipId,
                LeftNodeId = leftNodeId,
                RightNodeId = rightNodeId,
                Type = relationshipType
            };
            await _fileWrapper.CreateFile(
                relationshipFolderLocation + Path.DirectorySeparatorChar + relationshipId + "." +
                Constants.FileExtension,
                relationship.ToString());
            var relationshipTypeFolderLocation =
                Utils.GetRelationshipLocation(database, userId) + Path.DirectorySeparatorChar +
                Constants.TypesFolderName;

            CreateFolderIfNotExists(relationshipTypeFolderLocation);

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

        public async Task<Relationship> GetRelationship(string database, string relationshipId, string userId)
        {
            var dbLocation = Utils.GetDatabaseLocation(database, userId);
            if (!_fileWrapper.IsExists(dbLocation))
                throw new BubblesException(new DatabaseNotFoundException());
            var relationshipFileLocation = Utils.GetRelationshipLocation(database, userId) +
                                           Path.DirectorySeparatorChar +
                                           relationshipId + "." + Constants.FileExtension;
            if (!_fileWrapper.IsExists(relationshipFileLocation))
                throw new BubblesException(new RelationshipNotFoundException());
            return JsonConvert.DeserializeObject<Relationship>(
                await _fileWrapper.GetFileContents(relationshipFileLocation))!;
        }
    }
}