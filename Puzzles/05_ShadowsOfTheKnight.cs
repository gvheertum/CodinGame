using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
//https://www.codingame.com/ide/puzzle/shadows-of-the-knight-episode-1
/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.ShadowsOfTheKnight
{
	class Position
	{
		public int X {get;set;}
		public int Y {get;set;}
		public override string ToString()
		{
			return $"Position: x:{X} y:{Y}";
		}
		public Position GetCopy()
		{
			return new Position() { X = X, Y = Y };
		}
		public string GetAsJumpPosition()
		{
			return $"{X} {Y}";
		}
	}

	class Player
	{
		 //Note: this silly tower has its floors numbered from 0 on the top downwards

		//List of historical jumps, for finding the mid-point
		private static List<Position> jumps = new List<Position>();


		public static void Main(string[] args)
		{
			string[] inputs;
			inputs = Console.ReadLine().Split(' ');
			int width = int.Parse(inputs[0]); // width of the building.
			int height = int.Parse(inputs[1]); // height of the building.
			Position flatMinPos = new Position() { X = 0, Y = 0 };
			Position flatMaxPos = new Position() { X = width-1, Y = height-1 }; //Arrays start at 0 ;)

			int nrOfTurns = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
			inputs = Console.ReadLine().Split(' ');
			var batmanPos = new Position() { X = int.Parse(inputs[0]), Y = int.Parse(inputs[1])};
			Log($"Building: h: {height} w:{width}");
			Log($"Batman at: {batmanPos}");

			//Batmans is already in the grid, so add his pos
			jumps.Add(batmanPos.GetCopy());
			
			// game loop
			while (true)
			{
				Log($"{nrOfTurns} left");
				string bombDir = Console.ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
				Log($"Bomb is in {bombDir} direction of us");
				
				var jumpPos = jumps.Count() < 2 //If there are not 2 items in the list we can do an bound jump 
					? GetInitialMaxBound(batmanPos, flatMinPos, flatMaxPos, bombDir) 
					: DetermineJumpPositionBySplitting(jumps, bombDir);
				
				PerformJump(jumpPos);
				nrOfTurns--;
			}
		}

		//Perform the jump of the batty man and add it to the history list
		private static void PerformJump(Position pos)
		{
			Log($"Decided to move the battyman to: {pos}");
				
			// the location of the next window Batman should jump to.
			Console.WriteLine(pos.GetAsJumpPosition());
			jumps.Add(pos.GetCopy());
		}

		//Find a new spot to jump to by using a method to jump to the middle each time
		private static Position DetermineJumpPositionBySplitting(List<Position> positions, string direction)
		{
			var relPos = GetRelevantPositionsForSplit(positions, direction);
			var splitted = CalculateSplitPoint(relPos[0], relPos[1], direction);
			return splitted;
		}

		//See which points in the collection as best suitable for the new splitted position (base on the direction and the last few jumps)
		private static List<Position> GetRelevantPositionsForSplit(List<Position> positions, string direction)
		{
			if(positions.Count == 2) { return positions; } //In this case we just aim between thsese 2, since the first 2 steps ware batman and the outerbound

			//Determine the last position and find canidate to select as boundary
			var pCount = positions.Count();
			var p1 = positions[pCount -1];
			var p2Candidate1 = positions[pCount - 2];
			var p2Candidate2 = positions[pCount - 3];

			Log($"Trying to find the splice for: {p1} and {p2Candidate1} or {p2Candidate2} for direction {direction}");
			var p2Final = p1.GetCopy();
			if(BombIsLeft(direction)) { p2Final.X = Math.Min(p2Candidate1.X, p2Candidate2.X); }
			if(BombIsRight(direction)) { p2Final.X = Math.Max(p2Candidate1.X, p2Candidate2.X); }

			if(BombIsUp(direction)) { p2Final.Y = Math.Min(p2Candidate1.Y, p2Candidate2.Y); }
			if(BombIsDown(direction)) { p2Final.Y = Math.Max(p2Candidate1.Y, p2Candidate2.Y); }

			Log($"Determined: {p2Final} as best bound jump");

			return new List<Position>() { p2Final, p1 };
		}

		//Calculate the split point between 2 bounds
		private static Position CalculateSplitPoint(Position prev, Position curr, string direction)
		{
			Log($"Finding a split point between: {prev} and {curr}");
			var midPos = curr.GetCopy(); //Start with curr as aiming point
			//Only middle if the bomb is moved in a certain direction
			if(BombIsLeft(direction) || BombIsRight(direction)) 
			{ 
				Log("Need to middle for X axis");
				int newX = (prev.X + curr.X) / 2; 
				newX = newX != prev.X ? newX : curr.X; //Fore the item to move (if not moved due to rounding this will force it to move)
				if(newX == prev.X) { newX = ForceNudgeStepForX(newX, direction); }
				Log($"Setting X-> {newX}");
				midPos.X = newX;
			}

			if(BombIsUp(direction) || BombIsDown(direction)) 
			{
				Log("Need to middle for Y axis");
				int newY = (prev.Y + curr.Y) / 2; 
				newY = newY != prev.Y ? newY : curr.Y; //Fore the item to move (if not moved due to rounding this will force it to move)
				if(newY == prev.Y) { newY = ForceNudgeStepForY(newY, direction); }
				Log($"Setting Y-> {newY}");
				midPos.Y = newY;
			}

			Log($"Split point: {midPos}");
			return midPos;
		}

		

		//Determine the outerbound based on the direction, batmans pas and the max pos of the flat.
		private static Position GetInitialMaxBound(Position batman, Position minPos, Position maxPos, string direction)
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
		private static int ForceNudgeStepForX(int step, string direction) 
		{
			if(BombIsLeft(direction)) { return step-1; }
			if(BombIsRight(direction)) { return step+1; }
			Log("Force step is done, but there is no force needed");
			return step;
		}
	   
		private static int ForceNudgeStepForY(int step, string direction) 
		{
			if(BombIsDown(direction)) { return step+1; }
			if(BombIsUp(direction)) { return step-1; }
			Log("Force step is done, but there is no force needed");
			return step;
		}
		//Determine the next step based on direction: UD - LR and position of batman
		//The current implementation just lets batman jump in the direction of the bomb.
		//This is a simple quick solve, a smart algorithm with search/smart guessing is next.
		
		private static Position CalculateStepByStepPosition(string direction, Position batmanPos)
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
		private static bool BombIsDown(string dir) { return dir.IndexOf("D") > -1; }
		private static bool BombIsUp(string dir) { return dir.IndexOf("U") > -1; }
		private static bool BombIsLeft(string dir) { return dir.IndexOf("L") > -1; }
		private static bool BombIsRight(string dir) { return dir.IndexOf("R") > -1; }
		private static void Log(object obj)
		{
			Console.Error.WriteLine(obj);
		}
	}
}