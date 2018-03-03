using System;
using System.Collections.Generic;
using System.Linq;

/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class GameState 
	{
		public int MyTeam {get;set;}
		public List<SpawnBush> SpawnBushes {get;set;} = new List<SpawnBush>();
		public List<Item> Items {get;set;} = new List<Item>();
		public int Gold { get; internal set; }
		public int EnemyGold { get; internal set; }
		public int RoundType { get; internal set; }
		public List<Entity> Entities { get; internal set; }

		public string GetInitialGameStateString()
		{
			return $@"[Gamestate: ID={MyTeam} BushCount={SpawnBushes.Count} ItemCount={Items.Count}]";
		}

		public string GetEntityString()
		{
			return $@"[GameState: Entities= {Entities.Count}]{string.Join("\r\n",Entities.Select(e => e.GetEntityString()))}";
		}
	}
}