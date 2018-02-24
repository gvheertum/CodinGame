
namespace Challenges.CodeForLife
{
	//LIFEBOT ACTIONS
	//********************/
	public abstract class LifeBotAction
	{
		public abstract string GetOutput();
	}

	public class LifeBotWaitAction : LifeBotAction
	{
		public string Comment {get;set;}
		public override string GetOutput() { return $"WAIT {Comment}" ;}
	}

	public class LifeBotGotoAction : LifeBotAction
	{
		public string Destination {get;set;}
		public override string GetOutput() { return "GOTO " + Destination; }
	}

	public class LifeBotAnalyzeSampleAction : LifeBotAction
	{
		public int SampleId {get;set;}
		public override string GetOutput() { return "CONNECT " + SampleId; }
	}

	public class LifeBotTakeSampleAction : LifeBotAction
	{
		public int SampleId {get;set;}
		public override string GetOutput() { return "CONNECT " + SampleId; }
	}
	
	public class LifeBotTakeUndiagnosedAction : LifeBotAction
	{
		public int Rank {get;set;}
		public override string GetOutput() { return "CONNECT " + Rank; }
	}

	public class LifeBotTakeMoleculeAction : LifeBotAction
	{
		public string Molecule {get;set;}
		public override string GetOutput() { return "CONNECT " + Molecule; }
	}

	public class LifeBotMakeSampleAction : LifeBotAction
	{
		public int SampleId {get;set;}
		public override string GetOutput() { return "CONNECT " + SampleId; }
	}

}