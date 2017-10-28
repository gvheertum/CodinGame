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

		public override void Run() 
		{
			//Read the card stacks
			List<Card> p1Cards = new List<Card>();
			int nrOfCardsP1 = int.Parse(ReadLine()); 
			for (int i = 0; i < nrOfCardsP1; i++)
			{
				var card = new Card(ReadLine());
				Log($"Read card {card} for P1");
				p1Cards.Add(card); 
			}

			List<Card> p2Cards = new List<Card>();
			int nrOfCardsP2 = int.Parse(ReadLine());
			for (int i = 0; i < nrOfCardsP2; i++)
			{
				var card = new Card(ReadLine());
				Log($"Read card {card} for P2");
				p2Cards.Add(card); 
			}

			//Let's go to war!
			var warRes = RunWar(p1Cards, p2Cards);

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

		public class WarResult
		{
			public int? WinningPlayer {get;set;}
			public int? NrOfRounds {get;set;}
		}
		public WarResult RunWar(List<Card> p1Cards, List<Card> p2Cards)
		{
			return new WarResult();
		}
	}
}