
namespace Challenges.CodeForLife
{
	public class LifeSample
	{
		public bool Mine { get {return CarriedBy == 0; } }
		public int SampleID { get;set; }
		public int CarriedBy { get;set; }
		public int Rank { get;set; }
		public string ExpertiseGain { get;set; }
		public int Health { get;set; }
		public int CostA { get;set; }
		public int CostB { get;set; }
		public int CostC { get;set; }
		public int CostD { get;set; }
		public int CostE { get;set; }
		public int TotalCost { get { return CostA + CostB + CostC + CostD + CostE; } }
		public bool Diagnosed { get { return TotalCost > 0; } }
		public bool Claimable { get { return CarriedBy == -1; } }
		public string GetSampleDisplayString() 
		{
			return $"[Sample:{SampleID} R:{Rank} H:{Health} => A:{CostA} B:{CostB} C:{CostC} D:{CostD} E:{CostE}]";
		}
	}
	
}