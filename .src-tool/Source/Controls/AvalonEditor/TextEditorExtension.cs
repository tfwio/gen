/*
 * User: oio
 * Date: 12/21/2011
 * Time: 1:16 AM
 */

#region Using
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.Indentation.CSharp;

#endregion

namespace GeneratorTool.Controls
{
	static class TextEditorExtension
	{
		#region Prepare Document Strategy
		static public void SetStrategy(this Editor editor, IHighlightingDefinition highlighter, IIndentationStrategy indentation/*, AbstractFoldingStrategy folding*/, Brush background, Brush foreground)
		{
			editor.SyntaxHighlighting = highlighter;
			editor.TextArea.IndentationStrategy = indentation;
//			editor.FoldingStrategy = folding;
			editor.Background = background;
			editor.Foreground = foreground;
		}
		static public void SetStrategyFromExtension(this Editor editor, string extension, IIndentationStrategy indentation/*, AbstractFoldingStrategy folding*/, Brush background, Brush foreground)
		{
			editor.SetStrategy(
				HighlightingManager.Instance.GetDefinitionByExtension(extension),
				indentation,
				/*folding,*/ background, foreground
			);
		}
		
		/// <summary>
		/// Here we need to set foldings and also the IHighlightingDefinition.
		/// </summary>
		/// <param name="highlightingDefinition"></param>
		static public void PrepareDocumentStrategyFromHighlighting(this Editor editor, string highlightingDefinition)
		{
			System.Diagnostics.Debug.Print("Apply Highlignting Definition: {0}", highlightingDefinition);
			IHighlightingDefinition definition =
				HighlightingManager.Instance.GetDefinition(highlightingDefinition);
			editor.SyntaxHighlighting = definition;
			System.Diagnostics.Debug.Print("Attempted to change highlighting: {0}\n", highlightingDefinition);
		}
		static public void PrepareDocumentStrategy(this Editor editor, string fileName, bool treatAsExtension)
		{
			string ext = treatAsExtension ? fileName : Common.GetFileOrTplExtension(fileName);
			
			System.Diagnostics.Debug.Print(
				"PrepareDocumentStrategy(File: {0}, TreatAsExtension: {1})",
				System.IO.Path.GetFileName(fileName),
				treatAsExtension
			);
			editor.PrepareDocumentStrategyExt(ext /* parser service? */);
//			if (window.Document.Parser!=null) {
//				window.Document.ClearFragmentInfo();
//				// note that this is assigned when the active-document changes.
//				window.AppList.ItemsSource = null;
//				window.AppList.InvalidateVisual();
//			}
			
			if (!treatAsExtension)
			{
				if (System.IO.File.Exists(fileName) && ext!= ".css") editor.Load(fileName);
				else if(System.IO.File.Exists(fileName) && ext == ".css")
				{
					// parser service?
					editor.Load(fileName);
				}
				else throw new System.IO.FileLoadException();
			}
//			editor.SetFoldings(editor.FoldingStrategy);
//			window.Document.ResetTitle(fileName);
		}
		#endregion
		
		static public void SetFoldings(this Editor editor/*, AbstractFoldingStrategy strategy*/)
		{
			System.Diagnostics.Debug.Print("SetFoldings {{ this: {0} }}",editor);
			if (editor.FoldingManager != null)
			{
				FoldingManager.Uninstall(editor.FoldingManager);
				editor.FoldingManager = null;
			}
//			if (strategy != null)
//			{
//				editor.FoldingManager = FoldingManager.Install(editor.TextArea);
//				strategy.UpdateFoldings(editor.FoldingManager, editor.Document);
//			}
//			editor.FoldingStrategy = strategy;
		}
		
		static public void PrepareDocumentStrategyExt(this Editor editor, string fileExtension)
		{
//			editor.FoldingStrategy = null;
	//			foreach (IPlugin plugin in Loader.Plugins)
	//				if (plugin.
			switch (fileExtension)
			{
				case ".as":
				case ".as2":
				case ".as3":
				case ".cs":
				case ".java":
				case ".js":
					editor.SetStrategyFromExtension(
						fileExtension,
						new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(),
//						new CssCurlyFoldingStrategy(),
						Editor.defaultBack, Editor.defaultFore);
					break;
				case ".aspx":
				case ".ascx":
				case ".master":
					editor.SetStrategyFromExtension(
						fileExtension,
						new DefaultIndentationStrategy(),
//						new XmlFoldingStrategy(),
						Editor.defaultBack, Editor.defaultFore);
					break;
				case ".dtd":
				case ".html":
				case ".htm":
				case ".xaml":
				case ".xsd":
				case ".xshd":
				case ".xml":
					editor.SetStrategyFromExtension(
						fileExtension,
						new DefaultIndentationStrategy(),
//						new XmlFoldingStrategy(),
						Editor.defaultBack, Editor.defaultFore);
	//					PrepareDocumentStrategyMarkupLanguage(fileExtension);
					break;
				case ".css":
					{
						editor.SetStrategy(
							Common.CSS2,
							new CSharpIndentationStrategy(),
//							new CssCurlyFoldingStrategy(),
							Editor.cssBackground, Editor.cssForeground);
						// window.Document.Parser = new System.Cor3.Parsers.CascadingStyleSheets.CssParser(fileName,new CssParserOptions(){ParseOnConstructor=false});
						// Text = window.Document.Parser.Text;
						// window.Wcm.ParseAction(null,null);
//						editor.FoldingStrategy = new CssCurlyFoldingStrategy();
					}
					break;
				case null:
				default:
					editor.SetStrategyFromExtension(
						fileExtension,
						new DefaultIndentationStrategy(),
//						new XmlFoldingStrategy(),
						Editor.defaultBack, Editor.defaultFore);
					break;
			}
		}
	}
}
