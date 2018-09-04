/* oOo * 11/14/2007 : 10:22 PM */
using System.Diagnostics;

namespace System
{
	static public class Logger
	{
		static public ConsoleColor DefaultForeground = ConsoleColor.White;
		
		[Conditional("CONSOLE")]
		static public void LogY(string title, string filter, params object[] message)
		{
      Error(MessageType.Yellow,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void LogC(string title, string filter, params object[] message)
		{
      Error(MessageType.Cyan,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void LogG(string title, string filter, params object[] message)
		{
      Error(MessageType.Green,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void LogM(string title, string filter, params object[] message)
		{
      Error(MessageType.Magenta,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void Warn(string title, string filter, params object[] message)
		{
      Error(MessageType.Error,title,filter,message);
		}

    /// <summary>
    /// Write to stderr.  
    /// Note that (object) type can be `ConsoleColor`, `MessageType` or `logT` (all the same).
    /// </summary>
    [Conditional("CONSOLE")]
    static public void Error(object type, string title, string filter, params object[] message)
    {
      ConsoleColor cc = Console.ForegroundColor;
      Console.ForegroundColor = (ConsoleColor)type;
      Console.Error.Write("{0}: ",title);
      Console.ForegroundColor = DefaultForeground;
      Console.Error.Write(filter,message);
      Console.Error.Write("\n");
      Console.ForegroundColor = cc;
    }
    /// <summary>
    /// Write to stderr.  
    /// Note that (object) type can be `ConsoleColor`, `MessageType` or `logT` (all the same).
    /// </summary>
		[Conditional("CONSOLE")]
		static public void Log(object type, string title, string filter, params object[] message)
		{
			ConsoleColor cc = Console.ForegroundColor;
      Console.ForegroundColor = (ConsoleColor)type;
			Console.Write("{0}: ",title);
			Console.ForegroundColor = DefaultForeground;
			Console.Write(filter,message);
			Console.Write("\n");
			Console.ForegroundColor = cc;
		}
	}
}
