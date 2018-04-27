using System;
using System.Collections.Generic;
using System.Linq;

//https://www.codingame.com/ide/challenge/code-royale
namespace Challenges.CodeRoyal
{
	public class GameState
	{
		public List<Site> Sites { get; set; } = new List<Site>();
		public int NumberOfSites { get { return Sites.Count; } }

		public int Gold { get; internal set; }
		public int TouchedSite { get; internal set; }
		public List<Unit> Units { get; internal set; }

		public Site GetSite(int siteId)
		{
			return Sites.SingleOrDefault(s => s.SiteID == siteId);
		}

		public IEnumerable<Site> GetMySites()
		{
			return Sites.Where(s => s.Ownership == Ownership.Friendly);
		}

		public IEnumerable<Site> GetEnemySites()
		{
			return Sites.Where(s => s.Ownership == Ownership.Enemy);			
		}

		public IEnumerable<Site> GetFreeSites()
		{
			return Sites.Where(s => s.Ownership == Ownership.NotApplicable);
		}


		public Unit GetMyQueen() 
		{
			return Units.Single(u => u.Ownership == Ownership.Friendly && u.UnitType == UnitType.Queen);
		}


		public Unit GetEnemyQueen() 
		{
			return Units.SingleOrDefault(u => u.Ownership == Ownership.Enemy && u.UnitType == UnitType.Queen);
		}
	}
}