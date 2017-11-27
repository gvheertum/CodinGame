using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;
using Framework;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
 //https://www.codingame.com/ide/puzzle/don't-panic-episode-1
 //https://www.codingame.com/training/hard/don't-panic-episode-2

namespace Puzzles.DontPanic
{
	public class DontPanicPlayer : PuzzleMain
	{
		protected DontPanicPlayer(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			new DontPanicPlayer(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
			string[] inputs;
			inputs = ReadLine().Split(' ');
			int nbFloors = int.Parse(inputs[0]); // number of floors
			Log($"Nr of floors: {nbFloors}");
			
			int width = int.Parse(inputs[1]); // width of the area
			Log($"Width: {width}");

			int nbRounds = int.Parse(inputs[2]); // maximum number of rounds
			int exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
			int exitXPos = int.Parse(inputs[4]); // position of the exit on its floor
			var exitPos = new ElementPos() { Floor = exitFloor, XPos = exitXPos, ElementType = "Exit" };			
			Log(exitPos);
			
			int nbTotalClones = int.Parse(inputs[5]); // number of generated clones
			Log($"We have {nbTotalClones} clones");
			
			int nbAdditionalElevators = int.Parse(inputs[6]); // ignore (always zero)
			int nbElevators = int.Parse(inputs[7]); // number of elevators
			List<ElementPos> elevators = new List<ElementPos>();
			for (int i = 0; i < nbElevators; i++)
			{
				inputs = ReadLine().Split(' ');
				int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
				int elevatorPos = int.Parse(inputs[1]); // position of the elevator on its floor
				var elevator = new ElementPos() { Floor = elevatorFloor, XPos = elevatorPos, ElementType = "Elevator" }; 
				elevators.Add(elevator);
				Log(elevator);
			}

			int currRound = 0;
			// game loop
			while (true)
			{
				currRound++;
				Log($"Current round {currRound} of {nbRounds}");
				var clonePos = GetClonePosFromInput(ReadLine());
				Log($"Leading clone: {clonePos}");
			
				var action = GetAction(clonePos, exitPos, elevators);
				WriteLine($"{action}".ToUpperInvariant()); // action: WAIT or BLOCK or ELEVATOR
			}
		}

		public enum CloneAction 
		{
			Wait,
			Block,
			Elevator
		}

		private CloneAction GetAction(ClonePos clonePos, ElementPos exit, List<ElementPos> elevators) 
		{
			if(clonePos.XPos == -1 && clonePos.Floor == -1) { Log("Invalid clone pos, wait!"); return CloneAction.Wait; }

			bool cloneOnExitFloor = clonePos.Floor == exit.Floor;
			Log($"Drone is on exit floor: {cloneOnExitFloor}");

			var elementToNavTo = cloneOnExitFloor ? exit : elevators.FirstOrDefault(e => e.Floor == clonePos.Floor);
			if(elementToNavTo == null) 
			{
				Log("We want to move to an elevator on floor {}, but none is found, creating an elevator here!");
				var newEl = new ElementPos() { Floor = clonePos.Floor, XPos = clonePos.XPos, ElementType = "Own Elevator" };
				elevators.Add(newEl);
				return CloneAction.Elevator;
			}

			Log($"Moving drone to {elementToNavTo}");

			var expectedDirection = DetermineExpectedDirection(clonePos, elementToNavTo);
			Log($"We expect the drone to navigate: {expectedDirection}");
			if(expectedDirection == null) { Log("No direction expected, spot on location, wait"); return CloneAction.Wait; }
			if(expectedDirection == clonePos.Direction) { Log("Same floor, going correct way!"); return CloneAction.Wait; }
			if(expectedDirection != clonePos.Direction) { Log("Same floor, but going wrong way!"); return CloneAction.Block; }
			
			Log("No condition hit, just waiting this one!");
			return CloneAction.Wait;
		}

		private Direction? DetermineExpectedDirection(ClonePos clonePos, ElementPos posOfElement) 
		{
			Direction? expectedDirection = null;
			if(clonePos.XPos < posOfElement.XPos) { expectedDirection = Direction.Right; }
			if(clonePos.XPos > posOfElement.XPos) { expectedDirection = Direction.Left; }
			return expectedDirection;
		}

		//Clone positioning data and transforming
		public class ElementPos 
		{
			public string ElementType {get;set;}
			public int Floor {get;set;}
			public int XPos {get;set;}
			public override string ToString()
			{
				return $"[{ElementType}] Floor {Floor} at pos {XPos}";
			}
		}

		public class ClonePos : ElementPos
		{
			public Direction Direction {get;set;}
			public override string ToString()
			{
				return $"[Clone] Floor {Floor} at pos {XPos} moving {Direction}";
			}
		}
		public enum Direction
		{
			Left,
			Right
		}
		public ClonePos GetClonePosFromInput(string input)
		{
			var inputs = input.Split(' ');
			int cloneFloor = int.Parse(inputs[0]); // floor of the leading clone
			int clonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
			string direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT
			return new ClonePos()
			{
				Floor = cloneFloor,
				XPos = clonePos,
				Direction = string.Equals("Right", direction, StringComparison.OrdinalIgnoreCase) ? Direction.Right : Direction.Left
			};
		}

	}
}