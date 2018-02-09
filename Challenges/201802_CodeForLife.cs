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
	

	public class LifeBot
	{
		public LifeBotGotoAction DetermineAction(GameState state)
		{
			return new LifeBotGotoAction() { Destination = "MOLECULES" };
		}
	}


	public class LifeBotGotoAction 
	{
		public string Destination {get;set;}
		public string GetOutput() { return "GOTO " + Destination; }
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
			var bot = new LifeBot();
			//Log(projectCollection);

			// game loop
			while (IsRunning())
			{
				Log("Run loop!");
				var gameState = GetGameState();

				// Write an action using Console.WriteLine()
				// To debug: Console.Error.WriteLine("Debug messages...");
				var botAction = bot.DetermineAction(gameState);
				WriteLine("GOTO DIAGNOSIS");

			}
		
		}

		private GameState GetGameState()
		{
			var gameState = new GameState();
			string[] inputs = null;
			for (int i = 0; i < 2; i++)
			{
				inputs = ReadLine().Split(' ');
				string target = inputs[0];
				int eta = int.Parse(inputs[1]);
				int score = int.Parse(inputs[2]);
				int storageA = int.Parse(inputs[3]);
				int storageB = int.Parse(inputs[4]);
				int storageC = int.Parse(inputs[5]);
				int storageD = int.Parse(inputs[6]);
				int storageE = int.Parse(inputs[7]);
				int expertiseA = int.Parse(inputs[8]);
				int expertiseB = int.Parse(inputs[9]);
				int expertiseC = int.Parse(inputs[10]);
				int expertiseD = int.Parse(inputs[11]);
				int expertiseE = int.Parse(inputs[12]);
			}
			inputs = ReadLine().Split(' ');
			int availableA = int.Parse(inputs[0]);
			int availableB = int.Parse(inputs[1]);
			int availableC = int.Parse(inputs[2]);
			int availableD = int.Parse(inputs[3]);
			int availableE = int.Parse(inputs[4]);
			
			int sampleCount = int.Parse(Console.ReadLine());
			for (int i = 0; i < sampleCount; i++)
			{
				gameState.Samples.Add(ReadSample(ReadLine()));	
			}
			return gameState;
		}

		private LifeSample ReadSample(string sampleString)
		{
			var inputs = ReadLine().Split(' ');
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
			return col;
		}


		
	}

	// Storage objects
	public class GameState 
	{
		public List<LifeSample> Samples {get;set;} = new List<LifeSample>();
	}

	public class LifeSample
	{
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
}