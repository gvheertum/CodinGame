using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Helpers
{

	//TODO: This could be made smarter to only include references really needed, this can be headers in the file indicating that we want to incude a certain base file
	//TODO: Base files can include other required files
	//TODO: Allow folders to exist per puzzle allowing us to merge certain puzzle specific files
	public class FileMerger : FileMergerBase
	{
		public FileMerger(string sourcePath, string puzzlePath, string challengePath, string sharedPath, string frameworkPath, string outputPath) : base(sourcePath, puzzlePath, challengePath, sharedPath, frameworkPath, outputPath)
		{
		}

		public class ReadRes
		{
			public string FullFileName { get; set; }
			public string FileName { get; set; }
			public List<string> Requires {get;set;} = new List<string>();
			public List<string> Lines { get; set;} = new List<string>();
			public List<string> Usings { get; set; } = new List<string>();
		}

		//Start writing the merge files for the puzzle files in the puzzle folder
		public void MergePuzzleFiles()
		{
			GetCodeFilesInPath(_puzzlePath).ForEach(file => MergePuzzleFile(file, "puzzle"));
			GetCodeFilesInPath(_challengePath).ForEach(file => MergePuzzleFile(file, "challenge"));
		}

		private void MergePuzzleFile(CodeElement codeElement, string prefix)
		{
			string fileNameToUse = codeElement.IsFile 
				? new System.IO.FileInfo(codeElement.Location).Name 
				: new System.IO.DirectoryInfo(codeElement.Location).Name;
			LogInfo($"Merging file for: {fileNameToUse} (file: {codeElement.IsFile})");
			var outputFile = _sourcePath + "Merged/" + prefix + "." + fileNameToUse + ".merged";
			
			var files = new List<ReadRes>();
			var myFile = ReadFile(codeElement);
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
		private ReadRes ReadFile(CodeElement element) 
		{
			string firstFileToTake = element.IsFile ? element.Location : GetFirstReadFileForPuzzle(element.Location);
			if(!System.IO.File.Exists(firstFileToTake))
			{
				throw new Exception($"Unknown file: {firstFileToTake}");
			}
			var lines = System.IO.File.ReadAllLines(firstFileToTake);
			if(!element.IsFile)	
			{
				LogInfo($"{element.Location} is a folder, iterating through child nodes");
				lines = AppendSibblingFiles(lines, firstFileToTake).ToArray();
			}

			var fileInfo = new System.IO.FileInfo(firstFileToTake);
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
				FullFileName = element.Location,
				FileName = fileName,
				Lines = codeLines,
				Usings = usingLines,
				Requires = requireLines
			};
		}
		
		//Determine the first file to read from a puzzle folder, this is identified by puzzle or game as filename
		//or the first file containing static void Main
		private string GetFirstReadFileForPuzzle(string folder)
		{
			LogInfo($"Getting puzzle file for {folder}");
			var di = new System.IO.DirectoryInfo(folder);
			var files = di.GetFiles();
			var file = files.SingleOrDefault(f => 
				string.Equals(f.Name, "Puzzle.cs", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(f.Name, "Game.cs", StringComparison.OrdinalIgnoreCase)
			);

			if (file!=null) 
			{ 
				LogInfo($"Candidate: {file.FullName} (for name match)");
				return file.FullName; 
			}

			foreach(var fTemp in files)
			{
				var lines = System.IO.File.ReadAllText(fTemp.FullName);
				if(lines.IndexOf("static void Main(") > -1) 
				{ 
					LogInfo($"Candidate: {file.FullName} (for having static void Main)");
					return fTemp.FullName; 
				}
			}
			throw new Exception("No puzzle main could be identified");
		}

		//When reading a folder, we take a primary file and need to read the sibbling files to ensure all items are included
		private List<string> AppendSibblingFiles(IEnumerable<string> originalLines, string fileLocation)
		{
			var res = new List<string>(originalLines);
			var di = new FileInfo(fileLocation).Directory;
			//Exclude the already read file
			var files = di.GetFiles().Where(f => f.FullName != fileLocation);
			if(files.Any())
			{
				LogInfo($"Reading {files.Count()} sibblings in {di.FullName}");
				res.AddRange(files.SelectMany(f => 
				{
					var lines = new List<string>() { $"//File: Puzzlesub: {f.FullName}" };
					lines.AddRange(System.IO.File.ReadAllLines(f.FullName));
					return lines;
				}));
			}
			return res;
		}

		

		private class CodeElement
		{
			public bool IsFile {get;set;}
			public string Location {get;set;}
		}

		private List<CodeElement> GetCodeFilesInPath(string path)
		{
			var files = System.IO.Directory.GetFiles(path);
			var folders = System.IO.Directory.GetDirectories(path);
			List<CodeElement> elements = new List<CodeElement>();
			elements.AddRange(files.Where(f => f.EndsWith(".cs")).Select(f => new CodeElement() { IsFile = true, Location = f }).ToList());
			elements.AddRange(folders.Select(f => new CodeElement() { IsFile = false, Location = f }).ToList());
			return elements;
		}
	}
}