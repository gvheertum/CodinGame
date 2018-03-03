/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class Entity
	{
		public int UnitId {get;set;}
		public int Team {get;set;}
		public string UnitType {get;set;} // UNIT, HERO, TOWER, can also be GROOT from wood1
		public int X {get;set;}
		public int Y {get;set;}
		public int AttackRange {get;set;}
		public int Health {get;set;}
		public int MaxHealth {get;set;}
		public int Shield {get;set;} // useful in bronze
		public int AttackDamage {get;set;}
		public int MovementSpeed {get;set;}
		public int StunDuration {get;set;} // useful in bronze
		public int GoldValue {get;set;}
		public int CountDown1 {get;set;} // all countDown and mana variables are useful starting in bronze
		public int CountDown2 {get;set;}
		public int CountDown3 {get;set;}
		public int Mana {get;set;}
		public int MaxMana {get;set;}
		public int ManaRegeneration {get;set;}
		public string HeroType  {get;set;} // DEADPOOL, VALKYRIE, DOCTOR_STRANGE, HULK, IRONMAN
		public int IsVisible {get;set;} // 0 if it isn't
		public int ItemsOwned {get;set;} // useful from wood1

		public string GetEntityString()
		{
			return $"[Entity: Owner={Team} Type={UnitType} HeroType={HeroType}]";
		}
	}
}