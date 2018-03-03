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
	public class HeroAI : PuzzleMain
	{
		private Entity _hero;

		public HeroAI(IGameEngine gameEngine, Entity hero) : base(gameEngine)
		{
			_hero = hero;
		}

		public GameMoveBase GetHeroMove(GameState gameState)
		{
			Log($"Pondering for {_hero.HeroType}");
			var ponderedMove =  
				MoveDueToLowHealth(gameState, _hero) ??
				PlotAttack(gameState, _hero) ??
				//GetMoveToBaseIfOverMidPoint(gameState,_hero) ??
				GetMoveToUnit(gameState,_hero) ??
				new GameMoveWait() { Reason = $"{_hero.HeroType} needs to wait" };

			if(ponderedMove is IPosition) //Check if we accidentally get into the range of the tower
			{
				var enemyTower = gameState.EntitiesEnemy.First(Helpers.Unit.IsTower);
				if(enemyTower.DistanceTo(ponderedMove as IPosition) < enemyTower.AttackRange)
				{
					Log("This move will bring us within the range of the tower, please don't");
					return null; //TODO: Something more usefull?
				}
			}
			Log("Let's go!");
			return ponderedMove;
		}

		private const double CriticalHealth = 300;
		private GameMoveBase MoveDueToLowHealth(GameState gameState, Entity hero)
		{
			Log($"health: {hero.Health}");
			if(hero.Health < CriticalHealth) 
			{ 
				Log($"Our health is below {CriticalHealth}, do something");
				return 
					BuyHealthPotion(gameState, hero) ?? 
					MoveBackToBase(gameState, hero) ?? 
					new GameMoveWait() { Reason = "It's just not safe!"}; // TODO: We did some attack, but should only do so if there is someone near us { UnitType = BottersConstants.UnitTypes.Minion }; //Attack something close
			}
			Log("Health is okay, do something else");
			return null;
		}

		private GameMoveBase BuyHealthPotion(GameState gameState, Entity hero)
		{
			var hP = gameState.Items.Where(i => i.ItemCost < gameState.Gold && i.Health > 0).OrderBy(i => i.IsPotion ? 0: 1).ThenByDescending(i => i.ItemCost);
			if(!hP.Any()) { return null; }
			return new GameMoveBuy() { ItemName = hP.First().ItemName };
		}

		private GameMoveBase MoveBackToBase(GameState gameState, Entity hero)
		{
			var myTower = gameState.EntitiesMine.First(Helpers.Unit.IsTower);
			if(hero.DistanceTo(myTower) > myTower.AttackRange / 2) 
			{
				Log("Moving to our tower, make a trap");
				return new GameMoveMove() { X = myTower.X, Y = myTower.Y };
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
			return new GameMoveMoveAttack() { X = eToAttack.X, Y = eToAttack.Y, UnitId = eToAttack.UnitId }; 
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
			return new GameMoveMove() { X = eToMove.X, Y = eToMove.Y }; //This walks straight into the sword/arrows, keep distance
		}

		private GameMoveBase GetMoveToBaseIfOverMidPoint(GameState state, Entity hero)
		{
			//If we are past the middle of the field, go back to the base
			int? midPointX = state.EntitiesEnemy.First(Helpers.Unit.IsTower)?.X - state.EntitiesMine.First(Helpers.Unit.IsTower)?.X; 
			if(midPointX == null) { return null; }
			return null;
		}
	}
}