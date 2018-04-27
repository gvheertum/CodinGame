using System;

namespace Framework
{
	public abstract class LogInjectableClass
	{
		protected readonly Action<object> _log;
		public LogInjectableClass(Action<object> log)
		{
			_log = log;
		}
		protected virtual void Log(object o) 
		{
			if(_log != null) { _log(o); }
		}	
	}
}