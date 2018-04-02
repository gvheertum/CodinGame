using System.Linq;
using System.Collections.Generic;

/**
	https://www.codingame.com/ide/puzzle/tic-tac-toe
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.TicTacToe
{
	public class TicTacToeBoardHelper 
	{
		private readonly int _colMultiplier;
		private readonly int _rowMultiplier;
		public TicTacToeBoardHelper(int colMultipler, int rowMultiplier)
		{
			_colMultiplier = colMultipler;
			_rowMultiplier = rowMultiplier;
		}
		// Cell production and retrieval
		public IEnumerable<TicTacToeCell> GetAllPossibleBoardCells()
		{
			for(int i = 0; i < 3; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					yield return new TicTacToeCell() { Row = i, Col = j, ColMultiplier = _colMultiplier, RowMultiplier = _rowMultiplier };
				}
			}
		}

		public TicTacToeCell GetCellFromCollection(IEnumerable<TicTacToeCell> grid, int row, int col)
		{
			return grid.Single(g => g.Row == row && g.Col == col);
		}
	}
}