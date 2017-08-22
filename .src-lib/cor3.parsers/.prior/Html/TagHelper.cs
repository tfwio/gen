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


namespace System.Cor3.Parsers.Html
{
	public static class TagHelper
	{
		#region CONST
		/// <summary>
		/// HTML Document Template
		/// <para>Title		$(DocumentTitle)</para>
		/// <para>Styles	$(StyleSheets)</para>
		/// <para>Style		$(InlineStyleSheet)</para>
		/// <para>Scripts	$(JavascriptIncludes)</para>
		/// <para>Body		$(DocumentBody)</para>
		/// </summary>
		public static readonly string default_document					      = ResourceUtil.GetString("TemplateHtmlDocument");
		public static readonly string DefaultStyleSheetContent			  = ResourceUtil.GetString("TemplateStyleSheetContent");
		public static readonly string DefaultJavascriptInclude			  = ResourceUtil.GetString("TemplateJavascriptInclude");
		public static readonly string DefaultJavascriptContentInclude	= ResourceUtil.GetString("TemplateJavascriptContentInclude");
		public static readonly string DefaultStyleSheetInclude			  = ResourceUtil.GetString("TemplateStyleSheetInclude");
		#endregion
		
		// Section Constants
		public const string IsEditableAttributeTag = "$(IsEditable)";
		public const string StylesheetIncludeTag   = "$(StylesheetInclude)";
		public const string StylesheetContentTag   = "$(StylesheetContent)";
		public const string JavascriptIncludeTag   = "$(JavascriptInclude)";
		public const string JavascriptContentTag   = "$(JavascriptContent)";
		public const string DocumentBaseTag        = "$(DocumentBase)";
		public const string DocumentContentTag     = "$(DocumentContent)";
		public const string DocumentTitleTag       = "$(DocumentTitle)";

		// String Extensions
		
		#region TagReplace, TagClose
		
		/// <summary>
		/// Extension Method replace a new item in place of tag and append tag once again to the end of the (input) buffer.
		/// <para>Call TagClose(tagname) to close the respective tag once there are no more replacements.</para>
		/// </summary>
		/// <param name="input"></param>
		/// <param name="tag">A Tag such as $(Tag)</param>
		/// <param name="value">The value to replace the tag.</param>
		/// <returns>A string with the tag placed at the end.</returns>
		static public string TagReplace(this string input, string tag, string value) { return input.Replace(tag,string.Concat(value,tag)); }
		
		/// <summary>
		/// Extension Method Replaces the last occurance of a string.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="tag">The tag such as $(StyleSheets) to be stripped.</param>
		/// <returns></returns>
		static public string TagClose(this string input, string tag) { return input.Replace(tag,string.Empty); }

		static public string CRLF(this string input)
		{
			return input
				.Replace("\r\n","\n")
				.Replace("\n\r","\n")
				.Replace("\r","\n")
				.Replace("\n","\r\n")
				;
		}

		#endregion
		
		#region pif
		#if METAWEBLOG
		// Post Extensions
		
		static public string TagContent(this Post post)
		{
			
			return string.Concat(post.description.CRLF(),post.mt_text_more.CRLF())
//				.EntityFromChar()
				;
		}
		
		// this does not belong here.
		/// <summary>
		/// Write/Convert the Post's content to (templated) HTML Document.
		/// <para>See the other overload for more detail.</para>
		/// </summary>
		/// <param name="page"></param>
		/// <param name="reformat">If true, then </param>
		/// <returns></returns>
		static public string ToHtmlDocument(this Post page, bool reformat) { return page.ToHtmlDocument("http://vaio/b/",reformat); }
		
		/// <summary>
		/// Write/Convert the Post's content to (templated) HTML Document.
		/// <para>If NO reformat is called for, then 'page.TagContent()' is returned.</para>
		/// <para>The inputFragment is expected to be a Document-Body Content Fragment that we are going to place into a template.</para>
		/// </summary>
		/// <param name="page"></param>
		/// <param name="baseUri"></param>
		/// <param name="reformat"></param>
		/// <returns></returns>
		static public string ToHtmlDocument(this Post page, string baseUri, bool reformat)
		{
			if (reformat) return FormatHtml(page);
			return reformat ?
				default_document
				.TagReplace(DocumentTitleTag,page.Title).TagClose(DocumentTitleTag)
				.TagReplace(DocumentContentTag,page.TagContent()).TagClose(DocumentContentTag)
				.TagReplace(DocumentBaseTag,baseUri).TagClose(DocumentBaseTag)
				.TagReplace(IsEditableAttributeTag," contentEditable='true'").TagClose(IsEditableAttributeTag)
				.StyleSheetInline(TagHelper.DefaultStyleSheetContent).CloseStyleSheetInline()
				.CloseStyleSheetIncludes()
				.CloseJavascriptInc()
				.CloseJavascriptContent() :
				page.TagContent()
				;
		}
		static public string FormatHtml(Post page)
		{
			string formatted = null;
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
			// doc.Encoding = System.Text.Encoding.UTF8;
			doc.OptionDefaultStreamEncoding = System.Text.Encoding.UTF8;
			// doc.OptionCheckSyntax = true;
			// doc.OptionFixNestedTags = true;
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(page.TagContent());
			using (MemoryStream instream = new MemoryStream(buffer,0,buffer.Length,true))
			{
				doc.Load(instream);
				using (MemoryStream outstream = new MemoryStream(buffer,0,buffer.Length,true))
				{
					doc.Save(outstream);
					formatted = System.Text.Encoding.UTF8.GetString(outstream.ToArray());
				}
			}
			doc = null;
			buffer = null;
			return formatted;
		}
		
		/// <summary>
		/// in the future it might be a worthy idea to include AvalonEdit.TextEditor.Document
		/// or at least the Editor so that we can get to the elements within
		/// to do our writing natively to the Editor.
		/// </summary>
		/// <param name="page"></param>
		/// <param name="justText"></param>
		/// <returns></returns>
		static public string HtmlArtifacts(this Post page, bool justText)
		{
			string output = string.Empty;
//			List<string> output = new List<string>();
			
			// logging something for no apparent reason.
//			output.Add("created list");
			
			// Read Html Document
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
			doc.OptionDefaultStreamEncoding = System.Text.Encoding.UTF8;
			doc.OptionCheckSyntax = false;
			doc.OptionOutputAsXml = true;
			doc.OptionWriteEmptyNodes = true;
			
			// why are we buffering tagcontent?
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(page.TagContent());
			
			using (MemoryStream instream = new MemoryStream(buffer,0,buffer.Length,true))
			{
				doc.Load(instream);
				XPathNavigator n = doc.CreateNavigator();
				n.MoveToFirst();
				output += n.Iterate(true);
			}
			doc = null;
			buffer = null;
			return output;
		}
		#endif
		#endregion
		// Template String Extensions
		
		#region Template String Extensions
		static public string StyleSheetInline(this string input, string inlineCSS) { return input.TagReplace(StylesheetContentTag,inlineCSS); }
		static public string CloseStyleSheetInline(this string input) { return input.TagClose(StylesheetContentTag); }
		
		static public string StyleSheetInclude(this string input, string includeCSS) { return input.TagReplace(StylesheetIncludeTag,includeCSS); }
		static public string CloseStyleSheetIncludes(this string input) { return input.TagClose(StylesheetIncludeTag); }
		
		static public string JavascriptContent(this string input, string inlineJS) { return input.TagReplace(JavascriptContentTag,inlineJS); }
		static public string CloseJavascriptContent(this string input) { return input.TagClose(JavascriptContentTag); }
		
		static public string JavascriptInc(this string input, string includeJS) { return input.TagReplace(JavascriptIncludeTag,includeJS); }
		static public string CloseJavascriptInc(this string input) { return input.TagClose(JavascriptIncludeTag); }
		#endregion
	}
}
