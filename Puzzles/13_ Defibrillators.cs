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

		public override void Run()
		{
			string longitude = ReadLine();
			string latitude = ReadLine();
			int nrOfDefibs = int.Parse(ReadLine());
			for (int i = 0; i < nrOfDefibs; i++)
			{
				string DEFIB = ReadLine();
			}

			WriteLine("answer");
		}
	}
}