//https://www.codingame.com/ide/challenge/code-royale
using Shared;

namespace Challenges.CodeRoyal
{
	public class Site : Position
	{
		const int TrainingCostArcher = 100;
		const int TrainingCostKnight = 80;

		public int SiteID { get; set; }
		public int Radius { get; set; }
		public int Ignore1 { get; set; }
		public int Ignore2 { get; set; }
		public StructureType StructureType { get; set; }
		public UnitType? PlannedUnitType { get; set; }
		public Ownership Ownership { get; set; }
		public int TrainingCost 
		{
			get 
			{
				
				switch(PlannedUnitType)
				{
					case UnitType.Archer: return TrainingCostArcher;
					case UnitType.Knight: return TrainingCostArcher;
					default: return 0;
				}
			}
		}
		public int Param1 { get; set; }
		public int Param2 { get; set; }
	}
}