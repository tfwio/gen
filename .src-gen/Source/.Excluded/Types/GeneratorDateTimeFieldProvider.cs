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
	public class GeneratorDateTimeFieldProvider : GeneratorTypeProvider
	{
		public override void GetTypes(IDictionary<string, object> fparams)
		{
			DateTime dt = DateTime.Now;
//			System.Windows.Forms.MessageBox.Show("it's getting called.");
			fparams.Add("DATETIME",	string.Format("{0:yyyy-MM-dd HHmm}",dt));
			fparams.Add("Date",		string.Format("{0:MM/dd/yyyy}",dt));
			fparams.Add("Time",		string.Format("{0:hh:mm.ss fff tt}",dt));
			fparams.Add("DateTime",	string.Format("{0:MM/dd/yyyy hh:mm:ss.fff tt}",dt));
		}
	}
}
