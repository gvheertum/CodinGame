using System;
using Framework;


namespace Challenges.CodeForLife
{
	public class LifeBot : LogInjectableClass
	{
		public LifeBot() :base(null) {}
		public LifeBot(Action<object> log) : base(log)
		{
		}

		public string Target {get;set;}
		public 	int ETA {get;set;}
		public int Score {get;set;}
		public int StorageA {get;set;}
		public int StorageB {get;set;}
		public int StorageC {get;set;}
		public int StorageD {get;set;}
		public int StorageE {get;set;}
		public int StorageTotal { get { return StorageA + StorageB + StorageC + StorageD + StorageE; } }
		public int ExpertiseA {get;set;}
		public int ExpertiseB {get;set;}
		public int ExpertiseC {get;set;}
		public int ExpertiseD {get;set;}
		public int ExpertiseE {get;set;}
		public int ProjectedA { get { return StorageA + ExpertiseA; } }
		public int ProjectedB { get { return StorageB + ExpertiseB; } }
		public int ProjectedC { get { return StorageC + ExpertiseC; } }
		public int ProjectedD { get { return StorageD + ExpertiseD; } }
		public int ProjectedE { get { return StorageE + ExpertiseE; } }
		
	}
	
}