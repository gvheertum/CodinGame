using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Framework;
using Helpers.CustomGameEngines;

namespace Helpers.TestRunner
{

	public class PuzzleTestRunner
	{
		private Func<IGameEngine, PuzzleMain> _engineInit;
		public PuzzleTestRunner(Func<IGameEngine, PuzzleMain> engineInit)
		{
			_engineInit = engineInit;
		}

		public List<PuzzleTestCaseResult> RunTests(List<PuzzleTestCase> cases) 
		{
			return cases.Select(RunSingleTest).ToList();
		}

		private PuzzleTestCaseResult RunSingleTest(PuzzleTestCase testCase)
		{
			var ptcs = new PuzzleTestCaseResult()
			{ 
				CaseName = testCase.CaseName,
				Success = true,
				ExpectedResult = string.Join(Environment.NewLine, testCase.ExpectedOutput) 
			};
			
			try
			{
				var bufferGameEngine = new StringBufferGameEngine(testCase.Input);
				_engineInit(bufferGameEngine).Run();
				var response = string.Join(Environment.NewLine, bufferGameEngine.ReadBackWrittenLines());
				ptcs.ReceivedResult = response;
				if(!string.Equals(ptcs.ExpectedResult, ptcs.ReceivedResult, StringComparison.OrdinalIgnoreCase))
				{
					ptcs.Success = false;
					ptcs.Message = $"Result incorrect (exp: {ptcs.ExpectedResult} res: {ptcs.ReceivedResult})";
				}
			}
			catch(Exception e)
			{
				ptcs.Success = false;
				ptcs.Message = $"Exception during execution: {e.Message}";
			}
			return ptcs;
		}
	}
}