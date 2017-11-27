using System;
using System.Collections.Generic;
using Helpers.TestRunner;
using Shared;
using Framework;

namespace ProgramRunners.PuzzleTestRunners
{
	public class Spoon : PuzzleTestRunnerBase
	{
		public override string Name => "There is no spoon";

		protected override Func<IGameEngine, PuzzleMain> GetPuzzleEngineConstructor()
		{
			return (engine) => new Puzzles.ThereIsNoSpoon.Player(engine);
		}

		protected override IEnumerable<PuzzleTestCase> GetTestCases()
		{
			yield return new PuzzleTestCase() 
			{
				CaseName = "Spoon test 1",
				Input = new List<string>() 
				{ 
					"2",
					"2", 
					"00", 
					"0." 
				},
				ExpectedOutput = new List<string>() 
				{ 
					"0 0 1 0 0 1",
					"1 0 -1 -1 -1 -1",
					"0 1 -1 -1 -1 -1"
				}
			};

			yield return new PuzzleTestCase() 
			{
				CaseName = "Spoon test 2",
				Input = new List<string>() 
				{ 
					"5", 
					"1", 
					"0.0.0" 
				},
				ExpectedOutput = new List<string>() 
				{ 
					"0 0 2 0 -1 -1",
					"2 0 4 0 -1 -1",
					"4 0 -1 -1 -1 -1"
				}
			};
		}
	}
}