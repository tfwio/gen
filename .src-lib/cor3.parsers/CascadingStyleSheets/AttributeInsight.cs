/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Linq;
namespace System.Cor3.Parsers.CascadingStyleSheets
{
	/// <summary>
	/// Attribute Insight values derive from CSS Documentation Properties tables.
	/// </summary>
	public class AttributeInsight
	{
		public string Name { get; set; }
		public string Values { get; set; }
		public string InitialValue { get; set; }
		public string AppliesTo { get; set; }
		public string Inherited { get; set; }
		public string Percentages { get; set; }
		public string MediaGroups { get; set; }

		public AttributeInsight(string qname, string values, string initialValue, string appliesTo, string inherited, string percentages, string mediaGroups)
		{
			this.Name = qname;
			this.Values = values;
			this.InitialValue = initialValue;
			this.AppliesTo = appliesTo;
			this.Inherited = inherited;
			this.Percentages = percentages;
			this.MediaGroups = mediaGroups;
		}
	}
}
