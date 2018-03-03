using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class GameState 
	{
		//Indicate whether we are using a flipped board, to ensure we always are on the left
		public bool FlippedBoard {get;set;}
		public int MyTeam {get;set;}
		public List<SpawnBush> SpawnBushes {get;set;} = new List<SpawnBush>();
		public List<Item> Items {get;set;} = new List<Item>();
		public int Gold { get; set; }
		public int EnemyGold { get; set; }
		public int RoundType { get; set; }
		public List<Entity> Entities { get; set; } = new List<Entity>();
		public List<Entity> EntitiesMine { get; set; } = new List<Entity>();
		public List<Entity> EntitiesEnemy { get; set; } = new List<Entity>();
		public List<Entity> EntitiesMyHeros { get; set; }

		public void DistributeEntitiesOverSubCollections()
		{
			EntitiesMine = Entities.Where(e => e.Team == MyTeam).ToList();
			EntitiesEnemy = Entities.Where(e => e.Team != MyTeam).ToList();
			EntitiesMyHeros = EntitiesMine.Where(e => e.UnitType == BottersConstants.UnitTypes.Hero).ToList();
		}

		public string GetInitialGameStateString()
		{
			return $@"[Gamestate: ID={MyTeam} BushCount={SpawnBushes.Count} ItemCount={Items.Count}]";
		}

		public string GetEntityString(List<Entity> entityList, string prefix)
		{
			return $@"[GameState: {prefix}-> Entities= {entityList.Count}]{string.Join("\r\n",entityList.Select(e => e.GetEntityString()))}";
		}

		public void FlipItemPositions()
		{
			if(!FlippedBoard) { return; }
			this.Entities.ForEach(e => e.Flip());
			this.SpawnBushes.ForEach(e => e.Flip());
		}
	}
}