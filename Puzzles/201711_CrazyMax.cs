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
		Destroyer = 1,
		Doof = 2,
		WaterTank = 3,
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
		public bool IsSkill { get; set; }
		public override string ToString() 
		{
			return IsSkill ? $"SKILL {X} {Y}" : $"{X} {Y} {V}";
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

		private const int MyPlayerID = 0;
		public override void Run()
		{
			_supressDefaultIO = true; //Do not output our IO to the log
			// game loop
			while (IsRunning())
			{ 
				var gameState = ReadGameState();

				List<Unit> units = new List<Unit>();
				for (int i = 0; i < gameState.UnitCount; i++)
				{
					var unit = ReadUnitState();
					units.Add(unit);
				}
		
				var reapers = units.Where(u => u.UnitType == UnitType.Reaper).ToList();
				var destroyers = units.Where(u => u.UnitType == UnitType.Destroyer).ToList();
				var doofs = units.Where(u => u.UnitType == UnitType.Doof).ToList();
				var water = units.Where(u => u.UnitType == UnitType.Water).ToList();
				var tanks = units.Where(u => u.UnitType == UnitType.WaterTank).ToList();
				var myReapers = reapers.Where(u => u.Player == MyPlayerID).ToList();
				var myDestroyers = destroyers.Where(u => u.Player == MyPlayerID).ToList();
				var myDoofs = doofs.Where(u => u.Player == MyPlayerID).ToList();

				Log($"Found {myReapers.Count} reapers, {myDestroyers.Count} destroyers and {myDoofs.Count} doofs for me (total of {units.Count})");
				Log($"Found {water.Count} water units");
				Log($"Found {tanks.Count} water tanks");


				//Move my units
				myReapers.ForEach(u => 
				{
					Log($"Plotting move for unit {u.UnitId} (t: {u.UnitType})");
					//Move our reaper to the water sources, if no water present follow our destroyers to allow us to be at the water asap
					WriteUnitMove(GetUnitMove(u, water, myDestroyers));
				});

				myDestroyers.ForEach(d => 
				{
					Log($"Plotting move for unit {d.UnitId} (t: {d.UnitType})");
					var suggestedMove = GetUnitMove(d, tanks, null);

					if(gameState.MyRage > 100) 
					{
						Log("Time for a grenade!"); 
						suggestedMove.IsSkill = true; 
					}
					WriteUnitMove(suggestedMove);
				});

				myDoofs.ForEach(d => 
				{
					Log($"Plotting move for unit {d.UnitId} (t: {d.UnitType})");
					var doofMove = GetUnitMove(d, tanks);
					WriteUnitMove(doofMove);
				});

				//This is no longer relevant since we now make 3 moves
				// int expectedMoves = 3;
				// for(int i = myDestroyers.Count + myReapers.Count; i < expectedMoves; i++)
				// {
				// 	WriteUnitMove(null);
				// }
			}
		}

		private UnitMove GetUnitMove(Unit unit, IEnumerable<Unit> targetDestinations, IEnumerable<Unit> alternateDestinations = null)
		{
			if(unit == null) { return null; }

			Log($"Plotting move for {unit.UnitId} (t: {unit.UnitType})");
			if(!targetDestinations.Any()) { Log("There are no primary targets to navigate to, falling back"); }
			var targetsToUse = targetDestinations?.Any() == true ? targetDestinations : alternateDestinations;
			
			var moveTo = GetClosestTarget(unit, targetsToUse);
			if(moveTo == null) { Log("No element to navigate to on the map, ignoring turn"); return null; }
			Log($"Moving our unit to: {moveTo}");
			return new UnitMove() { X = moveTo.X, Y = moveTo.Y, V = 300 };
		}

		private const int Radius = 6000; //Due to radius the distance can be negative, use the radius to offset values to pos values
		private Unit GetClosestTarget(Unit unit, IEnumerable<Unit> targets)
		{
			if(!targets?.Any() == true) { return null; }
			//TODO: our distance seems to be a bit strange, possibly due to the negative positions
			return targets.OrderBy(w => w.DistanceTo(unit, Radius /2)).First();
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