/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 01/11/2013
 * Time: 10:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Cor3.Parsers.CascadingStyleSheets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using AvalonDock;

namespace AvalonEditor
{
	/// <summary>
	/// Description of IParserService.
	/// </summary>
	interface IParserService
	{
		/// <summary>
		/// Provide Insight to the application window.
		/// </summary>
		void RequestInsight();
		void UpdateRowInfo(TextCompositionEventArgs e, bool pre, bool unknown);
		
		string Buffer { get; }
		int CurrentPosition { get; }
		int CurrentLength { get; }
		int CurrentEndPosition { get; }
		char CurrentCharFirst { get; }
		char CurrentCharLast { get; }
		
		/// <summary>
		/// Provide access to the EditorDocument
		/// </summary>
		IEditorDocument Document { get; }
		
		/// <summary>
		/// Used to check weather or not the Sevice applies to the file extension.
		/// This method should bare template files in mind.
		/// (Ignores Character Case)
		/// </summary>
		/// <param name="fileName">File name input.</param>
		/// <returns>True if the extension is handled by the Service, otherwise False.</returns>
		bool CanHandleExtension(string fileName);
		/// <summary>
		/// 1. Add Commands to the editor.<br/>
		/// 2. Add any related Menu Items to the application when the document becomes active.<br/>
		/// </summary>
		/// <param name="doc">The document the service is created for.</param>
		void Attach(IEditorDocument doc);
		void Detach(IEditorDocument doc);
	}
	public interface IEditorDocument //: ISegment
	{
		void Activate();
//		event EventHandler<CancelEventArgs> Closing;
		object Tag { get;set; }
		bool Close();
		void AddDocumentEvents(IDocumentContext app);
		void RemDocumentEvents(IDocumentContext app);
		
		object DataContext { get; set; }
		CommandBindingCollection CommandBindings { get; }
		
		System.ComponentModel.Design.ServiceContainer ServiceContainer { get; }
		
		bool UseApplist { get; }
		
		/// <summary>Main Window Reference</summary>
		IDocumentContext mainWindow { get; }
		
		string FileN { get; }
		
		/// <summary>A pointer to the file-name.</summary>
		string FileName { get; set; }
		
		/// <summary>(main) TextEditor access point</summary>
		AvalonEditor Editor { get; }
	}
	public interface IDocumentContext: INotifyPropertyChanged
	{
		Fluent.SplitButton MenuFileOpenSplitButton { get; }
		Fluent.SplitButton MenuFileSaveSplitButton { get; }
		
		Grid MainGrid { get; }
//		RibbonWindowFeature WindowRibbon { get; }
		
		event EventHandler ActiveDocumentChanged;
		
		CommandBindingCollection CommandBindings { get; }
		
		EditorWindowManager EWM { get; }
		
		#region Window Control
		
		// The main list
		
		bool IsListVisible { get; set; }
		
		#region Status Bar
		
		string StatusMiscText { get;set; }
		string StatusPositionText { get;set; }
		string StatusTypeText { get;set; }
		
		string StatusColText { get;set; }
		string StatusRowText { get;set; }
		
		#endregion
		
		#endregion
		
		#region (Ribbon) Find UI Element Text
		/// <summary>
		/// Text being searched for.
		/// </summary>
		string TextToFind { get;set; }
		
		/// <summary>
		/// Same as text to find, however this is text that is used for the replace function.
		/// </summary>
		string TextToReplace { get; }

		/// <summary>
		/// Text being used as a replacement
		/// </summary>
		string TextToReplaceWith { get; }
		#endregion
		
		/// <summary>
		/// Get/Set the Insight Item.
		/// </summary>
		AttributeInsight InsightItem { get; set; }
		/// <summary>Get the active document.
		/// <para>The window should ALWAYS provide an active document.  Otherwise Null.</para>
		/// </summary>
		IEditorDocument Document { get; }
		/// <summary>
		/// </summary>
		DockingManager DockManager { get; }
		/// <summary>
		/// User/Window-level definitions/attributes.
		/// </summary>
		ListBox AppList { get; }
		ListBox DocList { get; }
		TabControl TabListsControl { get; }
		/// <summary>
		/// Sets file-name (header) when active-document changes.
		/// </summary>
		string FileNameHeader { set; }
		/// <summary>
		/// Determine weather or not to show Attributes/Values in <see cref="AppList"/>.
		/// </summary>
		bool IsGroupingEnabled { get; }
		/// <summary>
		/// Get/Set weather <see cref="AppList"/> always attempts to sync with the document-selection/caret.
		/// </summary>
		bool IsListSyncronized { get; }
		/// <summary>
		/// Determine weather or not to show Attributes/Values in <see cref="AppList"/>.
		/// </summary>
		bool ShowAttributes { get; set; }
		/// <summary>if <strong>fragment</strong> is present, sets the selected list item.
		/// <para>Make sure you're sending a supported value so as no to waste time.</para>
		/// </summary>
		/// <param name="fragment">input fragment</param>
		/// <param name="applyNulls">if True and the item isn't in the list, any selected item is de-selected.</param>
		void UpdateListItem(CssFragment fragment, bool applyNulls);
		/// <summary>
		/// Update status-bar information.
		/// </summary>
		/// <param name="e">Some events provide this value;  Set it to null when not calling through a TextComposition/Event</param>
		/// <param name="pre">if TRUE, the action is before changes are made to text.</param>
		/// <param name="unknown">An example unknown assertion would be Cut/Paste operations.</param>
		void UpdateRowInfo(TextCompositionEventArgs e, bool pre, bool unknown);
		/// <summary>
		/// </summary>
		event RoutedEventHandler WindowInitialized;
	}
}
