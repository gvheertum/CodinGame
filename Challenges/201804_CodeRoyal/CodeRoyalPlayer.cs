//require: Position.cs
using Framework;
using System;

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
			return new PlayerActions
			{
				QueenAction = "WAIT",
				TrainingAction = "TRAIN"
			};
		}
	}

	public class PlayerActions
	{
		public string QueenAction {get;set;}
		public string TrainingAction {get;set;}
	}
}