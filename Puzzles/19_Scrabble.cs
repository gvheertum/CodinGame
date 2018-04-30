using System;
using System.Collections.Generic;
using System.Linq;
using Framework;

namespace Puzzles
{
	//https://www.codingame.com/ide/puzzle/scrabble
	public class ScrabblePuzzle : PuzzleMain
	{
		protected ScrabblePuzzle(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			new ScrabblePuzzle(new CodingGameProxyEngine()).Run();
		}



		public override void Run()
		{
			int nrOfWords = int.Parse(ReadLine());
			Log($"Reading {nrOfWords} words from input");
			List<ScrabbleWord> words = new List<ScrabbleWord>();
			for (int i = 0; i < nrOfWords; i++)
			{
				string word = ReadLine();
				words.Add(new ScrabbleWord(word, (o) => Log(o)));
			}

			string lettersInHand = ReadLine();
			Log($"In hand: {lettersInHand}");

			Log("Got words: ");
			foreach(var w in words)
			{
				Log($"{w.Word} (score: {w.Score})");
			}

			//Check the words we can make
			var wordsWeCanMake = words.Where(w => w.CanMakeWord(lettersInHand)).OrderByDescending(w => w.Score).ToList();
			Log("Makeable words: ");
			foreach(var w in wordsWeCanMake)
			{
				Log($"{w.Word} (score: {w.Score})");
			}
			
			//Wordlist is ordered by the score desc, so the first is the best we can make
			WriteLine(wordsWeCanMake.First().Word);
		}
	}

	public class ScrabbleWord : LogInjectableClass
	{
		public string Word { get; private set; }
		public int Score { get; private set; }
		public ScrabbleWord(string word, Action<object> logAction) : base(logAction)
		{
			Word = word;
			Score = ScoreWord(word);
		}

		private int ScoreWord(string word)
		{
			return word.Sum(c => ScoreLetter(c));
		}


		private static char[] _score1Letters = new [] {'e', 'a', 'i', 'o', 'n', 'r', 't', 'l', 's', 'u'};
		private static char[] _score2Letters = new [] {'d','g'};
		private static char[] _score3Letters = new [] {'b', 'c', 'm', 'p'};
		private static char[] _score4Letters = new [] {'f', 'h', 'v', 'w', 'y'};
		private static char[] _score5Letters = new [] { 'k' };
		private static char[] _score8Letters = new [] { 'j', 'x'};
		private static char[] _score10Letters = new [] { 'q', 'z'};
		
		private int ScoreLetter(char letter)
		{
			var pairs = GetScorePairs();
			foreach(var p in pairs)
			{
				if(p.Value.Any(c => c == letter))
				{
					return p.Key;
				}
			}			
			throw new Exception($"No value match for char: {letter}");
		}

		private static List<KeyValuePair<int, char[]>> _keyPairs = null;
		private List<KeyValuePair<int, char[]>> GetScorePairs() 
		{
			return _keyPairs = _keyPairs ?? new List<KeyValuePair<int, char[]>>()
			{
				new KeyValuePair<int, char[]>(1, _score1Letters),
				new KeyValuePair<int, char[]>(2, _score2Letters),
				new KeyValuePair<int, char[]>(3, _score3Letters),
				new KeyValuePair<int, char[]>(4, _score4Letters),
				new KeyValuePair<int, char[]>(5, _score5Letters),
				new KeyValuePair<int, char[]>(8, _score8Letters),
				new KeyValuePair<int, char[]>(10, _score10Letters),
			};
		}

		//Check if we can make a certain word with the letters in hand
		public bool CanMakeWord(string lettersInHand)
		{
			if(lettersInHand.Length < Word.Length) { return false; }
			char?[] letters = lettersInHand.Select(l => (char?)l).ToArray();
			char[] reqLetters = Word.Select(l => l).ToArray();

			//Take for all our required letters from the hand collection
			foreach(var reqLetter in reqLetters)
			{
				bool found = false;
				for(int handIdx = 0; handIdx < letters.Length; handIdx++)
				{
					if(reqLetter == letters[handIdx])
					{
						letters[handIdx] = null; //Remove letter from hand
						found = true;
						break;
					}
				}
				if(!found) 
				{
					Log($"Missing {reqLetter} to create {Word}");
					return false;
				}
			}
			return true; //If we reach this we could make the word!
		}
	}
}