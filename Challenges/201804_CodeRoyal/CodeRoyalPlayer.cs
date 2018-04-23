//require: Position.cs
using Framework;
using Shared;
using System;
using System.Linq;

//https://www.codingame.com/ide/challenge/code-royale
namespace Challenges.CodeRoyal
{
	public class CodeRoyalPlayer : LogInjectableClass
	{
		public CodeRoyalPlayer(Action<object> log) : base(log)
		{
		}
		public PlayerActions GetMoves(GameState state)
		{
			var locationWeCanUse = GetClosestSiteToQueen(state);
			var actions = new PlayerActions();
			actions.QueenAction = GetQueenAction(state, locationWeCanUse);
			actions.TrainingAction = ("TRAIN " + GetTrainingActions(state)).Trim();
			return actions;
		}

		private string GetQueenAction(GameState state, PositionDistance<Unit,Site> selectedMove)
		{
			if(selectedMove == null) { Log("No site to use, returning empty train"); return "WAIT";}
			
			Log($"We are going to build a site: ");
			Log($"Site: {selectedMove.Destination.SiteID} pos:{selectedMove.Destination.X}:{selectedMove.Destination.Y}");
			Log($"Queen: pos:{selectedMove.Origin.X}:{selectedMove.Origin.Y}");

			if(selectedMove.Destination.UnitTypePlannedForMove == null) 
			{
				selectedMove.Destination.UnitTypePlannedForMove = 
					state.GetMySites().Count(s => s.DeductedBarrackUnitType == UnitType.Knight) > state.GetMySites().Count(s => s.DeductedBarrackUnitType == UnitType.Archer)
					? UnitType.Archer
					: UnitType.Knight;
			}
			Log($"Building type: {selectedMove.Destination.UnitTypePlannedForMove}");

			//This move will claim our destination, so update
			if(state.TouchedSite == selectedMove.Destination.SiteID)
			{
				Log("we are already there, so that's that");
			}

			return $"BUILD {selectedMove.Destination.SiteID} BARRACKS-{selectedMove.Destination.UnitTypePlannedForMove}";
		}

		private string GetTrainingActions(GameState state)
		{
			var barracks = state.GetMySites().Where(s => s.StructureType == StructureType.Barracks && s.DeductedBarrackWaitTime <= 0).ToList();
			if(!barracks.Any()) {Log("No barracks (ready), so no training"); return ""; }

			Log($"We have {barracks.Count()} barracks and {state.Gold} gold");
			var currGold = state.Gold;
			string res = "";
			while(currGold >= 0 && barracks.Any())
			{
				var pick = barracks.First();
				if(pick.TrainingCost <= currGold)
				{
					Log($"Issue train for site {pick.SiteID} (type {pick.DeductedBarrackUnitType}) costing: {pick.TrainingCost}");
					res += pick.SiteID + " "; 
					currGold = currGold - pick.TrainingCost;
				}
				barracks.Remove(pick);
			}
			return res.Trim();
		}

		

		private PositionDistance<Unit,Site> GetClosestSiteToQueen(GameState state)
		{
			var sitesWeCanTake = state.GetFreeSites();
			var siteDistances = state.GetMyQueen().DistancesToDetailed(sitesWeCanTake).OrderBy(d => d.Distance);
			return siteDistances.FirstOrDefault();
		}
	}

	public class PlayerActions
	{
		public string QueenAction {get;set;}
		public string TrainingAction {get;set;}
	}
}