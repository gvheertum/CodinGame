using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Shared
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
		
		public void WriteLine(string resp)
		{
			//Response is ignored
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