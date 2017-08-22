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
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		AppGlobal GlobalConfig { get; set; }
		
		/// <summary>
		/// For this instance, if App.xaml designates StartupUri,
		/// we would not need to create our main window OnStartup.
		/// </summary>
		[STAThread()]
		static void Main( params string[] args )
		{
			var app = new App();
			app.InitializeComponent();
			AvalonEditorUtils.LoadXshdRes("SQL","GeneratorTool.Source._rc.Sql.xshd");
			app.Run();
		}
		
		/// <inheritdoc/>
		protected override void OnStartup(StartupEventArgs e)
		{
			new MuiModern().Show();
//			new Window1(Args.ToArray()).Show();
		}
		
	}
}