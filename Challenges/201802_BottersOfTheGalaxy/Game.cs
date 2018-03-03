using System;
using System.IO;
using System.Text;
using System.Collections;
using Framework;

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
				Log(gameState.GetEntityString());
				// If roundType has a negative value then you need to output a Hero name, such as "DEADPOOL" or "VALKYRIE".
				// Else you need to output roundType number of any valid action, such as "WAIT" or "ATTACK unitId"
				WriteLine("WAIT");
			}
		}		
	}
}