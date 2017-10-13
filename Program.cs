using System;
using Shared;
using SpecificEngines;

namespace CodinGameExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Puzzle thingofabob");
			
			//RunThereIsNoSpoonExample();
			RunBatman();
        }


		private static void RunThereIsNoSpoonExample()
		{
			var buffer1 = new [] { "2","2", "00", "0." };
			var buffer2 = new [] { "5", "1", "0.0.0" };
			var engine = new StringBufferGameEngine(buffer1);
			new Puzzles.ThereIsNoSpoon.Player(engine).Run();
			Console.WriteLine("Done");
		}

		private static void RunBatman()
		{
			//Less Jumps Building: h: 33 w:25 - Batman at: Position: x:2 y:29 - bomb at 24-2
			var bmGame02 = new ShadowsOfTheKnightEngine(25, 33, new Position(2,29), new Position(24,2), 50); // Success
			//Tower: Building: h: 80 w:1 Batman at: Position: x:0 y:1 bomb at 0 36
			var bmGame04 = new ShadowsOfTheKnightEngine(1, 80, new Position(0,1), new Position(0,36), 6); //Fails
			//Correct cutting: Building: h: 50 w:50 Batman at: Position: x:0 y:0bomb at 22 22
			var bmGame05 = new ShadowsOfTheKnightEngine(50, 50, new Position(0,0), new Position(22,22), 6); //Fails
			
			new Puzzles.ShadowsOfTheKnight.Player(bmGame05).Run();
		}
    }
}
