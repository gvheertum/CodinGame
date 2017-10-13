using System;
using Shared;

namespace CodinGameExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

			RunThereIsNoSpoonExample();
        }


		private static void RunThereIsNoSpoonExample()
		{
			var buffer1 = new [] { "2","2", "00", "0." };
			var buffer2 = new [] { "5", "1", "0.0.0" };
			var engine = new StringBufferGameEngine(buffer1);
			new Puzzles.ThereIsNoSpoon.Player(engine).Run();
			Console.WriteLine("Done");
		}
    }
}
