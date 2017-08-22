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
	public struct HtmlAttributeValuePair
	{
		public string Name;
		public string Value;
		public string Content;
		public override string ToString()
		{
			return string.Format("{0}=\"{1}\"", Name, Value);
		}
		public HtmlAttributeValuePair(string Name, string Value, string Content)
		{
			this.Content	= Content;
			this.Name		= Name;
			this.Value		= Value;
		}
		public HtmlAttributeValuePair(string Name, string Value) : this(Name,Value,null)
		{
		}
	}
}
