using System;

namespace Helpers
{
	public abstract class FileMergerBase
	{
		protected string _sourcePath; //Root path of the sources folder
		protected string _puzzlePath; //Path to the puzzles
		protected string _challengePath; //Path to the challenges
		protected string _sharedPath; //Path to the shared files
		protected string _frameworkPath; //Path to the framework files
		protected string _outputPath; //Where to push results to
		protected FileMergerBase(string sourcePath, string puzzlePath, string challengePath, string sharedPath, string frameworkPath, string outputPath)
		{
			_sourcePath = GetSourcePathBasedOnRunPath(sourcePath);
			if(string.IsNullOrWhiteSpace(puzzlePath) || string.IsNullOrWhiteSpace(sharedPath) || string.IsNullOrWhiteSpace(outputPath))
			{
				throw new Exception("Invalid data");
			}
			_puzzlePath = _sourcePath + puzzlePath;
			_challengePath = _sourcePath + challengePath;
			_sharedPath = _sourcePath + sharedPath;
			_outputPath = _sourcePath + outputPath;
			_frameworkPath = _sourcePath + frameworkPath;
			if(!System.IO.Directory.Exists(_sourcePath)) { throw new Exception($"Working from: {_sourcePath}"); }
			if(!System.IO.Directory.Exists(_puzzlePath)) { throw new Exception($"Puzzle path invalid: {puzzlePath}"); }
			if(!System.IO.Directory.Exists(_challengePath)) { throw new Exception($"Challenge path invalid: {challengePath}"); }
			if(!System.IO.Directory.Exists(_sharedPath)) { throw new Exception($"Shared path invalid: {sharedPath}"); }
			if(!System.IO.Directory.Exists(_frameworkPath)) { throw new Exception($"Framework path invalid: {frameworkPath}"); }
			if(!System.IO.Directory.Exists(_outputPath)) { throw new Exception($"Merge path invalid: {outputPath}"); }

			LogDefault($"Merger started with parameters:");
			LogDefault($"Running path: {_sourcePath}");
			LogDefault($"Puzzle path: {puzzlePath}");
			LogDefault($"Puzzle path: {challengePath}");
			LogDefault($"Shared path: {sharedPath}");
			LogDefault($"Framework path: {frameworkPath}");
			LogDefault($"Output path: {outputPath}");
		}

		//Get the sources path based on the run path of the app (often bin/Debug).
		//TODO: This assumes /bin/Debug and therefor a Mac/Linux environment for windows go find the \bin\Debug
		private string GetSourcePathBasedOnRunPath(string runPath)
		{
			if(runPath.IndexOf("/bin/Debug/", StringComparison.OrdinalIgnoreCase) < 0) { return runPath; }
			var determinedBase = runPath.Substring(0, runPath.IndexOf("/bin/Debug/"));
			return determinedBase.EndsWith("/") ? determinedBase : determinedBase + "/";
		}

		protected void LogInfo(string message)
		{
			LogWithColor(ConsoleColor.Blue, message);
		}

		protected void LogDefault(string message)
		{
			LogWithColor(ConsoleColor.Gray, message);
		}

		protected void LogSuccess(string message)
		{
			LogWithColor(ConsoleColor.Green, message);
		}

		protected void LogError(string message)
		{
			LogWithColor(ConsoleColor.Red, message);
		}

		protected void LogWithColor(ConsoleColor color, string message)
		{
			var c = Console.ForegroundColor;
			Console.ForegroundColor = color;
			System.Console.WriteLine(message);
			Console.ForegroundColor = c;
		}
	}
}