using System.Linq;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
using BubblesEngine.Helpers;
using BubblesEngine.Models;

namespace BubblesEngine.Controllers.Implementation
{
    public class GraphController : IGraphController
    {
        private readonly IFileWrapper _fileWrapper;

        public GraphController(IFileWrapper fileWrapper)
        {
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
    }
}