using System;
using Generator.Elements;

namespace Generator.Elements.Basic
{
	/// <summary>
	/// The basic DatabaseCollection Information.
	/// In the future – may include some exposed methods.
	/// <para>IDbSelection SelectedInstance</para>
	/// <para>DatabaseCollection SelectedCollection</para>
	/// <para>DatabaseElement SelectedDatabase</para>
	/// <para>TableElement SelectedTable</para>
	/// <para>FieldElement SelectedField</para>
	/// </summary>
	public interface IDatabaseCollection
	{
		IDatabaseCollection Instance { get; }
		
		DatabaseCollection SelectedCollection { get; set; }
		DatabaseElement SelectedDatabase { get; set; }
		TableElement SelectedTable { get; set; }
		
		DataViewElement	SelectedView { get; set; }
		DataViewLink	SelectedLink { get; set; }
		
		FieldElement SelectedField { get; set; }
	}
}
