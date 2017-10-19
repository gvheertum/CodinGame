using Shared;

namespace ProgramRunners
{
	public class SpoonRunner
	{
		public void RunSpoon()
		{
			var buffer1 = new [] { "2","2", "00", "0." };
			var buffer2 = new [] { "5", "1", "0.0.0" };
			var engine = new StringBufferGameEngine(buffer1);
			new Puzzles.ThereIsNoSpoon.Player(engine).Run();
		}
	}
}