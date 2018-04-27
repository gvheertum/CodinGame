//require: Position.cs
using Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

//https://www.codingame.com/ide/challenge/code-royale
namespace Challenges.CodeRoyal
{
	public class CodeRoyalPlayerQueenAI : LogInjectableClass
	{
		public CodeRoyalPlayerQueenAI(Action<object> log) : base(log)
		{
		}

		

		public IEnumerable<Action> GetQueenActions(GameState state)
		{
			List<Action> actionsApplicable = new List<Action>();
			actionsApplicable.AddRange(GetQueenTowerBuildActions(state));
			actionsApplicable.AddRange(GetQueenBarrackBuildActions(state));
			actionsApplicable.AddRange(GetQueenFleeAction(state));
			return actionsApplicable;
		}


		public IEnumerable<Action> GetQueenFleeAction(GameState state)
		{
			return state.GetMySites()
				.Where(s => s.StructureType == StructureType.Tower)
				.Select(t => new Action() { ActionString = $"MOVE {t.X} {t.Y}", ActionRating = 10 });
		}

		public IEnumerable<Action> GetQueenTowerBuildActions(GameState state)
		{
			var visitSites = GetSitesVisitableByQueen(state);
			if(state.GetMySites().Count(t => t.StructureType == StructureType.Tower) > 1)
			{
				Log("No towers needed, have 2 already");
				return new List<Action>();
			}
			return visitSites
				.OrderBy(v => v.Distance)
				.Select(s => new Action() { ActionString = $"BUILD {s.Destination.SiteID} TOWER", ActionRating = 20})
				.Take(2);
		}

		public IEnumerable<Action> GetQueenBarrackBuildActions(GameState state)
		{
			var visitSites = GetSitesVisitableByQueen(state);

			if(visitSites?.Any() != true) { Log("No site to use, returning empty"); yield break;}
			
			foreach(var visitSite in visitSites)
			{
				Log($"We are going to build a site: ");
				Log($"Site: {visitSite.Destination.SiteID} pos:{visitSite.Destination.X}:{visitSite.Destination.Y}");
				Log($"Queen: pos:{visitSite.Origin.X}:{visitSite.Origin.Y}");

				if(visitSite.Destination.UnitTypePlannedForMove == null) 
				{
					visitSite.Destination.UnitTypePlannedForMove = 
						state.GetMySites().Count(s => s.DeductedBarrackUnitType == UnitType.Knight) > state.GetMySites().Count(s => s.DeductedBarrackUnitType == UnitType.Archer)
						? UnitType.Archer
						: UnitType.Knight;
				}
				Log($"Building type: {visitSite.Destination.UnitTypePlannedForMove}");

				//This move will claim our destination, so update
				if(state.TouchedSite == visitSite.Destination.SiteID)
				{
					Log("we are already at our node, so that's that");
					yield return new Action() { ActionString = $"BUILD {visitSite.Destination.SiteID} BARRACKS-{visitSite.Destination.UnitTypePlannedForMove}", ActionRating = 100 }; //Prefer this one as top
					
				}

				yield return new Action() { ActionString = $"BUILD {visitSite.Destination.SiteID} BARRACKS-{visitSite.Destination.UnitTypePlannedForMove}", ActionRating = 15 };
			}
		}

		//See if we can get a tower closest to our enemies

		//See if we need to move away (preferrable to our tower)		

		

		private IEnumerable<PositionDistance<Unit,Site>> GetSitesVisitableByQueen(GameState state)
		{
			var sitesWeCanTake = state.GetFreeSites();
			Log($"Found {sitesWeCanTake.Count()} sites that are free");
			//Eliminate nodes that we cannot visit since we are in danger of the tower
			sitesWeCanTake = sitesWeCanTake.Where(s => !SiteIsNearDangerZone(s, state));
			Log($"{sitesWeCanTake.Count()} sites are not near danger");

			//Calculate the distances
			var siteDistances = state.GetMyQueen().DistancesToDetailed(sitesWeCanTake).OrderBy(d => d.Distance);
			return siteDistances;
		}

		private bool SiteIsNearDangerZone(Site site, GameState state)
		{
			var enemyTowers = state.GetEnemySites().Where(s => s.StructureType == StructureType.Tower);
			if(!enemyTowers.Any()) { Log($"No enemy towers for site {site.SiteID}"); return false; }
			var dangerousDistances = site.DistancesToDetailed(enemyTowers).Where(d => d.Distance < d.Destination.DeductedTowerRange);
			if(dangerousDistances.Any())
			{
				Log($"Enemy towers targeted at site: {site.SiteID}");
			}
			return dangerousDistances.Any();
		}
	}
}