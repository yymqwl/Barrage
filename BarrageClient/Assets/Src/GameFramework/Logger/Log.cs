using System;

namespace GameFramework
{
	public static class Log
	{
#if SERVER
        private static readonly ILog globalLog = new ConsoleAdaper();//new NLogAdapter();
#else
        private static readonly ILog globalLog = new UnityLogAdaper();
#endif

        public static void Trace(string message)
		{
			globalLog.Trace(message);
		}

		public static void Warning(string message)
		{
			globalLog.Warning(message);
		}

		public static void Info(string message)
		{
			globalLog.Info(message);
		}

		public static void Debug(string message)
		{
			globalLog.Debug(message);
		}
        public static void Debug(object msg)
        {
            Debug(msg.ToString());
        }
        public static void Error(Exception e)
		{
			globalLog.Error(e.ToString());
		}

		public static void Error(string message)
		{
			globalLog.Error(message);
		}

        public static void Fatal(Exception e)
        {
            globalLog.Fatal(e.ToString());
        }

        public static void Fatal(string message)
        {
            globalLog.Fatal(message);
        }

		public static void Msg(object message)
		{
			Debug(message.ToString());
		}
    }
}
