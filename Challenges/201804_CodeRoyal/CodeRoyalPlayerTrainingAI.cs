//require: Position.cs
using Framework;
using System;
using System.Linq;

//https://www.codingame.com/ide/challenge/code-royale
namespace Challenges.CodeRoyal
{
	public class CodeRoyalPlayerTrainingAI : LogInjectableClass
	{
		public CodeRoyalPlayerTrainingAI(Action<object> log) : base(log)
		{
		}

		public Action GetTrainingAction(GameState state)
		{
			var barracks = state.GetMySites().Where(s => s.StructureType == StructureType.Barracks && s.DeductedBarrackWaitTime <= 0).ToList();
			if(!barracks.Any()) {Log("No barracks (ready), so no training"); return new Action() { ActionString = "TRAIN"}; }

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
			return new Action() { ActionString = "TRAIN " + res.Trim() };
		}

	}
}