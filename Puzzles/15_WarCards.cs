using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

namespace Puzzles.Warcards
{
	//https://www.codingame.com/ide/puzzle/winamax-battle
	public class WarCardsPuzzle : PuzzleMain
	{
		public WarCardsPuzzle(IGameEngine gameEngine) : base(gameEngine) { }

		public static void Main(string[] args)
		{
			new WarCardsPuzzle(new CodingGameProxyEngine()).Run();
		}

		//Reference list for the card strengths and powers
		public List<string> CardPower = new List<string>() 
		{ 
			"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"
		};

		public override void Run() 
		{
			var playBoard = ReadPlayBoard();
			var warRes = RunWar(playBoard);
			
			//Check if there is a result, if not we have a PAT
			if(warRes.WinningPlayer != null && warRes.NrOfRounds != null) 
			{
				WriteLine($"{warRes.WinningPlayer} {warRes.NrOfRounds}");
			}
			else
			{
				WriteLine("PAT");	
			}			
		}

		private PlayBoard ReadPlayBoard()
		{
			var playBoard = new PlayBoard();
			
			int nrOfCardsP1 = int.Parse(ReadLine()); 
			for (int i = 0; i < nrOfCardsP1; i++)
			{
				var card = new Card(ReadLine());
				playBoard.CardsPlayer1.Add(card); 
			}

			int nrOfCardsP2 = int.Parse(ReadLine());
			for (int i = 0; i < nrOfCardsP2; i++)
			{
				var card = new Card(ReadLine());
				playBoard.CardsPlayer2.Add(card); 
			}
			return playBoard;
		}

		
		public WarResult RunWar(PlayBoard playBoard)
		{
			var warRes = new WarResult() { NrOfRounds = 0 };
			while(true)
			{
				Log(playBoard.EchoCards());
				
				//Check if the players can still play
				if(!playBoard.CardsPlayer1.Any()) { warRes.WinningPlayer = 2; break; }
				if(!playBoard.CardsPlayer2.Any()) { warRes.WinningPlayer = 1; break; }
				
				//Play another round
				warRes.NrOfRounds++;

				var warCheck = PlayCausesWar(playBoard);
				if(warCheck == null) { throw new Exception("One of the players ran out, this is not possible"); }
				if(!warCheck.CausesWar) 
				{ 
					Log("No war"); 
					RedistCardsIfNotWar(playBoard, warCheck);
					continue; 
				}
				Log("This is war!");
				PlayWar(playBoard, warCheck);
			}

			return warRes;
		}

		
		protected WarCheck PlayCausesWar(PlayBoard playBoard)
		{
			var c1 = DeQueueCard(playBoard.CardsPlayer1);
			var c2 = DeQueueCard(playBoard.CardsPlayer2);
			
			Log($"Playing->  p1: {c1} p2: {c2}");
			if(c1 == null || c2 == null) { return null; }
			return new WarCheck()
			{
				P1Card = c1,
				P2Card = c2,
				CausesWar = c1.Strength == c2.Strength
			};
		}

		private void RedistCardsIfNotWar(PlayBoard playBoard, WarCheck warCheck)
		{
			if(CardPower.IndexOf(warCheck.P1Card.Strength) > CardPower.IndexOf(warCheck.P2Card.Strength))
			{
				playBoard.CardsPlayer1.Add(warCheck.P1Card);
				playBoard.CardsPlayer1.Add(warCheck.P2Card);
			}
			else
			{
				playBoard.CardsPlayer2.Add(warCheck.P2Card);
				playBoard.CardsPlayer2.Add(warCheck.P1Card);
			}
		}

		protected void PlayWar(PlayBoard playBoard, WarCheck warCheck)
		{
			return;
		}


		//Helper to dequeue the card from the beginning of the queue and return it
		private Card DeQueueCard(List<Card> cards)
		{
			if(!cards.Any()) { return null; }
			var card = cards.First();
			cards.RemoveAt(0);
			return card;
		} 
	}



	// ********************
	// ** HELPER CLASSES **
	// ********************
	public class PlayBoard
	{
		public List<Card> CardsPlayer1 { get;set; } = new List<Card>();
		public List<Card> CardsPlayer2 { get;set; } = new List<Card>();
		public string EchoCards() 
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"P1 cards: ");
			CardsPlayer1.ForEach(c => sb.Append($" [{c}] "));
			sb.AppendLine();				
			sb.Append($"P2 cards: ");
			CardsPlayer2.ForEach(c => sb.Append($" [{c}] "));
			return sb.ToString();
		}
	}

	public class Card
	{
		public Card() {}
		public Card(string cardString)
		{
			Strength = cardString.Substring(0, cardString.Length - 1);
			CardType = cardString.Last().ToString();
		}
		public string Strength {get;set;}
		public string CardType {get;set;}
		public override string ToString()
		{
			return $"{Strength}{CardType}";
		}
	}

	public class WarCheck
	{
		public Card P1Card {get;set;}
		public Card P2Card {get;set;}
		public bool CausesWar {get;set;}
	}
	
	public class WarResult
	{
		public int? WinningPlayer {get;set;}
		public int? NrOfRounds {get;set;}
	}
}