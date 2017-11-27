using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;
namespace Helpers.CustomGameEngines
{
	//Game engine based on a simple string buffer
	public class StringBufferGameEngine : IGameEngine
	{
		private List<string> _buffer = null;
		private int _bufferIdx = 0;
		public StringBufferGameEngine(IEnumerable<string> buffer) 
		{ 
			_buffer = buffer.ToList();
		}
		
		private List<string> _writtenLines = new List<string>();
		public void WriteLine(string resp)
		{
			_writtenLines.Add(resp);
		}

		//Get a list of responses done to this engine
		public List<string> ReadBackWrittenLines()
		{
			return _writtenLines;
		}

		public string ReadLine()
		{
			if(_bufferIdx >= _buffer.Count) { return null; }
			var item = _buffer[_bufferIdx];
			_bufferIdx++;
			return item;
		}

		public bool IsRunning()
		{
			return _buffer.Count < _bufferIdx; //While items in the buffer, we are considered running
		}
	}
}