/*
 * Created by SharpDevelop.
 * User: oIo
 * Date: 2/10/2011
 * Time: 9:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Cor3.Parsers;
using System.Text.RegularExpressions;

using Generator.Extensions;

namespace Generator.Parser
{
	/// <summary>
	/// Known to be used in SqlTemplateParser.
	/// </summary>
	/// <remarks>
	/// There appears to be a very generalized approach for using this if at all.
	/// </remarks>
	public class tempmatch
	{
		Match m;
		public string[] MethodsArray { get { return m.MMethodArray().ToArray(); } }
		public string[] ParamsArray { get { return m.MParamsArray().ToArray(); } }
		public string Method { get { return m.MMethod(); } }
		public string Params { get { return m.MParams(); } }
		public TextRange Range { get { return TextRange.FromMatch(m); } }
		public tempmatch(Match m)
		{
			this.m = m;
		}
	}
}
