/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class Item
	{
			public string ItemName {get;set;} // contains keywords such as BRONZE, SILVER and BLADE, BOOTS connected by "_" to help you sort easier
			public int ItemCost {get;set;} // BRONZE items have lowest cost, the most expensive items are LEGENDARY
			public int Damage {get;set;} // keyword BLADE is present if the most important item stat is damage
			public int Health {get;set;}
			public int MaxHealth {get;set;}
			public int Mana {get;set;}
			public int MaxMana {get;set;}
			public int MoveSpeed {get;set;} // keyword BOOTS is present if the most important item stat is moveSpeed
			public int ManaRegeneration {get;set;}
			public int IsPotion {get;set;} // 0 if it's not instantly consumed
	}
}