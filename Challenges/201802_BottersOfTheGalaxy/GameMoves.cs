/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public abstract class GameMoveBase
	{
		public abstract string GetMoveString();
	}

	public class GameMoveSpawnUnit : GameMoveBase
	{
		public string UnitName {get;set;}
		public string Message {get;set;}
		public override string GetMoveString()
		{
			return $"{UnitName}";
		}
	}

	
	public class GameMoveWait : GameMoveBase
	{
		public string Reason {get;set;}
		public override string GetMoveString()
		{
			return $"WAIT; {Reason}";
		}
	}

	public class GameMoveAttackClosest : GameMoveBase
	{
		public string UnitType{get;set;}
		public override string GetMoveString()
		{
			return $"ATTACK_NEAREST {UnitType}";
		}
	}

	public class GameMoveMove : GameMoveBase
	{
		public int X {get;set;}
		public int Y {get;set;}
		public override string GetMoveString()
		{
			return $"MOVE {X} {Y}";
		}
	}

	public class GameMoveAttack : GameMoveBase
	{
		public int UnitId {get;set;}
		public override string GetMoveString()
		{
			return $"ATTACK {UnitId}";
		}
	}

	public class GameMoveMoveAttack : GameMoveBase
	{
		public int X {get;set;}
		public int Y {get;set;}
		public int UnitId {get;set;}
		public override string GetMoveString()
		{
			return $"MOVE_ATTACK {X} {Y} {UnitId}";
		}
	}
}

/*

BUY itemName
SELL itemName
 */