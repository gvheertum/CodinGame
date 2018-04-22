//https://www.codingame.com/ide/challenge/code-royale
namespace Challenges.CodeRoyal
{

	public partial class CodeRoyalGame
	{
		public class Unit
		{
			public int X { get; set; }
			public int Y { get; set; }
			public Ownership Ownership { get; set; }
			public UnitType UnitType { get; set; }
			public int Health { get; set; }
		}

		
	}
}