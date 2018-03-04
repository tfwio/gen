using System;
using System.Linq;
namespace Generator.Elements.Basic
{
	public interface IDataView
	{
		string Database {
			get;
			set;
		}

		string Table {
			get;
			set;
		}

		string Fields {
			get;
			set;
		}

		string Alias {
			get;
			set;
		}
	}
}


