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
			var testCase1 = new [] { "2","2", "00", "0." };
			var testCase2 = new [] { "5", "1", "0.0.0" };
			Puzzles.ThereIsNoSpoon.Player.SetStringBuffer(testCase2);
			Puzzles.ThereIsNoSpoon.Player.Main(null);
			Console.WriteLine("Done");
		}
    }
}
