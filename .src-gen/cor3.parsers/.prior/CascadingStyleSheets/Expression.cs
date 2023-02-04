/*
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers.CascadingStyleSheets
{

	/// <summary>
	/// Encapsulation of Regular Expression (<see cref="T:System.Text.RegularExpressions.Regex" />)
	/// including a single addition of <see cref="Color" /> for convenience.
	/// </summary>
	public class Expression
	{
		internal Regex InnerExpression;
		internal static readonly Color DefaultColor = CommonDefinitions.DefaultForegroundColor;
		internal static readonly RegexOptions DefaultOptions = RegexOptions.Multiline|RegexOptions.Compiled;
		internal static readonly RegexOptions DefaultOptionsIgnoreCase = DefaultOptions|RegexOptions.IgnoreCase;
		
		/// <summary>
		/// For use in Syntax highlighting expressions.
		/// <para>Implementation of this was for facilitating a quick regex demonstration
		/// type of application, but is just seemed like it might come in handy.</para>
		/// </summary>
		public Color Color {get;set;}
		
		/// <summary>
		/// <see cref="M:System.Text.RegularExpressions.Match" />
		/// <seealso cref="M:System.Text.RegularExpressions.Match" />
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public Match Match(string input) { return InnerExpression.Match(input); }
		public MatchCollection Matches(string input) { return InnerExpression.Matches(input); }
		public bool IsMatch(string input) { return InnerExpression.IsMatch(input); }
		
		public Expression(string input) : this(DefaultColor,input,RegexOptions.Multiline|RegexOptions.Compiled)
		{
		}
		public Expression(Color color, string input, RegexOptions options)
		{
			this.Color = color;
			this.InnerExpression = new Regex(input,options);
		}
		public Expression(Color color, Regex regex)
		{
			this.Color = color;
			this.InnerExpression = regex;
		}
		
		
		static public implicit operator Expression(string input) { return new Expression(input); }
		static public implicit operator Expression(Regex input) { return new Expression(DefaultColor,input); }
		static public implicit operator Regex(Expression input) { return new Expression(DefaultColor,input); }
	}
}
