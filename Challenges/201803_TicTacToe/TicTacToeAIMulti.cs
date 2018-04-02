//require: IEnumerableHelpers.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Shared;
/**
https://www.codingame.com/ide/puzzle/tic-tac-toe
* Auto-generated code below aims at helping you parse
* the standard input according to the problem statement.
**/
namespace Puzzles.TicTacToe
{
	//Group all AI's in a single element
	public class TicTacToeAIMulti : Framework.LogInjectableClass
	{
		private IEnumerable<TicTacToeAI> _aiCollection;
		public TicTacToeAIMulti(IEnumerable<TicTacToeBoard> boards, Action<object> logFunc) : base(logFunc)
		{
			_aiCollection = boards.Select(b => new TicTacToeAI(b, (o) => Log(o))).ToList();
		}		

		public TicTacToeCalculatedAction DetermineMove(List<TicTacToeCell> cellsPlayable)
		{
			var allMoves = _aiCollection.SelectMany(a => a.CalculateMoves(cellsPlayable)).ToList();
			Log($"Found a total of {allMoves.Count()} moves that are determined by the AI");
			Log("******************");
			allMoves.ForEach(m => Log(m));
			Log("******************");

			//Find the best move
			var moveToMake =  
				allMoves.RandomFirstOrDefault(m => m.ActionType == TicTacToeCalculatedActionType.ICanComplete) ??
				allMoves.RandomFirstOrDefault(m => m.ActionType == TicTacToeCalculatedActionType.EnemyCanComplete) ??
				allMoves.RandomFirstOrDefault(m => m.ActionType == TicTacToeCalculatedActionType.DeadCenterMove) ??
				allMoves.RandomFirstOrDefault(m => m.ActionType == TicTacToeCalculatedActionType.CenterMove) ??
				allMoves.RandomFirstOrDefault(m => m.ActionType == TicTacToeCalculatedActionType.NeighbourMove) ??
				new TicTacToeCalculatedAction(cellsPlayable.RandomFirstOrDefault(), TicTacToeCalculatedActionType.Other);
			
			Log($"Making move: {moveToMake}");
			return moveToMake;
		}
	}
}