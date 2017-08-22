#define LOCALVLC1
// delete the 1
#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Xml;

using GeneratorTool.Views;

#endregion
namespace GeneratorTool
{
	class AppGlobal
	{
		internal List<string> Args, ArgsBackup;
		
		public AppGlobal(string[] args)
		{
			Initialize(args);
		}
		
		/// <summary>
		/// Check for '-create-db' parameters where we expect a following
		/// parameter contianing a path to the database.
		/// </summary>
		/// <param name="args"></param>
		void Initialize(string[] args)
		{
			ArgsBackup = new List<string>(Args = new List<string>(args));
		}
	}
}
