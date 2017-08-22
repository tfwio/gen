/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 12/1/2013
 * Time: 7:26 AM
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Cor3.Parsers.Internal;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Cor3.Parsers
{
	//	public class ParserStrategySerializable
	/// <summary>
	/// Construct a parser with a parser strategy.
	/// XML Encapsulation of parser definitions.
	/// </summary>
	public class ParserStrategy : SerializableClass<ParserStrategy>
	{
		[XmlArrayItem("item",typeof(StringValue))]
		[XmlArray("InsightReference")]
		public List<StringValue> Items { get;set; }
	}
}
