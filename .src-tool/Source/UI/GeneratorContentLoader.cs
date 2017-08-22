/* oio : 1/21/2014 9:33 AM */
using System;
using System.Linq;

using FirstFloor.ModernUI.Windows;

namespace GeneratorTool.Views
{
	/// <summary>
	/// Loads lorem ipsum content regardless the given uri.
	/// </summary>
	public class GeneratorContentLoader : DefaultContentLoader
	{
		public GeneratorModel Model;
		
		readonly MoxiView moxi = new MoxiView();
		readonly WriterTemplateControl writerControl = new WriterTemplateControl();
		readonly SQLiteView sqlTool = new SQLiteView();
		
		/// <summary>
		/// Loads the content from specified uri.
		/// </summary>
		/// <param name="uri">The content uri</param>
		/// <returns>The loaded content.</returns>
		protected override object LoadContent(Uri uri)
		{
      switch (uri.OriginalString) {
        case "/1":
          return moxi;
        case "/generator":
          return moxi;
        case "/generator/data":
          MoxiView.DatabaseViewCommand.Execute(null);
          return moxi;
        case "/generator/template":
          MoxiView.TemplateViewCommand.Execute(null);
          return moxi;
        case "/writerTemplate":
          return writerControl;
        case "/sqlTool":
          return sqlTool;
        //case "/3":
        //  FirstFloor.ModernUI.Windows.Controls.ModernDialog.ShowMessage(
        //    "This is a simple Modern UI styled message dialog. Do you like it?",
        //    "Message Dialog",
        //    System.Windows.MessageBoxButton.OK);
        //  return null;
        //case "/4":
        //  return new Uri("#4");
        //case "/2":
        //  MoxiView.StatePushCommand.Execute(null);
        //  var DataEditor = new DataEditorContent();
        //  DataEditor.DataContext = moxi.LastFactory;
        //  return DataEditor;
          
      }
			return base.LoadContent(uri);
		}
	}
}



