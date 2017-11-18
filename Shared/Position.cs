namespace Shared
{
	public class Position
	{
		public Position() {}
		public Position(int x, int y) { X = x; Y = y; }
		
		public int X {get;set;}
		public int Y {get;set;}
		public int PositionIndex { get; set; }
	
		public double DistanceTo(Position p, int correction = 0) //Correction can be used to offset negative values (when working on circles or non 0,0 grids)
		{
			var xDiff = System.Math.Abs((p.X + correction) - (X + correction));
			var yDiff = System.Math.Abs((p.Y + correction) - (Y + correction));
			return System.Math.Sqrt((xDiff*xDiff) + (yDiff - yDiff));
		}

		public override string ToString()
		{
			return $"Pos[{PositionIndex}] = x:{X} y:{Y}";
		}
		
		public Position GetCopy()
		{
			return new Position() { X = X, Y = Y, PositionIndex = PositionIndex };
		}
		
		public string GetAsJumpPosition()
		{
			return $"{X} {Y}";
		}
	}
}