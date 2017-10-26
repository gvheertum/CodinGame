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
			//_runSilent = true; // -> Enable this when running, since the output will slow stuff down severely
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
			var exitNodes = nodes.Where(n => n.IsExitNode).ToList();
			
			

			// game loop
			while (IsRunning())
			{
				int skynetNodeIndex = int.Parse(ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
				Log($"Skynet at position: {skynetNodeIndex}");
				var availableRoutesForSkyNet = GetPossibleRoutesForSkynet(nodeDictionary, nodes, exitNodes, skynetNodeIndex);
				// Write an action using WriteLine()
				// To debug: Error.WriteLine("Debug messages...");
				Log("Found the following routes for skynet:");
				availableRoutesForSkyNet.ForEach(r => Log(r));

				// Example: 0 1 are the indices of the nodes you wish to sever the link between
				var cutOff = MakeSuggestionForCutOff(availableRoutesForSkyNet, nodes);
				Log($"Cutting line: {cutOff}");
				WriteLine($"{cutOff}");

				//Remove the node link after the connection is broken
				RemoveNodeLinks(nodeDictionary, cutOff.Index1, cutOff.Index2);
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

		private List<NodeRoute<SkyNetNode>> GetPossibleRoutesForSkynet(Dictionary<int, SkyNetNode> nodes, List<SkyNetNode> allNodes, List<SkyNetNode> exitNodes, int skynetNodePos)
		{
			var skynetNode = nodes[skynetNodePos];
			var routeHelper = new RouteCalculator<SkyNetNode, SkyNetNode>();
			List<Node<SkyNetNode>> nodesAsSkyNetNode = allNodes.Select(n => n as Node<SkyNetNode>).ToList(); //generics and casting :(
			List<NodeRoute<SkyNetNode>> availableRoutes = new List<NodeRoute<SkyNetNode>>();
			
			//Query the paths to the exit nodes
			exitNodes.ForEach(exitNode => {
				var routes = routeHelper.FilterRoutesOnDestinationReached(routeHelper.CalculateRoutes(nodesAsSkyNetNode, skynetNode, exitNode, 5));
				availableRoutes.AddRange(routes.Select(r => r));
			});
			
			return availableRoutes;
		}
		private class CutOff
		{
			public int Index1 {get;set;}
			public int Index2 {get;set;}

			public override string ToString()
			{
				return $"{Index1} {Index2}";
			}
		}
		private CutOff MakeSuggestionForCutOff(List<NodeRoute<SkyNetNode>> routesForSkyNet, List<SkyNetNode> nodes)
		{
			var routeToBreak = routesForSkyNet.OrderBy(r => r.RouteLength).FirstOrDefault();
			if(routeToBreak == null) { return GetRandomCut(nodes); }

			return new CutOff() 
			{
				Index1 = routeToBreak.NodesToTake.First().NodeIndex,
				Index2 = routeToBreak.NodesToTake.Skip(1).First().NodeIndex
			};
		}

		private CutOff GetRandomCut(List<SkyNetNode> nodes) 
		{
			Log("!! The system wants a cut, but there seems to be no realistic route to the exit node, so server a random route");
			var nodeToBreak = nodes.First(n => n.LinkedNodes.Any());
			return new CutOff()
			{
				Index1 = nodeToBreak.NodeIndex,
				Index2 = nodeToBreak.LinkedNodes.First().NodeIndex
			};
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

		private void RemoveNodeLinks(Dictionary<int, SkyNetNode> nodes, int idx1, int idx2)
		{	
			RemoveNodeFromNode(nodes[idx1], nodes[idx2]);
			RemoveNodeFromNode(nodes[idx2], nodes[idx1]);
		}

		private void RemoveNodeFromNode(SkyNetNode node, SkyNetNode node2)
		{
			node.LinkedNodesIndex.Remove(node2.NodeIndex);
			node.LinkedNodes.Remove(node2);
		}
	}
}