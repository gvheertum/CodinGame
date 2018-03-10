//require: Position.cs
using Framework;

/**
 * Made with love by AntiSquid, Illedan and Wildum.
 * You can help children learn to code while you participate by donating to CoderDojo.
 **/
namespace Challenges.BottersOfTheGalaxy
{
	public class HeroAIFactory 
	{
		public HeroAIBase GetHeroAI(Entity hero, IGameEngine game)
		{
			if(string.Equals(hero.HeroType, BottersConstants.Heros.Deadpool))
			{
				return new HeroAIDeadpool(game, hero);
			}
			if(string.Equals(hero.HeroType, BottersConstants.Heros.Valkery))
			{
				return new HeroAIValkery(game, hero);
			}
			if(string.Equals(hero.HeroType, BottersConstants.Heros.DoctorStrange))
			{
				return new HeroAIDoctorStrange(game, hero);
			}
			if(string.Equals(hero.HeroType, BottersConstants.Heros.Hulk))
			{
				return new HeroAIHulk(game, hero);
			}
			throw new System.Exception($"No AI available for {hero.HeroType}");
		}
	}
}