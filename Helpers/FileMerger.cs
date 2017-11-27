using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Helpers
{
	//TODO: This could be made smarter to only include references really needed, this can be headers in the file indicating that we want to incude a certain base file
	//TODO: Base files can include other required files
	//TODO: Allow folders to exist per puzzle allowing us to merge certain puzzle specific files
	public class FileMerger
	{
		public class ReadRes
		{
			public string FileName { get; set; }
			public List<string> Lines { get; set;}
			public List<string> Usings { get; set; }
		}


		private string _sourcePath; //Root path of the sources folder
		private string _puzzlePath; //Path to the puzzles
		private string _sharedPath; //Path to the shared files
		private string _frameworkPath; //Path to the framework files
		private string _outputPath; //Where to push results to
		public FileMerger(string sourcePath, string puzzlePath, string sharedPath, string frameworkPath, string outputPath)
		{
			_sourcePath = GetSourcePathBasedOnRunPath(sourcePath);
			if(string.IsNullOrWhiteSpace(puzzlePath) || string.IsNullOrWhiteSpace(sharedPath) || string.IsNullOrWhiteSpace(outputPath))
			{
				throw new Exception("Invalid data");
			}
			_puzzlePath = _sourcePath + puzzlePath;
			_sharedPath = _sourcePath + sharedPath;
			_outputPath = _sourcePath + outputPath;
			_frameworkPath = _sourcePath + frameworkPath;
			if(!System.IO.Directory.Exists(_puzzlePath)) { throw new Exception($"Puzzle path invalid: {_puzzlePath}"); }
			if(!System.IO.Directory.Exists(_sharedPath)) { throw new Exception($"Shared path invalid: {_sharedPath}"); }
			if(!System.IO.Directory.Exists(_frameworkPath)) { throw new Exception($"Framework path invalid: {_frameworkPath}"); }
			if(!System.IO.Directory.Exists(_outputPath)) { throw new Exception($"Merge path invalid: {_outputPath}"); }

			System.Console.WriteLine($"Merger started with parameters:");
			System.Console.WriteLine($"Running path: {_sourcePath}");
			System.Console.WriteLine($"Puzzle path: {_puzzlePath}");
			System.Console.WriteLine($"Shared path: {_sharedPath}");
			System.Console.WriteLine($"Framework path: {_frameworkPath}");
			System.Console.WriteLine($"Output path: {_outputPath}");
		}

		

		//Start writing the merge files for the puzzle files in the puzzle folder
		public void MergePuzzleFiles()
		{
			GetCodeFilesInPath(_puzzlePath).ForEach(file => MergePuzzleFile(file));
		}

		private void MergePuzzleFile(string writeFile)
		{
			Console.WriteLine($"Merging file for: {writeFile}");
			var outputFile = _sourcePath + "Merged/" + new System.IO.FileInfo(writeFile).Name + ".merged";
			
			//TODO: identify per file what is needed
			var files = new List<ReadRes>();
			files.Add(ReadFile(writeFile));
			files.AddRange(GetSharedFiles());
			files.AddRange(GetFrameworkFiles());

			StringBuilder resBuilder = new StringBuilder();
			files.SelectMany(f => f.Usings).Distinct().ToList().ForEach(u => resBuilder.AppendLine(u));
			int fileIdx = 0;
			files.ForEach(f => 
			{
				resBuilder.AppendLine($"//File {fileIdx.ToString().PadLeft(2,'0')}: {f.FileName}");
				resBuilder.AppendJoin(Environment.NewLine, f.Lines);
			
				//For the first file flush additional empty lines to flag starting of shared files logic
				for(int i = 0; fileIdx == 0 && i < 20; i++) 
				{
					resBuilder.AppendLine();
				}

				resBuilder.AppendLine();
				resBuilder.AppendLine();
				fileIdx++;
			});
			System.IO.File.WriteAllText(outputFile, resBuilder.ToString());
			Console.WriteLine($"Merged file written to {outputFile}");
		}

		
		public void WatchPuzzleFiles()
		{
			var dirInfo = new System.IO.DirectoryInfo(_puzzlePath);
			var watchDir = dirInfo.FullName;
			Console.WriteLine($"Watching folder: {watchDir}");
			System.IO.FileSystemWatcher fsw = new FileSystemWatcher(watchDir);
			FileSystemEventHandler fswChanged = (sender, e) => 
			{
				Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")}: detected change");
				System.Console.WriteLine($"Detected: {e.ChangeType} @ {e.FullPath} ({e.Name}");
				MergePuzzleFiles(); //Merging all puzzles
				Console.Beep();
			};
			fsw.Changed += fswChanged;
			
			System.Console.WriteLine("Starting initial merge");
			fswChanged.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _sourcePath, "./")); 
			
			Console.WriteLine($"Waiting for changes");			
			while(true) { fsw.WaitForChanged(WatcherChangeTypes.Changed); }
		}


		private List<ReadRes> GetSharedFiles()
		{
			var fileRes = GetCodeFilesInPath(_sharedPath).Select(ReadFile).ToList();
			return fileRes;
		}

		private List<ReadRes> GetFrameworkFiles()
		{
			var fileRes = GetCodeFilesInPath(_frameworkPath).Select(ReadFile).ToList();
			return fileRes;
		}


		//Read a single file and split the usings and the file from each other
		private ReadRes ReadFile(string filePath) 
		{
			if(!System.IO.File.Exists(filePath))
			{
				throw new Exception($"Unknown file: {filePath}");
			}
			var lines = System.IO.File.ReadAllLines(filePath);

			var usingLines = new List<string>();
			var codeLines = new List<string>();
			lines.ToList().ForEach(line => 
			{
				if(line.StartsWith("using "))
				{
					usingLines.Add(line);
				}
				else 
				{
					codeLines.Add(line);
				}
			});

			return new ReadRes()
			{
				FileName = filePath,
				Lines = codeLines,
				Usings = usingLines
			};
		}
		
		//Get the sources path based on the run path of the app (often bin/Debug).
		//TODO: This assumes /bin/Debug and therefor a Mac/Linux environment for windows go find the \bin\Debug
		private string GetSourcePathBasedOnRunPath(string runPath)
		{
			if(runPath.IndexOf("/bin/Debug/", StringComparison.OrdinalIgnoreCase) < 0) { return runPath; }
			var determinedBase = runPath.Substring(0, runPath.IndexOf("/bin/Debug/"));
			return determinedBase.EndsWith("/") ? determinedBase : determinedBase + "/";
		}

		private List<string> GetCodeFilesInPath(string path)
		{
			var files = System.IO.Directory.GetFiles(path);
			return files.Where(f => f.EndsWith(".cs")).ToList();
		}

	}
}