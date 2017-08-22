/*
 * oio * 4/24/2012 * 10:11 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Cor3.Parsers;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using CultureInfo = System.Globalization.CultureInfo;

#endregion
namespace AvalonEditor
{
	/// <summary>This class is stable, however needs consolidation and
	/// implementation of a few more things.
	/// <para>This find service first started out with a snippit provided
	/// on stackoverflow.com, and was implemented as the FindHighlightTransform
	/// class as encapsulated here (quite possibly with a little tweaking).</para>
	/// <para>Later, a IBackgroundRenderer was cloned for use here.  The
	/// BackgroundRenderer was taken directly from ICSharpCode.AvalonEdit's
	/// Find Service/Control and was modified to implement a Regex based
	/// search-method.
	/// </para>
	/// <para>That means that we have two search classes.  You can toggle
	/// usage changing the value of UsingBackgroundRenderer (bool).</para>
	/// </summary>
	/// <remarks>
	/// This service acts differently from ICSharpCode.AvalonEdit's implementation
	/// in a number of ways, such as that you have to explicitly call
	/// Find() each time you want updated results, as opposed to a dependency-property
	/// firing events on (text-to-find) TextBox.Text is updated.  Furthermore,
	/// the ICSharpCode Implementation detaches when the text-search-control
	/// is closed.
	/// </remarks>
	public class EditorFindTextService
	{
		
		public Func<string> ProvideFindText {
			get { return provideFindText; }
			set { provideFindText = value; }
		} Func<string> provideFindText;
		
		public Action<string> ProvideSetText {
			get { return provideSetText; }
			set { provideSetText = value; }
		} Action<string> provideSetText;
		
		public Func<string> ProvideReplaceText {
			get { return provideReplaceText; }
			set { provideReplaceText = value; }
		} Func<string> provideReplaceText;
		
		public Action ClearText {
			get { return clearText; }
			set { clearText = value; }
		} Action clearText;
		
		protected Editor editor { get; set; }
		
		public string TextToFind
		{
			get { return provideFindText.Invoke(); }
			set { provideSetText.Invoke(value); OnTextToFindChanged(); }
		} string textToFind;
		
		const string info = @"{{
	sender: {0},
	document: {1},
	IsAttached: {2}
	...
	}}";
		
		static readonly Collection<CommandBinding> commandBindings = new Collection<CommandBinding>();

		bool IsAttached { get; set; }
	
		public EditorFindTextService(Editor document)
		{
			this.editor = document;
			if (commandBindings.Count == 0) CreateBindings();
		}
	
		public void Find(object sender, ExecutedRoutedEventArgs e)
		{
			string message = string.Format(CultureInfo.InvariantCulture, info, sender, editor, IsAttached);
			MessageBox.Show(message);
			TextToFind = provideFindText.Invoke();//this.document.mainWindow.TextToFind
			AttachFindSvc(editor.CommandBindings);
		}
		public void FindNext(object sender, ExecutedRoutedEventArgs e) { MessageBox.Show(string.Format(CultureInfo.InvariantCulture,"FindNext: {0}", sender)); }
		public void FindPrev(object sender, ExecutedRoutedEventArgs e) { MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "FindPrev: {0}", sender)); }
	
		void CanFindClear(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(TextToFind);
		}
		public void FindClear(object sender, ExecutedRoutedEventArgs e)
		{
			TextToFind = string.Empty;
//			DetachFindSvc();
			e.Handled = true;
		}
		
		#region Create, Attach, Bind
		
		void CreateBindings()
		{
			commandBindings.Add(new CommandBinding(EditorDocumentCommands.FindCommand,Find,CanFind));
			commandBindings.Add(new CommandBinding(EditorDocumentCommands.FindNextCommand,FindNext,CanFindNext));
			commandBindings.Add(new CommandBinding(EditorDocumentCommands.FindPrevCommand,FindPrev,CanFindPrev));
			commandBindings.Add(new CommandBinding(EditorDocumentCommands.FindClearCommand,FindClear,CanFindClear));
		}

		public void AttachFindSvc(CommandBindingCollection commandBindings)
		{
			if (commandBindings.Count==0)
				foreach (CommandBinding binding in commandBindings)
					commandBindings.Add(binding);
			RendererAdd(false);
		}
	
		bool IsRendererNull { get { return this.FindBackgroundHiglighter == null; } }
		bool IsRendererAlive { get { return IsRendererNull ? false : this.editor.TextArea.TextView.BackgroundRenderers.Contains(this.FindBackgroundHiglighter); } }
		
		public void DetachFindSvc(CommandBindingCollection commandBindings)
		{
			try {
				
				this.FindBackgroundHiglighter = null;
				foreach (CommandBinding binding in commandBindings)
					if (commandBindings.Contains(binding))
						commandBindings.Remove(binding);
				RendererRemove(false);
				this.editor.TextArea.TextView.Redraw();
			} catch (Exception) { }
		}
	
		void RendererAdd(bool kill)
		{
			if (IsRendererAlive && kill) RendererRemove(kill);
			if (IsRendererNull) this.FindBackgroundHiglighter = new BackgroundRenderer(TextToFind);
			
			System.Diagnostics.Debug.Assert(this.editor!=null);
			
			this.editor.TextArea.TextView.BackgroundRenderers.Add(FindBackgroundHiglighter);
			
		}
		void RendererRemove(bool destroy)
		{
			if (IsRendererNull) {
				return;
			}
			if (IsRendererAlive) this.editor.TextArea.TextView.BackgroundRenderers.Remove(this.FindBackgroundHiglighter);
			if (destroy) this.FindBackgroundHiglighter = null;
		}
		
		#endregion
	
		#region Find
		void CanFind(object sender, CanExecuteRoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(TextToFind)) e.CanExecute = false;
			e.CanExecute = true;
		}
		void CanFindNext(object sender, CanExecuteRoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(TextToFind)) e.CanExecute = false;
			e.CanExecute = true;
		}
		void CanFindPrev(object sender, CanExecuteRoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(TextToFind)) e.CanExecute = false;
			e.CanExecute = true;
		}
	
		/// <summary></summary>
		public bool IgnoreCase {
			get { return ignoreCase; }
			set { ignoreCase = value; }
		} bool ignoreCase = true;
	
		#region SearchContext
	
		public  event ExecutedRoutedEventHandler TextToFindChanged;
	
		private void OnTextToFindChanged()
		{
			ExecutedRoutedEventArgs e = default(ExecutedRoutedEventArgs);
			if (TextToFindChanged != null) {
				TextToFindChanged(this, e);
			}
		}
		#endregion
		
		#region IBackgroundRenderer
	
		/// <summary>
		/// Taken and modified AvalonEdit's SearchResultBackgroundRenderer.
		/// In Stead of using TextSegmentCollection, we're using a generic
		/// text-range list.
		/// </summary>
		class BackgroundRenderer : IBackgroundRenderer
		{
			string textToFind = null;
			Regex Expr { get;set; }
			public bool IgnoreCase = true;
			RegexOptions Options { get { return ( IgnoreCase ? RegexOptions.IgnoreCase : 0 ) | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled; } }
			
			public List<TextRange> CurrentResults {
				get { return currentResults; }
			} List<TextRange> currentResults = new List<TextRange>();
			
			public KnownLayer Layer {
				get {
					// draw behind selection
					return KnownLayer.Selection;
				}
			}
			
			public BackgroundRenderer(string textToFind/*, TextEditor editor*/)
			{
				this.textToFind = textToFind;
				MarkerBrush = new SolidColorBrush(Colors.LightGreen){Opacity=0.8};
				this.Expr = new Regex(this.textToFind,this.Options);
			}
			
			Brush markerBrush;
			Pen markerPen;
			
			public Brush MarkerBrush {
				get { return markerBrush; }
				set {
					this.markerBrush = value;
					markerPen = new Pen(MarkerBrush, 1);
				}
			}
	
			/// <summary>
			/// This is currently called from within the Draw method.
			/// It should in stead be A: Provided via external source or
			/// B: Updated when the text changes.
			/// </summary>
			/// <param name="view"></param>
			/// <param name="start"></param>
			/// <param name="length"></param>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "length")]
			void GetResults(TextView view, int start, int length)
			{
				CurrentResults.Clear();
				if (string.IsNullOrEmpty(textToFind)) return;
				if (string.IsNullOrEmpty(view.Document.Text)) return;
				foreach (Match m in Expr.Matches(view.Document.Text,start))
				{
					CurrentResults.Add(TextRange.FromMatch(m));
				}
			}
			
			/// <summary>
			/// note that this original method only renders from the first visual line to the
			/// last visual line.
			/// </summary>
			/// <param name="textView"></param>
			/// <param name="drawingContext"></param>
			public void Draw(TextView textView, DrawingContext drawingContext)
			{
				if (textView == null)
					throw new ArgumentNullException("textView");
				if (drawingContext == null)
					throw new ArgumentNullException("drawingContext");
				
				if (currentResults == null || !textView.VisualLinesValid)
					return;
				
				var visualLines = textView.VisualLines;
				if (visualLines.Count == 0)
					return;
				
				int viewStart = visualLines.First().FirstDocumentLine.Offset;
				int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;
				GetResults(textView,viewStart,viewEnd-viewStart);
	//				foreach (SearchResult result in currentResults.FindOverlappingSegments(viewStart, viewEnd - viewStart)) {
				foreach (TextRange result in currentResults
				         .Where( r => r.Position32 >= viewStart && (int)r.EndPosition <= viewEnd ))
				{
					BackgroundGeometryBuilder geoBuilder = new BackgroundGeometryBuilder();
					geoBuilder.AlignToMiddleOfPixels = true;
					geoBuilder.CornerRadius = 3;
					geoBuilder.AddSegment(textView, result.GetSegment());
					Geometry geometry = geoBuilder.CreateGeometry();
					if (geometry != null) {
						drawingContext.DrawGeometry(MarkerBrush, markerPen, geometry);
					}
				}
			}
		}
	
		#endregion
		
		#region FindHighlightTransform Class
		/// <summary>http://stackoverflow.com/questions/9223674/highlight-all-occurrences-of-selected-word-in-avalonedit
		/// <para>Note that the Regular Expression was never implemented in this and the input text added via the constructor is used.
		/// In other words, we're using code directly from the article, with minor exception to a few little changes.</para>
		/// </summary>
		/// <remarks>Gotta love stackoverflow.  The algo was updated to use TextRage/Regex</remarks>
		class ColorizingTransformer : DocumentColorizingTransformer
		{
			// TextEditor texteditor;
			string findMe = null;
			//	bool  { get;set; }
			bool IgnoreCase;
			Regex Expr;
			RegexOptions Options { get { return ( IgnoreCase ? RegexOptions.IgnoreCase : 0 ) | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled; } }
			
			public ColorizingTransformer(string findMe, bool ignoreCase)
			{
				this.findMe = findMe;
				this.IgnoreCase = ignoreCase;
				this.Expr = new Regex(findMe,Options);
			}
			
			protected override void ColorizeLine(DocumentLine line)
			{
				if (string.IsNullOrEmpty(findMe)){
					//					base.ColorizeLine(line);
					return;
				}
				try
				{
					int lineStartOffset = line.Offset;
					string text = CurrentContext.Document.GetText(line);
					int start = 0;
					int index;
					while ((index = text.IndexOf(findMe, start)) >= 0) {
						base.ChangeLinePart(
							lineStartOffset + index, // startOffset
							lineStartOffset + index + findMe.Length, // endOffset
							(VisualLineElement element) => {
								// This lambda gets called once for every VisualLineElement
								// between the specified offsets.
								Typeface tf = element.TextRunProperties.Typeface;
								// Replace the typeface with a modified version of
								// the same typeface
								element.TextRunProperties.SetTypeface(new Typeface(tf.FontFamily,FontStyles.Italic, FontWeights.Bold, tf.Stretch){});
								element.TextRunProperties.SetBackgroundBrush(Brushes.SkyBlue);
								element.TextRunProperties.SetForegroundBrush(Brushes.White);
							});
						start = index + 1; // search for next occurrence
					}
				} catch {}
			}
		}
		#endregion
		
	//		ColorizingTransformer FindHiglighter {get;set;}
		BackgroundRenderer FindBackgroundHiglighter {get;set;}
	
		public void FindTextGotoNext(object s, ExecutedRoutedEventArgs e)
		{
		}
		public void FindTextGotoPrev(object s, ExecutedRoutedEventArgs e)
		{
		}
	
	//		public void FindTextEnd(object s, ExecutedRoutedEventArgs e)
	//		{
	//			if (FindHiglighter==null) return;
	//			(s as TextEditor).TextArea.TextView.LineTransformers.Remove(FindHiglighter);
	//			FindHiglighter = null;
	//		}
		#endregion
	}
}
