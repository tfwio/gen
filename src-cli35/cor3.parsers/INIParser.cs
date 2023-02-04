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
	/// This class is considered Obsolete however is not marked
	/// as such (or deleted) in the case that it's in use in a project
	/// somewhere out there.
	/// </summary>
	public class INIParser : System.IO.MemoryStream
	{
		/// <summary><tt>RegexOptions.Multiline</tt></summary>
		static public readonly RegexOptions DefaultOptions = RegexOptions.Multiline;
		/// <summary>Its an artifact that hasn't found use in this class as of yet.</summary>
		static public readonly System.Text.Encoding DefaultEncoding;

		#pragma warning disable 169
		List<TextRange> rangeBegin;
		Dictionary<string,Regex> Expressions;
		#pragma warning restore 169

		byte[] ini_buffer_data { get { return this.ToArray(); } }
		
		/// <summary>
		/// there's got to be a way to report weather or not memory is filled.
		/// </summary>
		public string IniBufferString {
			get { return DefaultEncoding.GetString(ini_buffer_data); }
			set {
				this.Flush();
				this.SetLength(0);
				byte[] pool=DefaultEncoding.GetBytes(value);
				lastWriteCount = Read(pool,0,pool.Length);
				Array.Clear(pool,0,pool.Length);
			}
		}

		int lastWriteCount = -1;
		/// <summary>
		/// A reference to the (byte[]) buffer.
		/// </summary>
		public byte[] this[int start, int length]
		{
			// we're counting on
			get {
				byte[] mini = new byte[length];
				lastWriteCount = this.Read(mini,start,length);
				return mini;
			}
			protected set { this.Write(value,start,length); }
		}
		// byte[] bytestream;
		/// <summary>
		/// Loads <strong>filename</strong> into the (<tt>byte[]</tt>) buffer.
		/// </summary>
		/// <returns>The total number of bytes written to the buffer</returns>
		/// <param name="filename">A file to be loaded into the buffer.</param>
		/// <exception cref="System.IO.FileNotFoundException">Thrown if the file is not where it was expected.</exception>
		int Load(string filename) {
			IniBufferString = null;
			if (System.IO.File.Exists(filename))
			{
				byte[] data = System.IO.File.ReadAllBytes(filename);
				lastWriteCount = Read(data,0,data.Length);
				data = null;
				return lastWriteCount;
			}
			else throw new System.IO.FileNotFoundException();
		}

		/// <summary>
		/// for the ini reader, one might like to use <see cref="RegexOptions.IgnorePatternWhitespace" />.
		/// </summary>
		virtual public void Initialize()
		{
			Expressions = new Dictionary<string, Regex>();
			// • ignore the following lines:
			Expressions.Add(ParserTypes.LineComment,new Regex(@"^\#[^\n]*",DefaultOptions));
			// • trim trailing space
			// • or find first non white-space 0x09,0x10,0x13,0x20
			Expressions.Add("standard-element",new Regex(@"(\[[^\]]*\])([\[]*)[^\s\t\n]*([^\#\[]*)",DefaultOptions));
			
		}
//		static public void Parse(Dictionary<string, Regex> dic, string value)
//		{
//			List<TextRange> list = new List<TextRange>();
//			foreach (KeyValuePair<string,Regex> dictItem in dic) {
//				foreach (Match m in dictItem.Value.Matches(value))
//				{
//					if (m.Length > 0) list.Add(TextRange.FromMatch(m));
//				}
//			}
//		}

		public INIParser(string filename)
		{
			Load(filename);
			Initialize();
		}
		public INIParser() : base()
		{
		}
	}
}