using System;

namespace BubblesEngine.Helpers
{
    public class Logger
    {
        public static void LogInformation(string tag, string content)
        {
            Console.WriteLine($"{tag}: {content}");
        }

        public static void LogError(string tag, string content)
        {
            Console.WriteLine($"ERROR: {tag} -: {content}");
        }
    }
}