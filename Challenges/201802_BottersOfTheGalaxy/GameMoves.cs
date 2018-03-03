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
		public override string GetMoveString()
		{
			return $"{UnitName}";
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

	public class GameMoveWait : GameMoveBase
	{
		public string Reason {get;set;}
		public override string GetMoveString()
		{
			return $"WAIT {Reason}";
		}
	}
}