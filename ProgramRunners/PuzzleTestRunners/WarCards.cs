using System;
using System.Collections.Generic;
using Helpers.TestRunner;
using Shared;
using Framework;

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
			yield return Get26CardsMediumTestCase();
			yield return GetOneGameOneBattleTestCase();
			yield return GetPATTestCase();
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
				CaseName = "Battle test case",
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

		private PuzzleTestCase Get26CardsMediumTestCase()
		{
			return new PuzzleTestCase()
			{
				CaseName = "26 cards, medium game",
				Input = FromChunk(@"
				26
				6H
				7H
				6C
				QS
				7S
				8D
				6D
				5S
				6S
				QH
				4D
				3S
				7C
				3C
				4S
				5H
				QD
				5C
				3H
				3D
				8C
				4H
				4C
				QC
				5D
				7D
				26
				JH
				AH
				KD
				AD
				9C
				2D
				2H
				JC
				10C
				KC
				10D
				JS
				JD
				9D
				9S
				KS
				AS
				KH
				10S
				8S
				2S
				10H
				8H
				AC
				2C
				9H				
				"),
				ExpectedOutput = new List<string>() { "2 56" }
			};
		}	

		private PuzzleTestCase GetOneGameOneBattleTestCase()
		{
			return new PuzzleTestCase()
			{
				CaseName = "One game one battle",
				Input = FromChunk(@"
				26
				10H
				KD
				6C
				10S
				8S
				AD
				QS
				3D
				7H
				KH
				9D
				2D
				JC
				KS
				3S
				2S
				QC
				AC
				JH
				7D
				KC
				10D
				4C
				AS
				5D
				5S
				26
				2H
				9C
				8C
				4S
				5C
				AH
				JD
				QH
				7C
				5H
				4H
				6H
				6S
				QD
				9H
				10C
				4D
				JS
				6D
				3H
				8H
				3C
				7S
				9S
				8D
				2C		
				"),
				ExpectedOutput = new List<string>() { "1 52" }
			};
		}	

		private PuzzleTestCase GetPATTestCase()
		{
			return new PuzzleTestCase()
			{
				CaseName = "PAT",
				Input = FromChunk(@"
				26
				5S
				8D
				10H
				9S
				4S
				6H
				QC
				6C
				6D
				9H
				2C
				7S
				AC
				5C
				7D
				9D
				QS
				4D
				3C
				JS
				2D
				KD
				10S
				QD
				3H
				8H
				26
				4C
				JC
				8S
				10C
				5H
				7H
				3D
				AH
				KS
				10D
				JH
				6S
				2S
				KC
				8C
				9C
				KH
				3S
				AD
				JD
				4H
				7C
				2H
				QH
				5D
				AS	
				"),
				ExpectedOutput = new List<string>() { "PAT" }
			};
		}	
	}
}