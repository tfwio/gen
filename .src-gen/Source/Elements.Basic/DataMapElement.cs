/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
#endregion

namespace Generator.Elements.Basic
{

	public interface IGeneratorElement
	{
	}
	/// <summary>
	/// provides a basic (empty) structure for type-checking
	/// map elements.
	/// </summary>
	public abstract class DataMapElement : IGeneratorElement, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected internal void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
			}
		}
		/* not to be implemented until we have a use for this guy
		internal Dictionary<string,object> _keydata = new Dictionary<string, object>();
		public Dictionary<string, object> KeyData { get { return _keydata; } set { _keydata = value; } }
		 */
		/// <summary>
		/// Get/Set
		/// <para>for automation of conection process (I don't believe this is actually used).</para>
		/// <para>
		/// an editor for these values should consist of a Combobox and Textbox
		/// with a button to add a new element or delete the current element.
		/// </para>
		/// </summary>
		[XmlIgnore] public Dictionary<string, string> ConnectionParameters {
			get { return _connectionParameters; } set { _connectionParameters = value; }
		} internal Dictionary<string,string> _connectionParameters = new Dictionary<string,string>();

		/// <summary>
		/// Get/Set
		/// </summary>
		[XmlAttribute] public string Tags {
			get { return tags; } set { tags = value; }
		} string tags = null;
	}
}
