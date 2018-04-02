
using System;
/**
https://www.codingame.com/ide/puzzle/tic-tac-toe
* Auto-generated code below aims at helping you parse
* the standard input according to the problem statement.
**/
namespace Puzzles.TicTacToe
{
	public class TicTacToeCell
	{
		public TicTacToeCell() {}

		public TicTacToeCell(TicTacToeCell cell)
		{
			this.Col = cell.Col;
			this.Row = cell.Row;
			this.RowMultiplier = cell.RowMultiplier;
			this.ColMultiplier = cell.ColMultiplier;
			this.OwnedByPlayerID = cell.OwnedByPlayerID;
		}

		public const int PlayerID = 1;
		public const int EnemyID = -1;
		public int Row { get; set; }
		public int Col { get; set; }
		public int RowMultiplier { get; set; }
		public int ColMultiplier { get; set; }
		public int? OwnedByPlayerID = null;
		public override string ToString()
		{
			return $"[TicTacToeCell] {Row}(*{RowMultiplier}):{Col}(*{ColMultiplier})";
		}
		public bool IsRealMove() => Row > -1 && Col > -1;

		public string GetOutputString() 
		{
			return $"{Row + (3 * RowMultiplier)} {Col + (3*ColMultiplier)}";
		}

		//Elements that are on different board need to be normalized to the 0-3 index with a correct multiplier
		public void Normalize(Action<object> log = null) 
		{
			log = log ?? ((o) => {});
			int col = Col;
			int row = Row;
			log($"Normalizing: {Row}:{Col}");
			Col = col % 3;
			Row = Row % 3;
			ColMultiplier = col / 3;
			RowMultiplier = row / 3;
			log($"Normalized: {Row}(*{RowMultiplier}):{Col}(*{ColMultiplier})");
		}

	}
}