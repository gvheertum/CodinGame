using System.Collections.Generic;
using System.Linq;

namespace Shared
{

	public class RouteCalculator<TT,T> where TT : Node<T> where T : Node<T>
	{
		public List<NodeRoute<T>> FilterRoutesOnDestinationReached(List<NodeRoute<T>> routesList)
		{
			var nodesReachingDest = routesList.Where(r => r.DestinationReached);
			nodesReachingDest = nodesReachingDest.OrderBy(r => r.RouteLength);
			return nodesReachingDest.ToList();
		}

		public List<NodeRoute<T>> CalculateRoutes(List<Node<T>> nodes, Node<T> from, Node<T> to, int maxIterations)
		{
			var startNode = nodes.First(n => n.LinkedNodesIndex == from.LinkedNodesIndex);
			List<NodeRoute<T>> routeList = new List<NodeRoute<T>>();
			//First calculation based from our starting point, no route available yet, so start there and go through all the nodes through recursive calls
			CalculateRoutesToDestinationFromNodeToNode(routeList, null, startNode, to, maxIterations, 0);
			return routeList;
		}

		private void CalculateRoutesToDestinationFromNodeToNode(List<NodeRoute<T>> routeList, NodeRoute<T> currRoute, Node<T> nodeToEvaluate, Node<T> destination, int maxIterations, int currIteration)
		{
			if(currIteration >= maxIterations) { return; }
			//If we are tracing back (having a node already visited, we skip this route, since it makes no sense to revisit)
			if(currRoute?.NodesToTake?.Any(n => n.NodeIndex == nodeToEvaluate.NodeIndex) == true) { return; }
			
			currRoute = currRoute?.CopyRoute() ?? CreateInitialRoute(nodeToEvaluate, destination);
			currRoute.NodesToTake.Add(nodeToEvaluate); //Add our node to the list of naviation nodes
			currRoute.DestinationReached = destination.NodeIndex == nodeToEvaluate.NodeIndex;
			routeList.Add(currRoute); //This route is added to the list, even if not end point, since it is a route
			
			//We are done, stop executing sub elements
			if(currRoute.DestinationReached) { return; }
		
			//Check if we can reach by using our child nodes, these routes build on our route
			nodeToEvaluate.LinkedNodes.ForEach(n => CalculateRoutesToDestinationFromNodeToNode(routeList, currRoute, n, destination, maxIterations, currIteration +1));
		}

		private NodeRoute<T> CreateInitialRoute(Node<T> node, Node<T> endNode)
		{
			return new NodeRoute<T>()
			{
				DestinationReached = false,
				NodeStart = node,
				NodeEnd = endNode,
			};
		}
	}

}