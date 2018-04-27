//require: Position.cs
using Framework;
using System;
using System.Collections.Generic;
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
			var trainAI = new CodeRoyalPlayerTrainingAI(_log);
			var queenAI = new CodeRoyalPlayerQueenAI(_log);
			
			var actions = new PlayerActions();
			var queenActions = queenAI.GetQueenActions(state);
			actions.QueenAction = GetBestActionForQueen(queenActions);
			actions.TrainingAction = trainAI.GetTrainingAction(state);
			return actions;
		}

		private Action GetBestActionForQueen(IEnumerable<Action> queenActions)
		{
			Log($"Found {queenActions.Count()} actions for queen:");
			foreach(var a in queenActions)
			{
				Log($"{a.ActionString} (score: {a.ActionRating})");
			}
			return queenActions.OrderByDescending(a => a.ActionRating).FirstOrDefault() ?? new Action() { ActionString = "WAIT"};
		}
	}

	public class PlayerActions
	{
		public Action QueenAction {get;set;}
		public Action TrainingAction {get;set;} 
	}

	public class Action 
	{
		public string ActionString {get;set;}
		public int ActionRating {get;set;}
		public override string ToString()
		{
			return $"{ActionString} (rate: {ActionRating})";
		}
	}
}