using System;
using System.Windows.Input;
using Generator.AvalonEdit.Helpers;
using Generator.Classes;
using ICSharpCode.AvalonEdit.Folding;

namespace Generator.EditorExtension
{
	
	public static class EditorExtension
	{
		/// <summary>
		/// <para>Initialize Code-Completion</para>
		/// <para></para>
		/// </summary>
		/// <param name="editor"></param>
		/// <param name="config"></param>
		static public void InitializeControl(this TemplateEditor editor, UITemplateManager config)
		{
			editor.Factory = config;

			editor.avalonTextEditor.InputBindings.Add(new KeyBinding(ApplicationCommands.Save,Key.S,ModifierKeys.Control));
			editor.avalonTextEditor.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save,config.sTemplates.TemplateFromUIHandler,config.sTemplates.TemplateFromUIHandlerCan));
			//
			editor.Initialize_CodeCompletion_TextEnter();
			editor.InitEntering2();
			editor.avalonTextEditor.TextChanged += editor.TextChangedEventHandler;
			//
			editor.FoldingUpdateTimer.Interval = TimeSpan.FromMilliseconds(900);
			editor.FoldingUpdateTimer.Tick += editor.foldingUpdateTimer_Tick;
			editor.FoldingUpdateTimer.Start();
		}
		
		static public void foldingUpdateTimer_Tick(this TemplateEditor editor, object sender, EventArgs e)
		{
			editor.FoldingUpdateTimer.Stop();
			editor.InitEntering1();
		}
		
		// as to be visible to the extension
		static public void TextChangedEventHandler(this TemplateEditor editor, object sender, EventArgs e)
		{
			if (editor.FoldingUpdateTimer.IsEnabled) editor.FoldingUpdateTimer.Stop();
			editor.FoldingUpdateTimer.Start();
		}
		
		static public void Initialize_CodeCompletion_TextEnter( this TemplateEditor editor )
		{
			// this is our code-completion helper
			editor.Editor.TextArea.TextEntering += editor.textEditor_TextArea_TextEntering;
			// this particular little method is for code completion and is not used
			editor.Editor.TextArea.TextEntered  += editor.textEditor_TextArea_TextEntered;
		}

		static public void InitEntering1( this TemplateEditor editor )
		{
			try
			{
				if (editor.FoldingStrategy != null)
				{
					editor.FoldingStrategy.UpdateFoldings(editor.FoldingManager, editor.Editor.Document);
				}
			} catch (Exception) { }
		}

		static public void InitEntering2( this TemplateEditor editor )
		{
			if (editor.Editor.SyntaxHighlighting == null) {
				editor.FoldingStrategy = null;
			} else {
				switch (editor.Editor.SyntaxHighlighting.Name) {
					case "XML":
						editor.FoldingStrategy = new XmlFoldingStrategy();
						editor.Editor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						break;
					case "C#":
					case "CSS":
					case "C++":
					case "PHP":
					case "Java":
						editor.Editor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(editor.Editor.Options);
						editor.FoldingStrategy = new CSharpPragmaRegionFoldingStrategy();
						break;
					default:
						editor.Editor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						editor.FoldingStrategy = null;
						break;
				}
			}
			if (editor.FoldingStrategy != null) {
				if (editor.FoldingManager == null)
					editor.FoldingManager = FoldingManager.Install(editor.Editor.TextArea);
				editor.FoldingStrategy.UpdateFoldings(editor.FoldingManager, editor.Editor.Document);
			} else {
				if (editor.FoldingManager != null) {
					FoldingManager.Uninstall(editor.FoldingManager);
					editor.FoldingManager = null;
				}
			}
		}
	
	}
}
