using System;
using System.Collections.Generic;
using System.Linq;
using Framework;

namespace Puzzles
{
	//https://www.codingame.com/ide/puzzle/telephone-numbers
	public class PhoneNumbersPuzzle : Framework.PuzzleMain
	{
		protected PhoneNumbersPuzzle(IGameEngine gameEngine) : base(gameEngine)
		{
		}

		public static void Main(string[] args)
		{
			new PhoneNumbersPuzzle(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
			int amountOfNrs = int.Parse(ReadLine());
			List<string> numbers = new List<string>();
			for (int i = 0; i < amountOfNrs; i++)
			{
				numbers.Add(ReadLine());
			}

			Log($"Found {numbers.Count} numbers");

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");


			// The number of elements (referencing a number) stored in the structure.
			int calculatedStorage = CalculateStorageSpace(numbers);
			Log($"Storage size: {calculatedStorage}");
			WriteLine($"{calculatedStorage}");
		}

		private int CalculateStorageSpace(List<string> numbers)
		{
			NumberStorageNode rootNode = new NumberStorageNode((o) => Log(o));
			foreach(var n in numbers)
			{
				Log($"Processing: {n}");
				rootNode.ProcessNumber(n);
			}

			return rootNode.GetNodeCount();
		}
	}

	public class NumberStorageNode : LogInjectableClass
	{
		public NumberStorageNode(Action<object> log) : base(log)
		{
		}
		public int? NumberStored {get;set;}
		public int Depth {get; set;}
		public List<NumberStorageNode> SubNodes {get;set;} = new List<NumberStorageNode>();
		public void ProcessNumber(string number)
		{
			if(number.Length == Depth) { return; } //We are matched
			int currChar = Int32.Parse(number[Depth].ToString());	
			if(!SubNodes.Any(n => n.NumberStored == currChar))
			{
				Log($"Creating node: {currChar} under {NumberStored}");
				var node = new NumberStorageNode(_log) { Depth = Depth + 1, NumberStored = currChar };
				SubNodes.Add(node);
			}
			else
			{
				Log($"Node: {currChar} already present under {NumberStored}");
			}
			
			//Handover the processing of the number to our children
			SubNodes.First(s => s.NumberStored == currChar).ProcessNumber(number);
		}

		public int GetNodeCount()
		{
			return SubNodes.Count + SubNodes.Sum(n => n.GetNodeCount()); //Count all my children and let them count theirs
		}
	}
}