using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.AsciiGenerator
{
	public class AsciiLetter
	{
		private int _height;
		private int _width;
		public AsciiLetter(string character, int height, int width)
		{
			Character = character;
			_height = height;
			_width = width;
		}
		public string Character { get; private set; }
		public List<string> LineData { get; private set; } = new List<string>();
		public void AddLineData(string feed) 
		{
			if(LineData.Count >= _height) { throw new Exception("Cannot exceed char height"); }
			LineData.Add(feed);
		}

		public string GetLineData(int lineIdx)
		{
			if(lineIdx >= _height) { throw new Exception($"Line index {lineIdx} is outside of the bounds"); }
			return LineData[lineIdx];
		}
	}

	public class AsciiPuzzle : PuzzleMain
	{
		protected AsciiPuzzle(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			var asciiPuzzle = new AsciiPuzzle(new CodingGameProxyEngine());
			asciiPuzzle.Run();
		}

		public void Run()
		{
			int charWidth = int.Parse(ReadLine());
			int charHeight = int.Parse(ReadLine());
			string textToWrite = ReadLine();

			Log($"Char data: w:{charWidth} h:{charHeight}");
			Log($"Requested to write: {textToWrite}");
			var charList = GenerateCharacters(charHeight, charWidth);
			Log($"Generated {charList.Count} characters");

			for (int i = 0; i < charHeight; i++)
			{
				string currRow = ReadLine();
				SplitLineDataOverCharacters(currRow, charWidth, charList);
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");



			WriteLine("answer");
		}

		private List<AsciiLetter> GenerateCharacters(int charH, int charW) 
		{
			TextHelper th = new TextHelper(true);
			return th.GetAllCharactersAsCollection().Select(character => new AsciiLetter(character, charH, charW)).ToList();
		}

		private void SplitLineDataOverCharacters(string line, int charWidth, List<AsciiLetter> characters)
		{
			//line.Split
		}
	}
}