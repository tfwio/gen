//---------------------------------------------------------------------------
// 
// File: HtmlParser.cs
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// Description: Parser for Html-to-Xaml converter
//
//---------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;

// StringBuilder
// important TODOS:

namespace System.Cor3.Parsers.Html
{

	static public class ConvertionsExtension
	{
		static readonly Regex WhitespaceExpression = new Regex(@"^[\n\r\s\t]*|[\r\n\t\s]*$",RegexOptions.IgnoreCase|RegexOptions.Compiled|RegexOptions.CultureInvariant);
		
		static int GetDepth(this XPathNavigator nav)
		{
			int num = 0;
			while (nav.MoveToParent()) num++;
			return num;
		}
		
		static public bool HasFlag(this BlockWriteInfo input, BlockWriteInfo flag)
		{
			int i = (int) input;
			int f = (int) flag;
			return (i & f) == f;
		}
		
		static public bool IsBlocking(this XPathNavigator n)
		{
			if (n==null) return false;
			BlockWriteInfo mode = n.GetBlockType();
			bool returns = false;
			if (mode.HasFlag(BlockWriteInfo.Block) || mode.HasFlag(BlockWriteInfo.Unknown)) returns = true;
			mode = default(BlockWriteInfo);
			return returns;
		}
		
		static public bool IsCleanWhiteSpacePre(this XPathNavigator n)
		{
			return n.GetBlockType().HasFlag(BlockWriteInfo.CleanWhitespacePre);
		}
		
		static public bool IsLinePost(this XPathNavigator n)
		{
			return n.GetBlockType().HasFlag(BlockWriteInfo.LinePost);
		}
		
		static public bool IsDocument(this XPathNavigator n)
		{
			if (n==null) return false;
			return n.Name=="#document";
		}
		
		static public BlockWriteInfo PeekParentType(this XPathNavigator n)
		{
			XPathNavigator node = n.PeekParent();
			return node.GetBlockType();
		}
		
		/// <summary>
		/// Gets the Next Logical Block; Usually called from #text, in which
		/// case it seeks to the parent, and retrieves the sibling.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		static public XPathNavigator LogicalBlockNext(this XPathNavigator n)
		{
			XPathNavigator node = n.Clone();
			XPathNavigator parent = n.Clone();
			if (node.NodeType==XPathNodeType.Text) {
				parent.MoveToParent();
				node.MoveToParent();
			}
			
			while (node.MoveToNext()) {  }
			
//			if (node.ComparePosition())
				
				return node;
		}
		
		static public BlockWriteInfo LogicalBlockPrev(this XPathNavigator n)
		{
			XPathNavigator node = n.PeekPrev();
			return node.GetBlockType();
		}
		
		static public bool IsNull(this BlockWriteInfo info)
		{
			return (int) info == 0;
		}
		
		static public BlockWriteInfo GetBlockType(this XPathNavigator n)
		{
			if (n==null) return 0;
			BlockWriteInfo mode = 0;
			switch (n.Name)
			{
				case "br": case "hr":
					mode = BlockWriteInfo.Inline|BlockWriteInfo.LinePost;
					break;
				case "span":
				case "b":
				case "bold":
				case "oblique":
				case "strikeout": case "strikethrough": // is that even right?
				case "font":
				case "strong":
				case "a":
//					XPathNavigator prev = n.PeekPrev();
					mode = BlockWriteInfo.Inline;
//					if (prev!=null && prev.Value[prev.Value.Length-1]==' ') mode ^= BlockWriteInfo.CleanWhitespacePre;
//					prev = n.PeekNext();
//					if (prev!=null && prev.Value[0]==' ') mode ^= BlockWriteInfo.CleanWhitespacePost;
					break;
				case "#text":
					mode = BlockWriteInfo.Inline;
					if (string.IsNullOrEmpty(n.Value.RemoveWhitespace())) mode |= BlockWriteInfo.CleanWhitespace;
//					BlockWriteInfo tpar = n.PeekParentType(), tnext = n.PeekNextType(), tprev = n.PeekPrevType();
//					XPathNavigator parent = n.PeekParent();
//					bool hasPrev = !tprev.IsNull(), hasNext = !tnext.IsNull(), isdoc = parent.IsDocument();
//					if (hasPrev && !tprev.HasFlag(BlockWriteInfo.Block) && tprev.HasFlag(BlockWriteInfo.Inline))
//						mode |= BlockWriteInfo.CleanWhitespacePre;
//					if (hasNext && !tnext.HasFlag(BlockWriteInfo.Block) && tnext.HasFlag(BlockWriteInfo.Inline))
//						mode |= BlockWriteInfo.CleanWhitespacePost;
					
					break;
				case "li":
					mode = BlockWriteInfo.InlineBlock|BlockWriteInfo.CleanWhitespace|BlockWriteInfo.LinePost;
					break;
				case "header":
				case "h1": case "h2": case "h3": case "h4": case "h5": case "h6":
				case "p":
					mode = BlockWriteInfo.InlineBlock|BlockWriteInfo.LinePost|BlockWriteInfo.CleanWhitespace;
					break;
//				case "p": case "ul": case "ol": case "div": case "table": case "section":
//					return true;
				case "ul":
				case "ol":
				case "div":
					mode = BlockWriteInfo.Block|BlockWriteInfo.LinePost|BlockWriteInfo.LineContentWrap|BlockWriteInfo.CleanWhitespace;
					break;
				case "#document":
				default:
					mode = BlockWriteInfo.Block|BlockWriteInfo.CleanWhitespace;
					break;
			}
			
			return mode;
			
		}
		
		public static XPathNavigator PeekParent(this XPathNavigator n)
		{
			XPathNavigator p = n.Clone();
			bool hasParent = p.MoveToParent();
			if (!hasParent) {
				p = default(XPathNavigator);
				return null;
			}
			return p;
		}
		
		public static XPathNavigator PeekNext(this XPathNavigator n)
		{
			XPathNavigator p = n.Clone();
			bool hasParent = p.MoveToNext();
			if (!hasParent) {
				p = default(XPathNavigator);
				return null;
			}
			return p;
		}
		
		public static XPathNavigator PeekPrev(this XPathNavigator n)
		{
			XPathNavigator p = n.Clone();
			bool hasParent = p.MoveToPrevious();
			if (!hasParent) {
				p = default(XPathNavigator);
				return null;
			}
			return p;
		}
		
		public static void PrintNode(this XPathNavigator n, List<string> output, bool justText)
		{
			output.Add(n.Iterate(justText));
		}
		
		public static string NodeInfo(this XPathNavigator n, bool justText)
		{
			string temp = n.Value;
			XPathNavigator P = n.Clone();
			bool isclean = n.IsCleanWhiteSpacePre();
			
//			bool islinepost = false;
			
			if (P.MoveToParent() && !isclean)
			{
				if (P.IsCleanWhiteSpacePre() && n.Name=="#text") isclean = true;
			}
			P = default(XPathNavigator); // cleanup
			
			if (isclean) temp = temp.RemoveWhitespace();
			if (n.IsEmptyElement && !n.HasChildren)
			{
				string t = isclean ? temp : n.Value.RemoveWhitespace();
				if (string.IsNullOrEmpty(temp)) {
					if (n.Name=="#text") return string.Empty;
					if (n.Name=="#document") return string.Empty;
					return string.Format("<{0} />\n", n.Name, "@nothing");
				}
			}
			else if (n.HasChildren)
			{
				return string.Empty; //string.Format("@{0} {{ NodeType: {1} }}\n", n.Name, "@child");
			}
			
			string fmt = /*islinepost ? "{0}\n" : */"{0}";
			return justText ? string.Format(fmt,temp) : string.Format("@{0} {{ NodeType: {1}, HasChildren: {2}, IsEmptyElement: {3}, Value: {4} }}\n", n.Name, n.NodeType, n.HasChildren, n.IsEmptyElement, n.IsEmptyElement ? n.Value : "@child");
		}
		
		public static string Iterate(this XPathNavigator n, bool justText)
		{
			XPathNavigator c = n.Clone();
			BlockWriteInfo nfo = n.GetBlockType();
			bool istext = n.Name=="#text" || n.Name=="#document";
			string name = c.Name;
			string temp = c.Value.RemoveWhitespace(true);
			bool hasContent = !string.IsNullOrEmpty(temp);
			temp = default(string);
			string output = hasContent ? c.NodeInfo(justText) : string.Empty;
			c.MoveToFirstChild();
			{
				output += c.HasChildren ? c.Iterate(justText) : c.NodeInfo(justText);
			}
			while (c.MoveToNext()) output += c.HasChildren ? c.Iterate(justText) : c.NodeInfo(justText);
			
			string ofmt = istext ? "{epre}{cpre}{1}{cpost}{epost}" : "{epre}<{0}>{cpre}{1}{cpost}</{0}>{epost}";
			ofmt = ofmt
				.Replace("{cpre}",nfo.HasFlag(BlockWriteInfo.LinePreContent) ? "\n" : string.Empty)
				.Replace("{cpost}",nfo.HasFlag(BlockWriteInfo.LinePostContent) ? "\n" : string.Empty)
				.Replace("{epre}",nfo.HasFlag(BlockWriteInfo.LinePre) ? "\n" : string.Empty)
				.Replace("{epost}",nfo.HasFlag(BlockWriteInfo.LinePost) ? "\n" : string.Empty);
			System.Diagnostics.Debug.Print("{0} {{ Flags: {1:X6} }}", n.Name, Convert.ToInt32(nfo));
			output = nfo.HasFlag(BlockWriteInfo.CleanWhitespacePre) ? output.TrimStart() : output;
			output = nfo.HasFlag(BlockWriteInfo.CleanWhitespacePost) ? output.TrimEnd() : output;
			// we could check to see if the element contains blocking siblings, in which case we would certainly
			// pad the element with space if non-blocking.
			return string.Format(ofmt, name, output );
		}
		
		public static string PrintText(this XPathNavigator n, bool justText)
		{
			string o = string.Empty;
			string result = n.Value.RemoveWhitespace();
			if (n.IsEmptyElement && n.HasChildren && !string.IsNullOrEmpty(n.Value)) o += n.Iterate(justText);
			else if (n.IsEmptyElement && !n.HasChildren) o+= string.Empty;
			else if (!n.HasChildren && !n.IsEmptyElement && string.IsNullOrEmpty(result)) o+= string.Empty;
			else o += n.NodeInfo(justText);
			return o;
		}
		
		public static string RemoveWhitespace(this string input, bool htmlDecode)
		{
			if (htmlDecode) return WhitespaceExpression.Replace(input.HtmlDecode(),string.Empty);
			return WhitespaceExpression.Replace(input,string.Empty);
		}
		
		public static string HtmlDecode(this string input) { return System.Web.HttpUtility.HtmlDecode(input); }
		
		/// <summary>
		/// Overload: <see cref="RemoveWhitespace(string,bool)" /> (false);
		/// </summary>
		/// <param name="input">string</param>
		/// <returns>string with whitespace trimmed from the beginning and end.</returns>
		public static string RemoveWhitespace(this string input)
		{
			return input.RemoveWhitespace(false);
		}
		
		public static string EntityFromChar(this string input)
		{
			string output = HttpUtility.HtmlDecode(input);
			XLog.Clear();
			foreach (KeyValuePair<string,string> k in CharacterEntityConversions.Conversions)
			{
				if (input.Contains(k.Key)) XLog.WriteY("Entity",": {0} — {1}\n",k.Key,k.Value);
				output = output.Replace(k.Key,k.Value);
			}
			return output;
		}
		
		public static string EntityToChar(this string input)
		{
			string output = HttpUtility.HtmlDecode(input);
			foreach (KeyValuePair<string,string> k in CharacterEntityConversions.Conversions)
			{
				output = output.Replace(k.Value,k.Key);
			}
			return output;
		}
		
//		static public string EntityFromChar(this string input)
//		{
//			return System.Web.HttpUtility.HtmlDecode(input)
//				.Replace("’","'").Replace("‘","'").Replace("“","\"")
//				.Replace("”","\"").Replace("…","…").Replace("–","--")
//				.Replace("…","...").Replace("—","---");
//		}
		
		static public string RegexReplace(this string input, string pattern, string replacement)
		{
			
			return System.Text.RegularExpressions.Regex.Replace(input,pattern,replacement);
		}
		
		static public string ReformatSection(this string f2hInput)
		{
			return f2hInput
				.RegexReplace("<Section [^>]*>",@"<FlowDocument xml:space=""preserve"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">")
				.RegexReplace("</Section>",@"</FlowDocument>")
				;
		}
		
	}
}
