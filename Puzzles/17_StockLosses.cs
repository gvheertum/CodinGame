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
 //https://www.codingame.com/ide/puzzle/don't-panic-episode-1
 //https://www.codingame.com/training/hard/don't-panic-episode-2

namespace Puzzles.StockLosses
{
	public class StockLossesPuzzle : PuzzleMain
	{
		protected StockLossesPuzzle(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			new StockLossesPuzzle(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
			int n = int.Parse(ReadLine());
			string[] inputs = ReadLine().Split(' ');
			List<int> stockValues = new List<int>();
			for (int i = 0; i < n; i++)
			{
				int v = int.Parse(inputs[i]);
				stockValues.Add(v);
				//Console.Error.WriteLine($"Found: {v}");
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			int lowestDrop = GetLargestLoss(stockValues);
			Log($"Largest loss: {lowestDrop}");
			if(lowestDrop > 0) 
			{ 
				Log("Loss was positive, so make it 0");
				lowestDrop = 0; 
			}
			Console.WriteLine(lowestDrop);
			
		}
		static int GetLargestLoss(List<int> values)
		{
			List<int> lowestDropPerItem = new List<int>();
			for(int i = 0; i < values.Count; i++)
			{
				var currVal = values[i];
				if(i >= values.Count -1) { continue; } //Skip last
				var biggestDip = currVal - (values.Skip(i+1).Min());
				//Console.Error.WriteLine($"For {currVal} the biggest drop was: {biggestDip}");
				lowestDropPerItem.Add(biggestDip);
			}
			return -lowestDropPerItem.Max();
		}
	}
}