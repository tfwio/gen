/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */
using System;
using System.Text.RegularExpressions;
using Generator.Core.Markup;
using Global=System.Cor3.last_addon;
namespace Generator.Parser
{
	public enum FieldMatchType
	{
		None,
		FieldIf,
		FieldTemplate,
		TableTemplate,
		TableTemplateCdf,
		Set,
		Print,
		Begin,
		End,
		BeginFor,
		EndFor,
	}
	public class FieldMatch
	{
		#region Properties
		string name=string.Empty, values=string.Empty, orig=string.Empty;
		
		public string Name { get { return name; } }
		public bool HasName { get { return name!=string.Empty; } }

		public string Original { get { return orig; } }
		public bool HasOrig { get { return orig!=string.Empty; } }

		public string Values { get { return values; } }
		public bool HasValues { get { return values!=string.Empty; } }

		public string TemplateAlias { get { return !HasValues ? "Nothing" : Prams[0]; } }
		public string Var1 { get { return !HasValues ? "Nothing" : Prams[1]; } }

		public string[] Prams
		{
			get
			{
				string[] prams = Values.Split(',');
				int i = 0; while (i < prams.Length) { prams[i] = prams[i].Trim(); i++; }
				return prams;
			}
		}
		public int ParamCount {
			get
			{
				string[] prams = Prams;
				int count = prams.Length;
				prams = null;
				return count;
			}
		}
		#endregion

		#region Const
		const string format1 = @"
	Matches = {0}
	Match Values: {1}
	";
		const string format2 = @"
	Matches = {0}
	Match Values: {1}
	Groups: {2}
	Match Value[0]: {3}
	";
		#endregion

		public FieldMatchType MatchType
		{
			get
			{
				switch (Name)
				{
						case "FieldIf": return FieldMatchType.FieldIf;
						case "FieldTemplate": return FieldMatchType.FieldTemplate;
						case "TableTemplate": return FieldMatchType.TableTemplate;
						case "TableTemplateCdf": return FieldMatchType.TableTemplateCdf;
						case "begin": return FieldMatchType.Begin;
						case "end": return FieldMatchType.End;
						case "set": return FieldMatchType.Set;
						case "print": return FieldMatchType.Print;
						default: return FieldMatchType.None;
				}
			}
		}

		/// <summary>
		/// Creates a FieldMatch for
		/// </summary>
		static public FieldMatch GetMatches(string tableTemplate)
		{
			MatchCollection mc = ParseUtil.Match(
				TemplateReferenceUtil.regex_FieldAndIOMatch,
				tableTemplate);
			Logger.LogG("field-match(string)","count = {0}",mc.Count);
			if (mc==null) return null;
			FieldMatch fm = null;
			if (mc.Count >= 1) { fm = new FieldMatch(mc); }
			return fm;
		}

		static public string InfoString(MatchCollection mc)
		{
			Logger.LogG("field-match","InfoString");
			if (mc.Count == 0) return @"
no match groups…";
			string info1 = string.Format(
				format2,
				mc.Count,
				mc.Count>0 ? mc[0].Value : "no matches",
				mc[0].Groups.Count,
				mc.Count > 0 ? (mc[0].Groups.Count > 0 ? mc[0].Groups[0].Value : "no groups") : "no groups"
			);
			info1 += @"
	Groups:
";
			foreach (Group g in mc[0].Groups)
			{
				info1 += string.Format(@"		{0}
	",g.Value);
			}
			return info1;
		}

		/// <summary>
		/// See: GetMatches
		/// </summary>
		public FieldMatch(MatchCollection mc)
		{
			if (mc.Count >= 1)
			{
				Match m = mc[0];
				if (m.Groups.Count >=1)
				{
					Global.statG("orig");
					orig = mc[0].Groups[0].Value;
				}
				if (m.Groups.Count >=2)
				{
					Global.statG("name");
					name = mc[0].Groups[1].Value;
				}
				if (m.Groups.Count >=3)
				{
					Global.statG("values");
					values = mc[0].Groups[2].Value;
				}
			}
		}
	}
}
