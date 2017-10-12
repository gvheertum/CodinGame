using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.MarsLander
{
	public class Position
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int PositionIndex { get; set; }
		public override string ToString()
		{
			return $"Pos[{PositionIndex}] = x:{X} y:{Y}";
		}
	}

	public class LanderData
	{
		public int X { get; set; } //X pos of vesel
		public int Y { get; set; } //Y pos of vesel
		public int HSpeed { get; set; } // the horizontal speed (in m/s), can be negative.
		public int VSpeed { get; set; } // the vertical speed (in m/s), can be negative.
		public int Fuel { get; set; }// the quantity of remaining fuel in liters.
		public int Rotation { get; set; } // the rotation angle in degrees (-90 to 90).
		public int Power  { get; set; }// the thrust power (0 to 4). 
		public override string ToString()
		{
			return $"x: {X}, y: {Y}, HSpeed: {HSpeed}, VSpeed: {VSpeed}, Fuel: {Fuel}, Rotation: {Rotation}, Power: {Power}";
		}
	}

	public class Player
	{
		static void Main(string[] args)
		{
			var self = new Player();
			self.Run();
		}

		private void Run() 
		{
			var mapPos = GetMapPoints();
			Log($"Got {mapPos.Count} map positions");
			
			LanderData landerData = null; 
			// game loop
			while (true)
			{
				landerData = GetLanderDataFromString(ReadInput());

				// Write an action using Console.WriteLine()
				// To debug: Console.Error.WriteLine("Debug messages...");


				// 2 integers: rotate power. rotate is the desired rotation angle (should be 0 for level 1), power is the desired thrust power (0 to 4).
				Console.WriteLine("0 3");
			}
		}

		private LanderData GetLanderDataFromString(string input)
		{
			var inputs = input.Split(' ');
				
			var data = new LanderData() 
			{	
				X = int.Parse(inputs[0]),
				Y = int.Parse(inputs[1]),
				HSpeed = int.Parse(inputs[2]), // the horizontal speed (in m/s), can be negative.
				VSpeed = int.Parse(inputs[3]), // the vertical speed (in m/s), can be negative.
				Fuel = int.Parse(inputs[4]), // the quantity of remaining fuel in liters.
				Rotation = int.Parse(inputs[5]), // the rotation angle in degrees (-90 to 90).
				Power = int.Parse(inputs[6]) // the thrust power (0 to 4).
			};
			Log($"Ship data: {data}");
			return data;
		}

	

		public List<Position> GetMapPoints() 
		{
			List<Position> positions = new List<Position>();
			string[] inputs;
			int surfaceN = int.Parse(ReadInput()); // the number of points used to draw the surface of Mars.
			for (int i = 0; i < surfaceN; i++)
			{
				inputs = ReadInput().Split(' ');
				int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
				int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
				positions.Add(new Position() { X = landX, Y = landY, PositionIndex = i});
			}
			return positions;
		}

		List<string> readLines = new List<string>();
		private string ReadInput() //Allows us to override for manual input
		{
			var line = Console.ReadLine();
			Log($">{line}");
			readLines.Add(line);
			return line;
		}

		private void LogAllInputStrings()
		{
			Log("Inputted data:");
			readLines.ForEach(l => Log(l));
		}

		private void Log(object obj)
		{
			Console.Error.WriteLine(obj);
		}
	}
}