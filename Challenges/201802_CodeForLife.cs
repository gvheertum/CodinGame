using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;

//https://www.codingame.com/ide/puzzle/code4life
namespace Challenges.CodeForLife
{
	public class LifeConstants 
	{
		public const string ModuleSampler = "SAMPLES";
		public const string ModuleDiagnostics = "DIAGNOSIS";
		public const string ModuleMolecules = "MOLECULES";
		public const string ModuleLaboratory = "LABORATORY";
		public const int MaxNrOfMolecules = 10;
		public const int MaxNrOfSamples = 3;
		public const int SuggestedNrOfSamples = 2;
		public const int SuggestedNrOfUndiagnosedSamples = 2;
	}

	

	public class ControllableLifeBot : LifeBot
	{
		public ControllableLifeBot(Action<object> log): base(log) {}

		protected override void Log(object o) 
		{
			base.Log($"[BOT: {o}]");
		}

		//Recipe carried ready for working on with molecules
		public List<LifeSample> SamplesWorkingOn {get;set;} = new List<LifeSample>();
		//Items not yet analyzed, need to go to the analyzer
		public List<LifeSample> SamplesAnalyzing {get;set;} = new List<LifeSample>();
		
		public List<LifeBotAction> Actions {get;set;} = new List<LifeBotAction>();
		public LifeBotAction DetermineAction(GameState state)
		{
			var action = DetermineActionInternal(state);
			Actions.Add(action);
			return action;
		}

		private LifeBotAction DetermineActionInternal(GameState state)
		{		
			Log("Beep boop, plotting action");
			Log($"I have {SamplesWorkingOn.Count} samples (and {SamplesAnalyzing.Count} to be analyzed) working on and am currently at {Target}");
			Log($"Molecules A:{StorageA} B:{StorageB} C:{StorageC} D:{StorageD} E:{StorageE}");
			
			return 
				GetDiagnoseActionIfApplicable(state) ??
				GetDeliverForAnalysisIfApplicable(state) ??
				GetAnalysisActionIfApplicable(state) ??
				GetMoleculeRetrieveIfApplicable(state) ??
				GetManufacturingRetrieveIfApplicable(state) ??
				new LifeBotWaitAction() { Comment = "We cannot make any other move..."};
		}

		
		private int _analyzingCount = 0;
		private LifeBotAction GetDiagnoseActionIfApplicable(GameState state)
		{
			if(SamplesWorkingOn.Count > 0) { Log("No undiagnosed sampling, working on data"); return null; } //No analyze stuff needed
			if(state.Samples.Any(s => s.Diagnosed && s.Claimable)) { Log("Stuff in cloud, so no diagnosis"); return null; }
			
			if(SamplesAnalyzing.Count < LifeConstants.SuggestedNrOfUndiagnosedSamples)
			{
				var relocateAction = EnsureLocation(LifeConstants.ModuleSampler);
				if(relocateAction != null) { return relocateAction; }
				
				Log("Retrieving new sample to analyze");
				return new LifeBotTakeUndiagnosedAction() { Rank = 2};
			}
			Log("No actions needed for diagnosis");
			return null;
		}

		private LifeBotAction GetDeliverForAnalysisIfApplicable(GameState state)
		{
			if(SamplesAnalyzing.Count <= 0) { Log("Not carrying stuff to analyze"); return null; }

			var relocateAction = EnsureLocation(LifeConstants.ModuleDiagnostics);
			if(relocateAction != null) { return relocateAction; }
			
			var itemToAnalyze = SamplesAnalyzing.First();
			Log($"Putting item {itemToAnalyze.SampleID} for analysis");
			return new LifeBotAnalyzeSampleAction() { SampleId = itemToAnalyze.SampleID };
		}

		private LifeBotAction GetAnalysisActionIfApplicable(GameState state)
		{
			if(SamplesWorkingOn.Count >= 1)  // LifeConstants.SuggestedNrOfSamples) 
			{
				Log("We have enough samples, so no need to gather more"); 
				return null; 
			}
			
			//Do we need to get samples
			Log("Not enough samples, need to get some");
			var action = EnsureLocation(LifeConstants.ModuleDiagnostics);
			if (action!=null) { return action; }
			
			Log($"At the diagnostic module, filling up to {LifeConstants.SuggestedNrOfSamples}");
			
			var sampleToTake = DetermineSampleToWorkOn(state).FirstOrDefault();
			//Take a sample
			if(sampleToTake != null)
			{
				Log($"Sample found, getting sample {sampleToTake.SampleID}");
				SamplesWorkingOn.Add(sampleToTake);
				return new LifeBotTakeSampleAction() { SampleId = sampleToTake.SampleID };
			}
			

			Log("No analysis retrieve for me");
			return null;
		}


		private LifeBotAction GetMoleculeRetrieveIfApplicable(GameState state)
		{
			if(SamplesWorkingOn.Any(ResourceForSampleComplete))
			{
				Log("We can make a product, skip retrieval");
				return null;
			}

			//Get molecules (for at least one sample)
			Log("We have samples, but need molecules");
			var action = EnsureLocation(LifeConstants.ModuleMolecules);
			if (action!=null) { return action; }

			string moleculeToTake = DetermineMoleculeTake(SamplesWorkingOn.First(s => !ResourceForSampleComplete(s)), state);
			if(moleculeToTake == null) { Log("Not able to take a molecule"); return null; }
			return new LifeBotTakeMoleculeAction() { Molecule = moleculeToTake };
		}

		private LifeBotAction GetManufacturingRetrieveIfApplicable(GameState state)
		{
			//Make the sample
			Log("We have everything ready to make make stuff");
			var action = EnsureLocation(LifeConstants.ModuleLaboratory);
			if (action!=null) { return action; }

			var sampleToMake = SamplesWorkingOn.FirstOrDefault(ResourceForSampleComplete);
			if(sampleToMake == null)
			{
				Log("We came to the make part, but we have no suitable sample...");
				return null;
			}
			Log($"Making sample: {sampleToMake.SampleID}");
			SamplesWorkingOn.Remove(sampleToMake); //Remove the sample from our list
			return new LifeBotMakeSampleAction() { SampleId = sampleToMake.SampleID };
		}
		
		//Check if we are at a certain point, if not return a movement, otherwise return a null
		private LifeBotGotoAction EnsureLocation(string destination)
		{
			if(Target == destination) { return null; }
		
			//Move to other module
			Log($"We are not at {destination} (but at {Target}), moving out");
			return new LifeBotGotoAction() { Destination = destination };
		}

		private List<LifeSample> DetermineSampleToWorkOn(GameState state)
		{
			var makableSamples = state.Samples.Where(s=> SampleCanBeMadeWithWorldResources(s, state)).OrderByDescending(RankSample).ToList();
			Log($"Found {makableSamples.Count} samples that can be made with the resources in the world");
			return makableSamples;
		}

		private bool SampleCanBeMadeWithWorldResources(LifeSample sample, GameState state)
		{
			return sample.CarriedBy == -1 && //In the cloud
				sample.CostA <= state.AvailableA && 
				sample.CostB <= state.AvailableB && 
				sample.CostC <= state.AvailableC && 
				sample.CostD <= state.AvailableD && 
				sample.CostE <= state.AvailableE; //Should we mind the carried stuff from the other bot
		}

		private bool ResourceForSampleComplete(LifeSample sample)
		{
			return sample.CostA <= this.StorageA && 
				sample.CostB <= this.StorageB && 
				sample.CostC <= this.StorageC && 
				sample.CostD <= this.StorageD && 
				sample.CostE <= this.StorageE;
		}

		private string DetermineMoleculeTake(LifeSample sample, GameState state)
		{
			if(sample.CostA > StorageA && state.AvailableA > 0) { return "A"; }
			if(sample.CostB > StorageB && state.AvailableB > 0) { return "B"; }
			if(sample.CostC > StorageC && state.AvailableC > 0) { return "C"; }
			if(sample.CostD > StorageD && state.AvailableD > 0) { return "D"; }
			if(sample.CostE > StorageE && state.AvailableE > 0) { return "E"; }
			return null;
		}

		private int RankSample(LifeSample sample)
		{
			return sample.Health * (sample.Rank + 1);
		}
	}

	

	public class CodeForLifeGame : PuzzleMain
	{
		protected CodeForLifeGame(IGameEngine gameEngine) : base(gameEngine) { }
		public static void Main(string[] args)
		{
			new CodeForLifeGame(new Framework.CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
			var projectCollection = InitializeProjectCollection();
			GameState gameState = null;
				
			// game loop
			while (IsRunning())
			{
				Log("Run loop!");
				gameState = UpdateGameState(gameState);
				Log(gameState.LogStats());
				Log("Asking our bot for an action");
				var botAction = gameState.MyLifeBot.DetermineAction(gameState);
				Log($"Going for: {botAction.GetOutput()}");
				WriteLine(botAction.GetOutput());
			}
		
		}

		private GameState UpdateGameState(GameState currState)
		{
			Log("Updating the stats");
			var gameState = currState ?? new GameState((o) => Log(o));
			string[] inputs = null;
			for (int i = 0; i < 2; i++)
			{
				LifeBot botToWorkOn = i == 0 ? gameState.MyLifeBot : gameState.EnemyLifeBot;
				inputs = ReadLine().Split(' ');
				botToWorkOn.Target = inputs[0];
				botToWorkOn.ETA = int.Parse(inputs[1]);
				botToWorkOn.Score = int.Parse(inputs[2]);
				botToWorkOn.StorageA = int.Parse(inputs[3]);
				botToWorkOn.StorageB = int.Parse(inputs[4]);
				botToWorkOn.StorageC = int.Parse(inputs[5]);
				botToWorkOn.StorageD = int.Parse(inputs[6]);
				botToWorkOn.StorageE = int.Parse(inputs[7]);
				botToWorkOn.ExpertiseA = int.Parse(inputs[8]);
				botToWorkOn.ExpertiseB = int.Parse(inputs[9]);
				botToWorkOn.ExpertiseC = int.Parse(inputs[10]);
				botToWorkOn.ExpertiseD = int.Parse(inputs[11]);
				botToWorkOn.ExpertiseE = int.Parse(inputs[12]);
			}
			
			inputs = ReadLine().Split(' ');
			gameState.AvailableA = int.Parse(inputs[0]);
			gameState.AvailableB = int.Parse(inputs[1]);
			gameState.AvailableC = int.Parse(inputs[2]);
			gameState.AvailableD = int.Parse(inputs[3]);
			gameState.AvailableE = int.Parse(inputs[4]);
			
			int sampleCount = int.Parse(Console.ReadLine());
			Log($"Reading {sampleCount} samples");
			gameState.Samples.Clear(); //Purge old stuff
			for (int i = 0; i < sampleCount; i++)
			{
				gameState.Samples.Add(ReadSample(ReadLine()));	
			}

			//Update samples held by my bot
			gameState.MyLifeBot.SamplesWorkingOn = gameState.Samples.Where(s => s.Mine && s.Diagnosed).ToList();
			gameState.MyLifeBot.SamplesAnalyzing = gameState.Samples.Where(s => s.Mine && !s.Diagnosed).ToList();

			Log("Retrieved gamestate");
			return gameState;
		}

		private LifeSample ReadSample(string sampleString)
		{
			var inputs = sampleString.Split(' ');
			LifeSample s = new LifeSample()
			{
				SampleID = int.Parse(inputs[0]),
				CarriedBy = int.Parse(inputs[1]),
				Rank = int.Parse(inputs[2]),
				ExpertiseGain = inputs[3],
				Health = int.Parse(inputs[4]),
				CostA = int.Parse(inputs[5]),
				CostB = int.Parse(inputs[6]),
				CostC = int.Parse(inputs[7]),
				CostD = int.Parse(inputs[8]),
				CostE = int.Parse(inputs[9])
			};
			return s;
		}

		private LifeProjectCollection InitializeProjectCollection()
		{
			LifeProjectCollection col = new LifeProjectCollection();
			col.ProjectCount = int.Parse(ReadLine());
			for (int i = 0; i < col.ProjectCount; i++)
			{
				var projectCounts = ReadLine().Split(' ');
				LifeProject p = new LifeProject()
				{
					CountElementA = int.Parse(projectCounts[0]),
					CountElementB = int.Parse(projectCounts[1]),
					CountElementC = int.Parse(projectCounts[2]),
					CountElementD = int.Parse(projectCounts[3]),
					CountElementE = int.Parse(projectCounts[4]),
				};
				col.Projects.Add(p);
			}
			Log($"Found {col.Projects.Count} projecs");
			return col;
		}


		
	}

	// Storage objects
	public class GameState : LogInjectableClass
	{
		public GameState(Action<object> log) : base(log)
		{
			MyLifeBot = new ControllableLifeBot(log);
			EnemyLifeBot = new LifeBot(log);
		}

		public int AvailableA {get;set;}
		public int AvailableB {get;set;}
		public int AvailableC {get;set;}
		public int AvailableD {get;set;}
		public int AvailableE {get;set;}

		public List<LifeSample> Samples {get;set;} = new List<LifeSample>();
		public ControllableLifeBot MyLifeBot {get;set;}
		public LifeBot EnemyLifeBot {get;set;}

		public string LogStats()
		{
			return $@"
			[GameState] 
			[me: s {MyLifeBot.Score}, l {MyLifeBot.Target}] 
			[other: s {EnemyLifeBot.Score}, l {EnemyLifeBot.Target}]";
		}
	}

	public class LifeBot : LogInjectableClass
	{
		public LifeBot() :base(null) {}
		public LifeBot(Action<object> log) : base(log)
		{
		}

		public string Target {get;set;}
		public 	int ETA {get;set;}
		public 	int Score {get;set;}
		public 	int StorageA {get;set;}
		public 	int StorageB {get;set;}
		public int StorageC {get;set;}
		public int StorageD {get;set;}
		public int StorageE {get;set;}
		public int ExpertiseA {get;set;}
		public int ExpertiseB {get;set;}
		public int ExpertiseC {get;set;}
		public int ExpertiseD {get;set;}
		public int ExpertiseE {get;set;}
	}

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
	}

	public class LifeProjectCollection
	{
		public int ProjectCount {get;set;}
		public List<LifeProject> Projects {get;set;} = new List<LifeProject>();
	}

	public class LifeProject
	{
		public int CountElementA {get;set;}
		public int CountElementB {get;set;}
		public int CountElementC {get;set;}
		public int CountElementD {get;set;}
		public int CountElementE {get;set;}
	}	




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