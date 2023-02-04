/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Generator.Elements.Types
{
	/// <summary>
	/// This is used in Generator Table and Field Parameters.
	/// <para>Convert a given string (typename) to a TypeCode.</para>
	/// <para>Use TypeCode to convert into a string parameter value.</para>
	/// </summary>
	abstract public class GeneratorTypeConverter
	{
		virtual public bool CanConvert<T>()
		{
			if (typeof(T) == typeof(TypeCode)) return true;
			return false;
		}
		abstract public string Convert(TypeCode input);
		abstract public TypeCode Convert(string input);
	}

}
