//https://www.codingame.com/ide/challenge/code-royale
using Shared;

namespace Challenges.CodeRoyal
{
	public class Site : Position
	{
		public int SiteID { get; set; }
		public int Radius { get; set; }
		public int Ignore1 { get; internal set; }
		public int Ignore2 { get; internal set; }
		public StructureType StructureType { get; internal set; }
		public Ownership Ownership { get; internal set; }
		public int Param1 { get; internal set; }
		public int Param2 { get; internal set; }
	}
}