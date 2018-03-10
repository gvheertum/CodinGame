//require: Position.cs
using System.Collections.Generic;
using System.Linq;
using Framework;
using Shared;

/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class HeroAIBase : PuzzleHelper
	{
		private Entity _hero;

		public HeroAIBase(IGameEngine gameEngine, Entity hero) : base(gameEngine)
		{
			_hero = hero;
		}

		public GameMoveBase GetHeroMove(GameState gameState)
		{
			Log($"[{this.GetType().Name}] - Pondering for {_hero.HeroType}");
			var ponderedMove =  GetAndRateMoves(gameState).First();
			Log("Let's go for:");
			Log(GetMoveStringLog(ponderedMove));
			return ponderedMove;
		}

		protected IEnumerable<GameMoveBase> GetAndRateMoves(GameState gameState)
		{
			List<GameMoveBase> moves = new List<GameMoveBase>();
			moves.AddRange(GetGenericMoves(gameState).Where(m => m != null));
			moves.AddRange(GetSpecificMoves(gameState).Where(m => m != null));
			
			moves = moves.Where(m => !this.MoveWillPutUsInRangeOfTower(m, gameState, _hero)).ToList();

			moves = moves.OrderByDescending(m => m.Rating).ToList();
			Log($"Found {moves.Count} moves:");
			moves.ForEach(m => Log(GetMoveStringLog(m)));
			return moves;
		}

		private string GetMoveStringLog(GameMoveBase move)
		{
			return $"[{move.Rating}] {move.GetMoveString()}";
		}

		protected IEnumerable<GameMoveBase> GetGenericMoves(GameState gameState)
		{
			yield return MoveAwayFromEnemyTower(gameState, _hero);
			yield return BuyHealthPotion(gameState, _hero);
			yield return MoveBackToBase(gameState, _hero);
			yield return PlotAttack(gameState, _hero);
			//GetMoveToBaseIfOverMidPoint(gameState,_hero) ??
			yield return GetMoveToUnit(gameState,_hero);
			yield return new GameMoveWait() { Reason = $"{_hero.HeroType} needs to wait", Rating = -1 };
		}

		protected virtual IEnumerable<GameMoveBase> GetSpecificMoves(GameState gameState)
		{
			Log("No specific moves in our AI");
			yield break;
		}

		private GameMoveBase MoveAwayFromEnemyTower(GameState gameState, Entity hero)
		{
			var enemyTower = gameState.EntitiesEnemy.First(Helpers.Unit.IsTower);
			if(enemyTower.DistanceTo(hero) < enemyTower.AttackRange)
			{
				Log("We are in range of the tower, move out");
				return new GameMoveMove() { X = enemyTower.X - enemyTower.AttackRange - 5, Rating = 9999 };
			}
			Log("Not in range of tower, no action");
			return null;
		}

		//TODO: buy other stuff?

		private GameMoveBase BuyHealthPotion(GameState gameState, Entity hero)
		{
			if(!AreWeInCriticalHealth(hero)) { return null; }

			Log("We are in danger, try finding a health solution");
			var hP = gameState.Items.Where(i => i.ItemCost < gameState.Gold && i.Health > 0).OrderByDescending(i => i.Health).ThenByDescending(i => i.MaxHealth).ThenBy(i => i.ItemCost);
			if(!hP.Any()) { Log("We cannot buy a health thingy!"); return null; }
			
			var itemToBuy = hP.First();
			Log($"Pushing buy command for: {itemToBuy.ItemName} (for {itemToBuy.ItemCost})");
			return new GameMoveBuy() { ItemName = itemToBuy.ItemName, Rating = 1000 };
		}

		private GameMoveBase MoveBackToBase(GameState gameState, Entity hero)
		{
			if(!AreWeInCriticalHealth(hero)) { return null; }

			Log("We are in danger, consider moving to the tower");
			var myTower = gameState.EntitiesMine.First(Helpers.Unit.IsTower);
			if(hero.DistanceTo(myTower) > myTower.AttackRange / 2) 
			{
				Log("Moving to our tower, make a trap");
				return new GameMoveMove() { X = myTower.X, Y = myTower.Y, Rating = 999 }; //Potions are preferred 
			}
			Log("We are close to our tower, so bye");
			return null;
		}

		private GameMoveBase PlotAttack(GameState gameState, Entity hero)
		{
			var enemiesNear = GetEntitiesWithinRange(gameState, hero, hero.MovementSpeed + hero.AttackRange);
			if(!enemiesNear.Any()) { Log("No enemies in attack range"); return null; }	
			var eToAttack = enemiesNear.First();
			Log($"Attacking {eToAttack.UnitId}");
			return new GameMoveMoveAttack() { X = eToAttack.X, Y = eToAttack.Y, UnitId = eToAttack.UnitId, Rating = 100 }; 
		}

		private IEnumerable<Entity> GetEntitiesWithinRange(GameState gameState, Entity hero, int range)
		{
			return gameState.EntitiesEnemy
				.Where(e => !Helpers.Unit.IsTower(e))
				.Where(e => Helpers.Unit.IsNear(e, hero, range))
				.OrderBy(e => Helpers.Unit.IsRange(e) ? 0 : 1)
				.ThenBy(e => e.Health / e.MaxHealth);
		}

		private GameMoveBase GetMoveToUnit(GameState gameState, Entity hero)
		{
			var enemiesAlmostNear = GetEntitiesWithinRange(gameState, hero, 1000);
			if(!enemiesAlmostNear.Any()) { Log("No units in spot to move to!"); return null;}
			var eToMove = enemiesAlmostNear.First();
			return new GameMoveMove() { X = eToMove.X, Y = eToMove.Y, Rating = 99 }; //This walks straight into the sword/arrows, keep distance
		}

		private GameMoveBase GetMoveToBaseIfOverMidPoint(GameState state, Entity hero)
		{
			//If we are past the middle of the field, go back to the base
			int? midPointX = state.EntitiesEnemy.First(Helpers.Unit.IsTower)?.X - state.EntitiesMine.First(Helpers.Unit.IsTower)?.X; 
			if(midPointX == null) { return null; }
			return null;
		}



		//CHECK FUNCTIONS
		private bool MoveWillPutUsInRangeOfTower(GameMoveBase ponderedMove, GameState gameState, Entity hero)
		{
			if(ponderedMove is IPosition) //Check if we accidentally get into the range of the tower
			{
				var enemyTower = gameState.EntitiesEnemy.First(Helpers.Unit.IsTower);
				if(enemyTower.DistanceTo(ponderedMove as IPosition) < enemyTower.AttackRange)
				{
					Log($"Move {GetMoveStringLog(ponderedMove)} will bring us within the range of the tower, please don't");
					return true; //TODO: Something more usefull?
				}
			}
			return false;
		}

		private const double CriticalHealth = 200;
		private bool AreWeInCriticalHealth(Entity hero)
		{
		
			Log($"health: {hero.Health}");
			if(hero.Health < CriticalHealth) 
			{ 
				return true;
			}
			Log("Health is okay, do something else");
			return false;
		}

	}
}