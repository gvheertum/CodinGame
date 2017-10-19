using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
	public class BitHelper
	{
		private int _bitCount;
		public BitHelper(int bitCount)
		{
			_bitCount = bitCount;
		}		

		public string GetBitStringForValue(int number)
		{
			int[] range = GetBitRange();
			var sb = new StringBuilder();
			var currVal = number;
			for(int i = 0; i < range.Length; i++)
			{
				var currBitVal = range[i];
				if(currVal >= currBitVal) //Can we assign a part of this bit?
				{
					currVal -= currBitVal;
					sb.Append("1");
				}
				else
				{
					sb.Append("0");
				}
			}
			if(currVal > 0) { throw new Exception($"{number} left {currVal} after convert, expected 0"); }
			return sb.ToString();
		}

		private int[] GetBitRange()
		{
			var val = 1;
			List<int> values = new List<int>();
			for(var i = 0; i < _bitCount; i++)
			{
				values.Add(val);
				val = val * 2;
			}
			values.Reverse();
			return values.ToArray();
		}
	}
}