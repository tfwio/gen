// Copyright (c) 2009 Daniel Grunwald
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Diagnostics;
using System.Globalization;
namespace AvalonEdit.Sample
{
	/// <summary>
	/// Represents a simple segment (Offset,Length pair) that is not automatically updated
	/// on document changes.
	/// </summary>
	struct SimpleSegment : ISegment
	{
		public int Offset, Length;
		
		int ISegment.Offset {
			get { return Offset; }
		}
		
		int ISegment.Length {
			get { return Length; }
		}
		
		public int EndOffset {
			get {
				return Offset + Length;
			}
		}
		
		public SimpleSegment(int offset, int length)
		{
			this.Offset = offset;
			this.Length = length;
		}
		
		public SimpleSegment(ISegment segment)
		{
			Debug.Assert(segment != null);
			this.Offset = segment.Offset;
			this.Length = segment.Length;
		}
		
		public override int GetHashCode()
		{
			unchecked {
				return Offset + 10301 * Length;
			}
		}
		
		public override bool Equals(object obj)
		{
			return (obj is SimpleSegment) && Equals((SimpleSegment)obj);
		}
		
		public bool Equals(SimpleSegment other)
		{
			return this.Offset == other.Offset && this.Length == other.Length;
		}
		
		public static bool operator ==(SimpleSegment left, SimpleSegment right)
		{
			return left.Equals(right);
		}
		
		public static bool operator !=(SimpleSegment left, SimpleSegment right)
		{
			return !left.Equals(right);
		}
		
		/// <inheritdoc/>
		public override string ToString()
		{
			return "[Offset=" + Offset.ToString(CultureInfo.InvariantCulture) + ", Length=" + Length.ToString(CultureInfo.InvariantCulture) + "]";
		}
	}
}
