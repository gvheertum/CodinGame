using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Shared
{
	//Game engine to link into the puzzles. The gameengine determines the I/O for the puzzle.
	public interface IGameEngine
	{
		//Read an input line from the engine
		string ReadLine();
		//Write an input line to the engine
		void WriteLine(string resp);
		//Whether the engine is still running. This is only relevant for custom engines to prevent endless loops
		bool IsRunning();
	}
}