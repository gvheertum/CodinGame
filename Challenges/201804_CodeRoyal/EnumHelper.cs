using System;

//https://www.codingame.com/ide/challenge/code-royale
namespace Challenges.CodeRoyal
{

	public enum UnitType
	{
		Queen,
		Knight,
		Archer,
		Giant
	}

	public enum Ownership
	{
		NotApplicable,
		Friendly,
		Enemy
	}	

	public enum StructureType
	{
		NoStructure,
		Barracks,
		Tower
	}

	public class EnumHelper
	{
		public UnitType GetUnitType(int typeId)
		{
			// -1 = QUEEN, 0 = KNIGHT, 1 = ARCHER
			switch(typeId)
			{
				case -1: return UnitType.Queen;
				case 0: return UnitType.Knight;
				case 1: return UnitType.Archer;
				case 2: return UnitType.Giant;
				default: throw new Exception($"No type match for typeid: {typeId}");
			}
		}

		public Ownership GetOwnership(int ownerId)
		{
			// -1 = No structure, 0 = Friendly, 1 = Enemy
			switch(ownerId)
			{
				case -1: return Ownership.NotApplicable;
				case 0: return Ownership.Friendly;
				case 1: return Ownership.Enemy;
				default: throw new Exception($"No type match for ownerId: {ownerId}");
			}
		}

		public StructureType GetStructureType(int typeId)
		{
			// -1 = No structure, 2 = Barracks
			switch(typeId)
			{
				case -1: return StructureType.NoStructure;
				case 2: return StructureType.Barracks;
				case 1: return StructureType.Tower;
				default: throw new Exception($"No type match for structureType: {typeId}");
			}
		}
	}
}