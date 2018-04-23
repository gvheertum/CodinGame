using System.Collections.Generic;

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

		public static PositionDistance<T> DistanceToDetailed<T>(this T position, T p2)
			where T : IPosition
		{
			return new PositionDistance<T>()
			{
				Origin = position,
				Destination = p2,
				Distance = DistanceTo(position, p2)
			};
		}

		public static IEnumerable<PositionDistance<T>> DistanceToDetailed<T>(this T origin, IEnumerable<T> destinations)
			where T : IPosition
		{
			foreach(var dest in destinations)
			{
				yield return DistanceToDetailed(origin, dest);
			}
		}

		public static void Flip(this IPosition p)
		{
			p.X = -p.X;
			p.Y = -p.Y;
		}
	}

	public class PositionDistance<T> where T : IPosition
	{
		public T Origin {get;set;}
		public T Destination {get;set;}
		public double Distance { get;set;}
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