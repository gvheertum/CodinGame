using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;
//https://www.codingame.com/ide/puzzle/shadows-of-the-knight-episode-1
/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.ShadowsOfTheKnight
{
	//Revision II: Updating the algorithm to find the midway

	

	public class Player : PuzzleMain
	{
		 //Note: this silly tower has its floors numbered from 0 on the top downwards

		//List of historical jumps, for finding the mid-point
		private List<Position> jumps = new List<Position>();

		public Player(IGameEngine gameEngine) : base(gameEngine) {}

		public static void Main(string[] args)
		{
			new Player(null).Run();
		}
		public void Run()
		{
			string[] inputs;
			inputs = ReadLine().Split(' ');
			int width = int.Parse(inputs[0]); // width of the building.
			int height = int.Parse(inputs[1]); // height of the building.
			Position flatMinPos = new Position() { X = 0, Y = 0 };
			Position flatMaxPos = new Position() { X = width-1, Y = height-1 }; //Arrays start at 0 ;)

			int nrOfTurns = int.Parse(ReadLine()); // maximum number of turns before game over.
			inputs = ReadLine().Split(' ');
			var batmanPos = new Position() { X = int.Parse(inputs[0]), Y = int.Parse(inputs[1])};
			Log($"Building: h: {height} w:{width}");
			Log($"Batman at: {batmanPos}");

			//Batmans is already in the grid, so add his pos
			jumps.Add(batmanPos.GetCopy());
			
			// game loop
			while (IsRunning())
			{
				Log($"{nrOfTurns} left");
				string bombDir = ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
				Log($"Bomb is in {bombDir} direction of us");
				
				var jumpPos = jumps.Count() < 2 //If there are not 2 items in the list we can do an bound jump 
					? GetInitialMaxBound(batmanPos, flatMinPos, flatMaxPos, bombDir) 
					: DetermineJumpPositionBySplitting(jumps, bombDir);
				
				PerformJump(jumpPos);
				nrOfTurns--;
			}
		}

		//Perform the jump of the batty man and add it to the history list
		private void PerformJump(Position pos)
		{
			Log($"Decided to move the battyman to: {pos}");
				
			// the location of the next window Batman should jump to.
			WriteLine(pos.GetAsJumpPosition());
			jumps.Add(pos.GetCopy());
		}

		//Find a new spot to jump to by using a method to jump to the middle each time
		private Position DetermineJumpPositionBySplitting(List<Position> positions, string direction)
		{
			var relPos = GetRelevantPositionsForSplit(positions, direction);
			var splitted = CalculateSplitPoint(relPos[0], relPos[1], direction);
			return splitted;
		}

		//See which points in the collection as best suitable for the new splitted position (base on the direction and the last few jumps)
		private List<Position> GetRelevantPositionsForSplit(List<Position> positions, string direction)
		{
			//Determine the last position and find canidate to select as boundary
			var pCount = positions.Count();
			var p1 = positions[pCount -1];
			var p2= positions[pCount - 2];
			return new List<Position>() { p2, p1 };
		}

		//Calculate the split point between 2 bounds
		private Position CalculateSplitPoint(Position prev, Position curr, string direction)
		{
			Log($"Finding a split point between: {prev} and {curr}");
			
			var xJumpPrev = Math.Abs(prev.X - curr.X);
			var yJumpPrev = Math.Abs(prev.Y - curr.Y);
			
			var middledXJump = xJumpPrev / 2;
			var middledYJump = yJumpPrev / 2;
			if(middledXJump == 0) { Log("X jump was calculated to 0, forced to 1"); middledXJump = 1; }
			if(middledYJump == 0) { Log("Y jump was calculated to 0, forced to 1"); middledYJump = 1; }

			Log($"Last jump delta => x:{xJumpPrev} y:{yJumpPrev}, midPoint delta => x:{middledXJump} y:{middledYJump}");
			var newPos = curr.GetCopy(); //Start with curr as aiming point
			if(BombIsLeft(direction)) { newPos.X = newPos.X - middledXJump; }
			if(BombIsRight(direction)) { newPos.X = newPos.X + middledXJump; }
			
			if(BombIsUp(direction)) { newPos.Y = newPos.Y - middledYJump; }
			if(BombIsDown(direction)) { newPos.Y = newPos.Y + middledYJump; }

			Log($"New jump loc: {newPos}");
			return newPos;
		}

		//Determine the outerbound based on the direction, batmans pas and the max pos of the flat.
		private Position GetInitialMaxBound(Position batman, Position minPos, Position maxPos, string direction)
		{
			Log($"Determine the bound based on batman: {batman} min: {minPos} max: {maxPos} dir: {direction}");
			//In some cases the bomb is hiding is the corner, we can directly try to go there
			Position newPos = batman.GetCopy();
			if(BombIsUp(direction)) { newPos.Y = minPos.Y; }
			if(BombIsDown(direction)) { newPos.Y = maxPos.Y; }

			if(BombIsLeft(direction)) { newPos.X = minPos.X; }
			if(BombIsRight(direction)) { newPos.X = maxPos.X; }
			Log($"Took the outerbound as new move dir: {newPos}");
			return newPos;
		}

		//In some cases we end up with rounding issues. If the position does not move when splitting, but
		//when there IS a change required this function will force the step to be increased/decreased
		private int ForceNudgeStepForX(int step, string direction) 
		{
			if(BombIsLeft(direction)) { return step-1; }
			if(BombIsRight(direction)) { return step+1; }
			Log("Force step is done, but there is no force needed");
			return step;
		}
	   
		private int ForceNudgeStepForY(int step, string direction) 
		{
			if(BombIsDown(direction)) { return step+1; }
			if(BombIsUp(direction)) { return step-1; }
			Log("Force step is done, but there is no force needed");
			return step;
		}
		//Determine the next step based on direction: UD - LR and position of batman
		//The current implementation just lets batman jump in the direction of the bomb.
		//This is a simple quick solve, a smart algorithm with search/smart guessing is next.
		
		private Position CalculateStepByStepPosition(string direction, Position batmanPos)
		{   
			int yDelta = 0;
			int xDelta = 0;
		
			//Is the bomb up or down from us
			if(BombIsUp(direction)) { yDelta = -1; }
			else if(BombIsDown(direction)) { yDelta = +1; }
		
			//Is the bomb left or right from us
			if(BombIsLeft(direction)) { xDelta = -1; }
			else if(BombIsRight(direction)) { xDelta = +1; }
		
			batmanPos.X = batmanPos.X + xDelta;
			batmanPos.Y = batmanPos.Y + yDelta;

			return batmanPos;
		}
		
		

		//HELPER FUNCTIONS
		//***********************/
		private bool BombIsDown(string dir) { return dir.IndexOf("D") > -1; }
		private bool BombIsUp(string dir) { return dir.IndexOf("U") > -1; }
		private bool BombIsLeft(string dir) { return dir.IndexOf("L") > -1; }
		private bool BombIsRight(string dir) { return dir.IndexOf("R") > -1; }
		
	}
}