using System;
using Shared;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpecificEngines;
using Helpers;

namespace CodinGameExperiments
{
	public class RunAction 
	{
		public string Name { get; set; }
		public string Description { get;set; }
		public Action Action { get; set; }
	}

    class Program
    {	
		private const string PuzzlePath = "Puzzles/";
		private const string SharedPath = "Shared/";
		private const string MergePath = "Merged/";

        static void Main(string[] args)
        {
			Console.WriteLine("Puzzle solution");
			var action = GetRunAction(args.FirstOrDefault());
			if(action != null) 
			{
				System.Console.WriteLine($"Running action: {action.Name} ({action.Description})");
				action.Action();
				return;
			}
			
			System.Console.WriteLine("No command provided, use one of the following:");
			GetActions().ToList().ForEach(a => 
			{
				System.Console.WriteLine($"{a.Name}\t\t{a.Description}");
			});
			System.Console.WriteLine("Parameters:");
			System.Console.WriteLine($"PuzzlePath \t\t {PuzzlePath}");
			System.Console.WriteLine($"SharedPath\t\t {SharedPath}");
			System.Console.WriteLine($"MergePath\t\t {MergePath}");
        }

		private static RunAction GetRunAction(string action)
		{
			if(action == null) { return null; }
			return GetActions().FirstOrDefault(a => string.Equals(a.Name, action, StringComparison.OrdinalIgnoreCase));
		}

		private static IEnumerable<RunAction> GetActions() 
		{
			yield return new RunAction() { Name = "merge", Description = "Merge the files", Action = RunMerge };
			yield return new RunAction() { Name = "watch", Description = "Watch a file for changes and merge", Action = RunWatch };
			yield return new RunAction() { Name = "test-batman", Description = "Run batman test-case", Action = RunBatman };
			yield return new RunAction() { Name = "test-nospoon", Description = "Run no spoon test-case", Action = RunThereIsNoSpoonExample };
		}

		private static void RunMerge()
		{
			System.Console.WriteLine("Running the merger");
			var merger = GetFileMerger();
			merger.MergePuzzleFiles();
		}

		private static void RunWatch()
		{
			var merger = GetFileMerger();
			System.Console.WriteLine($"Starting puzzle file watcher");
			merger.WatchPuzzleFiles();
		}

		private static FileMerger GetFileMerger() 
		{
			return new FileMerger(AppContext.BaseDirectory, PuzzlePath, SharedPath, MergePath);
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
			var bmGame02 = new ShadowsOfTheKnightEngine(25, 33, new Position(2,29), new Position(24,2), 49); // Success
			//Tower: Building: h: 80 w:1 Batman at: Position: x:0 y:1 bomb at 0 36
			var bmGame04 = new ShadowsOfTheKnightEngine(1, 80, new Position(0,1), new Position(0,36), 6); //Success
			//Correct cutting: Building: h: 50 w:50 Batman at: Position: x:0 y:0bomb at 22 22
			var bmGame05 = new ShadowsOfTheKnightEngine(50, 50, new Position(0,0), new Position(22,22), 6); //Success
			
			//Evasive: Building: h: 100 w:100 Batman at: Pos[0] = x:5 y:98, bomb at 0 1,  7 left
			var bmGame06 = new ShadowsOfTheKnightEngine(100, 100, new Position(5,98), new Position(0,1), 7); //fail
			//Not there: Building: h: 9999 w:9999 Batman at: Pos[0] = x:54 y:77, bomb at 9456 4962, 14 left
			var bmGame07 = new ShadowsOfTheKnightEngine(9999, 9999, new Position(54,77), new Position(9456,4962), 14); //fail
			
			new Puzzles.ShadowsOfTheKnight.Player(bmGame06).Run();
		}
		//TODO: Createa logic to run multiple testcases and process the results
    }
}
