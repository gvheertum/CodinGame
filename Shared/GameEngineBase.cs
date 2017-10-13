using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Shared
{
	public class GameEngineBase
	{
		protected void Log(object obj)
		{
			Console.WriteLine($"Engine-Log: {obj}");
		}
	}
}