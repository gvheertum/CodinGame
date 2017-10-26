using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Shared
{
	//Shared base for the puzzles. This allows us to have a simpler working of the game objects with simplified shorthands.
	// Reading: Console.ReadLine -> ReadLine
	// Writing: Console.WriteLine -> WriteLine
	// Logging: Console.Error.Log -> Log
	public abstract class PuzzleMain
	{
		protected bool _runSilent = false;
		private IGameEngine _gameEngine;
		protected PuzzleMain(IGameEngine gameEngine)
		{
			if(gameEngine == null) { Log($"No game-engine set, using the CodingGameProxy"); }
			_gameEngine = gameEngine ?? new CodingGameProxyEngine();
			Log($"Running with game engine: {_gameEngine}");
		}

		public abstract void Run();

		//Exposes the running flag of the game engine
		protected bool IsRunning() { return _gameEngine.IsRunning(); }

		private List<string> _inputtedData = new List<string>();
		protected string ReadLine() 
		{
			var line = _gameEngine.ReadLine();
			_inputtedData.Add(line);
			Log($"ReadLine: {line}");
			return line;
		}

		//Flush the full set of inputted data to the logger
		public void EchoInputtedData()
		{
			Log("** [Received] **");
			_inputtedData.ForEach(i => Log(i));
			Log("** [/Received] **");
		}

		private List<string> _responseData = new List<string>();
		protected void WriteLine(string line)
		{
			Log($"WriteLine: {line}");
			_responseData.Add(line);
			_gameEngine.WriteLine(line);
		}

		//Flush the complete log of response data to the output
		public void EchoResponseData()
		{
			Log("** [Sent] **");
			_responseData.ForEach(i => Log(i));
			Log("** [/Sent] **");
		}

		protected void Log(object obj)
		{
			if(!_runSilent) 
			{
				Console.Error.WriteLine(obj);
			}
		}
	}
}