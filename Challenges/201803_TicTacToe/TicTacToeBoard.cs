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
	public class TicTacToeBoard : Framework.LogInjectableClass
	{
		public int BoardID { get; private set; }
		public int BoardColMultiplier { get; private set; }
		public int BoardRowMultiplier { get; private set; }
		private readonly TicTacToeBoardHelper _helper;
		public TicTacToeBoard(int boardID, int rowMultiplier, int colMultiplier, Action<object> log) : base(log)
		{
			BoardID = boardID;
			BoardRowMultiplier = rowMultiplier;
			BoardColMultiplier = colMultiplier;
			_helper = new TicTacToeBoardHelper(BoardRowMultiplier, BoardColMultiplier);
			InitializeBoard();
		}

		private List<TicTacToeCell> _grid = new List<TicTacToeCell>();
		private void InitializeBoard()
		{
			_grid = _helper.GetAllPossibleBoardCells().ToList();
		}

		//Update our grid to match the last move
		public void ProcessPlayerMove(TicTacToeCell playCell) 
		{
			if(!playCell.IsRealMove()) { Log($"Ignoring playcell since it is not a real move: {playCell}"); return; }

			if(playCell.OwnedByPlayerID == null) { throw new Exception("Cannot process move if no owner set"); }
			var gridCell = _helper.GetCellFromCollection(_grid, playCell.Row, playCell.Col);
			gridCell.OwnedByPlayerID = playCell.OwnedByPlayerID;
			Log($"Updated {gridCell} to be owned by playerID {playCell.OwnedByPlayerID}");
		}

		public IEnumerable<TicTacToeCell> GetBoardCells()
		{
			return _grid.Select(c => c);
		}

		public TicTacToeCell GetBoardCell(int row, int col)
		{
			return _helper.GetCellFromCollection(_grid, row, col);
		}

		public override string ToString()
		{
			return $"[Board] id:{BoardID} r*:{BoardRowMultiplier} c*:{BoardColMultiplier}";
		}
	}
}