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
			public string FullFileName { get; set; }
			public string FileName { get; set; }
			public List<string> Requires {get;set;} = new List<string>();
			public List<string> Lines { get; set;} = new List<string>();
			public List<string> Usings { get; set; } = new List<string>();
		}


		private string _sourcePath; //Root path of the sources folder
		private string _puzzlePath; //Path to the puzzles
		private string _challengePath; //Path to the challenges
		private string _sharedPath; //Path to the shared files
		private string _frameworkPath; //Path to the framework files
		private string _outputPath; //Where to push results to
		public FileMerger(string sourcePath, string puzzlePath, string challengePath, string sharedPath, string frameworkPath, string outputPath)
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

		

		//Start writing the merge files for the puzzle files in the puzzle folder
		public void MergePuzzleFiles()
		{
			GetCodeFilesInPath(_puzzlePath).ForEach(file => MergePuzzleFile(file, "puzzle"));
			GetCodeFilesInPath(_challengePath).ForEach(file => MergePuzzleFile(file, "challenge"));
		}

		private void MergePuzzleFile(string writeFile, string prefix)
		{
			
			var fileToWriteInfo = new System.IO.FileInfo(writeFile);
			LogInfo($"Merging file for: {fileToWriteInfo.Name}");
			var outputFile = _sourcePath + "Merged/" + prefix + "." + fileToWriteInfo.Name + ".merged";
			
			var files = new List<ReadRes>();
			var myFile = ReadFile(writeFile);
			files.Add(myFile);
			
			var fwFiles = GetFrameworkFiles();
			files.AddRange(fwFiles);
			
			var sharedFiles = FilterSharedFiles(GetSharedFiles(), myFile);
			files.AddRange(sharedFiles);

			LogInfo($"Writing 1 file with {fwFiles.Count()} framework and {sharedFiles.Count()} shared files");

			StringBuilder resBuilder = new StringBuilder();
			files.SelectMany(f => f.Usings).Distinct().ToList().ForEach(u => resBuilder.AppendLine(u));
			int fileIdx = 0;
			files.ForEach(f => 
			{
				resBuilder.AppendLine($"//File {fileIdx.ToString().PadLeft(2,'0')}: {f.FullFileName}");
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
			LogSuccess($"Merged file written to {outputFile}");
		}

		private IEnumerable<ReadRes> FilterSharedFiles(IEnumerable<ReadRes> sharedFiles, ReadRes puzzleFile)
		{
			var sharedFilesToInclude = puzzleFile.Requires.Select(req => sharedFiles.FirstOrDefault(shared => FileNameMatch(shared.FileName, req))).Where(i => i!= null).ToList();
			if(sharedFilesToInclude.Count != puzzleFile.Requires.Count) 
			{ 
				LogError($"X Include count for file incorrect, expected {puzzleFile.Requires.Count}, but found {sharedFilesToInclude.Count} shared");
			}
			return sharedFilesToInclude;
		}

		private bool FileNameMatch(string fileName, string requiredToken)
		{
			fileName = fileName.ToLowerInvariant().Replace(".cs", "");
			requiredToken = requiredToken.ToLowerInvariant().Replace(".cs", "");
			return string.Equals(fileName, requiredToken, StringComparison.OrdinalIgnoreCase);
		}

		public void WatchFolders()
		{
			List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
			watchers.Add(WatchSpecificFolder(_puzzlePath, false));
			watchers.Add(WatchSpecificFolder(_challengePath, false));
			watchers.Add(WatchSpecificFolder(_sharedPath, true));

			LogDefault($"Waiting for changes");							
			while(true) { Thread.Sleep(1000); }
		}

		public FileSystemWatcher WatchSpecificFolder(string folder, bool invokeAfterCreation)
		{
			var dirInfo = new System.IO.DirectoryInfo(folder);
			var watchDir = dirInfo.FullName;
			LogInfo($"Watching folder: {watchDir}");
			System.IO.FileSystemWatcher fsw = new FileSystemWatcher(watchDir);
			fsw.EnableRaisingEvents = true;
			FileSystemEventHandler fswChanged = (sender, e) => 
			{
				LogSuccess($"{DateTime.Now.ToString("HH:mm:ss")}: Detected change {e.ChangeType} @ {e.FullPath} ({e.Name}");
				MergePuzzleFiles(); //Merging all puzzles
				Console.Beep();
			};
			fsw.Changed += fswChanged;
			
			if(invokeAfterCreation)
			{
				LogInfo("Starting initial merge");
				fswChanged.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _sourcePath, "./")); 
			}
			return fsw;
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
			var fileInfo = new System.IO.FileInfo(filePath);
			var fileName = fileInfo.Name;

			var usingLines = new List<string>();
			var codeLines = new List<string>();
			var requireLines = new List<string>();
			lines.ToList().ForEach(line => 
			{
				if(line.StartsWith("using ", StringComparison.OrdinalIgnoreCase))
				{
					usingLines.Add(line);
				}
				else if(line.StartsWith("//require:", StringComparison.OrdinalIgnoreCase))
				{
					requireLines.Add(line.Split(new [] {':'}).Last().Trim());
				}
				else 
				{
					codeLines.Add(line);
				}
			});

			return new ReadRes()
			{
				FullFileName = filePath,
				FileName = fileName,
				Lines = codeLines,
				Usings = usingLines,
				Requires = requireLines
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

		private void LogInfo(string message)
		{
			LogWithColor(ConsoleColor.Blue, message);
		}

		private void LogDefault(string message)
		{
			LogWithColor(ConsoleColor.Gray, message);
		}

		private void LogSuccess(string message)
		{
			LogWithColor(ConsoleColor.Green, message);
		}

		private void LogError(string message)
		{
			LogWithColor(ConsoleColor.Red, message);
		}

		private void LogWithColor(ConsoleColor color, string message)
		{
			var c = Console.ForegroundColor;
			Console.ForegroundColor = color;
			System.Console.WriteLine(message);
			Console.ForegroundColor = c;
		}

	}
}