using System;

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
			Puzzles.ThereIsNoSpoon.Player.SetStringBuffer(new [] { "2","2", "00", "0." });
			Puzzles.ThereIsNoSpoon.Player.Main(null);
			Console.WriteLine("Done");
		}
    }
}
