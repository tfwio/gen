/* oIo : 11/15/2010 – 2:33 AM */
#region Using
using System;
using Generator.Core.Markup;

#endregion
namespace Generator.Elements.Types
{
	/// <summary>Description of Column.</summary>
	public class FieldTemplate : MarkupTemplate<FieldElement>
	{
		public FieldTemplate() : base(null) { }
		public FieldTemplate(FieldElement value) : base(value)
		{
		}
	}
}
