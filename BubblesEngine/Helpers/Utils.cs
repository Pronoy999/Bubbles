using System;

namespace BubblesEngine.Helpers
{
    public static class Utils
    {
        public static string GenerateNodeId(string graphName)
        {
            return graphName + "-node-" + Guid.NewGuid();
        }
    }
}