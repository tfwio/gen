using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Serialization;
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


