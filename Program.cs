using System;
using Shared;
using System.Linq;
using SpecificEngines;
using Helpers;

namespace CodinGameExperiments
{
    class Program
    {	
		private const string CurrentPuzzleFileForMerge = "Puzzles/05_ShadowsOfTheKnight.cs";
		private const string CurrentMergeOutput = "Merged/MergedCodingFile.cs.merged";
        static void Main(string[] args)
        {
			Console.WriteLine("Puzzle solution");
			if(args.Any(a => string.Equals("merge", a, StringComparison.OrdinalIgnoreCase))) 
			{
				RunMerge();
				return;
			}

			//RunThereIsNoSpoonExample();
			RunBatman();
        }

		private static void RunMerge()
		{
			System.Console.WriteLine("Running the merger");
			System.Console.WriteLine($"Merging shared and {CurrentPuzzleFileForMerge}");
			var merger = new FileMerger(AppContext.BaseDirectory, CurrentPuzzleFileForMerge);
			merger.WriteMergedFile(CurrentMergeOutput);
			System.Console.WriteLine($"Output written to: {CurrentMergeOutput}");
		}

		private static void RunThereIsNoSpoonExample()
		{
			var buffer1 = new [] { "2","2", "00", "0." };
			var buffer2 = new [] { "5", "1", "0.0.0" };
			var engine = new StringBufferGameEngine(buffer1);
			new Puzzles.ThereIsNoSpoon.Player(engine).Run();
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
