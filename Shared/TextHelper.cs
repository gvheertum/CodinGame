

using System;
using System.Collections.Generic;

namespace Shared
{
	public class TextHelper
	{
		private bool _includingUnknown;
		public TextHelper(bool includingUnknown)
		{
			_includingUnknown = includingUnknown;
		}

		public string GetAllCharacters()
		{
			return "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + (_includingUnknown? "?" : "");
		}

		public IEnumerable<string> GetAllCharactersAsCollection()
		{
			foreach(char i in GetAllCharacters())
			{
				yield return $"{i}";
			}
		}

		public int GetIndexOfChar(string character) 
		{
			string charString = GetAllCharacters();
			int charIdx = charString.IndexOf(character, StringComparison.OrdinalIgnoreCase);
			if(charIdx < 0 && _includingUnknown) { return charString.Length -1; } //Last is the unknown item
			if(charIdx < 0) { throw new Exception($"Character {character} not found"); }
			return charIdx;
		}
	}
}