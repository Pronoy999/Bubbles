using System;
using dotenv.net;

namespace BubblesEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            DotEnv.Load();
            Console.WriteLine("Hello World!");
        }
    }
}