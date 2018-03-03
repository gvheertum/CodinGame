using System;
using System.IO;
using System.Text;
using System.Collections;
using Framework;
using System.Collections.Generic;
using System.Linq;

/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{

	public class Game : PuzzleMain
	{
		private GameReader _gameReader = null;
		protected Game(IGameEngine gameEngine) : base(gameEngine) { _gameReader = new GameReader(gameEngine); }

		static void Main(string[] args)
		{
			new Game(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{ 
			Log("Run forrest run!");
			var gameState = _gameReader.ReadIntialGameState();
			Log(gameState.GetInitialGameStateString());

			// game loop
			while (IsRunning())
			{
				gameState = _gameReader.UpdateGameState(gameState);
				Log(gameState.GetEntityString(gameState.EntitiesMine, "MINE"));
				Log(gameState.GetEntityString(gameState.EntitiesMyHeros, "MY HEROS"));
				Log(gameState.GetEntityString(gameState.EntitiesEnemy, "ENEMY"));
				
				var gameMoves = DetermineGameMoves(gameState);
				Log($"Moving {gameMoves.Count()} moves");
				foreach(var m in gameMoves)
				{
					WriteLine(m.GetMoveString());
				}
			}
		}		

		public IEnumerable<GameMoveBase> DetermineGameMoves(GameState gameState)
		{
			// If roundType has a negative value then you need to output a Hero name, such as "DEADPOOL" or "VALKYRIE".
			// Else you need to output roundType number of any valid action, such as "WAIT" or "ATTACK unitId"
			int moveTick = gameState.RoundType;	
			if(gameState.RoundType < 0) 
			{ 
				yield return DetermineHeroDeploy(gameState); //Only one step
				yield break; 
			}

			foreach(var hero in gameState.EntitiesMyHeros)
			{
				var hs = DetermineStepForHero(gameState, hero);
				if(hs!=null) { yield return hs; moveTick--; }
				
			}

			//Fill with moves
			while(moveTick > 0)
			{
				yield return new GameMoveWait() { Reason = "No move plot, filling fashizzle" };
				moveTick--;
			}
		}

		private GameMoveBase DetermineHeroDeploy(GameState gameState)
		{
			var availableHeroes = BottersConstants.Heros.AllHeroes().ToList();
			int heroIdx = new Random().Next(0,availableHeroes.Count);
			Log($"Picking heroIdx={heroIdx} => {availableHeroes[heroIdx]}");
			return new GameMoveSpawnUnit() { UnitName = availableHeroes[heroIdx] };
		}

		private GameMoveBase DetermineStepForHero(GameState state, Entity hero)
		{
			Log($"Pondering for {hero.HeroType}");
			return new GameMoveWait() { Reason = $"{hero.HeroType} needs to wait" };
		}
	}
}