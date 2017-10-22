using System;
using System.Collections.Generic;
using Helpers.TestRunner;
using Shared;

namespace ProgramRunners.PuzzleTestRunners
{
	public class Defibrillators : ProgramRunners.PuzzleTestRunnerBase
	{
	
		public override string Name => "Defibrillators";

		protected override Func<IGameEngine, PuzzleMain> GetPuzzleEngineConstructor()
		{
			return (engine) => new Puzzles.Defibrillators.DefibrillatorsSolution(engine);
		}

		protected override IEnumerable<PuzzleTestCase> GetTestCases()
		{
			//Example 1
			yield return new PuzzleTestCase() 
			{ 
				CaseName = "Example",  
				ExpectedOutput = "Maison de la Prevention Sante", 
				Input = new List<string> 
				{ 
					"3,879483",
					"43,608177",
					"3",
					"1;Maison de la Prevention Sante;6 rue Maguelone 340000 Montpellier;;3,87952263361082;43,6071285339217",
					"2;Hotel de Ville;1 place Georges Freche 34267 Montpellier;;3,89652239197876;43,5987299452849",
					"3;Zoo de Lunaret;50 avenue Agropolis 34090 Mtp;;3,87388031141133;43,6395872778854"
				}
			};
		}
	}
}