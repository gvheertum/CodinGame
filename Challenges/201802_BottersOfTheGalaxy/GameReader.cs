using System.Linq;
using System.Collections.Generic;
using Framework;

/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class GameReader : PuzzleMain
	{
		public GameReader(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		public GameState ReadIntialGameState()
		{
			var gameState = new GameState();
			gameState.MyTeam = int.Parse(ReadLine());
			gameState.SpawnBushes = ReadSpawnBushes().ToList();
			gameState.Items = ReadItems().ToList();
			return gameState;
		}

		private IEnumerable<Item> ReadItems()
		{
			int itemCount = int.Parse(ReadLine()); // useful from wood2
			for (int i = 0; i < itemCount; i++)
			{	
				var inputs = ReadLine().Split(' ');
				yield return new Item() 
				{
					ItemName = inputs[0], // contains keywords such as BRONZE, SILVER and BLADE, BOOTS connected by "_" to help you sort easier
					ItemCost = int.Parse(inputs[1]), // BRONZE items have lowest cost, the most expensive items are LEGENDARY
					Damage = int.Parse(inputs[2]), // keyword BLADE is present if the most important item stat is damage
					Health = int.Parse(inputs[3]),
					MaxHealth = int.Parse(inputs[4]),
					Mana = int.Parse(inputs[5]),
					MaxMana = int.Parse(inputs[6]),
					MoveSpeed = int.Parse(inputs[7]), // keyword BOOTS is present if the most important item stat is moveSpeed
					ManaRegeneration = int.Parse(inputs[8]),
					IsPotion = int.Parse(inputs[9]) // 0 if it's not instantly consumed
				};
			}
		}

		private IEnumerable<SpawnBush> ReadSpawnBushes()
		{
			int bushAndSpawnPointCount = int.Parse(ReadLine()); // usefrul from wood1, represents the number of bushes and the number of places where neutral units can spawn
			for (int i = 0; i < bushAndSpawnPointCount; i++)
			{
				var inputs = ReadLine().Split(' ');
				yield return new SpawnBush()
				{
					EntityType = inputs[0], // BUSH, from wood1 it can also be SPAWN
					X = int.Parse(inputs[1]),
					Y = int.Parse(inputs[2]),
					Radius = int.Parse(inputs[3])
				};
			}
		}

		public GameState UpdateGameState(GameState gameState)
		{
			gameState.Gold = int.Parse(ReadLine());
			gameState.EnemyGold = int.Parse(ReadLine());
			gameState.RoundType = int.Parse(ReadLine()); // a positive value will show the number of heroes that await a command
			gameState.Entities = new List<Entity>();
			int entityCount = int.Parse(ReadLine());
			Log($"Reading {entityCount} entities");
			for (int i = 0; i < entityCount; i++)
			{
				gameState.Entities.Add(ReadEntity());
			}
			gameState.DistributeEntitiesOverSubCollections();
			return gameState;
		}

		private Entity ReadEntity()
		{
			var inputs = ReadLine().Split(' ');
			return new Entity()
			{
				UnitId = int.Parse(inputs[0]),
				Team = int.Parse(inputs[1]),
				UnitType = inputs[2], // UNIT, HERO, TOWER, can also be GROOT from wood1
				X = int.Parse(inputs[3]),
				Y = int.Parse(inputs[4]),
				AttackRange = int.Parse(inputs[5]),
				Health = int.Parse(inputs[6]),
				MaxHealth = int.Parse(inputs[7]),
				Shield = int.Parse(inputs[8]), // useful in bronze
				AttackDamage = int.Parse(inputs[9]),
				MovementSpeed = int.Parse(inputs[10]),
				StunDuration = int.Parse(inputs[11]), // useful in bronze
				GoldValue = int.Parse(inputs[12]),
				CountDown1 = int.Parse(inputs[13]), // all countDown and mana variables are useful starting in bronze
				CountDown2 = int.Parse(inputs[14]),
				CountDown3 = int.Parse(inputs[15]),
				Mana = int.Parse(inputs[16]),
				MaxMana = int.Parse(inputs[17]),
				ManaRegeneration = int.Parse(inputs[18]),
				HeroType = inputs[19], // DEADPOOL, VALKYRIE, DOCTOR_STRANGE, HULK, IRONMAN
				IsVisible = int.Parse(inputs[20]), // 0 if it isn't
				ItemsOwned = int.Parse(inputs[21]) // useful from wood1
			};
		}
	}
}