//require: Node.cs
//require: NodeRoute.cs
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Challenges.GhostInTheCell
{
	public class FactoryNode : Shared.Node<FactoryNode>
	{
		public int OwnedBy { get; set; }
		public int NrOfCyborgs { get; set; }
		public int Production { get; set; }
		public bool IsMine { get { return OwnedBy == 1; } }
		public bool IsNeutral { get { return OwnedBy == 0; } }
	}
	public class FactoryNodeLink
	{
		public FactoryNode Origin {get;set;}
		public FactoryNode Destination {get;set;}
		public int Distance {get;set;}
	}

	public class PlayingField
	{
		public List<FactoryNode> Factories {get;set;} = new List<FactoryNode>();

		public List<FactoryNodeLink> FactoryLinks {get;set;} = new List<FactoryNodeLink>();
	}

	

	public class GhostInTheCellPlayer : Framework.PuzzleMain
	{
		protected GhostInTheCellPlayer(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			new GhostInTheCellPlayer(new CodingGameProxyEngine()).Run();
		}
		public override void Run()
		{
			//Read the state of the playing field
			var gameState = CreatePlayingField();

			// game loop
			while (IsRunning())
			{
				UpdateGameState(gameState);

				// Write an action using WriteLine()
				// To debug: Error.WriteLine("Debug messages...");


				// Any valid action, such as "WAIT" or "MOVE source destination cyborgs"
				var action = CalculateAction(gameState);
				Log($"Action to take: {action}");
				WriteLine(action.ToString());
			}
		}

		private Action CalculateAction(PlayingField field)
		{
			var myFactories= field.Factories.Where(f => f.IsMine).ToList();
			var enemies = field.Factories.Where(f => !f.IsNeutral && !f.IsMine).ToList();
			var neutralFactories = field.Factories.Where(f => f.IsNeutral).ToList();
			//Check if there is a neutral factory attached to one of mine
			var possibleOriginsToNeutralFactories = myFactories.Where(f=> f.NrOfCyborgs > 1 && f.LinkedNodes.Any(l => l.IsNeutral));
			Log($"Found {possibleOriginsToNeutralFactories.Count()} nodes with neutral links");
			if(possibleOriginsToNeutralFactories.Any())
			{
				var origin = possibleOriginsToNeutralFactories.First();
				var dest = origin.LinkedNodes.First(d => d.IsNeutral);
				return new MoveTroopsAction()
				{
					From = origin.NodeIndex,
					To = dest.NodeIndex,
					AmountOfTroops = (int)Math.Ceiling(origin.NrOfCyborgs / 2.0),
				};
			}
			Log("No neutral nodes to take, trying to take the enemy");

			//If not, check if there is an enemy factory near, which I can take
			if(enemies.Any())
			{
				var enemyToTake = enemies.OrderBy(e => e.NrOfCyborgs).First(); //Smallest enemy camp
				var myDirectLink = enemyToTake.LinkedNodes.Where(n => n.IsMine && n.NrOfCyborgs > enemyToTake.NrOfCyborgs).OrderByDescending(n => n.NrOfCyborgs).FirstOrDefault();
				if(myDirectLink != null)
				{
					Log("Found a direct link to the enemy which we can directly take");
					return new MoveTroopsAction()
					{
						From = myDirectLink.NodeIndex,
						To = enemyToTake.NodeIndex,
						AmountOfTroops = enemyToTake.NrOfCyborgs + 1
					};
				}
				//Else send a random item or somehting	
			}
			Log("Dunno what to do, just wait here?");
			//Dunno, wait and weep
			return new WaitAction();
		}

		//TODO: we could value each step and pick the best one?

		//READING OF GAMESTATE IN TURNS
		//*************************************
		private void UpdateGameState(PlayingField field)
		{
			int entityCount = int.Parse(ReadLine()); // the number of entities (e.g. factories and troops)
			Log($"Reading state for {entityCount} entities");
			for (int i = 0; i < entityCount; i++)
			{
				string[] inputs = ReadLine().Split(' ');
				int entityId = int.Parse(inputs[0]);
				string entityType = inputs[1];
				int arg1 = int.Parse(inputs[2]);
				int arg2 = int.Parse(inputs[3]);
				int arg3 = int.Parse(inputs[4]);
				int arg4 = int.Parse(inputs[5]);
				int arg5 = int.Parse(inputs[6]);

				if(entityType == "FACTORY")
				{
					var factoryToUpdate = field.Factories.First(f => f.NodeIndex == entityId);
					Log($"Updating factor: {factoryToUpdate.NodeIndex}");
					factoryToUpdate.OwnedBy = arg1; //arg1: player that owns the factory: 1 for you, -1 for your opponent and 0 if neutral
					factoryToUpdate.NrOfCyborgs = arg2; //arg2: number of cyborgs in the factory
					factoryToUpdate.Production = arg3; //arg3: factory production (between 0 and 3)
				}
				else if(entityType == "TROOP")
				{
					Log("Found troop data, ignoring for now");
				}
			}
		}

		//INITIAL READING OF THE PLAYING FIELD
		//*************************************

		private PlayingField CreatePlayingField()
		{
			string[] inputs;
		
			//Initialize our factories
			int factoryCount = int.Parse(ReadLine()); // the number of factories
			Log($"Found {factoryCount} factories");
			List<FactoryNode> factories = new List<FactoryNode>();
			for(int i = 0; i < factoryCount; i++) //Factories start with 0
			{
				factories.Add(new FactoryNode() { NodeIndex = i});
			}

			//Initialize our factory links
			List<FactoryNodeLink> factoryLinks = new List<FactoryNodeLink>();
			int linkCount = int.Parse(ReadLine()); // the number of links between factories
			Log($"Found {linkCount} factory links");
			for (int i = 0; i < linkCount; i++)
			{
				inputs = ReadLine().Split(' ');
				int factory1 = int.Parse(inputs[0]);
				int factory2 = int.Parse(inputs[1]);
				int distance = int.Parse(inputs[2]);
				factoryLinks.Add(new FactoryNodeLink()
				{
					Origin = factories.First(f => f.NodeIndex == factory1),
					Destination = factories.First(f => f.NodeIndex == factory2),
					Distance = distance
				});
			}

			LinkFactories(factories, factoryLinks);

			return new PlayingField()
			{
				Factories = factories,
				FactoryLinks = factoryLinks
			};
		}

		private void LinkFactories(List<FactoryNode> factories, List<FactoryNodeLink> links)
		{
			//TODO: this does not yet factor in the distance of the nodes
			links.ForEach(l => 
			{
				l.Destination.LinkedNodes.Add(l.Origin);
				l.Origin.LinkedNodes.Add(l.Destination);
			});
		}










		//HELPER CLASSES





		//-> Actions
		public class Action
		{
			public override string ToString() 
			{
				throw new Exception("Action should implement the tostring");
			}
		}
		public class WaitAction : Action
		{
			public override string ToString() 
			{
				return "WAIT";
			}
		}
		public class MoveTroopsAction : Action
		{
			public int From {get;set;}
			public int To {get;set;}
			public int AmountOfTroops {get;set;}
			public override string ToString()
			{
				return $"MOVE {From} {To} {AmountOfTroops}";
			}
		}
	}
}