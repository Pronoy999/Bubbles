using System;
using System.IO;
using dotenv.net.Utilities;

namespace BubblesEngine.Helpers
{
    public static class Utils
    {
        public static string GenerateNodeId(string graphName)
        {
            return graphName + "-node-" + Guid.NewGuid();
        }

        public static string GenerateRelationshipId()
        {
            return "rs-" + Guid.NewGuid();
        }
        public static string GetDatabaseLocation(string databaseName)
        {
            return EnvReader.GetStringValue(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar +
                   databaseName;
        }

        public static string GetGraphLocation(string databaseName, string graphName)
        {
            return GetDatabaseLocation(databaseName) + Path.DirectorySeparatorChar + Constants.GraphFolderName +
                   Path.DirectorySeparatorChar + graphName;
        }

        public static string GetTypeLocation(string databaseName, string graphName, string type)
        {
            return GetGraphLocation(databaseName, graphName) + Path.DirectorySeparatorChar +
                   EnvReader.GetStringValue(Constants.TypesFolderName) + Path.DirectorySeparatorChar + type;
        }

        public static string GetNodeLocation(string database, string graphName, string nodeId)
        {
            return GetGraphLocation(database, graphName) + Path.DirectorySeparatorChar + nodeId;
        }

        public static string GetRelationshipLocation(string databaseName)
        {
            return GetDatabaseLocation(databaseName) + Path.DirectorySeparatorChar + Constants.RelationshipFolderName;
        }
    }
}