using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
	//Game engine to run in the coding game scope, this will forward the read/write lines to the engine of CodinGame
	public class CodingGameProxyEngine : IGameEngine
	{
		public void WriteLine(string resp)
		{
			Console.WriteLine(resp);
		}

		public string ReadLine()
		{
			return Console.ReadLine();
		}

		public bool IsRunning()
		{
			return true; //CodinGame always keeps running and kills our project when the end criteria is met
		}
	}
}