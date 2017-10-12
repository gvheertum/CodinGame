using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * ---
 * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
 **/
namespace Puzzles.PowerOfThor
{
	class Player
	{
		//Container to store the positions of certain artifacts (like thor and the target)
		class PosContainer
		{
			public int X {get;set;}
			public int Y {get;set;}
			public override string ToString()
			{
				return $"x:y {X}:{Y}";
			}
		}
		
		static void Main(string[] args)
		{
			string[] inputs = Console.ReadLine().Split(' ');
			int lightX = int.Parse(inputs[0]); // the X position of the light of power
			int lightY = int.Parse(inputs[1]); // the Y position of the light of power
			int initialTX = int.Parse(inputs[2]); // Thor's starting X position
			int initialTY = int.Parse(inputs[3]); // Thor's starting Y position

			//Put the data is a container that makes sense
			var targetPos = new PosContainer() { X = lightX, Y = lightY };
			var myPos = new PosContainer() { X = initialTX, Y = initialTY };
			// game loop
			while (true)
			{
				//Reading what the status is and log some data
				int remainingTurns = int.Parse(Console.ReadLine()); // The remaining amount of turns Thor can move. Do not remove this line.
				LogConsole($"I'm @ {myPos} and have {remainingTurns} to go to the target");
			
				//Determine the path and direction we want to move to
				var move = DeterminePath(myPos, targetPos);
				LogConsole($"I decided to head: {move}");
				
				// A single line providing the move to be made: N NE E SE S SW W or NW
				Console.WriteLine(move);
			}
		}

		//Based on the positions of the target and character determine the delta for x/y
		//and the path to take
		static string DeterminePath(PosContainer pos, PosContainer target)
		{
			//Determine move and update coords
			int xDelta = target.X - pos.X;
			int yDelta = target.Y - pos.Y;
			
			int xMove = 0;
			int yMove = 0;
			
			LogConsole($"I am x {xDelta} and y {yDelta} steps away from the target");
			
			if(xDelta != 0) { xMove = xDelta > 0 ? 1 : -1; }
			if(yDelta != 0) { yMove = yDelta > 0 ? 1 : -1; }

			LogConsole("moving: x {xMove} x {yMove}");
			pos.X += xMove;
			pos.Y += yMove;
			return DetermineDirection(xMove, yMove);
		}
		
		//Translate X/Y delta to a direction (wind directions: N, S, SE, etc...)
		static string DetermineDirection(int deltaX, int deltaY)
		{
			string res = "";
			if(deltaY > 0) { res = "S"; }
			else if (deltaY < 0) { res = "N"; }
			
			if(deltaX > 0) { res += "E"; }
			else if (deltaX < 0) { res += "W"; }
			
			if(res == "") { throw new Exception("No direction"); }
			
			return res;
		}
		static void LogConsole(object obj)
		{
			Console.Error.WriteLine(obj);
		}
	}
}