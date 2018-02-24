using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Helpers
{
	public class FileMergeWatcher : FileMergerBase
	{
		private FileMerger _merger;
		public FileMergeWatcher(string sourcePath, string puzzlePath, string challengePath, string sharedPath, string frameworkPath, string outputPath) : base(sourcePath, puzzlePath, challengePath, sharedPath, frameworkPath, outputPath)
		{
			_merger = new FileMerger(sourcePath,puzzlePath,challengePath,sharedPath,frameworkPath, outputPath);
		}

		public void WatchFolders()
		{
			List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
			watchers.AddRange(WatchSpecificFolder(_puzzlePath, false));
			watchers.AddRange(WatchSpecificFolder(_challengePath, false));
			watchers.AddRange(WatchSpecificFolder(_sharedPath, true));

			LogDefault($"Waiting for changes");							
			while(true) { Thread.Sleep(1000); }
		}


		//TODO: Watch subfolders
		public IEnumerable<FileSystemWatcher> WatchSpecificFolder(string folder, bool invokeAfterCreation)
		{
			var watchers = new List<FileSystemWatcher>();
			var dirInfo = new System.IO.DirectoryInfo(folder);
			var watchDir = dirInfo.FullName;
			LogInfo($"Watching folder: {watchDir}");
			System.IO.FileSystemWatcher fsw = new FileSystemWatcher(watchDir);
			fsw.EnableRaisingEvents = true;
			FileSystemEventHandler fswChanged = (sender, e) => 
			{
				LogSuccess($"{DateTime.Now.ToString("HH:mm:ss")}: Detected change {e.ChangeType} @ {e.FullPath} ({e.Name}");
				_merger.MergePuzzleFiles(); //Merging all puzzles
				Console.Beep();
			};
			fsw.Changed += fswChanged;
			
			if(invokeAfterCreation)
			{
				LogInfo("Starting initial merge");
				fswChanged.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _sourcePath, "./")); 
			}
			watchers.Add(fsw);

			//Check if we need to query subdirectories (for puzzles split in separate files)
			var subDirs = dirInfo.GetDirectories();
			foreach(var subDir in subDirs)
			{
				watchers.AddRange(WatchSpecificFolder(subDir.FullName, false));
			}
			return watchers;
		}

	}
}