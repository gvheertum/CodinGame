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
		//Suppress all console output (usefull for performance gain in large games)
		//This will also suppress IO logic since the whole logging is disabled
		protected bool _runSilent = false;
		
		//Option to supress the output of default IO (use this when a lot of IO is happening or when a echoing differently)
		//The default Log will still work in this case
		protected bool _echoDefaultSystemIO = false;
		private IGameEngine _gameEngine;
		protected PuzzleMain(IGameEngine gameEngine)
		{
			if(gameEngine == null) { Log($"No game-engine set, using the CodingGameProxy"); }
			_gameEngine = gameEngine ?? new CodingGameProxyEngine();
			Log($"Running with game engine: {_gameEngine}");
		}

		//Initializer for sub elements working on the puzzle root
		protected PuzzleMain(PuzzleMain puzzleMain) : this(puzzleMain._gameEngine)
		{
			this._runSilent = puzzleMain._runSilent;
			this._echoDefaultSystemIO = puzzleMain._echoDefaultSystemIO;
		}

		public virtual void Run() { Log("No run defined, this should not happen"); }

		//Exposes the running flag of the game engine
		protected bool IsRunning() { return _gameEngine.IsRunning(); }

		private List<string> _inputtedData = new List<string>();
		protected string ReadLine() 
		{
			var line = _gameEngine.ReadLine();
			_inputtedData.Add(line);
			if(_echoDefaultSystemIO) { Log($"ReadLine: {line}"); }
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
			if(_echoDefaultSystemIO) { Log($"WriteLine: {line}"); }
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

		public void Log(object obj)
		{
			if(!_runSilent) //If silent, no logging is performed
			{
				Console.Error.WriteLine(obj);
			}
		}
	}
}