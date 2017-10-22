using System;
using Shared;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpecificEngines;
using Helpers;
using ProgramRunners;
using ProgramRunners.PuzzleTestRunners;

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
			
			yield return new RunAction() { Name = "test-batman", Description = "Run batman test-case", Action = new Batman().RunBatman };
			yield return new RunAction() { Name = "test-spoon", Description = "Run spoon test-case", Action = new Spoon().RunSpoon };
			yield return new RunAction() { Name = "test-defib", Description = "Run defib test-case", Action = new Defibrillators().RunPuzzleTests };
			yield return new RunAction() { Name = "test-helpers", Description = "Run test cases for helpers", Action = new SharedElementTestRunner().RunHelperTests };
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

		//TODO: Createa logic to run multiple testcases and process the results
    }
}
