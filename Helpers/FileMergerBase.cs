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
		protected string _dirSplit = null;
		protected FileMergerBase(string sourcePath, string puzzlePath, string challengePath, string sharedPath, string frameworkPath, string outputPath)
		{
			_sourcePath = sourcePath;
			_dirSplit = _sourcePath.IndexOf("\\") > -1 ? "\\" : "/"; //Determine split char
			_sourcePath = FixPathForOSSeperator(sourcePath);
			
			if (string.IsNullOrWhiteSpace(puzzlePath) || string.IsNullOrWhiteSpace(sharedPath) || string.IsNullOrWhiteSpace(outputPath))
			{
				throw new Exception("Invalid data");
			}

			_puzzlePath = FixPathForOSSeperator(_sourcePath + puzzlePath);
			_challengePath = FixPathForOSSeperator(_sourcePath + challengePath);
			_sharedPath = FixPathForOSSeperator(_sourcePath + sharedPath);
			_outputPath = FixPathForOSSeperator(_sourcePath + outputPath);
			_frameworkPath = FixPathForOSSeperator(_sourcePath + frameworkPath);
			if(!System.IO.Directory.Exists(_sourcePath)) { throw new Exception($"Working from: {_sourcePath}"); }
			if(!System.IO.Directory.Exists(_puzzlePath)) { throw new Exception($"Puzzle path invalid: {_puzzlePath}"); }
			if(!System.IO.Directory.Exists(_challengePath)) { throw new Exception($"Challenge path invalid: {_challengePath}"); }
			if(!System.IO.Directory.Exists(_sharedPath)) { throw new Exception($"Shared path invalid: {_sharedPath}"); }
			if(!System.IO.Directory.Exists(_frameworkPath)) { throw new Exception($"Framework path invalid: {_frameworkPath}"); }
			if (!System.IO.Directory.Exists(_outputPath)) { System.IO.Directory.CreateDirectory(_outputPath); } //Try to create the output path
			if (!System.IO.Directory.Exists(_outputPath)) { throw new Exception($"Merge path invalid: {_outputPath}"); }

			LogDefault($"Merger started with parameters:");
			LogDefault($"Running path: {_sourcePath}");
			LogDefault($"Puzzle path: {puzzlePath}");
			LogDefault($"Puzzle path: {challengePath}");
			LogDefault($"Shared path: {sharedPath}");
			LogDefault($"Framework path: {frameworkPath}");
			LogDefault($"Output path: {outputPath}");
		}

		protected string FixPathForOSSeperator(string rawPath)
		{
			return rawPath.Replace("/", _dirSplit);
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