using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Helpers
{
	public class FileMerger
	{
		private string _sourcePath;
		private string _mainFile;
		public FileMerger(string sourcePath, string mainFile)
		{
			_sourcePath = GetSourcePathBasedOnRunPath(sourcePath);
			if(string.IsNullOrWhiteSpace(_mainFile) || !mainFile.EndsWith(".cs"))
			{
				throw new Exception("Invalid data");
			}
			_mainFile = mainFile;	
		}

		private static string GetSourcePathBasedOnRunPath(string runPath)
		{
			//This assumes /bin/Debug and therefor a Mac/Linux environment for windows go find the \bin\Debug
			if(runPath.IndexOf("/bin/Debug/", StringComparison.OrdinalIgnoreCase) < 0) { return runPath; }
			var determinedBase = runPath.Substring(0, runPath.IndexOf("/bin/Debug/"));
			return determinedBase.EndsWith("/") ? determinedBase : determinedBase + "/";
		}

		public void WriteMergedFile(string outputFile)
		{
			var fullFile = _sourcePath + outputFile;

			var mainFileContent = ReadFile(_mainFile);
			var content = GetMergedFileContent();
			System.IO.File.WriteAllText(outputFile, mainFileContent + Environment.NewLine + Environment.NewLine + content);
		}

		public string GetMergedFileContent()
		{
			var fileRes = string.Join(Environment.NewLine, GetMergeFilesShared().Select(ReadFile));
			return fileRes;
		}
		private string ReadFile(string filePath) 
		{
			string fileToLook = _sourcePath + filePath;
			if(!System.IO.File.Exists(_sourcePath + filePath))
			{
				throw new Exception($"Unknown file: {fileToLook}");
			}
			return System.IO.File.ReadAllText(fileToLook);
		}
		
		public IEnumerable<string> GetMergeFilesShared()
		{
			yield return "Shared/CodinGameProxyEngine.cs";
			yield return "Shared/IGameEngine.cs";        
			yield return "Shared/PuzzleMain.cs";
			yield return "Shared/GameEngineBase.cs";
			yield return "Shared/Position.cs"; 
			yield return "Shared/StringBufferGameEngine.cs";
		}

	}
}