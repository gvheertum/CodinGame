using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

/**
 * https://www.codingame.com/ide/puzzle/mime-type
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.Mime
{
	public class Solution : Shared.PuzzleMain
	{
		public class MimeType
		{
			public virtual string Extension {get;set;}
			public virtual string Code {get;set;}
			public override string ToString()
			{
				return $"Mime: {Extension} -> {Code}";
			}
		}

		public class UnkownMimeType : MimeType
		{
			public override string Extension { get; set; } = "*";
			public override string Code {get; set; } = "UNKNOWN";
		}

		protected Solution(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			var sol = new Solution(new Shared.CodingGameProxyEngine());
			sol.Run();
		}
		public void Run()
		{
			int nrOfMimeTypes = int.Parse(ReadLine()); // Number of elements which make up the association table.
			int nrOfFiles = int.Parse(ReadLine()); // Number Q of file names to be analyzed.
			List<MimeType> types = new List<MimeType>();
			for (int i = 0; i < nrOfMimeTypes; i++)
			{
				string[] inputs = ReadLine().Split(' ');
				types.Add(new MimeType() { Extension = inputs[0], Code = inputs[1] });
			}

			// For each of the Q filenames, display on a line the corresponding MIME type. If there is no corresponding type, then display UNKNOWN.
			List<MimeType> foundTypes = new List<MimeType>();
			for (int i = 0; i < nrOfFiles; i++)
			{
				string fileName = ReadLine(); // One file name per line.
				var mimeType = GetMimeTypeForFile(fileName, types);
				Log($"file {fileName} has mimetype {mimeType}");
				foundTypes.Add(mimeType);
			}
			Log("Pushing results to output");
			foundTypes.ForEach(t => WriteLine(t.Code));
		}

		private MimeType GetMimeTypeForFile(string file, List<MimeType> types)
		{
			string determinedExtension = GetExtensionFromFile(file);
			return types.FirstOrDefault(t => string.Equals(t.Extension,determinedExtension, StringComparison.OrdinalIgnoreCase)) ?? new UnkownMimeType();
		}

		private string GetExtensionFromFile(string file)
		{
			//File .ext is considered to have extension ext, file..ext has extension ext, file. has no extension, file has no extension
			int splitIdx = file.LastIndexOf(".");			
			string extension = splitIdx > -1 ? file.Substring(splitIdx).Remove(0,1) : null; //If there is a dot split the string and remove the . from the prefix
			Log($"File {file} resolved to extension: {extension}");
			return extension;
		}
	}
}