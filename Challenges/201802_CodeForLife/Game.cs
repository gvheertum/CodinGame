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
}