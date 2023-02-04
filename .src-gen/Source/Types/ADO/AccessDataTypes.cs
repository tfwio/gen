/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Data;
#endregion

namespace Generator.Elements.Types
{

	/// <summary></summary>
	public enum AccessDataTypes
	{
		// notably, not all AutoIncr values are going to be the primary key (or are they?)
		/// 3
		AutoIncr = 3, // should be used as primary key (of course)
		/// 3
		Number = 3,
		/// 6
		Currency = 6,
		/// 7
		DateTime = 7,
		/// 11
		YesNo = 11,
		/// 130
		Hyperlink = 130,
		/// 130
		Memo = 130,
		/// 128
		Ole = 128,
		// text will have a constrained maximum length of 255 (or less)
		/// 130
		Text = 130,
	}
}
