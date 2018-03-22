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
namespace Puzzles.TicTacToe {

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

			// game loop
			while (IsRunning())
			{
				var oppMove = ReadOpponentMove(ReadLine());
				Log($"Opponent: {oppMove}");
				
				var availableMoves = ReadAvailableMoves(Int32.Parse(ReadLine()));
				Log($"Found {availableMoves.Count()} available moves");
				availableMoves.ToList().ForEach(m => Log(m));

				// Write an action using Console.WriteLine()
				// To debug: Console.Error.WriteLine("Debug messages...");

				Console.WriteLine("0 0");
			}
		}


		//BOARD READING
		private TicTacToeMovePerformed ReadOpponentMove(string inputLine)
		{
			var	inputs = inputLine.Split(' ');
			int row = int.Parse(inputs[0]);
			int col = int.Parse(inputs[1]);

			return new TicTacToeMovePerformed()
			{
				Row = row,
				Col = col,
				MyMove = false,
			};
		} 

		private IEnumerable<TicTacToeMoveAvailable> ReadAvailableMoves(int availableMoves)
		{
			Log($"Reading {availableMoves} available moves");
			for(int i = 0; i < availableMoves; i++)
			{
				yield return ReadAvailableMoveFromInput(ReadLine());
			}
		}

		private TicTacToeMoveAvailable ReadAvailableMoveFromInput(string inputLine)
		{
			var	inputs = inputLine.Split(' ');
			int row = int.Parse(inputs[0]);
			int col = int.Parse(inputs[1]);

			return new TicTacToeMoveAvailable()
			{
				Row = row,
				Col = col,
			}; 
		}
	}

	public abstract class TicTacToeMoveBase
	{
		public int Row {get;set;}
		public int Col {get;set;}
		public override string ToString()
		{
			return $"[{this.GetType().Name}] row: {Row} col: {Col}";
		}
		public bool IsRealMove() => Row > -1 && Col > -1;
	}

	public class TicTacToeMoveAvailable : TicTacToeMoveBase
	{
	}

	public class TicTacToeMovePerformed : TicTacToeMoveBase
	{
		public bool MyMove {get;set;}
	}
}