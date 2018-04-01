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
		protected Game(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		public static void Main(string[] args)
		{
			new Game(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
		
			var myBoard = new TicTacToeBoard(0, (o) => Log(o));
			var myAI = new TicTacToeAI(myBoard, (o) => Log(o));
			// game loop
			while (IsRunning())
			{
				var oppMove = ReadOpponentMove(ReadLine());
				Log($"Opponent: {oppMove}");
				myBoard.ProcessPlayerMove(oppMove);
				
				//Read the available moves on the system
				var availableMoves = ReadAvailableMoves(Int32.Parse(ReadLine())).ToList();
				Log($"Found {availableMoves.Count()} available moves");
				availableMoves.ToList().ForEach(m => Log(m));

				//Determine what to do
				var moveToMake = myAI.CalculateBestMove(availableMoves);
				Log($"Making move: {moveToMake}");
				moveToMake.OwnedByPlayerID = TicTacToeCell.PlayerID;
				
				//Tell the board we are doing this!
				myBoard.ProcessPlayerMove(moveToMake);
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
}