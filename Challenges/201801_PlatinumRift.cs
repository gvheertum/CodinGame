
//require: Node.cs
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Shared;

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

		public Dictionary<int,RiftZone> Zones {get;set;} = new Dictionary<int, RiftZone>();
		public void AddOrReplaceZone(RiftZone zone)
		{
			if(Zones.Keys.Contains(zone.NodeIndex))
			{
				Zones[zone.NodeIndex] = zone;
			}
			else
			{
				Zones.Add(zone.NodeIndex, zone);
			}
		}

		public RiftZone GetZone(int zoneId)
		{
			if(!Zones.Keys.Contains(zoneId)) { throw new Exception($"No zone with id: {zoneId}"); }
			return Zones[zoneId];
		}

		public List<RiftZoneLink> ZoneLinks {get;set;} = new List<RiftZoneLink>();
	}

	public class RiftZone : Node<RiftZone>
	{
		public int PlatimumSource {get;set;}
		public int? OwningPlayerID {get;set;}
		public int AmountPodsPlayer0 {get;set;}
		public int AmountPodsPlayer1 {get;set;}
		public int AmountPodsPlayer2 {get;set;}
		public int AmountPodsPlayer3 {get;set;}
		public int GetAmountPodsForPlayer(int playerID)
		{
			switch(playerID)
			{
				case 0: return AmountPodsPlayer0;
				case 1: return AmountPodsPlayer1;
				case 2: return AmountPodsPlayer2;
				case 3: return AmountPodsPlayer3;
				default: throw new Exception($"Unknown playerID {playerID}");
			}
		}
		public void SetAmountPodsForPlayer(int playerID, int pods)
		{
			switch(playerID)
			{
				case 0: AmountPodsPlayer0 = pods; break;
				case 1: AmountPodsPlayer1 = pods; break;;
				case 2: AmountPodsPlayer2 = pods; break;;
				case 3: AmountPodsPlayer3 = pods; break;;
				default: throw new Exception($"Unknown playerID {playerID}");
			}
		}
	}

	public class RiftZoneLink : NodeRoute<RiftZone>
	{

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
				RiftZone rz = new RiftZone()
				{
					NodeIndex = int.Parse(inputs[0]),
					PlatimumSource = int.Parse(inputs[1])
				};
				game.AddOrReplaceZone(rz);
			}

			//TODO: Add routes
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
			Log($"Current platinum: {gameState.MyPlatinum}");

			//Update all the zones
			for (int i = 0; i < gameState.AmountOfZones; i++)
			{
				string[] inputs = ReadLine().Split(' ');
				int zId = int.Parse(inputs[0]); // this zone's ID
				var zone = gameState.GetZone(zId);
				int? oldOwner = zone.OwningPlayerID;
				
				zone.OwningPlayerID = int.Parse(inputs[1]); // the player who owns this zone (-1 otherwise)
				zone.AmountPodsPlayer0 = int.Parse(inputs[2]); // player 0's PODs on this zone
				zone.AmountPodsPlayer1 = int.Parse(inputs[3]); // player 1's PODs on this zone
				zone.AmountPodsPlayer2 = int.Parse(inputs[4]); // player 2's PODs on this zone (always 0 for a two player game)
				zone.AmountPodsPlayer3 = int.Parse(inputs[5]); // player 3's PODs on this zone (always 0 for a two or three player game)
				
				if(zone.OwningPlayerID != oldOwner)
				{
					Log($"Ownership of zone {zone.NodeIndex} changed from {oldOwner} to {zone.OwningPlayerID}");
				}
			}
			return gameState;
		}
	}
}