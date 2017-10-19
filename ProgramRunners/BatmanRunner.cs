using Shared;
using SpecificEngines;

namespace ProgramRunners
{
	public class BatmanRunner
	{
		public void RunBatman()
		{
			//Less Jumps Building: h: 33 w:25 - Batman at: Position: x:2 y:29 - bomb at 24-2
			var bmGame02 = new ShadowsOfTheKnightEngine(25, 33, new Position(2,29), new Position(24,2), 49); // Success
			//Tower: Building: h: 80 w:1 Batman at: Position: x:0 y:1 bomb at 0 36
			var bmGame04 = new ShadowsOfTheKnightEngine(1, 80, new Position(0,1), new Position(0,36), 6); //Success
			//Correct cutting: Building: h: 50 w:50 Batman at: Position: x:0 y:0bomb at 22 22
			var bmGame05 = new ShadowsOfTheKnightEngine(50, 50, new Position(0,0), new Position(22,22), 6); //Success
			
			//Evasive: Building: h: 100 w:100 Batman at: Pos[0] = x:5 y:98, bomb at 0 1,  7 left
			var bmGame06 = new ShadowsOfTheKnightEngine(100, 100, new Position(5,98), new Position(0,1), 7); //fail
			//Not there: Building: h: 9999 w:9999 Batman at: Pos[0] = x:54 y:77, bomb at 9456 4962, 14 left
			var bmGame07 = new ShadowsOfTheKnightEngine(9999, 9999, new Position(54,77), new Position(9456,4962), 14); //fail
			
			new Puzzles.ShadowsOfTheKnight.Player(bmGame06).Run();
		}
	}
}