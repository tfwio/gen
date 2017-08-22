/*
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers.CascadingStyleSheets
{

	/// <summary>
	/// CssParser is responsible for breaking CSS Content into sections
	/// of rule definitions.
	/// <para>
	/// An important aspect of CSS is the order which items are loaded.
	/// Duplicate items for example are stacked in a matter of prescedance,
	/// where all values are stored, and the most recently loaded item
	/// is applied to the loaded document.
	/// </para>
	/// </summary>
	public class CssParser : IServiceProvider, ISyntaxStrategy /* IDisposable (in ISyntaxStrategy) */
	{
		/// <summary>
		/// Finished Reading Block: {{ Block: {0}, Block.Tag: \"{1}\", Level: {2}, FragType: {3} }}
		/// </summary>
		const string dbg_level1_brace_info = "Finished Reading Block: {{ Block: {0}, Block.Tag: \"{1}\", Level: {2}, Level(1): {3} }}";

		#region IServiceProvider, ServiceContainer
		public ServiceContainer Services {
			get { return services; }
		} readonly ServiceContainer services = new ServiceContainer();
		object IServiceProvider.GetService(Type serviceType)
		{
			return services.GetService(serviceType);
		}

		#endregion
		
		#region ParserOptions
		static readonly public CssParserOptions DefaultOptions = new CssParserOptions(){};

		/// <summary>
		/// <see cref="CssParserOptions" /> controls generally how the
		/// parser handles input and memory.
		/// </summary>
		public CssParserOptions ParserOptions {
			get { return parserOptions; }
		} readonly CssParserOptions parserOptions = DefaultOptions;

		public string GetEOL()
		{
			if (this.parserOptions.EndOfLineTerminator== EolMode.CRLF) return "\r\n";
			else if (this.parserOptions.EndOfLineTerminator== EolMode.CRLF) return "\n\r";
			else if (this.parserOptions.EndOfLineTerminator== EolMode.CRLF) return "\r";
			return "\n";
		}
		
		#endregion
		
		#region Constants
		const string _info_format = @"Found:
  {0} Comments
  {1} Charset Elements
  {2} Namespace Elements
  {3} Media Elements
  {4} Import Elements
  {5} FontFace Elements
  {6} Block Sections (which still needs parsing)
";
		#endregion
		
		#region Fields
		/// <summary>
		/// Our default Encoding.
		/// </summary>
		static readonly System.Text.Encoding DefaultEncoding = System.Text.Encoding.UTF8;
		
		/// <summary>
		/// Content is loaded into this as our parser's buffer.
		/// </summary>
		public string Text {
			get { return textBuffer; }
			set { textBuffer = value; }
		} string textBuffer = null;
		
		/// <summary>
		/// Definition References
		/// <para>
		/// Any element recognized by the parser lands in here,
		/// until Parser Level #2 completes.
		/// </para>
		/// </summary>
		public CssParserReference Defs {
			get { return defs; }
			set { defs = value; }
		} CssParserReference defs = new CssParserReference();
		


		/// <summary>
		/// This is used to find blocks within our Content.
		/// </summary>
		ParserBlock CssBlockElement = new ParserBlock("{","}");
		#endregion
		
		#region Properties
		/// <summary>
		/// Indicates that Level#2 of our parsing methods have passed
		/// </summary>
		public bool IsParsed2 {
			get { return isParsed2; }
		} bool isParsed2 = false;
		/// <summary>
		/// Indicates that Level#3 of our parsing methods have passed
		/// </summary>
		public bool IsParsed3 {
			get { return isParsed3; }
		} bool isParsed3 = false;
		
		
		/// <summary>
		/// The local file unless initialized with a Stream.
		/// </summary>
		public string FileName {
			get { return fileName; }
		} string fileName = null;
		
		#endregion
		
		#region static helper: IsInRange(List<CssFragment>,TextRange), SetLogType(XLog.OutputMode)
		
		/// <summary>
		/// Used for checking weather or not a range is contained
		/// within a provided set of CssFragments in which cas it would be ignored.
		/// </summary>
		/// <param name="ranges"></param>
		/// <param name="current"></param>
		/// <returns></returns>
		static bool IsInRange(List<CssFragment> ranges, TextRange current)
		{
			foreach (CssFragment range in ranges)
				if (range.section.ContainsRange(current))
					return true;
			return false;
		}
		
		#region ignore logging
#if xlogging
    /// <summary>
		/// Sets the type of logging.  By default, logging is sent to
		/// <see cref="System.Diagnostics.Debug.Print(string,object[])" />.
		/// </summary>
		/// <param name="outputType"></param>
		static public void SetLogType(XLog.OutputMode outputType)
		{
			XLogController.OutputMode = outputType;
		}
#endif
    #endregion
		#endregion
		
		// Parsers
		// -------
		
		public void ResetBuffer(string text)
		{
			this.textBuffer = text;
			this.Defs.Clear();
			this.Parse();
		}
		
		/// <summary>
		/// Calls each parser:
		/// <see cref="ParseLevel1Pre()" />,
		/// <see cref="ParseLevel1()" />.
		/// <see cref="ParseLevel2()" />.
		/// <see cref="ParseLevel3()" />
		/// and prints some info to logger or debug.
		/// </summary>
		public void Parse()
		{
			this.ParseLevel1Pre();
			this.ParseLevel1();
			this.ParseLevel2();
			this.isParsed2 = true;
			this.ParseLevel3();
			this.isParsed3 = true;
			this.ParseLevel4();
			if (this.parserOptions.CleanAfterParse) this.Defs.Clean(/*this.ParserOptions*/);
			this.isParsed2 = true;
			#if DEBUG
			System.Diagnostics.Debug.Print("---- some info ----");
			System.Diagnostics.Debug.Print("——————————————————————————————————");
			System.Diagnostics.Debug.Print(this.PrintInfoLevel1());
			System.Diagnostics.Debug.Print("——————————————————————————————————");
			#else
			XLog.WriteLine("---- some info ----");
			XLog.WriteLine("——————————————————————————————————");
			XLog.WriteLine(this.PrintInfoLevel1());
			XLog.WriteLine("——————————————————————————————————");
			#endif
//			this.ParseDefinitions();
		}

		#region Parser #1: Regular Expressions Regex-to-List<CssFragment>
		/// <summary>
		/// Pass #1.
		/// <para>
		/// First and foremost, this method finds all comments, and start
		/// locations for most Global Tags such as Charset, Namespace,
		/// Import, FontFace and Media however it only finds the word
		/// such as '@media' and not the block elements or tags in full;
		/// This is for Pass #2.
		/// </para>
		/// </summary>
		void ParseLevel1Pre()
		{
			// all elements other then Comment are checked agains the
			// existing list of comments.
			// none of these elements actually need to be added to different
			// lists given that we're adding the type along with the item.
			this.Defs.Comment.Clear();
			// line breaks (not used at all in this script thus far)
			// in fact they are!—before parsing begins. (see Load(Stream))
			foreach (Match m in CssDefinitions.BreakerExpr.Matches(this.textBuffer))		this.Defs.Breaker.Add(new CssFragment(TextRange.FromMatch(m)){ FragmentType= CssFragmentType.Breaker});
			foreach (Match m in CssDefinitions.CommentExpr.Matches(this.textBuffer))		this.Defs.Comment.Add(new CssFragment(TextRange.FromMatch(m)){ FragmentType= CssFragmentType.CommentBlock, Tag="Comment"});
			// ————————————————————————————————————
			this.Defs.Charset.Clear();
			foreach (Match m in CssDefinitions.CharsetExpr.Matches(this.textBuffer))		this.AddListItem(CssFragmentType.Charset,this.Defs.Charset,TextRange.FromMatch(m));
			this.Defs.Namespace.Clear();
			foreach (Match m in CssDefinitions.NamespaceExpr.Matches(this.textBuffer))		this.AddListItem(CssFragmentType.Namespace,this.Defs.Namespace,TextRange.FromMatch(m));
			this.Defs.Import.Clear();
			foreach (Match m in CssDefinitions.ImportExpr.Matches(this.textBuffer))			this.AddListItem(CssFragmentType.Import,this.Defs.Import,TextRange.FromMatch(m));
			this.Defs.FontFace.Clear();
			foreach (Match m in CssDefinitions.FontFaceExpr.Matches(this.textBuffer))		this.AddListItem(CssFragmentType.FontFace,this.Defs.FontFace,TextRange.FromMatch(m));
			this.Defs.Keyframes.Clear();
			foreach (Match m in CssDefinitions.KeyframeExpr.Matches(this.textBuffer))		this.AddListItem(CssFragmentType.Keyframes,this.Defs.Keyframes,TextRange.FromMatch(m));
			this.Defs.Media.Clear();
			foreach (Match m in CssDefinitions.MediaExpr.Matches(this.textBuffer))			this.AddListItem(CssFragmentType.Media,this.Defs.Media,TextRange.FromMatch(m));
			this.Defs.Url.Clear();
			foreach (Match m in CssDefinitions.UrlExpr.Matches(this.textBuffer))			this.AddListItem(CssFragmentType.Url,this.Defs.Url,TextRange.FromMatch(m));
			this.Defs.Quoted.Clear();
			foreach (Match m in CssDefinitions.QuotedStringExpr.Matches(this.textBuffer))	this.AddListItem(CssFragmentType.StringQuoted,this.Defs.Quoted,TextRange.FromMatch(m));
			// this.DEFS.BlockLevel
		}
		
		/// <summary>
		/// Performs level-1 parsing providing a set of curly-block CssFragments.
		/// </summary>
		/// <exception cref="CssParserException">Throws an exception on inconsistent curly-blocks.</exception>
		void ParseLevel1()
		{
			// Set up our initial TextRange
			this.Defs.Root = new TextRange(0,this.textBuffer.Length);
			
			// Set our initial Read Position to 0
			int current = 0;
			
			// This is our output List.
			this.Defs.BlockLevels.Clear();
//			List<CssFragment> list = new List<CssFragment>();
			
			// The Stack.Count tells us what Block-Level were at.
			// It increments up each time we enter a new Block, and
			// increments down each time we exit a Block.
			Stack<CssFragment> stack = new Stack<CssFragment>();
			
			while ((current = this.Defs.Root.section.IndexOf(CssBlockElement, this.textBuffer, current, this.Defs.Root.section.Length32-current, false )) > -1)
			{
				// This is used to store the largest possible hierarchical block level.
				// such as 'GLOBAL { LEVEL-1 { LEVEL-2 { LEVEL-3 { ... } } } }'.
				int stackRecursion = 0;
				
				// Exit the loop if no more block elements have been found.
				if (current == -1) break;
				
				// Look into our buffer to tell if we're entering or exiting from a Block.
				char c = this.textBuffer[current];
				
				// Create a new CssFragment for using later.
				CssFragment temp = new CssFragment(new TextRange(current,1)){ FragmentType= CssFragmentType.Block };

				// if we're in a comment segment, we have no pending action.
				#if DEBUG
				if (this.IsInComment(temp)) { /* skipping */ System.Diagnostics.Debug.Print("Found a block inside a comment @{0:N0}—Skipping",current); }
				#else
				if (this.IsInComment(temp)) { /* skipping */ XLog.WriteLine("Found a block inside a comment @{0:N0}—Skipping",current); }
				#endif
				
				// When we enter a block we set the CssFragment's Tag to "block-level#{stack-count}". 
				else if (c=='{')
				{
					
					stack.Push(temp);
					
					// I wonder if this ever happens.
					if (IsInRange(this.Defs.Quoted,temp))
					{
						
					}
					// I wonder if this ever happens.
					else if (IsInRange(this.Defs.Url,temp))
					{
						
					}
					else
					{
						stack.Peek().Tag = string.Format("Block-Level#{0}", stack.Count);
					}
					
				}
				else if (c=='}')
				{
					
					int stackCount = stack.Count;
					
					// the following comment is perhaps dated to say the least.
					// if the stack isn't empty, something went wrong.
					
					/**
					 * If the stack isn't empty means that we have a @media or @keyframe fragment.
					 * We will set stackRescursion to the maximum level-number (which should be 1)
					 * and clear the value when stackCount is once again zero.
					 **/
					
					if (stackCount == 0 && stackCount > stackRecursion)
					{
						if (stackCount > stackRecursion) stackRecursion = stackCount;
					}
					
					try
					{
						temp = stack.Pop();
					}
					catch
					{
						throw new CssParserException(current,"Inconsistent curly '}' block. Closing tag not found.");
					}
					
					temp.largestContainedLevel = stackRecursion;
					
					temp.section.Length = current-temp.section.Position + 1;
					
//					temp.Tag = string.Format("curly block level is now {0}",stack.Count);
					// ¿ Anything to explain about the following code ?
					
					// Ensure once again that we are not in a string or url.
					if (IsInRange(this.Defs.Quoted,temp)){
					// Ensure once again that we are not in a string or url.
					} else if (IsInRange(this.Defs.Url,temp)){
					// Add the block
					} else {
						this.Defs.BlockLevels.Add(temp);
						#if DEBUG
						System.Diagnostics.Debug.Print(
							dbg_level1_brace_info,
							temp,
							temp.Tag,
							temp.LargestContainedLevel,
							temp.FragmentType);
						#else
						XLog.WriteLine(
							dbg_level1_brace_info,
							temp,
							temp.Tag,
							temp.LargestContainedLevel,
							temp.FragmentType);
						#endif
					}
				}
				current++;
			}
			stack.Clear();
			stack = null;
		}

		#endregion

		#region Parser #2: flatten, re-child @media
		/// <summary>A second-pass (level-2) parser.
		/// <para>
		/// The purpose of this parser is to deal with the data that's in the
		/// simple elements that have heen peeked into.
		/// </para>
		/// <para>
		/// What we have at this point is a set of starting points for our
		/// @ELEMENTS such as charset, namespace, media, import, and font-face
		/// where we've mentioned enough times how the @media element is the
		/// only element I've come across that breaks out of the mold of
		/// single-level blocks.
		/// </para>
		/// <para>
		/// Also we have the two (pre) hard parts done.  We've blocked out
		/// all of our Comment and Curly-Block elements which leaves us with
		/// our starting point: Read through all the un-blocked content.
		/// </para>
		/// <hr />
		/// <h2>Some Preliminary Notes</h2>
		/// <h3>Re-Group The Elements</h3>
		/// <para>
		/// Think SQL or Linq even.  We're simply going to add the info to
		/// a Database (actually) where our ROW is going to be the CssFragment.
		/// </para>
		/// Each CssFragment will contain:
		/// <pre>
		/// Create Table [CssFragment] (
		/// 	[type]			varchar default 'undefined',
		/// 	[pid]			integer, /* any null is document level */
		/// 	[id]			integer primary key,
		/// 	[tags]			varchar,
		/// 	[tag]			varchar,
		/// 	[block]			varchar
		/// );</pre>
		/// <para>
		/// Only thing that I'm really missing is attrubute parsing, though once
		/// we have a block, every existing style element is going to
		/// go to and from different implementations and if dealing with
		/// css, may implement their respective values.
		/// </para>
		/// <para>
		/// Each attribute is going to have to end up with their own
		/// parsers such as for borders.  We will of course know how to parse
		/// single value elements, and once we have our unit-dimensions
		/// parsed can tackle attribute-values that are x4 like border:
		/// "<tt>({number}{px|em|in|mm|cm|pc|pt}){0,4}|</tt>"
		/// </para>
		/// <para>I don't know why I'd gone and mentioned all of that,
		/// all we're interested in here is what's in the sql-table-definition.
		/// This basically accounts for a duality in the form of tags vs their
		/// blocks.<br />
		/// <strong>Identify and Group all content into a flat table per block,
		/// or definition.</strong>
		/// </para>
		/// </summary>
		void ParseLevel2()
		{
			using (XLogController log = new XLogController(XLog.OutputMode.Debugger,null))
			{
				this.Defs.Leveled.Clear();
				// flatten all elements to a single list.
				foreach (CssFragment fragment in this.Defs.Comment) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.Charset) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.Namespace) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.Import) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.FontFace) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.Keyframes) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.Media) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.Url) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.Quoted) this.Defs.Leveled.Add(fragment);
				foreach (CssFragment fragment in this.Defs.BlockLevels) this.Defs.Leveled.Add(fragment);
				
				// if we really wanted to, we could check for the only known
				// logic breaker: @media. and grab it's main block and perhaps
				// even it's children-blocks.
				// --------------
				// apparently we're going to also have to check for @keyframes.
				// ---------------
				// this is the duty of the next parsing stage.
//				this.CheckBlockingTag(this.defs.Leveled, CssFragmentType.Media, CssFragmentType.MediaBlock, CssFragmentType.MediaBlockDefinition);
//				this.CheckBlockingTag(this.defs.Leveled, CssFragmentType.Keyframes, CssFragmentType.KeyframesBlock, CssFragmentType.KeyframesBlockDefinition);
				// sort them.
				this.Defs.Leveled.Sort(CssFragment.SortCompare);
				
			}
			
			// starting at zero, read all text to the first element,
			// attribute or block.
			// this means we have a text-range that starts at zero.
			
			
			// at this point we can run until we hit:
			// '@', any word-character, ',' and etc.
		}
		
		void CheckBlockingTag(List<CssFragment> input, CssFragmentType fragmentType, CssFragmentType blockType, CssFragmentType blockDefinitionType)
		{
			if (input.Count > 0)
			{
				Stack<CssFragment> tagref = new Stack<CssFragment>();
				CssFragment fragment = null;
				// find all fragmentType tags.
				foreach (CssFragment frag in this.Defs.Leveled.Where( frg => frg.FragmentType == fragmentType ))
				{
					tagref.Push(frag);
				} // FIXME: handle css-syntax error?
				// act on the found fragments in a safe manner.
				while ( tagref.Count != 0 )
				{
					fragment = tagref.Pop();
					// our first block following media tag.
					CssFragment fm = this.Defs.BlockLevels
						.Where(frg=>frg.section.Position > fragment.section.EndPosition)
						.First();
					// change the block to the type we've searched for.
					fm.FragmentType = blockType;
					// next we should find all the chlidren (if any) and mark them as blockDefinitionType elements
					foreach (CssFragment child in FragmentsFromSection(fm))
						child.FragmentType = blockDefinitionType;
//					this.RechildBlocks(ref this.Defs.Leveled,fm);
					//fragment.Children.Sort(CssFragment.SortCompare);
				}
			} // end of @media element scanner.
		}
		#endregion

		#region Parser #3: re-child @media, parse property-definition-space
		/// <summary>
		/// Starting at the beginning of the document, we step into each character and collect
		/// everything and a single white-space into a stack of characters.
		/// when a block is encountered, we simply step past it.
		/// </summary>
		void ParseLevel3()
		{

			int position = 0;
			// Starting at Offset-Zero, read through and categorize
			// any un-categorized elements.
			while (true)
			{
//				nextComment is never used
//				CssFragment nextComment = FindNext(this.Defs.Comment,position);
				
				CssFragment nextBlock = FindNext(this.Defs.BlockLevels,position);
				if (nextBlock==null) break;
				CssFragment nextImport = FindNextFragment(CssFragmentType.Import,position);
				
				CssFragment next = FindNext(position);
				
				if (next.section.ContainsPosition(position))
				{
					position=(int)next.section.EndPosition;
					continue;
					
				}
				else if ((nextImport!=null) && (nextImport.section.ContainsPosition(nextImport.section)))
				{
					// Skip past the declaration.
					position=(int)nextImport.section.EndPosition;
					// Set the tag of the declaration to contain
					// the entire import statement.
					nextImport.Tag = nextImport.GetText(this.textBuffer);
					continue;
				}
				// read all content up to the next position
//				this.textBuffer.Substring();
				int length = (int)next.section.Position - (position+1);
				
				// definitions will end up broken into TextRange fragments until we Write content if need be.
				// in order to leave the content in tact, we're not going to EVER do this.
				// we're simply collecting info.
				TextRange range = new TextRange(position,length+1);
				string temp = range.Substring(textBuffer).Trim();
				if (!string.IsNullOrEmpty(temp))
				{
					CssFragment fragment = new CssFragment(range){FragmentType = CssFragmentType.Definition};
					// we used to call fragment.GetDefinitions here, however we are unable
					// to detect fragmented definitions, so we must do this after reading all
					// the definitions.
					this.Defs.Leveled.Add(fragment);
				}
				else this.Defs.Leveled.Add(new CssFragment(range){FragmentType = CssFragmentType.Whitespace});
				position=(int)next.section.EndPosition;
			}
			this.Defs.Leveled.Sort(CssFragment.SortCompare);
			foreach (CssFragment fragment in this.Defs.Leveled.Where(f=>f.FragmentType==CssFragmentType.Definition))
				fragment.Tag = fragment.GetDefinitions(this);
		}
		#endregion
		
		#region Parser #4: Attributes/Values
		
		/// <summary>
		/// <para>find all definitions groups.
		/// it is possible for comments to be between definitions,
		/// so we need to check for that.</para>
		/// </summary>
		/// <description>
		/// For this section we'll begin using CssAttributeFragment.
		/// <ul>Iterate through all Fragments.
		///   <li>Act on definition tags
		///     <ul>
		///       <li>collect any definitions separated by comments</li>
		///       <li>find the first curly-block (defines our attribute/value set)</li>
		///     </ul>
		///   </li>
		/// </ul>
		/// </description>
		/// <remarks>
		/// We are missing ERROR CHECKING which would provide insight such as
		/// ROW/COLUMN offset in addition to the error-message.
		/// </remarks>
		void ParseLevel4()
		{
			foreach (CssFragment fragment in this.Defs.BlockLevels)
			{
				// Previously, we left the last curly-terminator ('}') in place for detection of any (last) non-terminating value within a section.
				// Due to changes in the regular-expression used, we no longer need to do this.
				// Ignoring the curly is because we may be inspecting HTML STYLE tags where there are no curlies.
				TextRange newRange = new TextRange(fragment.Position+1,fragment.Length-2);
				string input = newRange.Substring(this.Text);
				foreach (Match m in CssDefinitions.AttributeValuePairExpr.Matches(input))
				{
					TextRange rangeName = new TextRange( m.Groups["attr"].Index+newRange.Position, m.Groups["attr"].Length);
					TextRange rangeValue = new TextRange( m.Groups["value"].Index+newRange.Position, m.Groups["value"].Length);
					
					string sName  = rangeName.Substring(Text).Trim('\n',':',';');
					string sValue  = rangeValue.Substring(Text).Trim();
					
					CssFragment fragName = new CssFragment(rangeName) { FragmentType = CssFragmentType.DefinitionAttribute,Tag=string.Format("{0}: {1};",sName,sValue)};
					CssFragment fragValue = new CssFragment(rangeValue){FragmentType = CssFragmentType.DefinitionAttributeValue /*Tag will be the value type-classification.*/ };
					
					// the one thing that we could be checking for is comment within either of these two ranges.
					// The following checks if a comment-block contains our name or value, in which case the group
					// can be ignored all-together.
					if (!IsInComment(fragName) && !IsInComment(fragValue) && !(fragName.Length==0))
					{
						this.defs.Leveled.Add(fragName);
						this.defs.Leveled.Add(fragValue);
					}
				}
			}
			this.defs.Leveled.Sort(CssFragment.SortCompare);
		}
		#endregion

		// Helpers
		// -------

		#region FindNext, FindNextFragment
		
		/// <summary>
		/// when calling this method, we know that at least one of the blocks
		/// is not going to exist.  Were only looking in Defs.Comment or Defs.BlockLevels.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		CssFragment FindNext(int pos)
		{
			CssFragment f1 = FindNext(this.Defs.Comment,pos);
			CssFragment f2 = FindNext(this.Defs.BlockLevels,pos);
			if ((f1==null) && (f2 !=null)) return f2;
			else if ( (f1!=null) && (f2==null)) return f1;
			//
			if (f1.section.Position < f2.section.Position) return f1;
			return f2;
		}

		/// <summary>
		/// Finds the next block starting from <strong>position</strong>.
		/// </summary>
		/// <param name="reference">The search criteria.</param>
		/// <param name="position">The int offset from the start of the textBuffer.</param>
		/// <returns></returns>
		CssFragment FindNext(IEnumerable<CssFragment> reference, int position)
		{
			IEnumerable<CssFragment> f = reference.Where(f1 => f1.section.Position >= position);
			if (f.Count()==0) return null;
			return f.First();
		}

		/// <summary>
		/// Find the next fragment starting at buffer-offset <strong>position</strong>
		/// where the fragment type is as specified.
		/// </summary>
		/// <returns>The CssFragment or NULL if no fragment was found.</returns>
		/// <param name="position">the offset within the textBuffer</param>
		/// <param name="fragmentType">The type of fragment to be found.</param>
		public CssFragment FindNextFragment(CssFragmentType fragmentType, int position)
		{
			if (!isParsed2) throw new InvalidOperationException("This method can not be used until parsing is complete.");
			IEnumerable<CssFragment> f = this.Defs.Leveled.Where(f1 => (f1.section.Position >= position) && (f1.FragmentType== fragmentType));
			if (f.Count()==0) return null;
			return f.First();
		}

		#endregion
		
		#region IsInComment(TextRange),IsInComment(int),AddListItem(CssFragmentType,List<CssFragment>,CssFragment)
		/// <summary>
		/// Determines weather or not reference is within a comment-block.
		/// </summary>
		/// <returns>TRUE if is in a comment block, otherwise FALSE.</returns>
		/// <param name="reference">A TextRange we're checking against existing comment-blocks.</param>
		/// <remarks>Note that the parser has to have been called in order for comments to be present to check against.</remarks>
		bool IsInComment(TextRange reference) { return IsInRange(this.Defs.Comment, reference); }
		bool IsInComment(int position) { return IsInRange(this.Defs.Comment, new TextRange(position,0)); }
		
		/// <summary>
		/// Adds the item to the provided List if the item doesn't start
		/// inside a comment block.
		/// </summary>
		/// <param name="list">Destination List&lt;<see cref="CssFragment"/>&gt;.</param>
		/// <param name="item">The item to add.</param>
		/// <param name="fragmentType">A <see cref="CssFragmentType" /> value.</param>
		void AddListItem(CssFragmentType fragmentType, List<CssFragment> list, CssFragment item)
		{
			item.FragmentType = fragmentType;
			if (!IsInComment(item)) list.Add(item);
		}
		
		/// <summary>
		/// Finds all comments within a specific range.
		/// </summary>
		/// <param name="fragment"></param>
		/// <returns>A List&lt;TextRange&gt; of contained comment sections, or NULL.</returns>
		List<CssFragment> GetContainedComments(CssFragment fragment)
		{
			if (!IsInComment(fragment)) return null;
			List<CssFragment> list = new List<CssFragment>();
			foreach (CssFragment reference in Defs.Comment)
			{
				if (reference.section.ContainsRange(fragment))
					list.Add(reference);
				else if (reference.section.ContainsBleed(fragment))
					list.Add(reference);
				if (reference.section.Position >= fragment.section.EndPosition)
					break;
			}
			return list;
		}

		#endregion

		#region RechildBlocks(List{CssFragment},CssFragment): Needs a re-write
		/// <summary>(Private method - Not working or not used properly)
		/// <para>This method uses (List&lt;<see cref="CssFragment"/>&gt;) <strong>source</strong>
		/// as reference finding all items that are contained within the provided fragment
		/// (<see cref="CssFragment"/>) <strong>parentFragment</strong>.</para>
		/// <para>Each child element found is then removed from the reference list and added
		/// to <strong>parentFragment</strong>'s list of Children.</para>
		/// </summary>
		/// <param name="source">A list of elements to be used as reference; this list is also modified.</param>
		/// <param name="parentFragment">the Parent element.</param>
		void RechildBlocks(ref List<CssFragment> source, CssFragment parentFragment)
		{
			if (parentFragment.section.Length==0) return;
			Stack<long> positions = new Stack<long>();
			foreach (CssFragment f1 in source.Where(c0=> (c0.section.Position > parentFragment.section.Position) && (c0.section.EndPosition < parentFragment.section.EndPosition)))
				positions.Push(f1.section.Position);
			if (positions.Count==0) return;
			while ( positions.Count > 0 )
			{
				CssFragment f = source.Where( b => b.section.Position==positions.Peek() ).First();
				if (!source.Remove(f)) System.Windows.Forms.MessageBox.Show(string.Format("Failed removing an element {0}",f));
				//parentFragment.Children.Add(f);
				positions.Pop();
			}
			
		}

		#endregion
		
		#region Debugging Info (parser result info) PrintInfoLevel1(), PrintInfoLevel2()
		
		/// <summary>
		/// Prints info about the information gathered by parser-level#1.
		/// </summary>
		/// <returns></returns>
		public string PrintInfoLevel1()
		{
			return string.Format(
				_info_format,
				this.Defs.Comment.Count,
				this.Defs.Charset.Count,
				this.Defs.Namespace.Count,
				this.Defs.Media.Count,
				this.Defs.Import.Count,
				this.Defs.FontFace.Count,
				this.Defs.BlockLevels.Count);
		}
		/// <summary>
		/// Prints info about the information gathered by parser-level#2.
		/// </summary>
		/// <returns></returns>
		public string PrintInfoLevel2()
		{
			string output = string.Empty;
			foreach (CssFragment fragment in this.Defs.Leveled)
				output += string.Format(
					"{{ Type: {0}, Pos1: {1}, Pos2: {2}, Tag: {3} }}\r\n",
					fragment.FragmentType,
					fragment.section.Position,
					fragment.section.EndPosition,
//					fragment.Children.Count,
					fragment.Tag);
			return output;
		}

		
		#endregion
		
		#region Read Text (System.IO)
		/// <summary>
		/// A private method which returns a string from the given input <strong>stream</strong>.
		/// This method is used by the constructor to load text into <see cref="textBuffer" />.
		/// </summary>
		/// <param name="stream">input stream</param>
		/// <param name="ignoreErrors">if TRUE, ignores throwing an exception if <strong>input</strong> is empty.</param>
		void GetText(Stream stream, bool ignoreErrors)
		{
			using (TextReader reader = new StreamReader(stream, DefaultEncoding, true)) textBuffer = reader.ReadToEnd();
			if (string.IsNullOrEmpty(textBuffer) && !ignoreErrors) throw new NullReferenceException("The input string can not be null or Empty");
			// note that diffent usage scenarios would typically use
			// a particular set of parser-options.
			// * If we were using this parser for a ui-editor, we would surely
			//   provide the option to auto-convert.  It is quite common for editors,
			//   via paste operations to assert different EOL-terminators within the
			//   document.
			if (parserOptions.AutoConvertEndL)
			{
				string temp_replacement = @"@~endl";
				while (CssDefinitions.BreakerExpr.IsMatch(textBuffer))
				{
					textBuffer = CssDefinitions.BreakerExpr.Replace(textBuffer,temp_replacement);
				}
				if (parserOptions.EndOfLineTerminator== EolMode.CRLF)
					textBuffer = textBuffer.Replace(temp_replacement,"\r\n");
				else if (parserOptions.EndOfLineTerminator== EolMode.LFCR)
					textBuffer = textBuffer.Replace(temp_replacement,"\n\r");
				else if (parserOptions.EndOfLineTerminator== EolMode.CR)
					textBuffer = textBuffer.Replace(temp_replacement,"\r");
				else
					textBuffer = textBuffer.Replace(temp_replacement,"\n");
			}
		}
		/// <summary>
		/// (Indirect: <see cref="GetText(Stream,bool)" />)<br />A private method which returns a string from the given input <strong>stream</strong>.
		/// This method is used by the constructor to load text into <see cref="textBuffer" />.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="ignoreErrors">if TRUE, ignores throwing an exception if <strong>input</strong> is empty.</param>
		void GetFile(string fileName, bool ignoreErrors)
		{
			using (Stream stream = File.OpenRead(fileName))
				GetText(stream,ignoreErrors);
		}
		
		#endregion

		// .ctor, .dtor
		// ------------
		
		#region .ctor, .dtor
		/// <summary>
		/// Creates a CssParser using Default CssParserOptions: Begins parsing immediately,
		/// and ignores empty file Exception.
		/// </summary>
		/// <param name="fileName"></param>
		public CssParser(string fileName) : this(fileName,DefaultOptions)
		{
		}
		/// <summary>
		/// Creates a CssParser and optionally begins parsing input.
		/// </summary>
		/// <param name="fileName">Input file.</param>
		/// <param name="parserOptions"><see cref="ParserOptions" /> change parser's features.</param>
		public CssParser(string fileName, CssParserOptions parserOptions)
		{
			this.parserOptions = parserOptions;
			this.fileName = fileName;
			this.GetFile(this.fileName, parserOptions.IgnoreEmptyFileException);
			if (parserOptions.ParseOnConstructor) this.Parse();
		}
		/// <summary>
		/// Creates a CssParser and optionally begins parsing input depending on parser options.
		/// </summary>
		/// <param name="textStream">Input stream.</param>
		/// <param name="parserOptions"><see cref="ParserOptions" /> change parser's features.</param>
		public CssParser(Stream textStream, CssParserOptions parserOptions)
		{
			services.AddService(typeof(CssParser),this);
			this.parserOptions = parserOptions;
			this.GetText(textStream, this.parserOptions.IgnoreEmptyFileException);
			if (parserOptions.ParseOnConstructor) this.Parse();
		}
		
		~CssParser()
		{
			(this as IDisposable).Dispose();
		}
		#endregion
		
		#region IDisposable
		void IDisposable.Dispose()
		{
			Clear();
		}
		public void Clear()
		{
			this.Defs.Clear();
			this.Defs = null;
			this.textBuffer = null;
			this.fileName = null;
			this.isParsed2 = false;
			this.isDisposed = true;
		}
		
		/// <summary>
		/// Weather or not the parser has been disposed.
		/// </summary>
		public bool IsDisposed {
			get { return isDisposed; }
		} bool isDisposed = false;
		#endregion
		
		// Range and Offset
		// 

		public CssFragment FirstFragmentFromOffset(int offset)
		{
			// check to see that level three is parsed
			if (offset >= textBuffer.Length) return null;
			IEnumerable<CssFragment> results = FragmentsFromOffset(offset);
			if (results.Count() <= 0) return null;
			CssFragment fragment = results.First();
			results = null;
			return fragment;
		}
		public CssFragment LastFragmentFromOffset(int offset)
		{
			// check to see that level three is parsed
			if (offset >= textBuffer.Length) return null;
			IEnumerable<CssFragment> results = FragmentsFromOffset(offset);
			if (results.Count() <= 0) return null;
			CssFragment fragment = results.Last();
			results = null;
			return fragment;
		}
		public IEnumerable<CssFragment> FragmentsFromOffset(int offset)
		{
			// check to see that level three is parsed
			if (offset >= textBuffer.Length) return null;
			return this.Defs.Leveled.Where( f => (f.Position <= offset) && (f.EndPosition > offset));
		}
		
		public IEnumerable<CssFragment> FragmentsFromSection(TextRange range)
		{
			// TODO: Do some checking
			return this.Defs.Leveled.Where(
				f =>
				(f.section.Position > range.Position) &&
				(f.section.EndPosition < range.EndPosition)
			);
		}
		public int GetRowOffset(int offset)
		{
			return GetRowOffset(offset,this.Defs.Breaker);
		}
		static public int GetRowOffset(int offset, IEnumerable<CssFragment> eolPositions)
		{
			int row = 0;
			using (IEnumerator<CssFragment> items = eolPositions.GetEnumerator())
				while (items.MoveNext())
			{
				if (items.Current.Position >= offset) break;
				row++;
			}
			return row;
		}
		
		public int GetColumnOffset(int offset)
		{
			return GetColumnOffset(offset,this.Defs.Breaker);
		}
		static public int GetColumnOffset(int offset, IEnumerable<CssFragment> eolPositions)
		{
			int row = GetRowOffset(offset,eolPositions);
			if (row==0) return offset;
//			int nextOffset = row==eolPositions.Count() ? this
			CssFragment fragment = eolPositions.ElementAt(row-1);
			return offset - ((int)fragment.EndPosition);
		}
		
		/**
		 * There are some OLE/Clipboard methods.
		 * I can't quite think of where these methods
		 * are at the moment, but I would like a Text
		 * portion, Rich Text portion, and a Range
		 * Collection portion added to the clip-board.
		 */
	}
	
}
