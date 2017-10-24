using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

//https://www.codingame.com/ide/puzzle/defibrillators
 

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.Defibrillators
{
	public class DefibrillatorsSolution : PuzzleMain
	{
		public DefibrillatorsSolution(IGameEngine gameEngine) : base(gameEngine) { }

		static void Main(string[] args)
		{
			new DefibrillatorsSolution(new CodingGameProxyEngine()).Run();
		}

		

		public class DefibData
		{
			public int Number { get; set; }
			public string Name { get; set; }
			public string Address { get; set; }
			public string Phone { get; set; }
			public LongLatPosition Coord { get; set;}
			public override string ToString()
			{
				return $"{Name}: coord:{Coord}";
			}
		}

		public override void Run()
		{
			string longitude = ReadLine();
			string latitude = ReadLine();
			var userLongLat = new LongLatPosition() { Longitude = GetDecimalFromInput(longitude), Latitude = GetDecimalFromInput(latitude) };
			var distCalc = new DistanceCalculator();

			List<DefibData> defibs = new List<DefibData>();
			int nrOfDefibs = int.Parse(ReadLine());
			for (int i = 0; i < nrOfDefibs; i++)
			{
				defibs.Add(GetDefibFromString(ReadLine()));
			}
			Log($"User at: {userLongLat}");
			Log("Defib before sort:");
			defibs.ForEach(d => Log(d));

			Log("Defib after log:");
			var sortedDefibs = defibs.OrderBy(d => distCalc.GetDistance(userLongLat, d.Coord)).ToList();
			sortedDefibs.ForEach(d => Log(d));
		
			WriteLine(sortedDefibs.First().Name);
		}

		

		private DefibData GetDefibFromString(string input)
		{
			var spl = input.Split(new char[] {';'}, StringSplitOptions.None);
			return new DefibData()
			{
				Number = Int32.Parse(spl[0]),
				Name = spl[1],
				Address = spl[2],
				Phone = spl[3],
				Coord = new LongLatPosition() { Longitude = GetDecimalFromInput(spl[4]), Latitude = GetDecimalFromInput(spl[5]) }
			};
		}

		private decimal GetDecimalFromInput(string input)
		{
			return decimal.Parse(input.Replace(",", "."),System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
	}
}