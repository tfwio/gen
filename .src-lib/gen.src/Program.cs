/* oio * 8/2/2014 * Time: 2:03 PM */
using System;

namespace GeneratorApp
{
	/// <summary>Class with program entry point.</summary>
	internal sealed class Program
	{
		/// <summary>Program entry point.</summary>
		[STAThread]
		private static void Main(string[] args)
		{
			var app = new GeneratorApplication(args);
		}
	}
}
