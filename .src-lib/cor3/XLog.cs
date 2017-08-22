/* oOo * 11/14/2007 : 10:22 PM */
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace System
{
	/// <summary>
	/// This is a wrapper around the <see cref="XLog" /> class
	/// which automates the process of attaching an event-handler
	/// to a <tt>using</tt> block so that the event-handler
	/// is automatically removed when execution steps 
	/// out of the particuar <tt>using</tt> block.
	/// </summary>
	public class XLogController : IDisposable
	{
		/// <summary>
		/// By default, is set to <tt><see cref="XLog.OutputMode" />.Console</tt>.
		/// </summary>
		static public XLog.OutputMode OutputMode = XLog.OutputMode.Console;
		/// <summary>
		/// This event handler is assigned via the constructor.
		/// </summary>
		EventHandler<XLogEventArgs> classHandler = null;
		/// <summary>
		/// The only Constructor—requires xlogHandler event.
		/// </summary>
		/// <param name="outputMode">See <see cref="XLog.OutputMode" />.</param>
		/// <param name="xlogHandler">See <see cref="EventHandler{XLogEventArgs}" />.</param>
		public XLogController(XLog.OutputMode outputMode, EventHandler<XLogEventArgs> xlogHandler)
		{
			if (xlogHandler!=null) XLog.XLogEvent += xlogHandler;
		}
		/// <summary>
		/// Disposal clears the associated (<see cref="EventHandler{XLogEventArgs}" />)
		/// <strong>classHandler</strong> <tt>EventHandler</tt>.
		/// </summary>
		void IDisposable.Dispose()
		{
			if (classHandler!=null) XLog.XLogEvent -= classHandler;
		}
		/// <summary>
		/// Default Print operation.
		/// <para>indirect call to <see cref="Print(string,object[])">main overload</see>.</para>
		/// </summary>
		/// <param name="output"></param>
		public void Print(object output) { Print("{0}",output); }
		public void Print(string format, params object[] output)
		{
			if (OutputMode== XLog.OutputMode.Console) Console.WriteLine(format,output);
			else if (OutputMode== XLog.OutputMode.Debugger) System.Diagnostics.Debug.Print(format,output);
			else XLog.WriteLine(format,output);
		}
	}
	/// <summary>
	/// XLogEventHandler is a handler you may connect to
	/// your applications for writing to the console.
	/// <br/>
	/// By default the <see cref="XLog" /> will always
	/// write to the console.
	/// </summary>
	public delegate void XLogEventHandler(XLogEventArgs xlogObj);
	/// <summary>
	/// Cor3.Lib has gone through various Logger classes, and <tt>XLog</tt>
	/// is the current Logger that's been used.
	/// <hr />
	/// Depreceated or <tt>Obsolete</tt> logger classes now include:
	/// <tt>Log</tt> and <tt>Logger</tt>.  Both of which may still be
	/// in use by some other old classes and tools—which is why they remain.
	/// </summary>
	public class XLog
	{
		/// <summary>
		/// OutputMode is also dependant upon the Compile-Time
		/// <tt>#define</tt> pragma or compiler-level defines.
		/// <h3>References</h3>
		/// <see cref="IsConsoleEnabled" />
		/// </summary>
		public enum OutputMode
		{
			/// <summary>
			/// Output is directed to <see cref="System.Diagnostics.Debug.Print(string,object[])" /> handlers.
			/// </summary>
			Debugger,
			/// <summary>
			/// Output is directed to <see cref="System.Console.WriteLine(string,object[])" />.
			/// </summary>
			Console,
			/// <summary>
			/// Output is handled by an event.
			/// </summary>
			XLog
		}
		/// <summary>
		/// If the application is compiled with compilation flags 'CONSOLE'
		/// then this value is set to true, otherwise if there is an error
		/// this value will automatically be set to false.
		/// <hr />
		/// <h3>The following is no longer applicable of 2012-04-18</h3>
		/// <div style='background: #AAA; padding: 4em;'>
		/// <para>by default we set this value to true.</para>
		/// <para>
		/// if there is an exception attempting to write to the console,
		/// an error sets this value to false, and the console
		/// is no longer referenced leaving the event handler in place.
		/// </para>
		/// </div>
		/// </summary>
		#if CONSOLE
		static public Boolean IsConsoleEnabled = true;
		#else
		static public Boolean IsConsoleEnabled = false;
		#endif
		static public event EventHandler<XLogEventArgs> XLogEvent;
		
		private static void OnXLogEvent()
		{
			if (XLogEvent != null) {
				XLogEvent(null,new XLogEventArgs(true));
			}
		}
		private static void OnXLogEvent(ConsoleColor Colour, string title, string format, params object[] args)
		{
			if (XLogEvent != null) {
				XLogEvent(null,new XLogEventArgs(Colour,title,format,args));
			}
		}
		private static void OnXLogEvent(ConsoleColor Colour, string title, string titleFilter, string format, params object[] args)
		{
			if (XLogEvent != null) {
				XLogEvent(null,new XLogEventArgs(Colour,title,titleFilter,format,args));
			}
		}
		
		public static void WarnError(string filter, params object[] args)
		{
			Write(ConsoleColor.Red,"{0}","Error",filter,args);
		}
		
		public static void Warn(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.Red,"{0}",title,filter,args);
		}
		
		public static void Write(ConsoleColor titleFg, string title, string filter, params object[] args)
		{
			Write(titleFg,"{0}",title,filter,args);
		}
		
		public static void Clear()
		{
			try
			{
				if (IsConsoleEnabled) Console.Clear();
			} catch { IsConsoleEnabled = false; }
			
			OnXLogEvent();
		}
		
		public static void WriteLine()
		{
			WriteLine("\n");
		}
		//		public static void WriteLine(string text)
		//		{
		//			WriteLine(text);
		//		}
		public static void WriteLine(string filter, params object[] args)
		{
			Write(string.Concat(filter,"\n"),args);
		}
		
		public static void Write(string filter, params object[] args)
		{
			Write(ConsoleColor.White,"",filter,args);
		}
		
		public static void Write(ConsoleColor titleFg, string titleFilter, string title, string filter, params object[] args)
		{
			ConsoleColor fg = Console.ForegroundColor;
			try
			{
				if (IsConsoleEnabled)
				{
					Console.ForegroundColor = titleFg;
					Console.Write(titleFilter,title);
					Console.ForegroundColor = fg;
					Console.Write(filter,args);
				}
			} catch { IsConsoleEnabled = false; }
			OnXLogEvent(titleFg,title,titleFilter,filter,args);
			
		}
		
		public static void WriteY(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.Yellow,"{0}",title,filter,args);
		}
		public static void WriteDY(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.DarkYellow,"{0}",title,filter,args);
		}
		public static void WriteC(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.Cyan,"{0}",title,filter,args);
		}
		public static void WriteDC(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.DarkCyan,"{0}",title,filter,args);
		}
		public static void WriteM(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.Magenta,"{0}",title,filter,args);
		}
		public static void WriteDM(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.DarkMagenta,"{0}",title,filter,args);
		}
		
		public static void WriteG(string title, string filter, params object[] args)
		{
			Write(ConsoleColor.Green,"{0}",title,filter,args);
		}
	}
}
