using System.Diagnostics;
using System.IO;

namespace GameFramework
{
	internal class StackInfoDecorater: ALogDecorater
	{
		public StackInfoDecorater(ALogDecorater decorater = null): base(decorater)
		{
			this.FileName = true;
			this.FileLineNumber = true;
		}

		public bool FileName { get; set; }

		public bool FileLineNumber { get; set; }

		public override string Decorate(string message)
		{
			if (this.m_Decorater != null)
			{
				message = this.m_Decorater.Decorate(message);
			}

			if (!this.FileLineNumber && !this.FileName)
			{
				return message;
			}

			string extraInfo = "";
			StackTrace stackTrace = new StackTrace(true);
			StackFrame frame = stackTrace.GetFrame(this.Level + 3);

			if (this.FileName)
			{
				string fileName = Path.GetFileName(frame.GetFileName());
				extraInfo += fileName + " ";
			}
			if (this.FileLineNumber)
			{
				int fileLineNumber = frame.GetFileLineNumber();
				extraInfo += fileLineNumber + " ";
			}
			return extraInfo + message;
		}
	}
}