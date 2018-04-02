using System;
using System.Linq;
using System.Collections.Generic;

/**
	https://www.codingame.com/ide/puzzle/tic-tac-toe
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.TicTacToe
{

	public class TicTacToeAI : Framework.LogInjectableClass
	{
		private readonly TicTacToeBoard _board;
		private readonly TicTacToeBoardHelper _helper; 
		public TicTacToeAI(TicTacToeBoard board, Action<object> logFunc) : base(logFunc)
		{
			_board = board;
			_helper = new TicTacToeBoardHelper(board.BoardRowMultiplier, board.BoardColMultiplier);
			Log($"Created ai for: {board}");
		}

		public IEnumerable<TicTacToeCalculatedAction> CalculateMoves(List<TicTacToeCell> cellsPlayable)
		{
			Log($"Running moves for AI on board: {_board}");
			
			Func<TicTacToeCalculatedAction, bool> cellIsPlayable = (cell) => cellsPlayable.Any(c => 
				c.Col == cell.Col && 
				c.Row == cell.Row &&
				c.RowMultiplier == cell.RowMultiplier &&
				c.ColMultiplier == cell.ColMultiplier
			);
			bool centerField = _board.BoardRowMultiplier == 1 && _board.BoardColMultiplier == 1;
			if(centerField) { Log("Currently in center of board"); }

			var cellsToCompleteForMe = GetCellsICanUseToComplete().Where(cellIsPlayable);
			var cellsToComplateForEnemy = GetCellsEnemyCanUseToComplete().Where(cellIsPlayable);
			var cellCentre = new [] 
			{ 
				new TicTacToeCalculatedAction() 
				{ 
					Col = 1, 
					Row = 1, 
					ColMultiplier = _board.BoardColMultiplier, 
					RowMultiplier = _board.BoardRowMultiplier, 
					ActionType = centerField 
						? TicTacToeCalculatedActionType.DeadCenterMove 
						: TicTacToeCalculatedActionType.CenterMove
				}
			}.Where(cellIsPlayable);
			var cellsAlternateMoves = GetNeighboursOnOwnedCells().Where(cellIsPlayable);

			//First complete my own element, then try to block the enemy, then take center, if that fails we try to take a neighbour, otherwise pick a move
			//Log($"Found moves: my complete: {cellsToCompleteForMe.Count()}, enemy complete: {cellsToComplateForEnemy.Count()}, centre: {cellCentre.Count()}, other {cellsAlternateMoves}");
			var list = new List<TicTacToeCalculatedAction>();
			list.AddRange(cellsToCompleteForMe); 
			list.AddRange(cellsToComplateForEnemy);
			list.AddRange(cellCentre);
			list.AddRange(cellsAlternateMoves);
			return list;
		}


		//See if we can make a complete line, if so that move should be done
		private IEnumerable<TicTacToeCalculatedAction> GetCellsICanUseToComplete()
		{
			return _helper.GetAllPossibleBoardCells()
				.Where(c => CellCanCompleteLine(c, TicTacToeCell.PlayerID) || CellCanCompleteCol(c, TicTacToeCell.PlayerID))
				.Select(c => new TicTacToeCalculatedAction(c, TicTacToeCalculatedActionType.ICanComplete));
		}


		//See what the enemy can complete so we can block that
		private IEnumerable<TicTacToeCalculatedAction> GetCellsEnemyCanUseToComplete() 
		{
			return _helper.GetAllPossibleBoardCells()
				.Where(c => CellCanCompleteLine(c, TicTacToeCell.EnemyID) || CellCanCompleteCol(c, TicTacToeCell.EnemyID))
				.Select(c => new TicTacToeCalculatedAction(c, TicTacToeCalculatedActionType.EnemyCanComplete));
		}

		private bool CellCanCompleteLine(TicTacToeCell cell, int playerID)
		{
			//for the row in the cell all items must be playerID OR the current item
			for(int colIdx = 0; colIdx < 3; colIdx++)
			{
				if(cell.Col == colIdx) { continue; } //Skip myself
				var gridCell = _board.GetBoardCell(cell.Row, colIdx);
				if(gridCell.OwnedByPlayerID != playerID) { return false; }
			}
			//All elements processed, so it seems this can complete the line
			return true;
		}

		private bool CellCanCompleteCol(TicTacToeCell cell ,int playerID)
		{
			//for the row in the cell all items must be playerID OR the current item
			for(int rowIdx = 0; rowIdx < 3; rowIdx++)
			{
				if(cell.Row == rowIdx) { continue; } //Skip myself
				var gridCell = _board.GetBoardCell(rowIdx, cell.Col);
				if(gridCell.OwnedByPlayerID != playerID) { return false; }
			}
			//All elements processed, so it seems this can complete the line
			return true;
		}

		private IEnumerable<TicTacToeCalculatedAction> GetNeighboursOnOwnedCells()
		{	
			//Return all neigbours to my element to go through
			var myCurrentMoves = _board.GetBoardCells().Where(g => g.OwnedByPlayerID == TicTacToeCell.PlayerID);
			foreach(var cMove in myCurrentMoves)
			{
				for(int row = 0; row < 3; row++)
				{
					if(row != cMove.Row)
					{
						yield return new TicTacToeCalculatedAction() 
						{ 
							Row = row, 
							Col = cMove.Col,
							ColMultiplier = _board.BoardColMultiplier,
							RowMultiplier = _board.BoardRowMultiplier,
							ActionType = TicTacToeCalculatedActionType.NeighbourMove 
						};
					}
				}

				for(int col = 0; col < 3; col++)
				{
					if(col != cMove.Col)
					{
						yield return new TicTacToeCalculatedAction() 
						{ 
							Row = cMove.Row, 
							Col = col,
							ColMultiplier = _board.BoardColMultiplier,
							RowMultiplier = _board.BoardRowMultiplier,
							ActionType = TicTacToeCalculatedActionType.NeighbourMove 
						};
					}
				}
			}
		}
	}
}