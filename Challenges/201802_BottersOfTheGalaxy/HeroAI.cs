//require: Position.cs
using System.Collections.Generic;
using System.Linq;
using Framework;

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
			return 
				PlotAttack(gameState, _hero) ??
				GetMoveToBaseIfOverMidPoint(gameState,_hero) ??
				new GameMoveWait() { Reason = $"{_hero.HeroType} needs to wait" };
		}

		private GameMoveBase PlotAttack(GameState gameState, Entity hero)
		{
			var enemiesNear = GetEntitiesNear(gameState, hero);
			if(!enemiesNear.Any()) { Log("No enemies near"); return null; }	
			var eToAttack = enemiesNear.First();
			Log($"Attacking {eToAttack.UnitId}");
			return new GameMoveMoveAttack() { X = eToAttack.X, Y = eToAttack.Y, UnitId = eToAttack.UnitId }; 
		}

		private IEnumerable<Entity> GetEntitiesNear(GameState gameState, Entity hero)
		{
			return gameState.EntitiesEnemy.Where(e => Helpers.Unit.IsNear(e, hero, 100)).OrderBy(e => e.Health / e.MaxHealth);
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