/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Cor3.Parsers.CascadingStyleSheets
{
//	public interface ICssParserControl
//	{
//		/// <summary>
//		/// Provides a set of options for UI.
//		/// </summary>
//		CssParserUserPreferences UserPreferences { get; }
//		/// <summary>
//		/// Memory based on the parser for the use.
//		/// This information is passed to the List of Definitions.
//		/// In WPF, this information is to become an ObservableCollection.
//		/// </summary>
//		List<CssFragment> Fragments { get; }
//		/// <summary>
//		/// Core/Feature CSS parser.
//		/// </summary>
//		System.Cor3.Parsers.CascadingStyleSheets.CssParser parser { get;set; }
//	}
//	public class CssParserController : ICssParserControl
//	{
//		#region Application Commands
//		static public ICommand LoadCssFileCommand = new RoutedCommand("Open Cascading Style Sheet",typeof(Window1));
//		static public RoutedCommand ParseCommand = new RoutedCommand("Refresh Parsed Content",typeof(Window1));
//		static public RoutedCommand ScrollSelectedItemIntoViewCommand = new RoutedCommand("Scroll the selected list-item into view.",typeof(Window1));
//		#endregion
//		
//		/// <inheritdoc/>
//		CssParserUserPreferences ICssParserControl.UserPreferences {
//			get { return userPrefs; }
//		} CssParserUserPreferences userPrefs;
//		
//		#region Parser and Memory
//		/// <inheritdoc/>
//		public List<CssFragment> Fragments { get;set; }
//		/// <inheritdoc/>
//		/// <summary>
//		/// Core/Feature CSS parser.
//		/// </summary>
//		public System.Cor3.Parsers.CascadingStyleSheets.CssParser parser;
//		#endregion
//
//	}
	/// <summary>
	/// Contains settings pertinant to user preferences
	/// </summary>
	[Serializable]
	public class CssParserUserPreferences
	{
		#region Category: Comment Fragment Visibility
		/// <summary>
		/// (Default=True)
		/// If set the TRUE, hides comments where the number of chars
		/// are less then <see cref="CommentMinLength" /> value.
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		[System.ComponentModel.Category("Comment Fragment Visibility")]
		private bool HideSmallComments {
			get { return hideSmallComments; }
			set { hideSmallComments = value; }
		} bool hideSmallComments=true;
		
		/// <summary>
		/// (Default=130)
		/// Minimum number of characters for comment-fragments
		/// visible to the UI's (displayed) list of elements.
		/// </summary>
		[System.ComponentModel.DefaultValue(130)]
		[System.ComponentModel.Category("Comment Fragment Visibility")]
		private int CommentMinLength {
			get { return commentMinLength; }
			set { commentMinLength = value; }
		} int commentMinLength=130;
		#endregion
		
		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public CssParserUserPreferences()
		{
		}
	}
}
