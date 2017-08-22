/* oOo * 11/14/2007 : 10:22 PM */
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace System
{

	static public class Logger
	{
		static public ConsoleColor DefaultForeground = ConsoleColor.White;
		
		[Conditional("CONSOLE")]
		static public void LogY(string title, string filter, params object[] message)
		{
			Log(MessageType.Yellow,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void LogC(string title, string filter, params object[] message)
		{
			Log(MessageType.Cyan,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void LogG(string title, string filter, params object[] message)
		{
			Log(MessageType.Green,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void LogM(string title, string filter, params object[] message)
		{
			Log(MessageType.Magenta,title,filter,message);
		}
		[Conditional("CONSOLE")]
		static public void Warn(string title, string filter, params object[] message)
		{
			Log(MessageType.Error,title,filter,message);
		}
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
