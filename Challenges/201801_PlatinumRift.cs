
//require: Node.cs
//require: NodeRoute.cs
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

		private List<RiftZone> _zonesList = new List<RiftZone>();
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
			_zonesList.Add(zone);
		}

		public RiftZone GetZone(int zoneId)
		{
			if(!Zones.Keys.Contains(zoneId)) { throw new Exception($"No zone with id: {zoneId}"); }
			return Zones[zoneId];
		}

		public IEnumerable<RiftZone> GetZonesWithMyPods()
		{
			return _zonesList.Where(z => z.GetAmountPodsForPlayer(MyPlayerID) > 0);
		}

		public IEnumerable<RiftZone> GetAllZones()
		{
			return _zonesList.Select(z => z);
		}

		public IEnumerable<RiftZone> GetZonesUnderMyControl()
		{
			return _zonesList.Where(z => z.OwningPlayerID == this.MyPlayerID);
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
		public int? GetAmountMovable(int myPlayerID)
		{
			if(OwningPlayerID != myPlayerID) { return null; }
			int runnerUp = GetPodAmounts().OrderByDescending(i => i).Skip(1).First();
			int movable = GetAmountPodsForPlayer(myPlayerID) - runnerUp;
			return movable > 0 ? movable : (int?)null;
		}

		public IEnumerable<int> GetPodAmounts()
		{
			yield return AmountPodsPlayer0;
			yield return AmountPodsPlayer1;
			yield return AmountPodsPlayer2;
			yield return AmountPodsPlayer3;
		}

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

	public class PodMovement
	{
		public int Amount {get;set;}
		public int ZoneOrigin {get;set;}
		public int ZoneDestination {get;set;}
		public string GetOutputString()
		{
			return $"{Amount} {ZoneOrigin} {ZoneDestination}";
		}
		public override string ToString()
		{
			return $"[Movement] {Amount} {ZoneOrigin}->{ZoneDestination}";
		}
	}

	public class PodPurchase
	{
		public int Zone {get;set;}
		public int Amount {get;set;}
		public string GetOutputString()
		{
			return $"{Amount} {Zone}";
			
		}
		public override string ToString()
		{
			return $"[Purchase] {Amount} @ z{Zone}";
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
				LogRepresentation(gameState);
				// first line for movement commands, second line for POD purchase (see the protocol in the statement for details)
				//TODO: split generate string and generate commands to different calls
				WriteLine(GenerateMovementString(gameState));
				WriteLine(GeneratePurchaseString(gameState));
			}

		}


		

		private string GenerateMovementString(RiftGame game)
		{
			/*Rules for moving:
				A POD (or group of PODs) can only make one move per game round.
				A POD can only move from one zone to a contiguous zone – either neutral, already owned or owned by an enemy.
				A POD located on a zone where a fight is ongoing – meaning a zone with enemy PODs on it – can only move to a neutral zone or a zone he/she owns. Simply put, a POD that flees a fight can only retreat on a zone which does not belong to an enemy. 
			*/

			
			/*
			Line 1: a series of movement commands. A movement command is composed of 3 integers podsCount zoneOrigin zoneDestination that indicate the number of PODs to move from one zone to another.
			For example 4 2 1 3 2 6 = two commands: moving four PODs from zone 2 to zone 1 and moving three PODs from zone 2 to 6.
			Just write WAIT if you do not wish to make any movements.
			*/
			int maxPodToPurchase = game.MyPlatinum / 20;
			var movements = GetMovements(game).ToList();
			if(!movements.Any()) { Log("No movements"); return "WAIT"; }

			Log($"Found {movements.Count()} movements");
			movements.ForEach(m => Log(m));
			return string.Join(" ", movements.Select(p=>p.GetOutputString()));
		}

		private IEnumerable<PodMovement> GetMovements(RiftGame game)
		{
			var controlledZones = game.GetZonesUnderMyControl();
			foreach(var z in controlledZones)
			{
				var movableInZone = z.GetAmountMovable(game.MyPlayerID);
				Log($"Can move {movableInZone} from zone {z.NodeIndex}");
				if(!(movableInZone > 0)) { continue; }
				
				//Find the node to move to
				var moveTo = z.LinkedNodes.FirstOrDefault(zz => zz.OwningPlayerID != game.MyPlayerID)
					?? z.LinkedNodes.FirstOrDefault();
				if(moveTo != null)
				{
					Log($"Moving {movableInZone} to {moveTo.NodeIndex}");
					yield return new PodMovement()
					{
						Amount = movableInZone.Value,
						ZoneOrigin = z.NodeIndex,
						ZoneDestination = moveTo.NodeIndex
					};
				}
			}
		}

		private const int PodCost = 20;

		private string GeneratePurchaseString(RiftGame game)
		{
			/* Rules for buying:
				Each player can buy as many troops as their Platinum stock allows. A war POD costs 20 Platinum bars.
				A freshly bought POD can only be placed on a neutral zone or on a zone owned by the buyer. 
			*/

			/*
			Line 2: a series of purchase commands. A purchase command is composed of 2 integers podsCount zoneDestination that indicate the number of PODs to add to a zone.
			For example 2 32 1 11 = two commands: buy two PODs for zone 32 and buy one POD for zone 11.
			Just write WAIT if you do not want to buy anything. 
			*/
			int maxPodToPurchase = game.MyPlatinum / 20;
			var purchases = GetPurchases(game, maxPodToPurchase).ToList();
			if(!purchases.Any()) { return "WAIT"; }

			Log($"Found {purchases.Count()} purchases");
			purchases.ForEach(p => Log(p));
			return string.Join(" ", purchases.Select(p=>p.GetOutputString()));
		}

		private IEnumerable<PodPurchase> GetPurchases(RiftGame game, int maxAmount)
		{
			//TODO: try to expand my node zones (with nodes but not under control)
			//TODO: skip nodes that are not neutral
			//TODO: strengthen our own nodes
			int amountToPurchase = 5;
			//TODO: purchse only a set of elements
			var zonesNotMine = game.GetAllZones().Where(z => z.OwningPlayerID == null);
			if(maxAmount > 0 && zonesNotMine.Any())
			{
				var zoneToTake = zonesNotMine.First();
				yield return new PodPurchase()
				{
					Amount = maxAmount,
					Zone = zoneToTake.NodeIndex
				};
			}
		}

		public void LogRepresentation(RiftGame game)
		{
			var podNodes = game.GetZonesWithMyPods().ToList();
			var controlNodes = game.GetZonesUnderMyControl().ToList();
			Log($"Current statistics: {podNodes.Count} with myNodes, {controlNodes} under control");
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
			
			//Read nodes
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

			//Link nodes
			for (int i = 0; i < game.AmountOfZoneLinks; i++)
			{
				inputs = ReadLine().Split(' ');
				var z1 = game.GetZone(int.Parse(inputs[0]));
				var z2 = game.GetZone(int.Parse(inputs[1]));
				z1.LinkedNodes.Add(z2);
				z2.LinkedNodes.Add(z1);
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
				zone.OwningPlayerID = zone.OwningPlayerID < 0 ? null : zone.OwningPlayerID; //Keep null
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