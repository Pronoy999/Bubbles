using System.Threading.Tasks;
using BubblesEngine.Engines;
using BubblesEngine.Engines.Implementations;
using BubblesEngine.Helpers;
using dotenv.net;
using dotenv.net.Utilities;

namespace BubblesEngine.Controllers.Implementation
{
    public class Controller : IController
    {
        private readonly IDomainFs _domainFs;

        public Controller(IDomainFs domainFs)
        {
            _domainFs = domainFs;
        }

        public Task<string> CreateGraph(string graphName)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> CreateDatabase(string databaseName)
        {
            /*var databasePath = EnvReader.GetStringValue(Constants.DbRootFolderKey) + databaseName;
            if (!_domainFs.IsExists(databasePath)){
                return await _domainFs.WriteFile(databasePath);
            }*/throw new System.NotImplementedException();
        }

        public Task<string> CreateNode(string databaseId, string graphId, string type, dynamic data)
        {
            throw new System.NotImplementedException();
        }
    }
}