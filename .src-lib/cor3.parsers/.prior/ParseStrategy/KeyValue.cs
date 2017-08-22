/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 12/1/2013
 * Time: 7:26 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace System.Cor3.Parsers.Internal
{
	public class KeyValue
	{
		[XmlAttribute("key")]
		virtual public string Key { get;set; }
		/// <summary>
		/// When overriding the value, we would re-name the xml-attribute to a desired name.
		/// </summary>
		[XmlAttribute("value")]
		virtual public string Value { get;set; }
	}
}
