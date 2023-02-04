/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 6/8/2011
 * Time: 11:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace System.Cor3.Parsers.Html
{

	static public class HtmlExtension
	{
		const string htmlAttrHrefStr = @" href=""{link}""";
		const string htmlAttrClassStr = @" class=""{class}""";
		const string htmlAttrClickStr = @" click=""{click}""";
		const string htmlAttrNameStr = @" name=""{name}""";
		const string htmlAttrStyleStr = @" style=""{style}""";
		
		const string htmlTagAnchorStr = @"<a{link}{class}{name}{style}{click}>{input}</a>";
		const string htmlTagFlatStr = @"<{name}{tags} />";
		const string htmlTagContentStr = @"<{name}{tags}>{content}</{name}{tags}>";
		
		/// <summary>
		/// FIXME: This is Erronious.
		/// </summary>
		static public readonly string TextArea = "textarea".ToHtmlTag(
			"{value}",
			new HtmlAttributeValuePair("name","{name}"),
			new HtmlAttributeValuePair("rows","{rows}"),
			new HtmlAttributeValuePair("cols","{cols}")
		);
		
		static public string ToHtmlTag(this string tag, params HtmlAttributeValuePair[] values)
		{
			string items = htmlTagFlatStr.Replace("{name}",tag);
			foreach (HtmlAttributeValuePair value in values)
			{
				items += value.ToString();
			}
			return items.Replace("{tags}",items);
		}
		static public string ToHtmlTag(this string tag, string content, params HtmlAttributeValuePair[] values)
		{
			string items = htmlTagContentStr.Replace("{name}",tag).Replace("{content}",content);
			List<string> list = new List<string>();
			foreach (HtmlAttributeValuePair value in values) list.Add( value.ToString() );
			string attrs = string.Join(" ",list.ToArray());
			if (string.IsNullOrEmpty(attrs)) return items.Replace("{tags}","");
			return items.Replace("{tags}",attrs);
		}
		
//		static public void Tag(this System.Web.Mvc.HtmlHelper page, string tag, string message)
//		{
//			page.ViewContext.Writer.Write(string.Format("<{tag}>{0}</{tag}>".Replace("{tag}",tag),message));
//		}
		/// <summary>General TAG impl.  This method is used internally and externally.
		/// Internally the method is used in most string-input-to-tag methods such
		/// as HtmlH1, etc…
		/// </summary>
		/// <param name="content">Content is (usually if not always) provided to the inner-text/inner-html portion of the created HTML ELEMENT.</param>
		/// <param name="tagName">The tag wrapping input <strong>content</strong>; EG: &lt;TAG /&gt;</param>
		/// <param name="attributes">any number of <see cref="HtmlAttributeValuePair" /> attributes may be added.</param>
		/// <returns></returns>
		static public string HtmlTag(this string content, string tagName, params HtmlAttributeValuePair[] attributes) {
			return tagName.ToHtmlTag(content,attributes);
		}
		
		#region Specific
//		static public void HtmlH1(this System.Web.Mvc.HtmlHelper page, string message) { page.Tag("h1",message); }
//		static public void HtmlH2(this System.Web.Mvc.HtmlHelper page, string message) { page.Tag("h2",message); }
//		static public void HtmlParagraph(this System.Web.Mvc.HtmlHelper page, string message) { page.Tag("p",message); }
//		static public void HtmlDiv(this System.Web.Mvc.HtmlHelper page, string message) { page.Tag("div",message); }
//		static public void HtmlSpan(this System.Web.Mvc.HtmlHelper page, string message) { page.Tag("span",message); }
		
		static public string HtmlH1(this string input) { return input.HtmlTag("h1"); }
		/// <summary>
		/// Create H2 ELEMENT from input.
		/// </summary>
		/// <returns>Formatted HTML using the respective tag.</returns>
		/// <param name="input">Input string.</param>
		static public string HtmlH2(this string input) { return input.HtmlTag("h2"); }
		/// <summary>
		/// Same as <see cref="HtmlParagraph" />
		/// </summary>
		/// <returns><strong>input</strong> text blocked into HTML P tags.</returns>
		/// <param name="input">Input string.</param>
		static public string HtmlPar(this string input) { return input.HtmlTag("p"); }
		/// <summary>
		/// Create a PARAGRAPH ('P') element from input.
		/// </summary>
		/// <returns>Formatted HTML using the respective tag.</returns>
		/// <param name="input">Input string.</param>
		static public string HtmlParagraph(this string input) { return input.HtmlTag("p"); }
		/// <summary>
		/// Create a DIV element from input.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns></returns>
		static public string HtmlDiv(this string input) { return input.HtmlTag("div"); }
		
		/// <summary>
		/// Create a SPAN element from input.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns></returns>
		static public string HtmlSpan(this string input) { return input.HtmlTag("span"); }
		/// <summary>
		/// Creates an ANCHOR tag from input.
		/// </summary>
		/// <param name="input">Input string provided to inner-html/text of the anchor.</param>
		/// <param name="url">The value provded to the url attribute of the ANCHOR tag.</param>
		/// <returns></returns>
		static public string HtmlA(this string input, string url)
		{
			return input.HtmlA(url,null,null,null,null);
		}
		static public string HtmlA(this string input, string url, string cls, string name, string style, string click)
		{
			return htmlTagAnchorStr
				.Replace("{link}", string.IsNullOrEmpty(url) ? " " : htmlAttrHrefStr.Replace("{link}",url) )
				.Replace("{class}", string.IsNullOrEmpty(cls) ? "" : htmlAttrClassStr.Replace("{class}",cls) )
				.Replace("{name}", string.IsNullOrEmpty(name) ? "" : htmlAttrNameStr.Replace("{name}",name) )
				.Replace("{style}", string.IsNullOrEmpty(style) ? "" : htmlAttrStyleStr.Replace("{style}",style) )
				.Replace("{click}", string.IsNullOrEmpty(click) ? "" : htmlAttrClickStr.Replace("{click}",click) )
				.Replace("{input}",input);
		}
		static public string HtmlUl(this string input) { return input.HtmlTag("ul"); }
		static public string HtmlOl(this string input) { return input.HtmlTag("ol"); }
		static public string HtmlLi(this string input) { return input.HtmlTag("li"); }
		
		#endregion
		
	}
}
