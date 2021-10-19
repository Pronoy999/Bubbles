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

        public static string GetDatabaseLocation(string databaseName, string userId)
        {
            return EnvReader.GetStringValue(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar + userId +
                   Path.DirectorySeparatorChar + databaseName;
        }

        public static string GetGraphLocation(string databaseName, string graphName, string userId)
        {
            return GetDatabaseLocation(databaseName, userId) + Path.DirectorySeparatorChar + Constants.GraphFolderName +
                   Path.DirectorySeparatorChar + graphName;
        }

        public static string GetTypeLocation(string databaseName, string graphName, string type, string userId)
        {
            return GetGraphLocation(databaseName, graphName, userId) + Path.DirectorySeparatorChar +
                   EnvReader.GetStringValue(Constants.TypesFolderName) + Path.DirectorySeparatorChar + type;
        }

        public static string GetNodeLocation(string database, string graphName, string nodeId, string userId)
        {
            return GetGraphLocation(database, graphName, userId) + Path.DirectorySeparatorChar + nodeId;
        }

        public static string GetRelationshipLocation(string databaseName, string userId)
        {
            return GetDatabaseLocation(databaseName, userId) + Path.DirectorySeparatorChar +
                   Constants.RelationshipFolderName;
        }
    }
}