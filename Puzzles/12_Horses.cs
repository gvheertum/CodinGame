using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

namespace Puzzles.Horses
{

/**
https://www.codingame.com/ide/puzzle/horse-racing-duals

Game Input

Input
Line 1: Number N of horses

The N following lines: the strength Pi of each horse. Pi is an integer.

Output
The difference D between the two closest strengths. D is an integer greater than or equal to 0.


	* Auto-generated code below aims at helping you parse
	* the standard input according to the problem statement.
	**/
	public class Solution : PuzzleMain
	{
		protected Solution(IGameEngine gameEngine) : base(gameEngine) { }

		static void Main(string[] args)
		{
			new Solution(new CodingGameProxyEngine()).Run();
		}
		public void Run()
		{
			int nrOfHorses = int.Parse(ReadLine());
			List<int> horses = new List<int>();
			for (int i = 0; i < nrOfHorses; i++)
			{
				horses.Add(int.Parse(ReadLine()));
			}
			Log($"Found {horses.Count}");

			int lowest = GetLowestDifference(horses);
			Log($"Lowest difference: {lowest}");
			Console.WriteLine($"{lowest}");
		}

		public int GetLowestDifference(List<int> values)
		{
			int? lowestDiff = null;
			//Iterate through all values, we only need to look forward to cover comparison with all items (since it will already be compared with previous items in their iteration)
			for(int i = 0; i < values.Count; i++)
			{
				for(int j = i + 1; j < values.Count; j++) //Compare with the next item (not with self, would be a strange race)
				{
					int currDiff = Math.Abs(values[i] - values[j]);	
					if(currDiff < lowestDiff || lowestDiff == null) 
					{
						Log($"Setting lowest diff from {lowestDiff} to {currDiff} as result of h1={values[i]} h2={values[j]}");
						lowestDiff = currDiff;
					}
				}
			}
			if(lowestDiff == null) { throw new Exception("No lowest diff found"); }
			return lowestDiff.Value;
		}
	}
}