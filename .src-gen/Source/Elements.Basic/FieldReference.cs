/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.Xml.Serialization;
using Generator.Elements.Basic;
namespace Generator.Elements.Basic
{
	public class FieldReference : DataMapElement
	{
		#region Properties
		[XmlAttribute]
		public string database;
		[XmlAttribute]
		public string field;
		#endregion
		#region .Ctor
		public FieldReference()
		{
		}
		public FieldReference(DatabaseElement db, FieldElement field)
		{
			this.database = db.Name;
			this.field = field.DataName;
		}
		#endregion
	}
	
}
