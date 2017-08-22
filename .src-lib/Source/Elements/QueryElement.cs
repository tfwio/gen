/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.Xml.Serialization;
using Generator.Elements.Basic;

namespace Generator.Elements
{
	
	public partial class QueryElement : DataMapElement
	{
		[XmlAttribute] public string name;
		[XmlAttribute] public string source;
		[XmlAttribute] public string context;
		[XmlAttribute] public string mode;
		[XmlElement] public string sql;
		

		public QueryElement()
		{
			
		}
		public QueryElement(string name, string sql)
		{
			this.name	= name;
			this.sql	= sql;
		}
		public QueryElement(string name, string sql, string source, string context)
			: this(name,sql)
		{
			this.source = source;
			this.context = context;
		}
		
		
	}
}
