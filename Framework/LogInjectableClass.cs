using System;

namespace Framework
{
	public abstract class LogInjectableClass
	{
		private Action<object> _log;
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