using System;
using System.Xml.Serialization;

namespace Generator.Core.Entities.Types
{
	public interface IEnumProvider
	{
		string Name { get; }
		Array Types { get; }
	}
	abstract public class EnumProvider : IEnumProvider
	{
		/// <summary>
		/// eventually (perhaps), we will implement a enum based category.
		/// </summary>
		virtual public string Category { get { return "Default"; } }
		
		abstract public string Name { get; }
		abstract public Array Types { get; }
		
		virtual public bool CanDoFromNative { get { return false; } }
		virtual public bool CanDoToNative { get { return false; } }
		
		abstract public TypeCode ToNative(string name);
		
		abstract public string ProvideTypeCode(TypeCode toConvert);
		
		virtual public string FromNative(string name)
		{
			TypeCode toConvert;
			bool gotConversion = Enum.TryParse<TypeCode>(name, out toConvert);
			if (!gotConversion) throw new ArgumentException("Couldn't convert from {0} to native TypeCode",name);
			return ProvideTypeCode(toConvert);
		}
		
	}
}
