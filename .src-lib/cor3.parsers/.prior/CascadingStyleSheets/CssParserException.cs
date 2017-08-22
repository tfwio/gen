/*
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers.CascadingStyleSheets
{
	public class CssParserException : Exception {
		public int Position { get;set; }
		public string ErrorMessage { get;set; }
		public CssParserException(int position, string message)
		{
			this.Position = position;
			this.ErrorMessage = message;
		}
	}
}
