//https://www.codingame.com/ide/challenge/code-royale
using Shared;

namespace Challenges.CodeRoyal
{
	public class Site : Position
	{
		const int TrainingCostGiant = 100;
		const int TrainingCostArcher = 100;
		const int TrainingCostKnight = 80;

		public int SiteID { get; set; }
		public int Radius { get; set; }
		public int Ignore1 { get; set; }
		public int Ignore2 { get; set; }
		public StructureType StructureType { get; set; }
		
	
		public Ownership Ownership { get; set; }
		public int TrainingCost 
		{
			get 
			{
				switch(DeductedBarrackUnitType)
				{
					case UnitType.Archer: return TrainingCostArcher;
					case UnitType.Knight: return TrainingCostKnight;
					case UnitType.Giant: return TrainingCostGiant;
					default: return 0;
				}
			}
		}

		public int Param1 { get; set; }
		public int Param2 { get; set; }

		//Set by the queen moves
		public UnitType? UnitTypePlannedForMove { get; set; }


		//Helper properties
		//TODO: Perhaps split towers and barracks
		public int? DeductedBarrackWaitTime { get { return StructureType == StructureType.Barracks ? (int?)Param1 : null; } }

		public UnitType? DeductedBarrackUnitType 
		{ 
			get 
			{
				if(StructureType != StructureType.Barracks) { return null; }
				return new EnumHelper().GetUnitType(Param2);
			} 
		}
		
		public int? DeductedTowerHealth { get { return StructureType == StructureType.Tower ? (int?)Param1 : null; } }
		public int? DeductedTowerRange { get { return StructureType == StructureType.Tower ? (int?)Param2 : null; } }
	}
}