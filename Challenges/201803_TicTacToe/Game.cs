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

	public class Game : PuzzleMain
	{
		private bool GameIsMultiBoard = true;

		protected Game(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		public static void Main(string[] args)
		{
			new Game(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
			//Generate the boards and stuff
			var myBoards = (GameIsMultiBoard ? GenerateBoardsForMultiTTT() : new [] { new TicTacToeBoard(0, 0, 0, (o) => Log(o)) }).ToList();
			var ais = new TicTacToeAIMulti(myBoards, (o) => Log(o));
			
			// game loop
			while (IsRunning())
			{
				var oppMove = ReadOpponentMove(ReadLine());
				Log($"Opponent: {oppMove}");
				GetBoardForCell(oppMove, myBoards).ProcessPlayerMove(oppMove);
				
				//Read the available moves on the system
				var availableMoves = ReadAvailableMoves(Int32.Parse(ReadLine())).ToList();
				Log($"Retrieved {availableMoves.Count()} available moves from input");
				//availableMoves.ToList().ForEach(m => Log(m));

				//Determine what to do
				var moveToMake = ais.DetermineMove(availableMoves);
				Log($"Making move: {moveToMake}");
				moveToMake.OwnedByPlayerID = TicTacToeCell.PlayerID;
				
				//Tell the board we are doing this!
				GetBoardForCell(oppMove, myBoards).ProcessPlayerMove(moveToMake);
				Console.WriteLine(moveToMake.GetOutputString());
			}
		}


		//BOARD READING
		private TicTacToeCell ReadOpponentMove(string inputLine)
		{
			var	inputs = inputLine.Split(' ');
			int row = int.Parse(inputs[0]);
			int col = int.Parse(inputs[1]);

			var c = new TicTacToeCell()
			{
				Row = row,
				Col = col,
				OwnedByPlayerID = TicTacToeCell.EnemyID,
			};
			c.Normalize();
			return c;
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

			var c = new TicTacToeCell()
			{
				Row = row,
				Col = col,
			}; 
			c.Normalize();
			return c;
		}


		//Helpers to get the corresponding board for a given move
		private TicTacToeBoard GetBoardForCell(TicTacToeCell cell, IEnumerable<TicTacToeBoard> boards)
		{
			return boards.Single(b => b.BoardRowMultiplier == cell.RowMultiplier && b.BoardColMultiplier == cell.ColMultiplier);
		}

		private IEnumerable<TicTacToeBoard> GenerateBoardsForMultiTTT()
		{
			int boardId = 0;
			for(int r = 0; r < 3; r++)
			{
				for(int c = 0; c < 3; c++)
				{
					yield return new TicTacToeBoard(++boardId, r, c, (o) => Log(o));
				}
			}
		}
	}
}