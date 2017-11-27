using System;
using System.Collections.Generic;
using Shared;
using Helpers.TestRunner;
using System.Linq;
using Framework;

namespace ProgramRunners
{
	//Base class for puzzle tests. This will allow you to setup a puzzle and the test cases
	public abstract class PuzzleTestRunnerBase
	{
		public abstract string Name { get; }
		
		protected abstract IEnumerable<PuzzleTestCase> GetTestCases();
		protected abstract Func<IGameEngine, PuzzleMain> GetPuzzleEngineConstructor();
		public void RunPuzzleTests() 
		{
			System.Console.WriteLine($"Running puzzle tests: {Name}");
			var cases = GetTestCases().ToList();
			LogTestResults(new PuzzleTestRunner(GetPuzzleEngineConstructor()).RunTests(cases));
		}

		private void LogTestResults(List<PuzzleTestCaseResult> testRes) 
		{
			Log($"{testRes.Count} tests; {testRes.Count(t=>t.Success)} success; {testRes.Count(t=>!t.Success)} failed");
			testRes.ForEach(t => Log(t));
		}

		private void Log(object msg)
		{
			System.Console.WriteLine(msg);
		}

		protected List<string> FromChunk(string chunck)
		{
			return chunck.Replace("\t","").Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
		}
	}
}