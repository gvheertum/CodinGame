using System;
using System.Collections.Generic;
using Framework;

namespace Challenges.CodeForLife
{
	// Storage objects
	public class GameState : LogInjectableClass
	{
		public GameState(Action<object> log) : base(log)
		{
			MyLifeBot = new ControllableLifeBot(log);
			EnemyLifeBot = new LifeBot(log);
		}

		public int AvailableA {get;set;}
		public int AvailableB {get;set;}
		public int AvailableC {get;set;}
		public int AvailableD {get;set;}
		public int AvailableE {get;set;}

		public List<LifeSample> Samples {get;set;} = new List<LifeSample>();
		public ControllableLifeBot MyLifeBot {get;set;}
		public LifeBot EnemyLifeBot {get;set;}

		public string LogStats()
		{
			return $@"
			[GameState] 
			[me: s {MyLifeBot.Score}, l {MyLifeBot.Target}] 
			[other: s {EnemyLifeBot.Score}, l {EnemyLifeBot.Target}]";
		}
	}
	
}