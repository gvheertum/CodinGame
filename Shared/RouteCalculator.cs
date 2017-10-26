using System.Collections.Generic;
using System.Linq;

namespace Shared
{

	public class RouteCalculator<TT,T> where TT : Node<T> where T : Node<T>
	{
		public List<NodeRoute<T>> CalculateRoutes(List<Node<T>> nodes, Node<T> from, Node<T> to, int maxIterations)
		{
			var startNode = nodes.First(n => n.LinkedNodesIndex == from.LinkedNodesIndex);
			List<NodeRoute<T>> routeList = new List<NodeRoute<T>>();
			from.LinkedNodes.ForEach(n=> NavigateFromNodeToNode(routeList, null, n, to, maxIterations, 0));
			return routeList;
		}

		private void NavigateFromNodeToNode(List<NodeRoute<T>> routeList, NodeRoute<T> currRoute, Node<T> currNode, Node<T> to, int maxIterations, int currIteration)
		{
			if(currIteration >= maxIterations) { return; }
			
			currRoute = currRoute?.CopyRoute() ?? CreateInitialRoute(currNode);
			currRoute.NodesToTake.Add(currNode); //Add our node to the list of naviation nodes
			currRoute.DestinationReached = to.NodeIndex == currNode.NodeIndex;
			routeList.Add(currRoute);
			
			//We are done, stop executing sub elements
			if(currRoute.DestinationReached) { return; }
		
			//Check if we can reach by using our child nodes
			currNode.LinkedNodes.ForEach(n => NavigateFromNodeToNode(routeList, currRoute, n, to, maxIterations, currIteration +1));
		}

		private NodeRoute<T> CreateInitialRoute(Node<T> node)
		{
			return new NodeRoute<T>()
			{
				DestinationReached = false,
				NodeStart = node,
				NodeEnd = node,
			};
		}
	}

}