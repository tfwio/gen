/*
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 */
using System;
using MarkdownSharp;

namespace System
{
	static public class MmdExtension
	{
		static public string ToMarkdownHtml(this string input)
		{
			Markdown mmd = new Markdown();
			string output = mmd.Transform(input);
			mmd = null;
			return output;
		}
	}
}
