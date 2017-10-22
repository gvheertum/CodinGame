using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;

//https://www.codingame.com/ide/puzzle/there-is-no-spoon-episode-1

/**
 * Don't let the machines win. You are humanity's last hope...


The game is played on a rectangular grid with a given size. Some cells contain power nodes. The rest of the cells are empty.

The goal is to find, when they exist, the horizontal and vertical neighbors of each node.
 	Rules

To do this, you must find each (x1,y1) coordinates containing a node, and display the (x2,y2) coordinates of the next node to the right, and the (x3,y3) coordinates of the next node to the bottom within the grid.

If a neighbor does not exist, you must output the coordinates -1 -1 instead of (x2,y2) and/or (x3,y3).

You lose if:
You give an incorrect neighbor for a node.
You give the neighbors for an empty cell.
You compute the same node twice.
You forget to compute the neighbors of a node.


 **/
namespace Puzzles.ThereIsNoSpoon
{
	public class Node
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Node Neigbor1 { get; set; }
		public Node Neigbor2 { get; set; }

		//Echo or full details string, our coords and our neighbor coords go here
		public string GetDetails()
		{
			return $"{GetCoord()} {(Neigbor1 ?? new NotANode()).GetCoord()} {(Neigbor2 ?? new NotANode()).GetCoord()}";
		}

		//Get the coord string of the element
		public virtual string GetCoord()
		{
			return $"{X} {Y}";
		}

		public override string ToString()
		{
			return $"[Node {GetCoord()} (n1: {(Neigbor1 ?? new NotANode()).GetCoord()}, n2: {(Neigbor2 ?? new NotANode()).GetCoord()})]";
		}
	}

	public class NotANode : Node
	{
		public override string GetCoord()
		{
			return "-1 -1"; 
		}

		public override string ToString()
		{
			return "NOT A NODE";
		}
	}

	public class Player : PuzzleMain
	{
		public Player(IGameEngine engine) : base(engine) {}
		public static void Main(string[] args)
		{
			new Player(null).Run();
		}

		public override void Run()
		{
			List<string> fieldLines = new List<string>();

			int width = int.Parse(ReadLine()); // the number of cells on the X axis
			int height = int.Parse(ReadLine()); // the number of cells on the Y axis
			for (int i = 0; i < height; i++)
			{
				string line = ReadLine(); // width characters, each either 0 or .
				fieldLines.Add(line);	
			}

			Log($"Field h: {height} w: {width}");
			//Parse the nodes from the collection (note they are not yet linked)
			var nodeCollection = GetNodesFromInput(fieldLines);
			var flatNodes = nodeCollection.SelectMany(m => m);
			Log($"Found {flatNodes.Count()} nodes");

			//Start linking the nodes
			LinkNeigbors(flatNodes);

			// Three coordinates: a node, its right neighbor, its bottom neighbor
			
			//Get the coordinates of all nodes and echo them.
			var respStrings = flatNodes.Select(n => n.GetDetails()).ToList();
			Log($"Going to echo {respStrings.Count} response nodes");
			respStrings.ForEach(r => 
			{ 
				WriteLine(r);
			});
		}
		
		private List<List<Node>> GetNodesFromInput(List<string> lines)
		{
			Log($"Processing {lines.Count} lines");
			List<List<Node>> listList = new List<List<Node>>();
			for(int i = 0; i < lines.Count; i++) 
			{
				listList.Add(GetNodesFromLine(lines[i], i));
			}
			return listList;
		}

		private List<Node> GetNodesFromLine(string line, int lineIndex)
		{
			Log($"Line[{lineIndex}]: {line}");
			List<Node> nodes = new List<Node>();
			for(int i = 0; i < line.Length; i++)
			{
				if(line[i] == '0') 
				{ 
					var node = new Node() { X = i, Y = lineIndex }; 
					Log($"Found node: {node}");
					nodes.Add(node);
				}
			}
			Log($"Found {nodes.Count}");
			return nodes;
		}

		private void LinkNeigbors(IEnumerable<Node> nodes)
		{
			//It would be nice to have new items, immutability and such, but meh
			nodes.ToList().ForEach(n => 
			{
				FindNeighborsForNode(n, nodes);
			});
		}

		private void FindNeighborsForNode(Node node, IEnumerable<Node> nodes)
		{
			if(node is NotANode) {return; } //Skip void nodes

			node.Neigbor1 = FindRightNeigbor(node, nodes);
			node.Neigbor2 = FindBottomNeighbor(node, nodes);
		}

		private Node FindRightNeigbor(Node node, IEnumerable<Node> nodes)
		{
			return nodes.OrderBy(n => n.X).FirstOrDefault(n => n.X > node.X && n.Y == node.Y) ?? new NotANode();
		}

		private Node FindBottomNeighbor(Node node, IEnumerable<Node> nodes)
		{
			return nodes.OrderBy(n => n.Y).FirstOrDefault(n => n.Y > node.Y && n.X == node.X) ?? new NotANode();		
		}
	}
}