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
		public List<CodeRoyalGame.Unit> Units { get; internal set; }

		public Site GetSite(int siteId)
		{
			return Sites.SingleOrDefault(s => s.SiteID == siteId);
		}
	}
}