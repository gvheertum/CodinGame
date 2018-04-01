using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;

/**
	https://www.codingame.com/ide/puzzle/tic-tac-toe
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.TicTacToe 
{
	
	public class TicTacToePlayer : PuzzleMain
	{
		protected TicTacToePlayer(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		public static void Main(string[] args)
		{
			new TicTacToePlayer(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
		
			string[] inputs;
			var myBoard = new TicTacToeBoard(0, (o) => Log(o));
			// game loop
			while (IsRunning())
			{
				var oppMove = ReadOpponentMove(ReadLine());
				Log($"Opponent: {oppMove}");
				myBoard.ProcessPlayerMove(oppMove);
				
				var availableMoves = ReadAvailableMoves(Int32.Parse(ReadLine())).ToList();
				Log($"Found {availableMoves.Count()} available moves");
				availableMoves.ToList().ForEach(m => Log(m));

				var moveToMake = myBoard.CalculateBestMove(availableMoves);
				Log($"Making move: {moveToMake}");
				moveToMake.OwnedByPlayerID = TicTacToeCell.PlayerID;
				myBoard.ProcessPlayerMove(moveToMake);
				// Write an action using Console.WriteLine()
				// To debug: Console.Error.WriteLine("Debug messages...");

				Console.WriteLine($"{moveToMake.Row} {moveToMake.Col}");
			}
		}


		//BOARD READING
		private TicTacToeCell ReadOpponentMove(string inputLine)
		{
			var	inputs = inputLine.Split(' ');
			int row = int.Parse(inputs[0]);
			int col = int.Parse(inputs[1]);

			return new TicTacToeCell()
			{
				Row = row,
				Col = col,
				OwnedByPlayerID = TicTacToeCell.EnemyID,
			};
		} 

		private IEnumerable<TicTacToeCell> ReadAvailableMoves(int availableMoves)
		{
			Log($"Reading {availableMoves} available moves");
			for(int i = 0; i < availableMoves; i++)
			{
				yield return ReadAvailableMoveFromInput(ReadLine());
			}
		}

		private TicTacToeCell ReadAvailableMoveFromInput(string inputLine)
		{
			var	inputs = inputLine.Split(' ');
			int row = int.Parse(inputs[0]);
			int col = int.Parse(inputs[1]);

			return new TicTacToeCell()
			{
				Row = row,
				Col = col,
			}; 
		}
	}

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


	public class TicTacToeBoard : Framework.LogInjectableClass
	{
		private int _boardId;
		public TicTacToeBoard(int boardId, Action<object> log) : base(log)
		{
			_boardId = boardId;
			InitializeBoard();
		}

		private List<TicTacToeCell> _grid = new List<TicTacToeCell>();
		private void InitializeBoard()
		{
			_grid = GetAllBoardCells().ToList();
		}

		public void ProcessPlayerMove(TicTacToeCell playCell) 
		{
			if(!playCell.IsRealMove()) { Log($"Ignoring playcell since it is not a real move: {playCell}"); return; }

			if(playCell.OwnedByPlayerID == null) { throw new Exception("Cannot process move if no owner set"); }
			var gridCell = GetCell(playCell.Row, playCell.Col);
			gridCell.OwnedByPlayerID = playCell.OwnedByPlayerID;
			Log($"Updated {gridCell} to be owned by playerID {playCell.OwnedByPlayerID}");
		}

		public TicTacToeCell CalculateBestMove(List<TicTacToeCell> cellsPlayable)
		{
			Func<TicTacToeCell ,bool> cellIsPlayable = (cell) => cellsPlayable.Any(c => c.Col == cell.Col && c.Row == cell.Row);

			var cellsToCompleteForMe = GetCellsICanUseToComplete().Where(cellIsPlayable);
			var cellsToComplateForEnemy = GetCellsEnemyCanUseToComplete().Where(cellIsPlayable);
			var cellsAlternateMoves = GetAlternateMoveCandidates().Where(cellIsPlayable);

			Log($"Found moves: my complete: {cellsToCompleteForMe.Count()}, enemy complete: {cellsToComplateForEnemy.Count()}, other {cellsAlternateMoves}");
			return cellsToCompleteForMe.FirstOrDefault() ??
				cellsToComplateForEnemy.FirstOrDefault() ??
				cellsAlternateMoves.FirstOrDefault() ??
				cellsPlayable.FirstOrDefault();
		}

		private IEnumerable<TicTacToeCell> GetCellsEnemyCanUseToComplete() 
		{
			return GetAllBoardCells().Where(c => CellCanCompleteLine(c, TicTacToeCell.EnemyID) || CellCanCompleteCol(c, TicTacToeCell.EnemyID));
		}

		private IEnumerable<TicTacToeCell> GetCellsICanUseToComplete()
		{
			return GetAllBoardCells().Where(c => CellCanCompleteLine(c, TicTacToeCell.PlayerID) || CellCanCompleteCol(c, TicTacToeCell.PlayerID));
		}

		private bool CellCanCompleteLine(TicTacToeCell cell, int playerID)
		{
			//for the row in the cell all items must be playerID OR the current item
			for(int colIdx = 0; colIdx < 3; colIdx++)
			{
				if(cell.Col == colIdx) { continue; } //Skip myself
				var gridCell = GetCell(cell.Row, colIdx);
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
				var gridCell = GetCell(rowIdx, cell.Col);
				if(gridCell.OwnedByPlayerID != playerID) { return false; }
			}
			//All elements processed, so it seems this can complete the line
			return true;
		}

		private IEnumerable<TicTacToeCell> GetAlternateMoveCandidates()
		{
			yield return new TicTacToeCell() { Row = 1, Col = 1 }; //Always start opening on center
		}

		// Cell production and retrieval

		private IEnumerable<TicTacToeCell> GetAllBoardCells()
		{
			for(int i = 0; i < 3; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					yield return new TicTacToeCell() { Row = i, Col = j };
				}
			}
		}
		
		private TicTacToeCell GetCell(int row, int col)
		{
			return _grid.Single(g => g.Row == row && g.Col == col);
		}
	}
}