using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;
using Framework;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
//https://www.codingame.com/ide/puzzle/ascii-art
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

		public override void Run()
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
			
			//Get the chars to draw, and iterate each line of those chars
			var charsToDraw = GetDrawingCharacters(textToWrite, charList);
			for(int i = 0; i < charHeight; i++)
			{
				WriteLine(RenderLineForLetters(i, charsToDraw));
			}
		}

		private List<AsciiLetter> GenerateCharacters(int charH, int charW) 
		{
			TextHelper th = new TextHelper(true);
			return th.GetAllCharactersAsCollection().Select(character => new AsciiLetter(character, charH, charW)).ToList();
		}

		private void SplitLineDataOverCharacters(string line, int charWidth, List<AsciiLetter> characters)
		{
			if(line.Length < characters.Count * charWidth) { throw new Exception("The line length is not matching the expected length"); }
			//line.Split
			
			for(int i = 0; i < characters.Count; i++)
			{
				string res = line.Substring(i * charWidth, charWidth);
				characters[i].AddLineData(res);	
			}
		}

		private List<AsciiLetter> GetDrawingCharacters(string input, List<AsciiLetter> knownLetters)
		{
			List<AsciiLetter> letters = new List<AsciiLetter>();
			TextHelper th = new TextHelper(true);
			foreach(char i in input)
			{
				int idx = th.GetIndexOfChar($"{i}");
				var letterToUse = knownLetters[idx];
				Log($"Char {i} has idx {idx} and represented by {letterToUse.Character}");
				letters.Add(letterToUse);
			}
			return letters;
		}

		private string RenderLineForLetters(int lineIdx, List<AsciiLetter> letters)
		{
			StringBuilder sb = new StringBuilder();
			letters.ForEach(l => sb.Append(l.GetLineData(lineIdx)));
			return sb.ToString();
		}
	}
}