using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
//https://www.codingame.com/ide/puzzle/temperatures
/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.Temperatures
{
	class Solution
	{
		class Temperature
		{
			public int TemperatureValue { get; set; }
			public int DifferenceFromZero { get { return Math.Abs(0 - TemperatureValue); } }
			public int ReadingIndex { get; set; }
			public override string ToString()
			{
				return $"Temperature[{ReadingIndex}]: {TemperatureValue} (diff to 0-> {DifferenceFromZero})";
			}
		}


		static void Main(string[] args)
		{
			List<Temperature> temperatures = new List<Temperature>();

			int n = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
			string[] inputs = Console.ReadLine().Split(' ');
			for (int i = 0; i < n; i++)
			{
				int t = int.Parse(inputs[i]); // a temperature expressed as an integer ranging from -273 to 5526
				temperatures.Add(new Temperature() { TemperatureValue = t, ReadingIndex = i });
			}

			Log($"Read {temperatures.Count} temperatures");
			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...")
			var elementsClosest = GetTemperaturesClosestToZero(temperatures);
			var bestMatch = GetBestMatch(elementsClosest);
			Log($"Closest: {bestMatch}");
			Console.WriteLine(bestMatch.TemperatureValue);
		}

		//Get the temps that are closest to 0
		private static IEnumerable<Temperature> GetTemperaturesClosestToZero(IEnumerable<Temperature> temperatures) 
		{	
			if(!temperatures.Any()) { return new List<Temperature>(); }

			var lowestDiff = temperatures.Min(t => t.DifferenceFromZero);
			Log($"The closest to 0 have a diff of: {lowestDiff}");

			return temperatures.Where(t => t.DifferenceFromZero == lowestDiff);
		}

		//From a list of temperatures closes to 0 get the best match (pos if available)
		private static Temperature GetBestMatch(IEnumerable<Temperature> temperatures)
		{
			//In case there are no temp we force a 0 as result
			if(!temperatures.Any()) 
			{
				Log("No temperatures, faking a temp of 0");
				return new Temperature() { ReadingIndex = -404, TemperatureValue = 0 };
			}

			//Order by having the highest (positive) temperature on top and pop the first item
			return temperatures.OrderByDescending(t => t.TemperatureValue).First();
		}

		private static void Log(object obj)
		{
			Console.Error.WriteLine(obj);
		}
	}
}