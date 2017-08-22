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
	/// <summary>
	/// CssFragment contains a internal TextRange
	/// and some additional CSS information such as the type of content
	/// in the range.
	/// <para>
	/// Though Chidren and ChildStack Properties were added, they didn't quite pan out
	/// and are not supported.
	/// </para>
	/// </summary>
	public class CssFragment
	{
		public int LargestContainedLevel {
			get { return largestContainedLevel; }
		} internal int largestContainedLevel = 0;
		
		#region Position,EndPosition,Length
		/// <summary>
		/// Indirect TextRange Property: this.section.Position
		/// </summary>
		public long Position { get { return section.Position; } }
		/// <summary>
		/// indirect TextRange Property: this.section.EndPosition
		/// </summary>
		public long EndPosition { get { return section.EndPosition; } }
		/// <summary>
		/// indirect TextRange Property: this.section.Length
		/// </summary>
		public long Length { get { return section.Length; } }
		#endregion
		
		#region GetDefinitions
		
		/// <summary>A utility function to quickly get all Definition names for a range of definitions.
		/// <para>A definition is what we term any declaration before the curly-block of attributes/values.</para>
		/// EG: <tt>definition-term1 :selector { attribute: value; }</tt>
		/// <para>Depends on (boolean) <see cref="CssParserOptions"/>.CleanDefinitionWhiteSpace value.</para>
		/// </summary>
		/// <returns></returns>
		/// <param name="parser">CssParser provides the text-buffer.</param>
		/// <exception cref="ArgumentException">Thrown if you try to get definitions from a tag that does not contain any.</exception>
		public string GetDefinitions(CssParser parser)
		{
			System.Diagnostics.Debug.Print("Reading Definitions from element.");
			if ( (this.FragmentType != CssFragmentType.Definition)/* || (this.FragmentType != CssFragmentType.DefinitionFragment)*/ )
				throw new ArgumentException("Fragment MUST be CssFragmentType.Definition.");// 

			// obtain a reference to the next curly-block.
			CssFragment nextBlock = parser.FindNextFragment(CssFragmentType.Block, (int)this.Position);
			
			// TODO: report mal-formed CSS.
			if (nextBlock==null)
			{
				System.Diagnostics.Debug.Print("Mal-Formed CSS @{0}",this.Position);
				return null;
			}
			
			// Find fragmented terms, and comments
			IEnumerable<CssFragment> resultset = parser.Defs.Leveled.Where(
				f =>
				(f.Position < nextBlock.Position) &&
				(f.Position > this.Position) &&
				((f.FragmentType == CssFragmentType.Definition)||(f.FragmentType == CssFragmentType.CommentBlock))
			);
			
			// Get Text for the current element fro the buffer
			string temp = this.section.Substring(parser.Text);
			// clean up the text if need be
			if (parser.ParserOptions.CleanDefinitionWhiteSpace) temp = CleanupDefinition(temp);
			
			// Get text for fragmented definitions and if need be, comments.
			using (IEnumerator<CssFragment> rs = resultset.GetEnumerator())
			{
				while (rs.MoveNext())
				{
					System.Diagnostics.Debug.Print("Fragment: {0}", rs.Current.Position);
					
					// CssFragmentType.DefinitionFragmentComment
					if ( (rs.Current.FragmentType== CssFragmentType.CommentBlock) && (!parser.ParserOptions.CleanDefinitionFragmentedComments) )
					{
						System.Diagnostics.Debug.Print("Found Fragmented Comment at Position: {0}",rs.Current.Position);
						rs.Current.FragmentType = CssFragmentType.DefinitionFragmentComment;
						temp += parser.GetEOL() + rs.Current.section.Substring(parser.Text) + parser.GetEOL();
					}
					
					// CssFragmentType.DefinitionFragment
					else if ((rs.Current.FragmentType== CssFragmentType.Definition))
					{
						System.Diagnostics.Debug.Print("Found Fragmented Definition at Position: {0}",rs.Current.Position);
						rs.Current.FragmentType = CssFragmentType.DefinitionFragment;
						temp += parser.ParserOptions.CleanDefinitionWhiteSpace ?
							CleanupDefinition(rs.Current.section.Substring(parser.Text)) :
							rs.Current.section.Substring(parser.Text);
						
					}
					// This is a bad idea.
					this.section.Length += rs.Current.Length;
				}
			}
			
			return temp;
		}
		string CleanupDefinition(string input)
		{
			int i=0;
			string[] split = input.Trim(CommonTextDefinitions.chrClean).Split(',');
			while (i<split.Length) { split[i] = split[i].Trim(CommonTextDefinitions.chrClean); i++; }
			input = string.Join(", ",split);
			Array.Clear(split,0, split.Length);
			split = null;
			return input;
		}
		#endregion
		
		/// <summary>
		/// Gets the Text-Range's Text using a call
		/// to <see cref="System.Cor3.Parsers.TextRange.Substring(string)" />
		/// using the provided text.
		/// <para>This is the same as calling this.section.Substring(string)</para>
		/// </summary>
		/// <param name="textBuffer"></param>
		/// <returns></returns>
		public string GetText(string textBuffer)
		{
			return this.section.Substring(textBuffer);
		}
		
		/// <summary>
		/// Provides simple info: FragmentType, Section(TextRange info) and the Tag.
		/// </summary>
		/// <returns>
		/// "<tt>[FragmentType=…, Section=[…], Tag=…]</tt>"
		/// </returns>
		public override string ToString()
		{
			return string.Format("[FragmentType={2}, Section={1}, Tag={0}]", _tag, section, fragmentType);
		}
		
		#region Sorting Methods
		/// <summary>
		/// Sorts elements using Linq.
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		static public IOrderedEnumerable<CssFragment> SortLinq(IEnumerable<CssFragment> items)
		{
			return items.OrderBy(item => item.section.Position);
		}
		/// <summary>
		/// The .NET v2.0 standard for sorting.
		/// </summary>
		/// <returns></returns>
		/// <param name="a"></param>
		/// <param name="b"></param>
		static public int SortCompare(CssFragment a, CssFragment b)
		{
			return a.section.Position.CompareTo(b.section.Position);
		}
		
		#endregion
		#region no longer supported
		/*
		/// <summary>
		/// By default the Root-Level is imaginary Level -1.
		/// It's up to the parser to make use of the Parent element
		/// The Parent Element should be set when any TextRange
		/// adds a Child.
		/// <para>Obsolete: this property hasn't been removed yet,
		/// however is obsolted through making use of additional
		/// CssFragmentType definitions such as DefinitionFragment
		/// and DefinitionFragmentComment and so fourth.</para>
		/// </summary>
		/// <remarks>
		/// A finalized/parsed CssFile would provide each element
		/// a reverse-lookup to it's parenting section.
		/// </remarks>
		public TextRange Parent { get { return parent; }
		} internal TextRange parent;

		#region (List<CssFragment>) Children
		
		/// <summary>
		/// The set of Children elements is for use as our final
		/// product of CssFragments.
		/// <para>
		/// When all parsing is done, we should end up with a
		/// hierarchiccal set of Chldren that can be Added to
		/// and Removed from.
		/// </para>
		/// <remarks>
		/// <para>
		/// Also when parsing is complete, and we have our completed
		/// set of hierarchical Child elements, these elements should
		/// generally be put to use for a Write() method that is
		/// a reversal of our Parse() method which interprets—producing
		/// an good as new clone (tidied up a bit) of the CSS input
		/// file.
		/// </para>
		/// </remarks>
		/// </summary>
		public List<CssFragment> Children {
			get { return childSections; }
		} internal List<CssFragment> childSections = new List<CssFragment>();

		/// <summary>
		/// This is an indirect call to <tt>this.Children.Add()</tt>.
		/// </summary>
		/// <param name="sectionParent">Allows you to assign the Parent (CssFragment) element.</param>
		/// <param name="range">The TextRange to be added.</param>
		public void Add(CssFragment sectionParent, CssFragment range)
		{
			range.parent = sectionParent;
			this.Children.Add(range);
		}
		/// <summary>
		/// A indirect call to <tt>this.Children.Remove(CssFragment)</tt>.
		/// </summary>
		/// <returns>True if successfuly.</returns>
		/// <param name="range"></param>
		public bool Remove(CssFragment range)
		{
			return this.Children.Remove(range);
		}
		#endregion

		#region (Stack<CssFragment>) ChildStack
		/// <summary>
		/// The ChildStack is provided here for use by a Parser.
		/// </summary>
		public Stack<CssFragment> ChildStack {
			get { return childStack; }
		} internal Stack<CssFragment> childStack = new Stack<CssFragment>();
		/// <summary>
		/// Indirect call to <tt>this.ChildStack.Push(CssFragment)</tt>.
		/// </summary>
		/// <param name="range"></param>
		public void Push(CssFragment range)
		{
			this.ChildStack.Push(range);
		}
		/// <summary>
		/// Indirect call to <tt>this.ChildStack.Pop()</tt>.
		/// </summary>
		/// <returns></returns>
		public CssFragment Pop()
		{
			return this.ChildStack.Pop();
		}
		#endregion
		
		/// <summary>
		/// Like the ChildStack, BlockLevel is to be used by a parser.
		/// Any Element Not at Level#0 should be contained as a Child Element.
		/// </summary>
		public int BlockLevel { get { return blockLevel; }
		} int blockLevel = 0;
		 */
		#endregion
		
		/// <summary>
		/// Optional Tag element, for helping the parser.
		/// </summary>
		public object Tag { get { return _tag; } set { _tag = value; }
		} object _tag;
		
		/// <summary>Optional Grouping construct.
		/// <para>The idea would be to set this as unique per Definition Block.
		/// Un-contained Comment-Fragments would perhaps be assigned their own group.</para>
		/// </summary>
		public object Group { get { return _group; } set { _group = value; }
		} object _group = null;
		
		/// <summary>Optional Grouping Construct
		/// <para>Example useage would be to set the category to it's containing group.</para>
		/// </summary>
		public object Category { get { return _category; } set { _category = value; }
		} object _category = null;
		
		public TextRange section;

		/// <summary>
		/// Get/Set CssFragmentType value.
		/// </summary>
		public CssFragmentType FragmentType { get { return fragmentType; } set { fragmentType = value; }
		} CssFragmentType fragmentType = CssFragmentType.Undetermined;

		#region implicit operator
		/// <summary>
		/// Allows for quick generation of CssFragment from TextRange.
		/// </summary>
		/// <param name="trange">TextRange input.</param>
		/// <returns>new CssFragment(range)</returns>
		static public implicit operator CssFragment(TextRange trange) { return new CssFragment(trange); }
		/// <summary>
		/// Returns a new TextRange from the CssFragment, for use as a clone tactic.
		/// If you would like a direct reference, just use the 'section' field.
		/// </summary>
		/// <param name="trange"></param>
		/// <returns>new TextRange(section.Position,section.Length)</returns>
		static public implicit operator TextRange(CssFragment trange) { return new TextRange(trange.section.Position,trange.section.Length); }
		
		#endregion

		/// <summary>
		/// Parameterless Constructor
		/// <para>section is defauted to TextRange.Empty.
		/// EG: <tt>{ Position=-1, Length=0 }</tt></para>
		/// </summary>
		public CssFragment() : this(TextRange.Empty)
		{
		}
		
		/// <summary>
		/// Initialize from TextRange.
		/// </summary>
		/// <remarks>Generally this is rarely used as opposed to <see cref="TextRange.FromMatch(System.Text.RegularExpressions.Match)" />.</remarks>
		/// <param name="range">TextRange input.</param>
		public CssFragment(TextRange range)
		{
			this.section = range;
		}
	}
}
