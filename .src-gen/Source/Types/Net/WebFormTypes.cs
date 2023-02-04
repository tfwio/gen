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
	/// <summary>
	/// <seealso cref="http://www.w3.org/TR/html401/interact/forms.html#h-17.2.1">HTML401 WebForms</seealso>
	/// </summary>
	public enum WebFormTypes
	{
		NONE,
		BUTTON,
		CHECKBOX,
		FILE,
		FIELDSET,
		LEGEND,
		LABEL,
		HIDDEN,
		PASSWORD,
		RADIO,
		RESET,
		SELECT,
		SUBMIT,
		TEXT,
		TEXTAREA,
		TYPE,
	}
}
