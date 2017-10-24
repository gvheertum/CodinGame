namespace Shared
{
	public class LongLatPosition
	{
		public decimal Longitude {get;set;}
		public decimal Latitude {get;set;}
		public override string ToString()
		{
			return $"long {Longitude} lat {Latitude}";
		}
	}
}