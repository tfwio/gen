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

using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.Indentation.CSharp;

#endregion

namespace GeneratorTool.Controls
{

	/// <summary>
	/// Interaction logic for AvalonEditor.xaml
	/// </summary>
	public partial class Editor : ICSharpCode.AvalonEdit.TextEditor
	{
		#region Brushes
		static public readonly SolidColorBrush cssForeground = new SolidColorBrush(Color.FromArgb(255,193,193,193));
		static public readonly SolidColorBrush cssBackground = new SolidColorBrush(Color.FromArgb(96,0,0,0));
		static public readonly SolidColorBrush defaultBack = new SolidColorBrush(SystemColors.WindowColor){Opacity=0.9};
		static public readonly SolidColorBrush defaultFore = SystemColors.WindowTextBrush;
		#endregion
		
		#region Prepare Document Strategy
		void SetStrategy(IHighlightingDefinition highlighter, IIndentationStrategy indentation/*, AbstractFoldingStrategy folding*/, Brush background, Brush foreground)
		{
			SyntaxHighlighting = highlighter;
			TextArea.IndentationStrategy = indentation;
//			FoldingStrategy = folding;
			Background = background;
			Foreground = foreground;
		}
		void SetStrategyFromExtension(string extension, IIndentationStrategy indentation/*, AbstractFoldingStrategy folding*/, Brush background, Brush foreground)
		{
			SetStrategy(
				HighlightingManager.Instance.GetDefinitionByExtension(extension),
				indentation,/*folding,*/ background, foreground
			);
		}
		void PrepareDocumentStrategyExt(string fileExtension)
		{
//			foldingStrategy = null;
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
					SetStrategyFromExtension(
						fileExtension,
						new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(),
//						new CssCurlyFoldingStrategy(),
						defaultBack, defaultFore);
					break;
				case ".aspx":
				case ".ascx":
				case ".master":
					SetStrategyFromExtension(
						fileExtension,
						new DefaultIndentationStrategy(),
//						new XmlFoldingStrategy(),
						defaultBack, defaultFore);
					break;
				case ".dtd":
				case ".html":
				case ".htm":
				case ".xaml":
				case ".xsd":
				case ".xshd":
				case ".xml":
					SetStrategyFromExtension(
						fileExtension,
						new DefaultIndentationStrategy(),
//						new XmlFoldingStrategy(),
						defaultBack, defaultFore);
//					PrepareDocumentStrategyMarkupLanguage(fileExtension);
					break;
//				case ".css":
//					{
//						SetStrategy(
//							Common.CSS2,
//							new CSharpIndentationStrategy(),
//							new CssCurlyFoldingStrategy(),
//							cssBackground, cssForeground);
//							// window.Document.Parser = new System.Cor3.Parsers.CascadingStyleSheets.CssParser(fileName,new CssParserOptions(){ParseOnConstructor=false});
//							// Text = window.Document.Parser.Text;
//							// window.Wcm.ParseAction(null,null);
//						FoldingStrategy = new CssCurlyFoldingStrategy();
//					}
//					break;
				case null:
				default:
					SetStrategyFromExtension(
						fileExtension,
						new DefaultIndentationStrategy(),
//						new XmlFoldingStrategy(),
						defaultBack, defaultFore);
					break;
			}
		}
		/// <summary>
		/// Here we need to set foldings and also the IHighlightingDefinition.
		/// </summary>
		/// <param name="highlightingDefinition"></param>
		public void PrepareDocumentStrategyFromHighlighting(string highlightingDefinition)
		{
			System.Diagnostics.Debug.Print("Apply Highlignting Definition: {0}", highlightingDefinition);
			IHighlightingDefinition definition =
				HighlightingManager.Instance.GetDefinition(highlightingDefinition);
			this.SyntaxHighlighting = definition;
			System.Diagnostics.Debug.Print("Attempted to change highlighting: {0}\n", highlightingDefinition);
		}
		public void PrepareDocumentStrategy(string fileName, bool treatAsExtension)
		{
			string ext = treatAsExtension ? fileName : Common.GetFileOrTplExtension(fileName);
			
			System.Diagnostics.Debug.Print(
				"PrepareDocumentStrategy(File: {0}, TreatAsExtension: {1})",
				System.IO.Path.GetFileName(fileName),
				treatAsExtension
			);
			PrepareDocumentStrategyExt(ext /* parser service? */);
//			if (window.Document.Parser!=null) {
//				window.Document.ClearFragmentInfo();
//				// note that this is assigned when the active-document changes.
//				window.AppList.ItemsSource = null;
//				window.AppList.InvalidateVisual();
//			}
			
			if (!treatAsExtension)
			{
				if (System.IO.File.Exists(fileName) && ext!= ".css") Load(fileName);
				else if(System.IO.File.Exists(fileName) && ext == ".css")
				{
					// parser service?
					Load(fileName);
				}
				else throw new System.IO.FileLoadException();
			}
//			SetFoldings(this.FoldingStrategy);
//			window.Document.ResetTitle(fileName);
		}
		#endregion

		#region Services (Embedded)
		
		/// <summary>Service: Find</summary>
//		EditorFindTextService FindService { get; set; }
		
		#endregion
		
		#region Folding
//		/// <inheritdoc />
//		public AbstractFoldingStrategy FoldingStrategy {
//			get { return foldingStrategy; } set { SetFoldings(value); }
//		} AbstractFoldingStrategy foldingStrategy;
//		
//		void SetFoldings(AbstractFoldingStrategy strategy)
//		{
//			System.Diagnostics.Debug.Print("SetFoldings {{ this: {0} }}",this);
//			if (this.FoldingManager != null)
//			{
//				FoldingManager.Uninstall(this.FoldingManager);
//				this.FoldingManager = null;
//			}
//			if (strategy != null)
//			{
//				this.FoldingManager = FoldingManager.Install(this.TextArea);
//				strategy.UpdateFoldings(FoldingManager, this.Document);
//			}
//			foldingStrategy = strategy;
//		}
		
		/// <inheritdoc />
		public FoldingManager FoldingManager { get; set; }
		
		#endregion
		
		/// <summary>
		/// Constructor
		/// </summary>
		public Editor()
		{
			InitializeComponent();
			
//			this.FindService = new EditorFindTextService(this);
			Options.AllowScrollBelowDocument = true;
		}
		
		/// <summary>
		/// Service Disposal/Destrucor
		/// </summary>
		~Editor()
		{
//			WheelService.DetachWheelEvent();
//			WheelService = null;
			try{
//				FindService.DetachFindSvc(this.CommandBindings);
//				FindService = null;
			} catch {
				System.Diagnostics.Debug.Print("FindService.DetachFindSvc() failed");
			}
		}
		
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
		}
	}

}