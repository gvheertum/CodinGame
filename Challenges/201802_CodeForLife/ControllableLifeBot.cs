using System;
using System.Linq;
using System.Collections.Generic;

namespace Challenges.CodeForLife
{
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
			SamplesWorkingOn.ForEach(s => Log(s.GetSampleDisplayString()));
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
			if(!LifeConstants.EngineHasUndiagnosedElements) { Log("Undiagnosed elements are not part of the engine now"); return null; }
			if(!_forceDiagnosis && SamplesWorkingOn.Count > 0) { Log("No undiagnosed sampling, working on data"); return null; } //No analyze stuff needed
			if(!_forceDiagnosis && state.Samples.Any(s => s.Diagnosed && s.Claimable)) { Log("Stuff in cloud, so no diagnosis"); return null; }
			
			if(SamplesAnalyzing.Count < LifeConstants.SuggestedNrOfUndiagnosedSamples && SamplesAnalyzing.Count + SamplesWorkingOn.Count < LifeConstants.MaxNrOfSamples) //Check if we can still carry items and if we should (suggested and abs max)
			{
				var relocateAction = EnsureLocation(LifeConstants.ModuleSampler);
				if(relocateAction != null) { return relocateAction; }
				
				Log("Retrieving new sample to analyze");
				return new LifeBotTakeUndiagnosedAction() { Rank = SamplesAnalyzing.Count == 2 ? 3 : 2 }; //Take 1 of each
			}
			_forceDiagnosis = false; //If this is hit we assume that the force flag is performed
			Log("No actions needed for diagnosis");
			return null;
		}

		private LifeBotAction GetDeliverForAnalysisIfApplicable(GameState state)
		{
			if(!LifeConstants.EngineHasUndiagnosedElements) { Log("Undiagnosed elements are not part of the engine now"); return null; }
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

		//TODO: Consider putting stuff in the cloud on forehand
		//TODO: in some cases we cannot make medicine since the molecule count > 10, so we might not be able to even perform them

		private int _moleculeMissCounter = 0;
		private const int MoleculeMissMax = 3;
		private bool _forceDiagnosis = false; 
		private LifeBotAction GetMoleculeRetrieveIfApplicable(GameState state)
		{
			if(SamplesWorkingOn.Any(ResourceForSampleComplete))
			{
				Log("We can make a product, skip retrieval");
				_moleculeMissCounter = 0;
				return null;
			}

			//Force a diagnosis tick to get new samples if too many missed
			if(_moleculeMissCounter >= MoleculeMissMax)
			{
				if(SamplesAnalyzing.Count + SamplesWorkingOn.Count < LifeConstants.MaxNrOfSamples)
				{ 
					Log("We missed to many molecules, try retrieving additional samples");
					_forceDiagnosis = true; 
				}
				else
				{
					Log("We are missing molecule hits, but have the max amount of samples.. Uh oh");
				}
			}

			if(StorageTotal >= LifeConstants.MaxNrOfMolecules) 
			{
				Log("WARNING: Max number of molecules reached");
				_moleculeMissCounter++;
				return null;
			}

			//Get molecules (for at least one sample)
			Log("We have samples, but need molecules");
			var action = EnsureLocation(LifeConstants.ModuleMolecules);
			if (action!=null) { return action; }
 
			//Get molecules for items we want to work on (and CAN work on)
			var samplesStillNeedingItems = SamplesWorkingOn.Where(s => !ResourceForSampleComplete(s));
			Log($"{samplesStillNeedingItems.Count()} samples still need molecules");
			//Try polling ANY needed molecule
			string moleculeToTake = samplesStillNeedingItems.Select(s => DetermineMoleculeTake(s,state)).FirstOrDefault(s => s != null);
			if(moleculeToTake == null) { Log("Not able to take a molecule"); _moleculeMissCounter++; return null; }
			_moleculeMissCounter = 0; //Reset miss counter
			return new LifeBotTakeMoleculeAction() { Molecule = moleculeToTake };
		}

		private LifeBotAction GetManufacturingRetrieveIfApplicable(GameState state)
		{
			var sampleToMake = SamplesWorkingOn.FirstOrDefault(ResourceForSampleComplete);
			if(sampleToMake == null)
			{
				Log("We want to the make part, but we have no suitable sample nor the molecules...");
				return null;
			}

			//Make the sample
			Log("We have everything ready to make make stuff");
			var action = EnsureLocation(LifeConstants.ModuleLaboratory);
			if (action!=null) { return action; }
			
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
			var makableSamples = state.Samples.Where(s=> s.CarriedBy == -1 && SampleCanBeMadeWithWorldResources(s, state)).OrderByDescending(RankSample).ToList();
			Log($"Found {makableSamples.Count} samples that can be made with the resources in the world");
			return makableSamples;
		}

		private bool SampleCanBeMadeWithWorldResources(LifeSample sample, GameState state)
		{
			bool canBeCreated = sample.CostA <= state.AvailableA + ExpertiseA && 
				sample.CostB <= state.AvailableB + ExpertiseB && 
				sample.CostC <= state.AvailableC + ExpertiseC && 
				sample.CostD <= state.AvailableD + ExpertiseD && 
				sample.CostE <= state.AvailableE + ExpertiseE; //Should we mind the carried stuff from the other bot
			return canBeCreated;
		}

		private bool ResourceForSampleComplete(LifeSample sample)
		{
			bool sampleResourcesComplete = sample.CostA <= this.ProjectedA && 
				sample.CostB <= this.ProjectedB && 
				sample.CostC <= this.ProjectedC && 
				sample.CostD <= this.ProjectedD && 
				sample.CostE <= this.ProjectedE;
			return sampleResourcesComplete;
		}

		private string DetermineMoleculeTake(LifeSample sample, GameState state)
		{
			if(sample == null) { Log("No sample deemed creatable with our molecules (we have non viable products in hand)"); return null; }
			//TODO: We should prefer taking resources that are scarse
			if(sample.CostA > ProjectedA && state.AvailableA > 0) { return "A"; }
			if(sample.CostB > ProjectedB && state.AvailableB > 0) { return "B"; }
			if(sample.CostC > ProjectedC && state.AvailableC > 0) { return "C"; }
			if(sample.CostD > ProjectedD && state.AvailableD > 0) { return "D"; }
			if(sample.CostE > ProjectedE && state.AvailableE > 0) { return "E"; }
			Log($"For sample {sample.SampleID} we could not determine a molecule");
			return null;
		}

		private int RankSample(LifeSample sample)
		{
			return sample.Health * (sample.Rank + 1);
		}
	}
	
}