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
			public string Longitude { get; set; }
			public string Latitude { get; set; }
			public override string ToString()
			{
				return $"{Name}: {Longitude}-{Latitude}";
			}
		}

		public override void Run()
		{
			string longitude = ReadLine();
			string latitude = ReadLine();
			List<DefibData> defibs = new List<DefibData>();

			int nrOfDefibs = int.Parse(ReadLine());
			for (int i = 0; i < nrOfDefibs; i++)
			{
				defibs.Add(GetDefibFromString(ReadLine()));
			}
			defibs.ForEach(d => Log(d));
			WriteLine("answer");
		}

		private DefibData GetDefibFromString(string input)
		{
			var spl = input.Split(";");
			return new DefibData()
			{
				Number = Int32.Parse(spl[0]),
				Name = spl[1],
				Address = spl[2],
				Phone = spl[3],
				Longitude = spl[4],
				Latitude = spl[5]
			};
		}
	}
}