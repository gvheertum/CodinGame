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
	public class GameState
	{
		public int MyScore { get; set; }
		public int EnemyScore1 { get; set; }
		public int EnemyScore2 { get; set; }
		public int MyRage { get; set; }
		public int EnemyRage1 { get; set; }
		public int EnemyRage2 { get; set; }
		public int UnitCount { get; set; }

		public List<Entity> AllEntities { get; set; } = new List<Entity>();
		public List<Entity> Reapers { get; set; } = new List<Entity>();
		public List<Entity> Destroyers { get; set; } = new List<Entity>();
		public List<Entity> Doofs { get; set; } = new List<Entity>();
		public List<Entity> Water { get; set; } = new List<Entity>();
		public List<Entity> Tanks { get; set; } = new List<Entity>();
		public List<Entity> MyReapers { get; set; } = new List<Entity>();
		public List<Entity> MyDestroyers { get; set; } = new List<Entity>();
		public List<Entity> MyDoofs { get; set; } = new List<Entity>();

		public override string ToString()
		{
			var scoreLine = $"[Scores] M: {MyScore}, E1: {EnemyScore1}, E2: {EnemyScore2}";
			var rageLine = $"[Rage] M: {MyRage} E1: {EnemyRage1} E2: {EnemyRage2}";
			var unitCount = $"[UnitCount] M: {UnitCount}";
			return $"[[GameState]]\n{scoreLine}\n{rageLine}\n{unitCount}";
		}
	}

	public enum EntityType
	{
		Reaper = 0,
		Destroyer = 1,
		Doof = 2,
		WaterTank = 3,
		Water = 4
	}

	public class Entity : Position
	{
		public int UnitId { get; set; }
		public EntityType UnitType { get; set; }
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

		public override void Run()
		{
			
			// game loop
			while (IsRunning())
			{ 
				var gameState = new MeanMaxBoardReader(this).ReadGameState();

				Log($"Found {gameState.AllEntities.Count} entities on the board");
				Log($"Found {gameState.Water.Count} water units");
				Log($"Found {gameState.Tanks.Count} water tanks");


				//Move my units
				var aiFactory = new AIFactory(this);
				List<UnitMove> moves = new List<UnitMove>();
				moves.AddRange(gameState.MyReapers.Select(u => aiFactory.GetAIForUnit(u).GetMove(gameState)));
				moves.AddRange(gameState.MyDestroyers.Select(u => aiFactory.GetAIForUnit(u).GetMove(gameState)));
				moves.AddRange(gameState.MyDoofs.Select(u => aiFactory.GetAIForUnit(u).GetMove(gameState)));
				moves.ForEach(m => WriteUnitMove(m));
			}
		}

		

		private void WriteUnitMove(UnitMove move)
		{
			/*
				X Y THROTTLE three integers with (X,Y) the direction of your acceleration and 0 ≤ THROTTLE ≤ 300
				WAIT
			*/
			WriteLine(move?.ToString() ?? "WAIT"); //No move = wait
		}

		
	}

	

	public class UnitAIReaper : UnitAIBase
	{
		public UnitAIReaper(PuzzleMain puzzle, Entity entity) : base(puzzle, entity) {}

		public override UnitMove GetMove(GameState fullState)
		{
			//Move our reaper to the water sources, if no water present follow our destroyers to allow us to be at the water asap
			return GetMoveFromElementToTargetOrAlternateTarget(Entity, fullState.Water, fullState.MyDestroyers);
		}
	}

	public class UnitAIDestroyer : UnitAIBase
	{
		public UnitAIDestroyer(PuzzleMain puzzle, Entity entity) : base(puzzle, entity) {}
		public override UnitMove GetMove(GameState fullState)
		{
			var suggestedMove = GetMoveFromElementToTargetOrAlternateTarget(Entity, fullState.Tanks, null);

			if(fullState.MyRage > 60) 
			{
				//TODO: grenades should only be thrown when items are close
				_puzzle.Log("Time for a grenade!"); 
				suggestedMove.IsSkill = true; 
			}
			return suggestedMove;
		}
	}

	public class UnitAIDoof : UnitAIBase
	{
		public UnitAIDoof(PuzzleMain puzzle, Entity entity) : base(puzzle, entity) {}
		public override UnitMove GetMove(GameState fullState)
		{
			return GetMoveFromElementToTargetOrAlternateTarget(Entity, fullState.Destroyers);
		}
	}

	public class AIFactory 
	{
		private PuzzleMain _puzzle;
		public AIFactory(PuzzleMain puzzle) 
		{
			_puzzle = puzzle;	
		}

		public UnitAIBase GetAIForUnit(Entity entity)
		{
			if(entity.UnitType == EntityType.Destroyer) { return new UnitAIDestroyer(_puzzle, entity); }
			else if(entity.UnitType == EntityType.Doof) { return new UnitAIDoof(_puzzle, entity); }
			else if(entity.UnitType == EntityType.Reaper) { return new UnitAIReaper(_puzzle, entity); }
			else { throw new Exception($"Cannot create AI for {entity.UnitType}"); }
		}
	}


	//Base entry for the Unit AI logic. Contains helper functions to determine AI behavior
	public abstract class UnitAIBase
	{
		protected PuzzleMain _puzzle;
		protected Entity Entity { get; set; }
		protected UnitAIBase(PuzzleMain puzzle, Entity entity) 
		{ 
			_puzzle = puzzle; 
			Entity = entity;
		}

		public abstract UnitMove GetMove(GameState fullState);
		protected UnitMove GetMoveFromElementToTargetOrAlternateTarget(Entity unit, IEnumerable<Entity> targetDestinations, IEnumerable<Entity> alternateDestinations = null)
		{
			if(unit == null) { return null; }

			_puzzle.Log($"Plotting move for {unit.UnitId} (t: {unit.UnitType})");
			if(!targetDestinations.Any()) { _puzzle.Log("There are no primary targets to navigate to, falling back"); }
			var targetsToUse = targetDestinations?.Any() == true ? targetDestinations : alternateDestinations;
			
			var moveTo = GetClosestTargetForElement(unit, targetsToUse);
			if(moveTo == null) { _puzzle.Log("No element to navigate to on the map, ignoring turn"); return null; }
			_puzzle.Log($"Moving our unit to: {moveTo}");
			return new UnitMove() { X = moveTo.X, Y = moveTo.Y, V = 300 };
		}

		private const int Radius = 6000; //Due to radius the distance can be negative, use the radius to offset values to pos values
		private Entity GetClosestTargetForElement(Entity unit, IEnumerable<Entity> targets)
		{
			if(!targets?.Any() == true) { return null; }
			return targets.OrderBy(w => w.DistanceTo(unit, Radius /2)).First();
		}

	}



	//Handling of the state of the board by reading the console, should not really take additional changes after the
	//first installments
	public class MeanMaxBoardReader : PuzzleMain
	{
		public MeanMaxBoardReader(PuzzleMain puzzleMain) : base(puzzleMain)
		{
		}

		public GameState ReadGameState()
		{
			GameState state = new GameState();
			state.MyScore = int.Parse(ReadLine());
			state.EnemyScore1 = int.Parse(ReadLine());
			state.EnemyScore2 = int.Parse(ReadLine());
			state.MyRage = int.Parse(ReadLine());
			state.EnemyRage1 = int.Parse(ReadLine());
			state.EnemyRage2 = int.Parse(ReadLine());
			state.UnitCount = int.Parse(ReadLine());
			
			
			state = ReadUnitsIntoGameState(state, state.UnitCount);
			return state;
		}

		private const int MyPlayerID = 0;

		private GameState ReadUnitsIntoGameState(GameState state, int nrOfUnits)
		{
			List<Entity> units = new List<Entity>();
			for (int i = 0; i < nrOfUnits; i++)
			{
				var unit = ReadUnitStateFromInput();
				units.Add(unit);
			}

			state.AllEntities = units;
			state.Reapers = units.Where(u => u.UnitType == EntityType.Reaper).ToList();
			state.Destroyers = units.Where(u => u.UnitType == EntityType.Destroyer).ToList();
			state.Doofs = units.Where(u => u.UnitType == EntityType.Doof).ToList();
			state.Water = units.Where(u => u.UnitType == EntityType.Water).ToList();
			state.Tanks = units.Where(u => u.UnitType == EntityType.WaterTank).ToList();
			state.MyReapers = state.Reapers.Where(u => u.Player == MyPlayerID).ToList();
			state.MyDestroyers = state.Destroyers.Where(u => u.Player == MyPlayerID).ToList();
			state.MyDoofs = state.Doofs.Where(u => u.Player == MyPlayerID).ToList();

			return state;
		}

		private Entity ReadUnitStateFromInput()
		{
			string[] inputs = ReadLine().Split(' ');
			var unitState = new Entity();

			unitState.UnitId = int.Parse(inputs[0]);
			unitState.UnitType = (EntityType)int.Parse(inputs[1]);
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