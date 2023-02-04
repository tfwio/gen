/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Generator.Backend
{
	public class UniqueKey
	{
		#region Properties
		[XmlAttribute]
		public string name;

		[XmlElement]
		public List<FieldReference> fields;

		#endregion
		#region .Ctor
		public UniqueKey()
		{
		}

		public UniqueKey(string name, params FieldReference[] fields)
		{
			this.name = name;
			this.fields = new List<FieldReference>(fields);
		}
	#endregion
	}
}


