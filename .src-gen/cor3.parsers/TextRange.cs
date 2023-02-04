/*
 * Date: 5/25/2011
 * Time: 1:20 PM
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers
{
	// implementing ICSharpCode's ISegment for heling AvalonEdit TextEditor Transforms.
	public interface ISegment
	{
		int Offset {get; }
		int Length { get; }
		int EndOffset { get; }
	}
	/// <summary>
	/// The idea behind TextRange it to provide helpers for parsing
	/// text.  Generally, the TextRange is used in conjunction with
	/// <see cref="System.Text.RegularExpressions.Regex" /> and the
	/// <see cref="Regex.Matches(string)" /> inside a for loop
	/// to provide a list of matches.
	/// <example>
	/// <para>The following example demonstrates getting a list of
	/// Regular Expression Match objects from a <see cref="Regex" />
	/// <strong>Expression</strong> using <strong>inputStr</strong>
	/// as the text being searched.</para>
	/// <para>Note also that the static methods
	/// <see cref="FromMatchCollection(MatchCollection)" /> and
	/// <see cref="FromRegex" /> take care of the following.</para>
	/// <code>
	/// // …
	/// List&lt;TextRange&gt; items = new List&lt;TextRange&gt;();
	/// foreach (Match m in Epxression.Matches(inputStr))
	/// {
	/// 	items.Add(TextRange.FromMatch(m));
	/// }
	/// // …
	/// </code>
	/// <para>Note: by now we've probably implemented a method to
	/// consolidate usage of the above.</para>
	/// </example>
	/// </summary>
	public struct TextRange : ITextRange, IEquatable<TextRange>, IComparable<TextRange>, ISegment
	{
		int ISegment.Offset { get { return this.Position32; } }
		int ISegment.Length { get { return this.Length32; } }
		int ISegment.EndOffset { get { return Convert.ToInt32(this.EndPosition); } }
		
		#region public TextRange.ContainsPosition, ContainsRange and ContainsBleed
		/// <summary>
		/// Check if the reference TextRange fits within the boundary of this TextRange.
		/// </summary>
		/// <param name="source">The TextRange forming our basis.</param>
		/// <param name="reference">The element being checked.</param>
		/// <returns>TRUE if both Position and EndPosition are within the bounds of this TextRange.</returns>
		static public bool ContainsRange(TextRange source, TextRange reference)
		{
			return source.ContainsRange(reference);
		}
		
		/// <summary>
		/// Check if the reference TextRange fits within the boundary of this TextRange.
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>TRUE if both Position and EndPosition are within the bounds of this TextRange.</returns>
		public bool ContainsRange(TextRange reference)
		{
			bool containsPosition = ContainsPosition(reference);
			bool RefEndPositionIsGreaterOrEqualPos = reference.EndPosition >= this.Position;
			bool EndPositionIsGreaterOrEqualEnd = this.EndPosition >= reference.EndPosition;
			
			return containsPosition && RefEndPositionIsGreaterOrEqualPos && EndPositionIsGreaterOrEqualEnd;
		}
		/// <summary>
		/// Check if the position (offset) is contained within this TextRange.
		/// </summary>
		/// <param name="position">the reference offset/position.</param>
		/// <returns>True if the reference position is within this TextRange.</returns>
		public bool ContainsPosition(int position)
		{
			bool PositionIsLessOrEqual = this.Position <= position;
			bool PositionIsLessOrEqualEnd = this.EndPosition >= position;
			
			return PositionIsLessOrEqual && PositionIsLessOrEqualEnd;
		}
		/// <summary>
		/// Check if the reference.Position is contained within this TextRange.
		/// <para>Note that we're only checking against the start point of the element.</para>
		/// </summary>
		/// <param name="reference">the reference TextRange.</param>
		/// <returns>True if the reference section's start point is within this TextRange.</returns>
		public bool ContainsPosition(TextRange reference)
		{
			bool PositionIsLessOrEqual = this.Position <= reference.Position;
			bool PositionIsLessOrEqualEnd = this.EndPosition >= reference.Position;
			
			return PositionIsLessOrEqual && PositionIsLessOrEqualEnd;
		}
		/// <summary>
		/// Check if the section starts in this TextRange and surpasses the EndPosition.
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>TRUE if the reference position starts within and exceeds the EndPosition of this TextRange.</returns>
		/// <remarks>I can't imagine this actually being useful, but here it is.</remarks>
		public bool ContainsBleed(TextRange reference)
		{
			bool containsPosition = this.ContainsPosition(reference);
			bool ExceedsPosition = this.EndPosition < reference.EndPosition;
			return containsPosition && ExceedsPosition;
		}

		#endregion
		
		#region public ITextRange.Properties
		/// <summary>
		/// Note that <see cref="System.Convert.ToInt32(long)" /> does not detect Endian-ness.
		/// </summary>
		public long Position {
			get { return position; }
			set { position = value; }
		} long position;
		/// <summary>
		/// A 32-bit representation of the (long) this.Position value.
		/// </summary>
		public int Position32 { get { return Convert.ToInt32(this.position); } }
		
		public long Length {
			get { return length; }
			set { length = value; }
		} long length;
		
		/// <summary>
		/// A 32-bit representation of the (long) this.Length value.
		/// </summary>
		public int Length32 { get { return Convert.ToInt32(this.Length); } }
		
		#endregion
		
		#region static TextRange.Properties (readonly TextRange.Empty)

		public static TextRange Empty { get { return empty; }
		} static readonly TextRange empty = new TextRange(-1, 0);

		#endregion
		
		#region public TextRange.Properties
		
		public long EndPosition { get { return position+length; } }
		
		#endregion
		
		#region Constructor(long pos, long len)

		/// <summary>
		/// Default Contstructor
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="len"></param>
		public TextRange(long pos, long len)
		{
			position = pos;
			length = len;
		}

		#endregion
		
		#region static public TextRange.Method-Helpers
		
		static public int GetLine(TextRange range, params int[] lineIndexes)
		{
			int indexNo = 0;
			foreach (int i in lineIndexes)
			{
				if (i >= range.Length+range.Position) return indexNo;
				indexNo++;
			}
			return -1;
		}
		static public TextRange ShrinkRight(TextRange range, int amount)
		{
			TextRange range1 = range;
			range1.Position += amount;
			range1.Length-= amount;
			return range1;
		}
		static public TextRange Shrink(TextRange range, int amount)
		{
			//			int len = m.Groups[1].Index - m.Groups[0].Index;
			//			range.Position += len;
			TextRange range1 = range;
			if (amount <= range1.Length)
			{
				range1.Length-=amount;
			}
			return range1;
		}
		
		/// <summary>
		/// Provide a TextRange from Regular Expression <see cref="System.Text.RegularExpressions.Match" /> Object.
		/// </summary>
		/// <param name="match">The <see cref="System.Text.RegularExpressions.Match" />.</param>
		/// <returns></returns>
		public static TextRange FromMatch(Match match)
		{
			return new TextRange(match.Index, match.Length);
		}
		
		/// <summary>
		/// Sets text within the range to the current value within the range.
		/// <para>A utility Method.</para>
		/// </summary>
		/// <param name="position">Position in text to write to.</param>
		/// <param name="text">Text being written to.</param>
		/// <param name="textReplacement">Text to be written.</param>
		static public void OverwriteString(int position, ref string text, string textReplacement)
		{
			char[] temp = text.ToCharArray();
			textReplacement.CopyTo(0,temp,position,textReplacement.Length);
			text = temp.ToString();
			Array.Clear(temp,0,temp.Length);
			temp = null;
		}
		#endregion
		
		#region public Methods

		public int GetLine(params int[] lineIndexes)
		{
			return GetLine(this,lineIndexes);
		}

		public TextRange ShrinkRight(int amount)
		{
			TextRange tr = ShrinkRight(this,amount);
			Length=tr.Length;
			Position = tr.Position;
			return tr;
		}
		public TextRange Shrink(int amount)
		{
			TextRange tr = Shrink(this,amount);
			Length=tr.Length;
			Position = tr.Position;
			return tr;
		}

		#endregion
		
		#region public ITextRange.Methods

		/// <summary>
		/// Returns a substring from the given input string.
		/// <para>ArgumentException: if position or length is greater then int.MaxValue</para>
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Substring(string input)
		{
			if (position > int.MaxValue) throw new ArgumentException();
			if (length > int.MaxValue) throw new ArgumentException();
			return input.Substring((int)position, (int)length);
		}

		#endregion
		
		#region IComparable<TextRange>.Compare, and static Compare method.

		/// <summary>Only Compares position</summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public int CompareTo(TextRange b)
		{
			return CompareTo(this,b);
		}
		/// <summary>Only Compares position</summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		static public int CompareTo(TextRange a, TextRange b)
		{
			return a.Position.CompareTo(b.Position);
		}

		#endregion

		#region IEquatable<TextRange>, GetHashCode impl.
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * position.GetHashCode();
				hashCode += 1000000009 * length.GetHashCode();
			}
			return hashCode;
		}
		
		/// <summary>
		/// Does not compare Strings.  Compares TextRange properties Position and Length.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return (obj is TextRange) && Equals((TextRange)obj);
		}
		
		/// <summary>
		/// DOES NOT COMPARE STRING!
		/// <para>This method compares TextRange element Position and Length properties.</para>
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(TextRange other)
		{
			return this.position == other.position && this.length == other.length;
		}
		
		#endregion

		#region static operators '==' and '!=' (dependant upon IEquatable Properties)

		public static bool operator ==(TextRange lhs, TextRange rhs)
		{
			return lhs.Equals(rhs);
		}
		public static bool operator !=(TextRange lhs, TextRange rhs)
		{
			return !(lhs == rhs);
		}

		#endregion
		
		#region Object.ToString()

		public override string ToString()
		{
			return string.Format("Text Range {{ Position = {0}, Length = {1} }}",Position,Length);
		}

		#endregion
		
		#region IndexOf(ParserBlock)
		/// <summary>
		/// See the main main <see cref="IndexOf(ParserBlock,string,int,int,System.StringComparison,bool)">IndexOf</see>
		/// overload for full reference.<br />
		/// <see cref="System.StringComparison" /> = StringComparison.InvarientCulture by default.
		/// </summary>
		/// <returns>
		/// <para>
		/// The Zero based position of value if that ParserBlock is found, or -1
		/// if not. If input is String.Empty, the return value is 0.
		/// </para>
		/// <para>
		/// Note that if textRangePositionIsZero is TRUE then our result value will be relative
		/// to the TextRange's offset (this.Position is zero) within the input and not the input
		/// string's starting point.
		/// </para>
		/// </returns>
		/// <param name="value"></param>
		/// <param name="input">The string being searched.
		/// Note that a section within the string is searched using this TextRange.Position, which is rarely zero.
		/// </param>
		/// <param name="textRangePositionIsZero">(To be used with caution) See overloads.</param>
		public int IndexOf(ParserBlock value, string input, bool textRangePositionIsZero)
		{
			return this.IndexOf(value, input, this.Position32, this.Length32, System.StringComparison.InvariantCulture, textRangePositionIsZero);
		}
		/// <summary>
		/// Reports the index of the first occurance of the specified
		/// ParserBlock in the instance of the provided input string.
		/// </summary>
		/// <remarks>
		/// Note that this overload is generally provided to enable less extensive
		/// parameter-bound overloads of this operation.
		/// <para>Q:
		/// Hey Tom, why not just use a Regular Expression?
		/// </para>
		/// <para>Tom: I hadn't thought of it yet.  This <oblique>might</oblique> be faster then a compiled regular expression.</para>
		/// <para>Note that this method still depends on <see cref="String.IndexOf(string,int,int,StringComparison)" />, so the
		/// Exceptions and <see cref="System.Globalization.CultureInfo" /> handles just the same.</para>
		/// </remarks>
		/// <returns>
		/// <para>
		/// The Zero based position of value if that ParserBlock is found, or -1
		/// if not. If input is String.Empty, the return value is 0.
		/// </para>
		/// <para>
		/// Note that if textRangePositionIsZero is TRUE then our result value will be relative
		/// to the TextRange's offset (this.Position is zero) within the input and not the input
		/// string's starting point.
		/// </para>
		/// </returns>
		/// <param name="value">ParserBlock providing 'Start' and 'End' criterium to index.</param>
		/// <param name="input">the string to seek.</param>
		/// <param name="comparisonType">One of the <see cref="System.StringComparison" /> values.</param>
		/// <param name="textRangePositionIsZero">
		/// If TRUE, then <tt>Position = 0</tt> where Offset/Position of this textrange
		/// within the provided input String is treated as Zero.
		/// <para>
		/// A value of FALSE will provide the actual offsets with respect to the input string.
		/// </para>
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// input is null.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">startindex being provided by the TextRange.Position
		/// -count- being provided by TextRange.Length.
		/// Count or startIndex is negative. -or- count plus startIndex specify a position not within this instance.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// comparisonType is not a valid System.StringComparison value.
		/// </exception>
		public int IndexOf(ParserBlock value, string input, StringComparison comparisonType, bool textRangePositionIsZero)
		{
			return this.IndexOf(value,input,this.Position32,this.Length32, comparisonType,textRangePositionIsZero);
		}
		/// <summary>
		/// See <see cref="IndexOf(ParserBlock,string,int,int,System.StringComparison,bool)" /> for documentation.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="input"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <param name="textRangePositionIsZero"></param>
		/// <returns></returns>
		public int IndexOf(ParserBlock value, string input, int startIndex, int count, bool textRangePositionIsZero)
		{
			return this.IndexOf(value,input,startIndex,count,System.StringComparison.InvariantCulture,textRangePositionIsZero);
		}
		/// <summary>
		/// Reports the index of the first occurance of the specified
		/// ParserBlock in the instance of the provided input string.
		/// </summary>
		/// <remarks>
		/// Note that this overload is generally provided to enable less extensive
		/// parameter-bound overloads of this operation.
		/// <para>Q:
		/// Hey Tom, why not just use a Regular Expression?
		/// </para>
		/// <para>Tom: I hadn't thought of it yet.  This <oblique>might</oblique> be faster then a compiled regular expression.</para>
		/// <para>Note that this method still depends on <see cref="String.IndexOf(string,int,int,StringComparison)" />, so the
		/// Exceptions and <see cref="System.Globalization.CultureInfo" /> handles just the same.</para>
		/// </remarks>
		/// <returns>
		/// <para>
		/// The Zero based position of value if that ParserBlock is found, or -1
		/// if not. If input is String.Empty, the return value is 0.
		/// </para>
		/// <para>
		/// Note that if textRangePositionIsZero is TRUE then our result value will be relative
		/// to the TextRange's offset (this.Position is zero) within the input and not the input
		/// string's starting point.
		/// </para>
		/// </returns>
		/// <param name="value">ParserBlock providing 'Start' and 'End' criterium to index.</param>
		/// <param name="input">the string to seek.</param>
		/// <param name="startIndex">The search starting position.</param>
		/// <param name="count">the number of character positions to examine.</param>
		/// <param name="comparisonType">One of the <see cref="System.StringComparison" /> values.</param>
		/// <param name="textRangePositionIsZero">
		/// If TRUE, then <tt>Position = 0</tt> where Offset/Position of this textrange
		/// within the provided input String is treated as Zero.
		/// <para>
		/// A value of FALSE will provide the actual offsets with respect to the input string.
		/// </para>
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// input is null.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// count or startIndex is negative. -or- count plus startIndex specify a position not within this instance.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// comparisonType is not a valid System.StringComparison value.
		/// </exception>
		public int IndexOf(ParserBlock value, string input, int startIndex, int count, StringComparison comparisonType, bool textRangePositionIsZero)
		{
//			string toMatch = this.Substring(input);
			int indexOfA = input.IndexOf(value.BlockBegin,startIndex,count,comparisonType);
			int indexOfB = input.IndexOf(value.BlockEnd,startIndex,count,comparisonType);
//			toMatch = null;
			bool hasIndexA = indexOfA > -1, hasIndexB = indexOfB > -1;
			if (hasIndexA && hasIndexB)
			{
				if (indexOfA < indexOfB) return textRangePositionIsZero ? indexOfA : indexOfA+this.Position32;
				else return textRangePositionIsZero ? indexOfB : indexOfB+this.Position32;
			}
			else if (hasIndexA && !hasIndexB) return textRangePositionIsZero ? indexOfA : indexOfA+this.Position32;
			else if (!hasIndexA && hasIndexB) return textRangePositionIsZero ? indexOfB : indexOfB+this.Position32;
			return -1;
		}

		#endregion

		#region Helpers: ExecuteExpression(string,Regex), Shift(int,bool)
		
		/// <summary>
		/// I can't really say how helpful this is.
		/// </summary>
		/// <remarks>
		/// This method has not been tested.
		/// It should be.
		/// We might have to subtract 1 from the resulting position.
		/// </remarks>
		/// <param name="text"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public List<TextRange> ExecuteExpression(string text, Regex expression)
		{
			List<TextRange> list = new List<TextRange>();
			string subString = this.Substring(text);
			foreach (Match m in expression.Matches(subString)) list.Add(TextRange.FromMatch(m));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Shift(this.Position32,true);
			}
			subString = null;
			return list;
		}
		/// <summary>
		/// Shifts the position of the textrange up or down if value
		/// is positive or negative.
		/// <para>Length of the string is not effected.</para>
		/// </summary>
		/// <returns>A TextRange with the applied offset.</returns>
		/// <param name="value">An offset value; Positive adds to the postiion, negative subtracts.</param>
		/// <param name="apply">Weather or not to apply the offset to this TextRange.</param>
		public TextRange Shift(int value, bool apply)
		{
			if (apply) {
				this.Position += value;
				return this;
			}
			return new TextRange(this.Position+value, this.Length);
			
		}
		
//		public TextRange SubRange(TextRange reference)
//		{
//		}
//		static public TextRange operator +(TextRange a, TextRange b)
//		{
//			return new TextRange(a.Position + b.Position, a.Length + b.Length);
//		}
		#endregion
		
		/// <summary>
		/// Get a list of TextRange objects from a provided <see cref="MatchCollection" />.
		/// </summary>
		/// <returns>A the requested List&lt;TextRange&gt;.</returns>
		/// <param name="matchCollection"></param>
		static public List<TextRange> FromMatchCollection(MatchCollection matchCollection)
		{
			List<TextRange> result = new List<TextRange>();
			foreach (Match m in matchCollection) result.Add(TextRange.FromMatch(m));
			return result;
		}
		/// <summary>
		/// Provides a <see cref="List{TextRange}" /> from the provided
		/// Regular Expression (<see cref="Regex" />).
		/// <para>Note that if Regular-Expression Grouping constructs are
		/// used by the expression you're going to end up with bunches of
		/// artifacts (and/or redundant Match objects).
		/// </para>
		/// <h3>See</h3>
		/// <ul>
		/// <li><see cref="Regex" /></li>
		/// <li><see cref="Match" /></li>
		/// <li><see cref="MatchCollection" /></li>
		/// </ul>
		/// </summary>
		/// <returns>An empty list if there are no results, or the result-set of text-ranges.</returns>
		/// <param name="regex"></param>
		/// <param name="input"></param>
		static public List<TextRange> FromRegex(Regex regex, string input)
		{
			return FromMatchCollection(regex.Matches(input));
		}
	}
}
