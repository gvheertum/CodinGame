//require: Node.cs
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;

//https://www.codingame.com/ide/puzzle/the-great-dispatch
namespace Challenges.TheGreatDispatch
{
	public class Box
	{
		public float Volume {get;set;}
		public float Weight {get;set;}
		//Received order
		public int Order { get;set;}
		//Truck to load the element in
		public int TruckID {get;set;}
		public override string ToString()
		{
			return $"[Box {Order}] w: {Weight} v: {Volume}";
		}
	}

	public class Truck
	{
		public int TruckID {get;set;}
		public float TotalVolumeLoaded {get; private set;}
		public float TotalWeightLoaded {get; private set;}
		public float AvailableVolume {get; private set;} = 100;
		public int NrOfBoxes { get { return Boxes.Count(); } }
		private List<Box> Boxes {get;set;} = new List<Box>();
		public void AddBox(Box box)
		{
			box.TruckID = TruckID;
			TotalVolumeLoaded += box.Volume;
			TotalWeightLoaded += box.Weight;
			AvailableVolume -= box.Volume;
			Boxes.Add(box);
		}

		public override string ToString()
		{
			return $"[Truck {TruckID}] bc: {NrOfBoxes} w: {TotalWeightLoaded} v: {TotalVolumeLoaded} (avail: {AvailableVolume})";
		}
	}

	public class DispatchPlayer : PuzzleMain
	{

		//public const int MaxWeightPerTruck = 100;
		public const int MaxAmountOfTrucks = 100;

		protected DispatchPlayer(IGameEngine gameEngine) : base(gameEngine) { }

		public static void Main(string[] args)
		{
			new DispatchPlayer(new Framework.CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{   
			int boxCount = int.Parse(ReadLine());
			List<Box> boxes = new List<Box>();
			float totalWeight = 0;
			float totalVolume = 0;
			for (int i = 0; i < boxCount; i++)
			{
				string[] inputs = ReadLine().Split(' ');
				float weight = float.Parse(inputs[0]);
				float volume = float.Parse(inputs[1]);
				boxes.Add(new Box() { Weight = weight, Volume = volume, Order = i });
				totalVolume += volume;
				totalWeight += weight;
			}
			Log($"Totals: boxes-> {boxCount} volume-> {totalVolume} weight-> {totalWeight}");

			//Calculate solution
			var trucks = GetTrucks(boxes);
			Log($"Decided to dispatch {trucks.Count} trucks");
			PutBoxesInTrucks(trucks, boxes);

			LogTruckStats(trucks);

			string boxOutput = GetDispatchString(boxes);
			WriteLine(boxOutput);
		}

		private List<Truck> GetTrucks(List<Box> boxes)
		{
			int nrOfTrucks = 100;
			List<Truck> trucks = new List<Truck>();
			for(int i = 0; i < nrOfTrucks; i++)
			{
				trucks.Add(new Truck() { TruckID = i });
			}
			return trucks;
		}

		private void PutBoxesInTrucks(List<Truck> trucks, List<Box> boxes)
		{
			//Just put them in a round robin way
			List<Truck> trucksToFill = trucks.Select(b => b).ToList();
			boxes.OrderByDescending(b => b.Weight).ToList().ForEach(b => //Start with the biggest boxes
			{
				var truckToFill = trucksToFill.FirstOrDefault(t => t.AvailableVolume >= b.Volume);
				if(truckToFill == null) 
				{
					throw new Exception($"Cannot find a truck for weight: {b.Volume}");
				}
				//Log($"{b} -> {truckToFill}");
				truckToFill.AddBox(b);
			});
		}

		private string GetDispatchString(List<Box> boxes)
		{
			return string.Join(" ", boxes.Select(b => b.TruckID));
		}

		private void LogTruckStats(List<Truck> trucks)
		{
			trucks.ForEach(truck => 
			{
				Log(truck);
			});
		}
	}
}