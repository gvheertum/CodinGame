using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.Challenge_CrazyMax
{
	public class GameSate
	{
		public int MyScore { get; set; }
		public int EnemyScore1 { get; set; }
		public int EnemyScore2 { get; set; }
		public int MyRage { get; set; }
		public int EnemyRage1 { get; set; }
		public int EnemyRage2 { get; set; }
		public int UnitCount { get; set; }

		public override string ToString()
		{
			var scoreLine = $"[Scores] M: {MyScore}, E1: {EnemyScore1}, E2: {EnemyScore2}";
			var rageLine = $"[Rage] M: {MyRage} E1: {EnemyRage1} E2: {EnemyRage2}";
			var unitCount = $"[UnitCount] M: {UnitCount}";
			return $"[[GameState]]\n{scoreLine}\n{rageLine}\n{unitCount}";
		}
	}

	public enum UnitType
	{
		Reaper = 0,
		Water = 4
	}

	public class Unit : Position
	{
		public int UnitId { get; set; }
		public UnitType UnitType { get; set; }
		public int Player { get; set; }
		public float Mass { get; set; }
		public int Radius { get; set; }
		public int VelocityX { get; set; }
		public int VelocityY { get; set; }
		public int Extra { get; set; }
		public int Extra2 { get; set; }
	
		public override string ToString()
		{
			var id = $"[ID] P: {Player} U: {UnitId}";
			var stats = $"[Stats] T: {UnitType} M: {Mass} R: {Radius}";
			var pos  = $"[Pos] X: {X} Y: {Y} vX: {VelocityX} vY: {VelocityY}";
			return $"[[Unit]]\n{id}\n{stats}\n{pos}";
		}
	}

	public class UnitMove 
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int V { get; set; }
		public override string ToString() 
		{
			return $"{X} {Y} {V}";
		}
	}

	public class CrazyMaxPlayer : Shared.PuzzleMain
	{
		protected CrazyMaxPlayer(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			new CrazyMaxPlayer(new CodingGameProxyEngine()).Run();
		}
		public override void Run()
		{
			_supressDefaultIO = true; //Do not output our IO to the log
			// game loop
			while (IsRunning())
			{ 
				var gameState = ReadGameState();
				Log(gameState);

				List<Unit> units = new List<Unit>();
				for (int i = 0; i < gameState.UnitCount; i++)
				{
					var unit = ReadUnitState();
					units.Add(unit);
				}
		
				var reapers = units.Where(u => u.UnitType == UnitType.Reaper).ToList();
				var water = units.Where(u => u.UnitType == UnitType.Water).ToList();
				var myUnits = reapers.Where(u => u.Player == 0).ToList();

				Log($"Found {myUnits.Count} units for me (total of {units.Count})");
				Log($"Found {water.Count} water units");


				//Move my units
				myUnits.ForEach(u => 
				{
					WriteUnitMove(GetUnitMove(myUnits.First(), water));
				});

				//For phase 1 -> 2x wait
				//Flush the rest of the expected moves (we are expected to do 3) as wait
				int expectedMoves = 3;
				for(int i = myUnits.Count; i < expectedMoves; i++)
				{
					WriteUnitMove(null);
				}
			}
		}

		private UnitMove GetUnitMove(Unit unit, IEnumerable<Unit> water)
		{
			if(unit == null) { return null; }
			var moveTo = GetClosestPuddle(unit, water);
			Log($"Moving our reaper to: {moveTo}");
			return new UnitMove() { X = moveTo.X, Y = moveTo.Y, V = 300 };
		}

		private Unit GetClosestPuddle(Unit unit, IEnumerable<Unit> water)
		{
			return water.OrderBy(w => w.DistanceTo(unit)).First();
		}


		private void WriteUnitMove(UnitMove move)
		{
			/*
				X Y THROTTLE three integers with (X,Y) the direction of your acceleration and 0 ≤ THROTTLE ≤ 300
				WAIT
			*/
			WriteLine(move?.ToString() ?? "WAIT"); //No move = wait
		}

		private GameSate ReadGameState()
		{
			GameSate state = new GameSate();
			state.MyScore = int.Parse(ReadLine());
			state.EnemyScore1 = int.Parse(ReadLine());
			state.EnemyScore2 = int.Parse(ReadLine());
			state.MyRage = int.Parse(ReadLine());
			state.EnemyRage1 = int.Parse(ReadLine());
			state.EnemyRage2 = int.Parse(ReadLine());
			state.UnitCount = int.Parse(ReadLine());
			return state;
		}

		private Unit ReadUnitState()
		{
			string[] inputs = ReadLine().Split(' ');
			var unitState = new Unit();

			unitState.UnitId = int.Parse(inputs[0]);
			unitState.UnitType = (UnitType)int.Parse(inputs[1]);
			unitState.Player = int.Parse(inputs[2]);
			unitState.Mass = float.Parse(inputs[3]);
			unitState.Radius = int.Parse(inputs[4]);
			unitState.X = int.Parse(inputs[5]);
			unitState.Y = int.Parse(inputs[6]);
			unitState.VelocityX = int.Parse(inputs[7]);
			unitState.VelocityY = int.Parse(inputs[8]);
			unitState.Extra = int.Parse(inputs[9]);
			unitState.Extra2 = int.Parse(inputs[10]);

			return unitState;
		}
	}
}