namespace Helpers.TestRunner
{
	public class PuzzleTestCaseResult
	{
		public string CaseName { get; set; }		
		public bool Success { get; set; }
		public string ExpectedResult { get; set; }
		public string ReceivedResult { get; set; }
		public string Message { get; set; }

		public override string ToString()
		{
			string caseRes = "";
			caseRes += Success ? "✔" : "-";
			caseRes += " ";
			caseRes += CaseName;
			caseRes += "\t";
			caseRes += Message ?? "";
			return caseRes;
		}
	}
}