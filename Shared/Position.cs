namespace Shared
{
	public class Position
	{
		public Position() {}
		public Position(int x, int y) { X = x; Y = y; }
		
		public int X {get;set;}
		public int Y {get;set;}
		public int PositionIndex { get; set; }
	
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