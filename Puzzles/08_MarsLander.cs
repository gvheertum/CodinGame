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
namespace Puzzles.MarsLander
{
	public class LanderConditions
	{
		public LanderConditions(LanderData data)
		{
			Angle = data.Rotation == 0; //Needs to be in upright position
			VSpeed = Math.Abs(data.VSpeed) <= 15; //20 is max, 15 is safe
			HSpeed = Math.Abs(data.HSpeed) <= 15; //20 is max, 15 is safe
		}

		public bool Angle { get; private set; }
		public bool HSpeed { get; private set; }
		public bool VSpeed { get; private set; }

		public bool AllClear 
		{
			get { return Angle && HSpeed && VSpeed; }
		}
	}

	

	public class LanderData : Position
	{
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

	public class Player : PuzzleMain
	{
		protected Player(IGameEngine gameEngine) : base(gameEngine) { }

		static void Main(string[] args)
		{
			var self = new Player(new CodingGameProxyEngine());
			self.Run();
		}

		public override void Run() 
		{
			var mapPos = GetMapPoints();
			Log($"Got {mapPos.Count} map positions");
			
			LanderData landerData = null; 
			LanderConditions conditions = null;
			// game loop
			while (true)
			{
				landerData = GetLanderDataFromString(ReadLine());
				conditions = new LanderConditions(landerData);
				//We can take the height into consideration to see when we need to power up again, but for now simple dumb landing pattern
				int powerPerc = conditions.VSpeed ? 0 : 4; //If our Vspeed is okay we disable truster, otherwise we start it back up
				WriteLine(GetAction(0,powerPerc));
			}
		}

		
		private string GetAction(int rotatePower, int thrustPower)
		{
			// 2 integers: rotate power. rotate is the desired rotation angle (should be 0 for level 1), power is the desired thrust power (0 to 4).
			Log($"Peforming-> rot {rotatePower} thrust {thrustPower}");
			return $"{rotatePower} {thrustPower}";
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
			int surfaceN = int.Parse(ReadLine()); // the number of points used to draw the surface of Mars.
			for (int i = 0; i < surfaceN; i++)
			{
				inputs = ReadLine().Split(' ');
				int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
				int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
				positions.Add(new Position() { X = landX, Y = landY, PositionIndex = i});
			}
			return positions;
		}

	}
}