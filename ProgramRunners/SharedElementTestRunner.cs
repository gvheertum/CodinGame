using System;
using Shared;

namespace ProgramRunners
{

	public class SharedElementTestRunner
	{

		public void RunHelperTests()
		{
			RunBitTests();
			RunCharTests();
		}

		private void RunBitTests()
		{
			BitHelper bh = new BitHelper(2);
			AssertAreEqual("10", bh.GetBitStringForValue(2));
			AssertAreEqual("11", bh.GetBitStringForValue(3));

			bh = new BitHelper(3);
			AssertAreEqual("011", bh.GetBitStringForValue(3));

			System.Console.WriteLine("Bit tests succeeded");
		}

		private void RunCharTests()
		{
			var th = new TextHelper(true);
			AssertAreEqual(67, th.GetCharNumber('C'));
			System.Console.WriteLine("Char tests succeeded");
		}

		private void AssertAreEqual(int val, int val2)
		{
			if(val != val2) { throw new Exception($"Expected {val}, got {val2}"); }
		}

		private void AssertAreEqual(string val, string val2)
		{
			if(val != val2) { throw new Exception($"Expected {val}, got {val2}"); }
		}
	}
}