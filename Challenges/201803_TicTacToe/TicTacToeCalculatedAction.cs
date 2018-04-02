/**
	https://www.codingame.com/ide/puzzle/tic-tac-toe
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.TicTacToe
{
	public class TicTacToeCalculatedAction : TicTacToeCell
	{
		public TicTacToeCalculatedActionType? ActionType { get; set; }
		public TicTacToeCalculatedAction() {}
		public TicTacToeCalculatedAction(TicTacToeCell cell, TicTacToeCalculatedActionType type) : base(cell) 
		{
			ActionType = type;
		}

		public override string ToString()
		{
			return $"[TicTacToeMove] [{ActionType}] {Row}(*{RowMultiplier}):{Col}(*{ColMultiplier})";
		}
	}

	public enum TicTacToeCalculatedActionType
	{
		None,
		ICanComplete,
		EnemyCanComplete,
		DeadCenterMove,
		CenterMove,
		NeighbourMove,
		Other
	}
}