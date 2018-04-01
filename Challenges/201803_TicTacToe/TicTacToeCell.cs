/**
	https://www.codingame.com/ide/puzzle/tic-tac-toe
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.TicTacToe
{
	public class TicTacToeCell
	{
		public const int PlayerID = 1;
		public const int EnemyID = -1;
		public int Row {get;set;}
		public int Col {get;set;}
		public int? OwnedByPlayerID = null;
		public override string ToString()
		{
			return $"[TicTacToeCell] row: {Row} col: {Col}";
		}
		public bool IsRealMove() => Row > -1 && Col > -1;
		public bool MyMove {get { return OwnedByPlayerID == 1; }}
	}
}