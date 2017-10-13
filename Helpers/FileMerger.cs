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
		public class ReadRes
		{
			public string FileName { get; set; }
			public List<string> Lines { get; set;}
			public List<string> Usings { get; set; }
		}


		private string _sourcePath;
		private string _mainFile;
		public FileMerger(string sourcePath, string mainFile)
		{
			_sourcePath = GetSourcePathBasedOnRunPath(sourcePath);
			if(string.IsNullOrWhiteSpace(mainFile) || !(mainFile.EndsWith(".cs") || mainFile.EndsWith(".cs.merged")))
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

			var files = new List<ReadRes>();
			files.Add(ReadFile(_mainFile));
			files.AddRange(GetMergedFileContent());

			StringBuilder resBuilder = new StringBuilder();
			files.SelectMany(f => f.Usings).Distinct().ToList().ForEach(u => resBuilder.AppendLine(u));

			files.ForEach(f => 
			{
				resBuilder.AppendLine();
				resBuilder.AppendLine();
				resBuilder.AppendLine($"//File: {f.FileName}");
				resBuilder.AppendJoin(Environment.NewLine, f.Lines);
			});
		
			System.IO.File.WriteAllText(outputFile, resBuilder.ToString());
		}

		public List<ReadRes> GetMergedFileContent()
		{
			var fileRes = GetMergeFilesShared().Select(ReadFile).ToList();
			return fileRes;
		}
		private ReadRes ReadFile(string filePath) 
		{
			string fileToLook = _sourcePath + filePath;
			if(!System.IO.File.Exists(_sourcePath + filePath))
			{
				throw new Exception($"Unknown file: {fileToLook}");
			}
			var lines = System.IO.File.ReadAllLines(fileToLook);

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