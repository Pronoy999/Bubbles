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
    public class RelationshipController : IRelationshipController
    {
        private readonly IFileWrapper _fileWrapper;

        public RelationshipController(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public async Task<Relationship> GetRelationship(string database, string relationshipId, string userId)
        {
            var dbLocation = Utils.GetDatabaseLocation(database, userId);
            if (!_fileWrapper.IsDirectoryExists(dbLocation))
                throw new BubblesException(new DatabaseNotFoundException());

            var relationshipFileLocation = Utils.GetRelationshipLocation(database, userId) +
                                           Path.DirectorySeparatorChar +
                                           relationshipId + "." + Constants.FileExtension;
            if (!_fileWrapper.IsExists(relationshipFileLocation))
                throw new BubblesException(new RelationshipNotFoundException());

            return JsonConvert.DeserializeObject<Relationship>(
                await _fileWrapper.GetFileContents(relationshipFileLocation))!;
        }

        public List<string> GetAllRelationships(string database, string userId)
        {
            var dbLocation = Utils.GetDatabaseLocation(database, userId);
            if (!_fileWrapper.IsDirectoryExists(dbLocation))
                throw new BubblesException(new DatabaseNotFoundException());

            var relationshipFolderLocation =
                dbLocation + Path.DirectorySeparatorChar + Constants.RelationshipFolderName;
            if (!_fileWrapper.IsDirectoryExists(relationshipFolderLocation))
                throw new BubblesException(new RelationshipNotFoundException());

            var files = _fileWrapper.GetAllFiles(relationshipFolderLocation);

            return files.Select(file => file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1)).ToList();
        }
    }
}