/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Xml.Serialization;
#endregion

namespace Generator.Elements.Types
{
	public enum WebFormAttributeType
	{
		/// <summary>%URI; #REQUIRED </summary>
		action,
		/// <summary>GET|POST</summary>
		method,
		/// <summary>%ContentType;</summary>
		enctype,
		/// <summary>%ContentType;</summary>
		accept,
		/// <summary>CDATA</summary>
		name,
		/// <summary>%Script;</summary>
		onsubmit,
		/// <summary>%Script;</summary>
		onreset,
		/// <summary>%Charsets;</summary>
		[XmlEnum("accept-charset")]
		acceptCharset,
		
	}
}
