
using System.Collections.Generic;

namespace Challenges.CodeForLife
{
	public class LifeProjectCollection
	{
		public int ProjectCount {get;set;}
		public List<LifeProject> Projects {get;set;} = new List<LifeProject>();
	}

	public class LifeProject
	{
		public int CountElementA {get;set;}
		public int CountElementB {get;set;}
		public int CountElementC {get;set;}
		public int CountElementD {get;set;}
		public int CountElementE {get;set;}
	}	
	
}