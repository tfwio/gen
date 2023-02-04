/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Generator.Core.Entities.Types
{
	abstract public class GeneratorTypeProvider
	{
		abstract public /*IDictionary<string,string>*/void GetTypes(IDictionary<string,object> input);
		static public void GetTypes<T>(IDictionary<string,object> input) where T:GeneratorTypeProvider, new()
		{
			T t = new T();
			t.GetTypes(input);
			t = null;
		}
	}
}
