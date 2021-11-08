using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
using BubblesEngine.Helpers;
using BubblesEngine.Models;
using dotenv.net;
using Newtonsoft.Json;

namespace BubblesEngine.Controllers.Implementation
{
    public class DbController : IDbController
    {
        private readonly IFileWrapper _fileWrapper;

        public DbController(IFileWrapper fileWrapper)
        {
            DotEnv.Load();
            _fileWrapper = fileWrapper;
        }
        
        

        public bool CreateGraph(string graphName, string databaseName, string userId)
        {
            var graphPath = Utils.GetGraphLocation(databaseName, graphName, userId);
            return _fileWrapper.CreateFolder(graphPath);
        }
        
        

        public Graph GetGraph(string databaseName, string graphName, string userId)
        {
            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(graphName))
                throw new BubblesException(new GraphNotFoundException());
            var location = Utils.GetGraphLocation(databaseName, graphName, userId);
            if (!_fileWrapper.IsDirectoryExists(location))
                throw new BubblesException(new GraphNotFoundException());
            var nodesIds = _fileWrapper.GetAllFilesNames(location);
            var nodes = nodesIds.Select(oneNodeFile => new Node { Id = oneNodeFile.Split(".")[0] }).ToList();
            return new Graph
            {
                GraphName = graphName,
                Nodes = nodes
            };
        }
        

       

        

        public async Task<Node> SearchNodeById(string databaseName, string nodeId, string userId)
        {
            var databaseLocation = Utils.GetDatabaseLocation(databaseName, userId);
            var nodeFileName = nodeId + "." + Constants.FileExtension;
            var result = _fileWrapper.SearchFiles(databaseLocation, nodeFileName);
            if (string.IsNullOrEmpty(result)){
                throw new BubblesException(new NodeNotFoundException());
            }

            var nodeData = await _fileWrapper.GetFileContents(result);
            return JsonConvert.DeserializeObject<Node>(nodeData)!;
        }

        public async Task<Node> SearchNodeByData(string databaseName, string data, string userId)
        {
            var searchLocation = Utils.GetDatabaseLocation(databaseName, userId) + Path.DirectorySeparatorChar +
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