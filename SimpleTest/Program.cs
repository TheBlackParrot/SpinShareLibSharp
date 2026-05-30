using System;
using SpinShareLib;
using System.Threading.Tasks;
using SpinShareLib.Types;

namespace SimpleTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SSAPI ssapi = new SSAPI();
            Task.Run(async () => {
                Content thing = await ssapi.ping();
                Console.WriteLine(thing.status);
            }).GetAwaiter().GetResult();
        }
    }
}