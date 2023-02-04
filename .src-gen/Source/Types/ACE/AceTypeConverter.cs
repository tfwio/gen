using System;

namespace Generator.Elements.Types
{
	public class AceTypeConverter
	{
		
		/// <summary></summary>
		static public string TypeCodeToAccessTStr(int code)
		{
			// the type code will never be out of the realm of OleDbType.Values.
			switch (code) {
				case 3:
					return "Number";
				case 6:
					return "Currency";
				case 7:
					return "DateTime";
				case 130:
					return "Memo";
				case 11:
					return "YesNo";
				default:
					return null;
			}
		}
		
	}
}
