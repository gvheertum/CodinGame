using System.Collections.Generic;

namespace Helpers.TestRunner
{
	public class PuzzleTestCase
	{
		public string CaseName {get;set;}
		public List<string> Input {get;set;} = new List<string>();
		public string ExpectedOutput { get; set; }
	}
}