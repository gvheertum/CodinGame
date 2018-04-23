//https://www.codingame.com/ide/challenge/code-royale
using Shared;

namespace Challenges.CodeRoyal
{
	public class Unit : Position
	{
		public Ownership Ownership { get; set; }
		public UnitType UnitType { get; set; }
		public int Health { get; set; }
	}
}