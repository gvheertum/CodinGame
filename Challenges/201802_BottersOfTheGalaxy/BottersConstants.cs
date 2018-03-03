
using System.Collections.Generic;
/**
* Made with love by AntiSquid, Illedan and Wildum.
* You can help children learn to code while you participate by donating to CoderDojo.
**/
namespace Challenges.BottersOfTheGalaxy
{
	public class BottersConstants
	{
		public class UnitTypes 
		{
			public const string Hero = "HERO";
			public const string Tower = "TOWER";
			public const string Groot = "GROOT";
			public const string Unit = "UNIT";
		}
		public class Heros
		{
			public const string Deadpool = "DEADPOOL";
			public const string Valkery = "VALKYRIE";
			public const string DoctorStrange = "DOCTOR_STRANGE";
			public const string Hulk = "HULK";
			public const string Ironman = "IRONMAN"; 

			public static IEnumerable<string> AllHeroes()
			{
				yield return Deadpool;
				yield return Valkery;
				yield return DoctorStrange;
				yield return Hulk;
				yield return Ironman;
			}
		}
	}
}