//https://www.codingame.com/ide/puzzle/skynet-revolution-episode-1

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.Skynet
{
	public class SkynetPlayer : Shared.PuzzleMain
	{
		protected SkynetPlayer(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		static void Main(string[] args)
		{
			new SkynetPlayer(new CodingGameProxyEngine()).Run();
		}

		public class SkyNetNode : Shared.Node<SkyNetNode>
		{
			public bool IsExitNode { get; set; }
		}

		public override void Run()
		{

			string[] inputs;
			inputs = ReadLine().Split(' ');
			
			//Initialize nodes
			int nrOfNodes = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
			var nodes = CreateNodes(nrOfNodes).ToList();
			var nodeDictionary = CreateNodeDictionary(nodes);
			
			//Link the nodes
			int nrOfLinks = int.Parse(inputs[1]); // the number of links
			LinkNodes(nodeDictionary, nrOfLinks);
			
			//Flag exits
			int nrOfExitGates = int.Parse(inputs[2]); // the number of exit gateways
			FlagExitNodes(nodeDictionary, nrOfExitGates);
			
			
			

			// game loop
			while (IsRunning())
			{
				int SI = int.Parse(ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

				// Write an action using WriteLine()
				// To debug: Error.WriteLine("Debug messages...");


				// Example: 0 1 are the indices of the nodes you wish to sever the link between
				WriteLine("0 1");
			}
		}

		private IEnumerable<SkyNetNode> CreateNodes(int nodeCount)
		{
			for(int i = 0; i < nodeCount; i++)
			{ 
				yield return new SkyNetNode() { NodeIndex = i };
			}
		}

		private Dictionary<int, SkyNetNode> CreateNodeDictionary(List<SkyNetNode> nodes)
		{
			Dictionary<int, SkyNetNode> dict = new Dictionary<int, SkyNetNode>();
			nodes.ForEach(n => dict.Add(n.NodeIndex, n));
			return dict;
		}

		private void LinkNodes(Dictionary<int, SkyNetNode> nodes, int nrOfLinks)
		{
			for (int i = 0; i < nrOfLinks; i++)
			{
				var inputs = ReadLine().Split(' ');
				int nodeId1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
				int nodeId2 = int.Parse(inputs[1]);

				Log($"Linking: {nodeId1} <-> {nodeId2}");
				var node1 = nodes[nodeId1];
				var node2 = nodes[nodeId2];
				node1.LinkedNodes.Add(node2);
				node1.LinkedNodesIndex.Add(node2.NodeIndex);

				node2.LinkedNodes.Add(node1);
				node2.LinkedNodesIndex.Add(node1.NodeIndex);
			}
		}

		private void CalculateRoutesBetweenPoint(SkyNetNode from, SkyNetNode to, int maxSteps)
		{

		}

		private void FlagExitNodes(Dictionary<int, SkyNetNode> nodes, int nrOfExitGates)
		{
			for (int i = 0; i < nrOfExitGates; i++)
			{
				int nodeId = int.Parse(ReadLine()); // the index of a gateway node
				Log($"Node {nodeId} -> Exit");
				nodes[nodeId].IsExitNode = true;
			}
		}
	}
}