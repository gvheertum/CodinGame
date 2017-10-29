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

		
		public WarGameEndResult RunWar(PlayBoard playBoard)
		{
			var warRes = new WarGameEndResult() { NrOfRounds = 0 };
			while(true)
			{
				Log(playBoard.EchoCards());
				
				//Check if the players can still play
				if(!playBoard.CardsPlayer1.Any()) { warRes.WinningPlayer = 2; break; }
				if(!playBoard.CardsPlayer2.Any()) { warRes.WinningPlayer = 1; break; }
				
				//Play another round
				warRes.NrOfRounds++;

				var warCheck = SingleCardWar(playBoard);
				if(warCheck == null) { throw new Exception("One of the players ran out, this is not valid at this location"); }
				if(!warCheck.CausesWar) 
				{ 
					Log("No war, returning cards"); 
					playBoard.PlayerWin(warCheck.WinningPlayer.Value);
					continue; 
				}
				else
				{
					Log("This is war!");			
					MultiCardWar(playBoard);

				}
			}
			Log($"War is over: {warRes}");
			return warRes;
		}

		
		protected WarCheck SingleCardWar(PlayBoard playBoard)
		{
			var c1 = playBoard.PopCardPlayer1();
			var c2 = playBoard.PopCardPlayer2();
			
			Log($"Playing->  p1: {c1} p2: {c2}");
			if(c1 == null || c2 == null) { return null; }
			return new WarCheck()
			{
				P1Card = c1,
				P2Card = c2,
				CausesWar = c1.Strength == c2.Strength,
				WinningPlayer = GetWinningPlayer(c1,c2)
			};
		}

		//Based on 2 cards get the winning player. Null if equal
		private int? GetWinningPlayer(Card p1Card, Card p2Card)
		{
			var idxC1 = CardPower.IndexOf(p1Card.Strength);
			var idxC2 = CardPower.IndexOf(p2Card.Strength);
			if(idxC1 > idxC2) { return 1; }
			if(idxC2 > idxC1) { return 2; }
			return null;
		}

		private const int _warCardCount = 3;
		//War executed on equal cards, will lay off 3 cards and check the fourth
		protected WarOutcome MultiCardWar(PlayBoard playBoard)
		{
			var outcome = new WarOutcome();
			//Pop the amount of cards
			for(int i = 0; i < _warCardCount; i++)
			{
				if(playBoard.PopCardPlayer1() == null) { outcome.WinningPlayer = 2; outcome.ExitBecauseOutOfCards = true; return outcome; }
				if(playBoard.PopCardPlayer2()==null) { outcome.WinningPlayer = 1; outcome.ExitBecauseOutOfCards = true; return outcome; }
			}
			//Try getting a normal run
			var singleRes = SingleCardWar(playBoard);
			if(singleRes.WinningPlayer == null) { Log("War resulted in more war!"); return MultiCardWar(playBoard); }
			outcome.WinningPlayer = singleRes.WinningPlayer.Value;
			return outcome;
		}
	}



	// ********************
	// ** HELPER CLASSES **
	// ********************
	public class PlayBoard
	{
		public List<Card> CardsPlayer1 { get;set; } = new List<Card>();
		public List<Card> CardsPlayedPlayer1 {get;set;} = new List<Card>();
		public List<Card> CardsPlayer2 { get;set; } = new List<Card>();
		public List<Card> CardsPlayedPlayer2 {get;set;} = new List<Card>();

		public Card PopCardPlayer1() 
		{
			if(!CardsPlayer1.Any()) { return null; }

			var card = CardsPlayer1.First();
			CardsPlayer1.RemoveAt(0);
			CardsPlayedPlayer1.Add(card);
			return card;
		}

		public Card PopCardPlayer2() 
		{
			if(!CardsPlayer2.Any()) { return null; }

			var card = CardsPlayer2.First();
			CardsPlayer2.RemoveAt(0);
			CardsPlayedPlayer2.Add(card);
			return card;
		}
		
		public void PlayerWin(int playerID)
		{
			if(playerID == 1) { Player1Win(); }
			else if (playerID == 2) { Player2Win(); }
			else { throw new Exception($"PlayerID {playerID} invalid"); }
		}

		public void Player1Win()
		{
			CardsPlayer1.AddRange(CardsPlayedPlayer1);
			CardsPlayer1.AddRange(CardsPlayedPlayer2);

			CardsPlayedPlayer1.Clear();
			CardsPlayedPlayer2.Clear();
		}

		public void Player2Win()
		{
			CardsPlayer2.AddRange(CardsPlayedPlayer2);
			CardsPlayer2.AddRange(CardsPlayedPlayer1);
			CardsPlayedPlayer1.Clear();
			CardsPlayedPlayer2.Clear();
		}
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
		public int? WinningPlayer {get;set;}
	}
	public class WarOutcome
	{
		public int? WinningPlayer {get;set;}
		public bool ExitBecauseOutOfCards {get;set;}
	}
	public class WarGameEndResult
	{
		public int? WinningPlayer {get;set;}
		public int? NrOfRounds {get;set;}
	}
}