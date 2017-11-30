//require: Node.cs
//require: NodeRoute.cs
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;

//https://www.codingame.com/ide/puzzle/ghost-in-the-cell
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

		public override string ToString()
		{
			return $"[Factory:{NodeIndex}] Cyb: {NrOfCyborgs} Prod: {Production} Owner: {OwnedBy}";
		}
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
				// Any valid action, such as "WAIT" or "MOVE source destination cyborgs"
				//WOOD 2 allows us to use multiple actions
				var actions = CalculateActions(gameState).ToList();
				actions.ForEach(action => Log($"Action to take: {action}"));
				WriteLine(string.Join(";", actions.Select(a => a.ToString())));
			}
		}

		private IEnumerable<Action> CalculateActions(PlayingField field)
		{
			List<Action> actions = new List<Action>();
			actions.AddRange(GetNeutralAttackActions(field));
			actions.AddRange(GetEnemyAttackActions(field));
			actions.Add(new MessageAction($"Taking: {actions.Count} actions"));
			actions.Add(new WaitAction());
			
			return actions;
		}

		private IEnumerable<Action> GetNeutralAttackActions(PlayingField field)
		{
			var myFactories= field.Factories.Where(f => f.IsMine).ToList();
			var neutralFactories = field.Factories.Where(f => f.IsNeutral).ToList();

			List<Action> neutralGainActions = new List<Action>();
			//Filter enemies we are linked to and where we have more nodes
			return neutralFactories.Where(e => e.LinkedNodes.Any(l => l.IsMine)).Select(e => GetBestMoveToNode(e, myFactories, false)).Where(e => e != null).ToList();

		}

		private IEnumerable<Action> GetEnemyAttackActions(PlayingField field)
		{
			var myFactories= field.Factories.Where(f => f.IsMine).ToList();
			var enemies = field.Factories.Where(f => !f.IsNeutral && !f.IsMine).ToList();
		
			//Filter enemies we are linked to and where we have more nodes
			return enemies.Where(e => e.LinkedNodes.Any(l => l.IsMine)).Select(e => GetBestMoveToNode(e, myFactories, true)).Where(e => e != null).ToList();
		}



		//Get the best node to move to
		private Action GetBestMoveToNode(FactoryNode destinationNode, IEnumerable<FactoryNode> myNodes, bool requireUpperHand)
		{
			var myDirectLink = destinationNode.LinkedNodes
				.Where(n => n.IsMine && (!requireUpperHand || n.NrOfCyborgs > destinationNode.NrOfCyborgs))
				.OrderByDescending(n => n.NrOfCyborgs)
				.FirstOrDefault();
			int amountToSend = requireUpperHand ? destinationNode.NrOfCyborgs : (int)Math.Ceiling((myDirectLink?.NrOfCyborgs ?? 0) / 2.0);
			return myDirectLink != null ? GenerateMovementAction(myDirectLink, destinationNode, amountToSend) : null;
		}

		//Generate a movement and substract the amount of troops
		private Action GenerateMovementAction(FactoryNode from, FactoryNode to, int amountOfTroops)
		{
			if(!from.IsMine) {throw new Exception($"Attempting to send from node {from} which is not ours"); }
			if(from.NrOfCyborgs < amountOfTroops) { Log($"!! Sending all or more troops ({amountOfTroops}) than available from {from}"); }
			from.NrOfCyborgs = from.NrOfCyborgs - amountOfTroops;
			return new MoveTroopsAction()
			{
				From = from.NodeIndex,
				To = to.NodeIndex,
				AmountOfTroops = amountOfTroops
			};
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
					factoryToUpdate.OwnedBy = arg1; //arg1: player that owns the factory: 1 for you, -1 for your opponent and 0 if neutral
					factoryToUpdate.NrOfCyborgs = arg2; //arg2: number of cyborgs in the factory
					factoryToUpdate.Production = arg3; //arg3: factory production (between 0 and 3)
				}
				else if(entityType == "TROOP")
				{
					//Log("Found troop data, ignoring for now");
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

		public class MessageAction : Action
		{
			public MessageAction(string msg) { Message = msg; }
			public string Message {get;set;}
			public override string ToString() 
			{
				return $"MSG {Message}";
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