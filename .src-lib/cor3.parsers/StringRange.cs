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
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers
{
	/// <summary>
	/// The StringRage is perhaps another attempt at writing (generally the same)
	/// <see cref="TextRange" /> type.
	/// </summary>
	public struct StringRange : ITextRange
	{
		long position, length;
		System.Text.Encoding encoding;
		readonly static public System.Text.Encoding DefaultEncoder = System.Text.Encoding.Default;
		string text;

		public System.Text.Encoding Encoding { get { return encoding; } set { encoding = value; } }
		public long Position { get { return position; } set { position = value; } }
		public long Length { get { return length; } set { length = value; } }
	//		string 
		public string Text { get { return text; } set { text = value.Length >= length ? value.Substring((int)position,(int)length) : string.Copy(value); } }

		public StringRange(long len) : this(0,len,DefaultEncoder,string.Empty)
		{
		}
		public StringRange(string value) { encoding = DefaultEncoder; text = value; position = 0; length = value.Length; }
		public StringRange(long ndx, long len, string value) : this(ndx,len,DefaultEncoder,value)
		{
		}
		public StringRange(long ndx, long len, System.Text.Encoding enc, string value)
		{
			encoding = enc;
			position = ndx; length = len; text = value;
		}

		#region Utility Fun
		public string Substring(string input)
		{
			if (position > int.MaxValue)
				throw new ArgumentException();
			if (length > int.MaxValue)
				throw new ArgumentException();
			return input.Substring((int)position, (int)length);
		}
		#endregion

		public static StringRange FromMatch(Match match) { return new StringRange(match.Index, match.Length, match.Value); }
		public static StringRange Empty { get { return new StringRange(-1, 0, string.Empty); } }
		/// <summay>FromString(value,true)</summay>
		static public StringRange FromString(string value) { return FromString(value,true); }

		static public implicit operator StringRange(string range) { return StringRange.FromString(range); }
		//static public implicit operator StringRange(Stream range) { return StringRange.FromString(range); }
		//static public implicit operator TextRange(string inputstring) { return 

		static StringRange FromString(string value, bool copy)
		{
			StringRange range = new StringRange(value);
			range.text = copy ? string.Copy(value) : value;
			range.position = 0;
			range.length = range.text.Length;
			return range;
		}
		static StringRange FromStream(System.IO.MemoryStream stream, System.Text.Encoding enc)
		{
			string str = enc.GetString(stream.ToArray());
			StringRange range = new StringRange(str);
			return range;
		}
	}
}
