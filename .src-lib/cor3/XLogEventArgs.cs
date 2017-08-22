/* oOo * 11/14/2007 : 10:22 PM */
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace System
{
	public class XLogEventArgs : EventArgs
	{
		public bool ClearConsole = false;
		static public string Separator = string.Empty;
		public ConsoleColor Colour { get;set; }
		public string Title { get;set; }
		public string TitleFilter { get;set; }
		public string Format { get;set; }
		public object[] Arguments { get;set; }
		
		public override string ToString()
		{
			string otitle = string.Format(TitleFilter ?? "{0}",Title ?? string.Empty);
			string omsg = string.Format(Format??"{0}", Arguments == null ? new object[]{ string.Empty } : Arguments);
			return string.Concat(otitle,Separator,omsg);
		}
	
		
		public XLogEventArgs(Boolean clear)
		{
			ClearConsole = true;
		}
		public XLogEventArgs(ConsoleColor Colour, string title, string titleFilter, string format, params object[] args)
			: this(Colour,title,format,args)
		{
			this.TitleFilter = titleFilter;
		}
		public XLogEventArgs(ConsoleColor Colour, string title, string format, params object[] args)
		{
			this.Colour = Colour;
			this.Title = title;
			this.Format = format;
			this.Arguments = args;
		}
	}
}
