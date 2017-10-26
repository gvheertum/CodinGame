using System.Collections.Generic;
using System.Linq;

namespace Shared
{
	public class NodeRoute<T> where T : Node<T>
	{
		public List<Node<T>> NodesToTake { get; set; } = new List<Node<T>>();
		public bool DestinationReached {get;set;}
		public int? RouteLength { get { return NodesToTake.Count; } }
		public Node<T> NodeStart { get; set; }
		public Node<T> NodeEnd { get; set; }
		public NodeRoute<T> CopyRoute() 
		{
			return new NodeRoute<T>() 
			{ 
				NodesToTake = NodesToTake.Select(t => t).ToList(),
				DestinationReached = DestinationReached,
				NodeStart = NodeStart,
				NodeEnd = NodeEnd
			};
		}
	}
}