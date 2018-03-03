//require: Position.cs

/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class Helpers
	{
		public class Unit
		{
			public static bool IsTower(Entity e)
			{
				return e.UnitType == BottersConstants.UnitTypes.Tower;
			}

			public static bool IsHero(Entity e)
			{
				return e.UnitType == BottersConstants.UnitTypes.Hero;
			}

			public static bool IsMinion(Entity e)
			{
				return e.UnitType == BottersConstants.UnitTypes.Unit;
			}

			public static bool IsNear(Entity e, Entity e2, int maxDist)
			{
				return (e.DistanceTo(e2) < maxDist);
			}
		}
	}
}