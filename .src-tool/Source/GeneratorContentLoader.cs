/* oio : 1/21/2014 9:33 AM */
using System;
using System.Linq;
using System.Windows;

using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace GeneratorTool.Views
{
	
	/// <summary>
	/// Loads lorem ipsum content regardless the given uri.
	/// </summary>
	public class GeneratorContentLoader : DefaultContentLoader
	{
		public GeneratorModel Model;
		
		MoxiView moxi = new MoxiView();
		WriterTemplateControl writerControl = new WriterTemplateControl();
		SQLiteView sqlTool = new SQLiteView();
		
		/// <summary>
		/// Loads the content from specified uri.
		/// </summary>
		/// <param name="uri">The content uri</param>
		/// <returns>The loaded content.</returns>
		protected override object LoadContent(Uri uri)
		{
			// return a new LoremIpsum user control instance no matter the uri
			if (uri.OriginalString == "/1") return moxi;
			else if (uri.OriginalString == "/generator") return moxi;
			else if (uri.OriginalString == "/generator/data") {
				MoxiView.DatabaseViewCommand.Execute(null);
				return moxi;
			}
			else if (uri.OriginalString == "/generator/template") {
				MoxiView.TemplateViewCommand.Execute(null);
				return moxi;
			}
			else if (uri.OriginalString == "/writerTemplate") return writerControl;
			else if (uri.OriginalString == "/sqlTool") return sqlTool;
//			else if (uri.OriginalString == "/3")
//			{
//				ModernDialog.ShowMessage("This is a simple Modern UI styled message dialog. Do you like it?", "Message Dialog", MessageBoxButton.OK);
//				return null;
//			}
//			else if (uri.OriginalString == "/4") return new Uri("#4");
//			else if (uri.OriginalString == "/2")
//			{
//				MoxiView.StatePushCommand.Execute(null);
//				
//				var DataEditor = new DataEditorContent();
//				DataEditor.DataContext = moxi.LastFactory;
//				
//				return DataEditor;
//			}
			return base.LoadContent(uri);
		}
	}
}



