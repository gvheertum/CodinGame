using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;
using Framework;
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

		//For each horse check if we have a distance smaller than the current, if 0 we close our loop (since we can't go lower)
		int MaxHorseStrength = 10000000;
		public override void Run()
		{
			int nrOfHorses = int.Parse(ReadLine());
			bool[] horseStrengthsHitList = new bool[MaxHorseStrength]; //Whether we hit a certain strength somewhere
			List<int> horseStrengths = new List<int>();

			Log($"Processing {nrOfHorses} horses");
			int? currLowestDiff = null;
			
			for (int horseIdx = 0; horseIdx < nrOfHorses; horseIdx++)
			{
				int horseStrength = int.Parse(ReadLine());
				horseStrengths.Add(horseStrength);

				if(horseIdx == 1) //On the first 2 horses we set the first delta
				{ 
					currLowestDiff = Math.Abs(horseStrengths[0] - horseStrengths[1]);
					Log($"Setting intial strength diff to {currLowestDiff}");
				}
				else //Check if we can find a lower distance
				{
					currLowestDiff = GetLowestDistanceForHorseStrength(currLowestDiff, horseIdx, horseStrength, horseStrengthsHitList);
				}

				//If we hit 0 we can stop looking!
				if(currLowestDiff == 0) 
				{
					Log("Found distance of 0, so terminating the loop");
				}

				//Add the item to the list now (if we already had it there it's not good since we will always have a 0
				horseStrengthsHitList[horseStrength] = true;
			}

			Log($"Lowest difference: {currLowestDiff??0}");
			Console.WriteLine($"{currLowestDiff??0}");
		}

		private int? GetLowestDistanceForHorseStrength(int? currLowest, int horseIdx, int currStrengths, bool[] foundStrengths)
		{
			if(currLowest == null) { return null; } //If there is no current lowest we are not going to loop
			int? foundLowest = currLowest;

			//Check if we can find an even lower diff for this speed
			for(int diffToLookFor = currLowest.Value; diffToLookFor >= 0; diffToLookFor--)
			{
				int speedLookup1 = currStrengths - diffToLookFor;
				int speedLookup2 = currStrengths + diffToLookFor;
				if(foundStrengths[speedLookup1] || foundStrengths[speedLookup2]) 
				{
					Log($"Found lowest value of {diffToLookFor} (curr: {currStrengths})"); 
					foundLowest = diffToLookFor;
				}
			}
			return foundLowest;
		}

		// Simple loop solution (just look for each horse what the lowest horse diff is)
		public void RunSimple()
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