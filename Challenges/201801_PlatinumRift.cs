
//require: Node.cs
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;

//https://www.codingame.com/ide/puzzle/platinum-rift-episode-1
namespace Challenges.PlatinumRift
{
	public class RiftGame
	{
		public int AmountOfPlayers {get;set;}
		public int MyPlayerID {get;set;}
		public int AmountOfZones {get;set;}
		public int AmountOfZoneLinks {get;set;}
		public int MyPlatinum {get;set;}
		
		public override string ToString()
		{
			return $@"
				[Board] players={AmountOfPlayers}, zones={AmountOfZones}, links={AmountOfZoneLinks}
				[Player] id={MyPlayerID}, platinum={MyPlatinum}"
			.Replace("\t","");
		}	
	}


	public class PlatinumRiftPlayer : PuzzleMain
	{

		protected PlatinumRiftPlayer(IGameEngine gameEngine) : base(gameEngine) { }

		public static void Main(string[] args)
		{
			new PlatinumRiftPlayer(new Framework.CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
			var gameState = GetGameFromInitialInput();

			Log("Game initialized");			
			Log(gameState);
			
			// game loop
			while (IsRunning())
			{
				Log("Game tick ");
				UpdateGameState(gameState);
				Log(gameState);

				// first line for movement commands, second line for POD purchase (see the protocol in the statement for details)
				//TODO: split generate string and generate commands to different calls
				WriteLine(GenerateMovementString(gameState));
				WriteLine(GeneratePurchaseString(gameState));
			}

		}

		private string GenerateMovementString(RiftGame game)
		{
			return "WAIT";
		}

		private string GeneratePurchaseString(RiftGame game)
		{
			return "1 73";
		}


		// **** Status readers

		//Read game state/parameters (init)
		private RiftGame GetGameFromInitialInput()
		{
			var game = new RiftGame();
			string[] inputs = ReadLine().Split(' ');
			game.AmountOfPlayers = int.Parse(inputs[0]); // the amount of players (2 to 4)
			game.MyPlayerID = int.Parse(inputs[1]); // my player ID (0, 1, 2 or 3)
			game.AmountOfZones = int.Parse(inputs[2]); // the amount of zones on the map
			game.AmountOfZoneLinks = int.Parse(inputs[3]); // the amount of links between all zones
			
			for (int i = 0; i < game.AmountOfZones; i++)
			{
				inputs = ReadLine().Split(' ');
				int zoneId = int.Parse(inputs[0]); // this zone's ID (between 0 and zoneCount-1)
				int platinumSource = int.Parse(inputs[1]); // the amount of Platinum this zone can provide per game turn
			}
			for (int i = 0; i < game.AmountOfZoneLinks; i++)
			{
				inputs = ReadLine().Split(' ');
				int zone1 = int.Parse(inputs[0]);
				int zone2 = int.Parse(inputs[1]);
			}

			return game;
		}


		//Update the current game state (on start of tick)
		private RiftGame UpdateGameState(RiftGame gameState)
		{
			Log("Updating gamestate");
			gameState.MyPlatinum = int.Parse(ReadLine()); // my available Platinum
			for (int i = 0; i < gameState.AmountOfZones; i++)
			{
				string[] inputs = ReadLine().Split(' ');
				int zId = int.Parse(inputs[0]); // this zone's ID
				int ownerId = int.Parse(inputs[1]); // the player who owns this zone (-1 otherwise)
				int podsP0 = int.Parse(inputs[2]); // player 0's PODs on this zone
				int podsP1 = int.Parse(inputs[3]); // player 1's PODs on this zone
				int podsP2 = int.Parse(inputs[4]); // player 2's PODs on this zone (always 0 for a two player game)
				int podsP3 = int.Parse(inputs[5]); // player 3's PODs on this zone (always 0 for a two or three player game)
			}
			return gameState;
		}
	}
}