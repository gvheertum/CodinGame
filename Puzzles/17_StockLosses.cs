using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
 //https://www.codingame.com/ide/puzzle/stock-exchange-losses


namespace Puzzles.StockLosses
{
	public class StockRange
	{
		public int Highest { get; set; }
		public int Lowest { get; set; }
		public int Diff { get { return Highest-Lowest; } }
		public override string ToString()
		{
			return $"{Highest} - {Lowest} -> {Diff}";
		}
	}
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
			List<StockRange> ranges = new List<StockRange>();

			for (int i = 0; i < n; i++)
			{
				int value = int.Parse(inputs[i]);
				if(IsNewHigh(value, ranges))
				{
					var newRange = new StockRange() { Highest = value, Lowest = value };
					ranges.Add(newRange);
				} //Since the value is a new high we are sure this will not need to update the lowest for the others (doesnt make sense)
				else
				{
					UpdateLowestInRanges(value, ranges);
				}
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");
			Log($"Found {ranges.Count} delta points");
			var rangeToUse = ranges.OrderByDescending(r => r.Diff).First();
			Console.WriteLine(-rangeToUse.Diff);	
		}

		//Check if there is a new high point found, this will create a new range item
		private bool IsNewHigh(int value, List<StockRange> ranges)
		{
			if(!ranges.Any()) { return true; }
			return ranges.All(r => r.Highest < value); 
		}

		//Check which ranges need to be updated to the newest low point (we can not pick any lowest, since if the highest is after the lowpoint the delta is not correct)
		private void UpdateLowestInRanges(int value, List<StockRange> ranges)
		{
			ranges.Where(r => r.Lowest > value).ToList().ForEach(r => r.Lowest = value);
		}

		//Old legacy function for brutforcing algorithm
		private int GetLargestLoss(List<int> values)
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