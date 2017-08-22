using System;
using System.Cor3.Parsers;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Generator.Core.Markup
{
	/// <summary>
	/// 
	/// </summary>
	public struct QuickMatch
	{
		/// <summary>
		/// The Template-Tag Name.
		/// </summary>
		public string Name;
		/// <summary>
		/// 
		/// </summary>
		public string Value;
		/// <summary>
		/// if the template tag contains multiple values (delimited by comma (‘,’))
		/// then they are stored in this string array.
		/// </summary>
		public string[] Params;
		/// <summary>
		/// (This may function as two things)
		/// <para>
		/// The name of our tag, including parameters if any; (as authored in the template)
		/// </para>
		/// </summary>
		public string FullString;
		/// <summary>
		/// Initially set to ‘TextRange(-1, 0)’ or ‘TextRange.Empty’ (by the string based constructor).
		/// <para>
		/// The TextRange indicates the source location of the referenced
		/// text within a specific range of text.
		/// </para>
		/// </summary>
		public TextRange range;
		/// <summary>
		/// Thus far, we haven't any use for the range object since most
		/// usage of the QuickMatch are directly used to modify the content
		/// of a string rendering the Range un-usable.
		/// </summary>
		public TextRange Range { get { return range; } }
		/// <summary>
		/// Checks weather or not there are any parameters in the tag.
		/// <para>For table templates, there must be at least one Param to specify the template.</para>
		/// </summary>
		public bool HasParams
		{
			get
			{
				if (Params==null) return false;
				if (Params.Length==0) return false;
				return true;
			}
		}
		/// <summary>
		/// Returns true if there are two or more ‘Params’.
		/// If there are two or more params, then there are specific tables being
		/// referenced by the $(TableTemplate: templateName, tableReference1)
		/// </summary>
		public bool HasMultipleParams
		{
			get
			{
				if (!HasParams) return false;
				if (Params.Length >= 2) return true;
				return false;
			}
		}
		/// <summary>
		/// Note that Groups [0-2] are referenced in QuickMatch.
		/// <para>
		/// this call makes use of the the overload(string,string,string)
		/// and sets the TextRange.Position and TextRange.Length properties
		/// based on the values in the match—so this constructor is quite a 
		/// bit different then the (orig,name,value) overload.
		/// </para>
		/// </summary>
		/// <param name="match">A regular expression match for a respective template-tag</param>
		public QuickMatch(Match match) : this(
			match.Groups[0].Value,
			match.Groups[1].Value,
			match.Groups[2].Value)
		{
			range.Position = match.Index;
			range.Length = match.Length;
		}
		/// <summary>
		/// Note that this is a GENERAL USE overload and it does not take advantage
		/// of setting the values within the (TextRange) ‘range’ Field.
		/// </summary>
		/// <remarks>a demo tag: ‘$(TagName: param[n+0], param[n+1], param[n++], …)’</remarks>
		/// <param name="orig">The full, original string</param>
		/// <param name="name">The name of the tag $(TagName: …)</param>
		/// <param name="value">I'm not exactly sure what this is at the moment!</param>
		public QuickMatch(string orig, string name, string value)
		{
			Name = name;
			Value = string.Copy(value);
			FullString = string.Copy(orig);
			
			string[] px = Value.Split(',');
			Params = new string[px.Length];
			for (int i=0; i < px.Length; i++) Params[i] = px[i].Trim();
			range = TextRange.Empty;
		}
		/// <summary>
		/// Convert to ToolStripMenuItem.
		/// </summary>
		/// <returns>
		/// a ToolStripMenuItem with the tag set to this QuickMatch element.
		/// </returns>
		public ToolStripMenuItem ToMenu()
		{
			ToolStripMenuItem item = new ToolStripMenuItem(Params[0]);
			item.Tag = this;
			return item;
		}
	}
}
