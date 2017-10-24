using System;

namespace Shared
{
	public class DistanceCalculator
	{
		public decimal GetDistance(Position pos1, Position pos2)
		{
			throw new NotImplementedException("Not here yet!");
		}

		public decimal GetDistance(LongLatPosition pos1, LongLatPosition pos2)
		{
			var x = (double)(pos2.Longitude - pos1.Longitude) * Math.Cos(((double)(pos1.Latitude + pos2.Latitude) / 2d));
			var y = (double)(pos2.Latitude - pos1.Latitude);
			var d = Math.Sqrt((x*x) + (y*y)) * 6371;
			return (decimal)d;
		}
	}
}