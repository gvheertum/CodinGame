using System;
using System.Collections.Generic;
using Helpers.TestRunner;
using Shared;

namespace ProgramRunners.PuzzleTestRunners
{
	public class WarCards : ProgramRunners.PuzzleTestRunnerBase
	{
		public override string Name => "WarCards test runner";

		protected override Func<IGameEngine, PuzzleMain> GetPuzzleEngineConstructor()
		{
			return (engine) => new Puzzles.Warcards.WarCardsPuzzle(engine);
		}

		protected override IEnumerable<PuzzleTestCase> GetTestCases()
		{
			yield return GetExampleTestCase();
			yield return GetBattleTestCase();
		}

		private PuzzleTestCase GetExampleTestCase()
		{
			return new PuzzleTestCase()
			{
				CaseName = "Example test case",
				Input = new List<string>()
				{
					"3",
					"AD",
					"KC",
					"QC",
					"3",
					"KH",
					"QS",
					"JC"
				},
				ExpectedOutput = new List<string>() { "1 3" }
			};
		}

		private PuzzleTestCase GetBattleTestCase()
		{
			return new PuzzleTestCase()
			{
				CaseName = "Example test case",
				Input = new List<string>()
				{
					"5",
					"8C",
					"KD",
					"AH",
					"QH",
					"2S",
					"5",
					"8D",
					"2D",
					"3H",
					"4D",
					"3S"
				},
				ExpectedOutput = new List<string>() { "2 1" }
			};
		}	
	}
}