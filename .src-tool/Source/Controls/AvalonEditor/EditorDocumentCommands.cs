#region Using

using System;
using System.Cor3.Parsers.CascadingStyleSheets;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using IRange = ICSharpCode.AvalonEdit.Document.ISegment;

//using XSegment = System.Cor3.Parsers.ISegment;

#endregion

namespace AvalonEditor
{
	public static class EditorDocumentCommands
	{

		#region RoutedCommand
		static public readonly RoutedCommand RefreshFoldings = new RoutedCommand(
			"RefreshFoldings", typeof(EditorDocumentCommands));
		/// <summary>
		/// ParseCommand; Refresh Parsed Content
		/// </summary>
		static public readonly RoutedCommand ParseCommand = new RoutedCommand(
			"ParseCommand",typeof(EditorDocumentCommands),
			new InputGestureCollection { new KeyGesture(Key.F5)}
		);
		/// <summary>
		/// FindCommand
		/// </summary>
		static public readonly RoutedCommand FindCommand = new RoutedCommand(
			"FindCommand",typeof(EditorDocumentCommands),
			new InputGestureCollection { new KeyGesture(Key.F,ModifierKeys.Control)}
		);
		/// <summary>
		/// FindNextCommand
		/// </summary>
		static public readonly RoutedCommand FindNextCommand = new RoutedCommand(
			"FindNextCommand",typeof(EditorDocumentCommands),
			new InputGestureCollection { new KeyGesture(Key.F3,ModifierKeys.Control)}
		);
		/// <summary>
		/// FindPrevCommand
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prev")]
		static public readonly RoutedCommand FindPrevCommand = new RoutedCommand(
			"FindPrevCommand",typeof(EditorDocumentCommands),
			new InputGestureCollection { new KeyGesture(Key.F3,ModifierKeys.Control|ModifierKeys.Shift)}
		);
		/// <summary>
		/// FindPrevCommand
		/// </summary>
		static public readonly RoutedCommand FindClearCommand = new RoutedCommand(
			"FindClearCommand",typeof(EditorDocumentCommands)
		);
		/// <summary>
		/// Toggle the visibility of attribute/value pairs in the main list.
		/// </summary>
		static public readonly RoutedCommand CssToggleCommentCommand = new RoutedCommand(
			"CssToggleCommentCommand",typeof(EditorDocumentCommands),
			new InputGestureCollection {
				new KeyGesture(Key.OemQuestion,ModifierKeys.Control),
				new KeyGesture(Key.Divide,ModifierKeys.Control)
			}
		);
		/// <summary>
		/// Show the file in windows-explorer.
		/// </summary>
		static public readonly RoutedCommand ShowFileInExplorer = new RoutedCommand("ShowFileInExplorer",typeof(EditorDocumentCommands));
		#endregion
		
		const string explorerFile = @"/select,""{fname}""";

//		static public void ShowFileInExplorerAction(object sender, ExecutedRoutedEventArgs e)
//		{
//			if (sender is IDocumentContext) {
//				IDocumentContext wm = sender as IDocumentContext;
//				if (wm==null || string.IsNullOrEmpty(wm.Document.FileName)) return;
//				string fname = wm.Document.FileName;
//				Process.Start("explorer",explorerFile.Replace("{fname}",fname));
//			} else if (sender is IEditorDocument) {
//				IEditorDocument doc = sender as IEditorDocument;
//				if (doc==null || string.IsNullOrEmpty(doc.FileName)) return;
//				string fname = doc.FileName;
//				Process.Start("explorer",explorerFile.Replace("{fname}",fname));
//			} else {
//				MessageBox.Show("sender isn't css document.  It's a {t}".Replace("{t}",sender.GetType().Name));
//				e.Handled = true;
//				return;
//			}
//			e.Handled = true;
//		}
	}
}
