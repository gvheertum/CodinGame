using System.Collections.Generic;

namespace Shared
{
	public abstract class Node<T> where T : Node<T>
	{
		public int NodeIndex { get; set; }
		public List<T> LinkedNodes { get; set; } = new List<T>();
		public List<int> LinkedNodesIndex { get; set; } = new List<int>();
	}

	public class Node : Node<Node>
	{

	}
}