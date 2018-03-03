namespace Shared
{
	public interface IPosition
	{
		int X {get;set;}
		int Y {get;set;}
	}

	public static class PositionExtensions
	{
		public static double DistanceTo(this IPosition p, IPosition p2) 
		{
			var xDiff = p.X - p2.X;
			var yDiff = p.Y - p2.Y;
			return System.Math.Sqrt((xDiff*xDiff) + (yDiff*yDiff));
		}

		public static void Flip(this IPosition p)
		{
			p.X = -p.X;
			p.Y = -p.Y;
		}
	}

	public class Position : IPosition
	{
		public Position() {}
		public Position(int x, int y) { X = x; Y = y; }
		
		public int X {get;set;}
		public int Y {get;set;}
		public int PositionIndex { get; set; }
	
		// public double DistanceTo(Position p) 
		// {
		// 	var xDiff = p.X - X;
		// 	var yDiff = p.Y - Y;
		// 	return System.Math.Sqrt((xDiff*xDiff) + (yDiff*yDiff));
		// }

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