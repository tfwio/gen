/* oIo : 2/10/2011 : 9:52 PM */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Input;

using System.Cor3.Parsers;

using Generator.Data;
using Generator.Extensions;
using Generator.Elements;
using Generator.Elements.Types;

namespace Generator.Parser
{
	// this class is going to rely heavily on XHelpers
	public interface IResolveType<TTypeFrom>
		// where TTypeFrom:class // it's actually an enumeration
	{
		Type EnumerationType { get; }
		string ServiceID { get; }
		/// <summary>
		/// Usually, we would default to return a specific type if we can't figure
		/// out what the actually type is as it had been defined.
		/// </summary>
		/// <param name="dataSchema">a result of GetSchema().  You must at least provide the Tables, Columns and DataTypes of manually building this table</param>
		/// <param name="rowColumn"></param>
		/// <returns></returns>
		TTypeFrom	GetNativeType	(DataSet dataSchema, DataRowView rowColumn);
		/// <summary>
		/// This one is always going to be quite easy, the DataTypes table provides
		/// an exact value that can be used to provide this value and this function
		/// is responsible for directing that value to a TypeCode enumeration value.
		/// </summary>
		/// <param name="dataSchema">
		/// a result of GetSchema().  You must at least provide the Tables, Columns
		/// and DataTypes (as well as Indexes) of manually building this table
		/// </param>
		/// <param name="rowColumn"></param>
		/// <returns></returns>
		TypeCode	GetDataType		(DataSet dataSchema, DataRowView rowColumn);
		/// <summary></summary>
		int			GetMaxLength	(DataSet dataSchema, DataRowView rowColumn);
		/// <summary></summary>
		bool		HasMaxLength	(DataSet dataSchema, DataRowView rowColumn);
		/// <summary></summary>
		string		GetPrimaryKey	(DataSet dataSchema, DataRowView rowColumn);
	}

	public abstract class TypeResolutionClass<TTypeEnum> : IResolveType<TTypeEnum>
	{
		public Type EnumerationType { get { return typeof(TTypeEnum); } }
		virtual public string ServiceID { get { throw new NotImplementedException(); } }
		/// <summary>
		/// ANY VIEW FROM THIS CLASS MUST BE DISPOSED
		/// </summary>
		internal class SchemaNavigator
		{
			readonly DataSet ds;
			DataViewRowState rs = DataViewRowState.CurrentRows|DataViewRowState.ModifiedCurrent;
			public DataView ViewIndexes		{ get { return new DataView(ds.Tables[Gen.Strings.Schema_Indexes],"","",rs); } }
			public DataView ViewTables		{ get { return new DataView(ds.Tables[Gen.Strings.Schema_Tables],"","",rs); } }
			public DataView ViewColumns		{ get { return new DataView(ds.Tables[Gen.Strings.Schema_Columns],"","",rs); } }
			public DataView ViewDataTypes	{ get { return new DataView(ds.Tables[Gen.Strings.Schema_DataTypes],"","",rs); } }
	
			public SchemaNavigator(DataSet ds)
			{
				this.ds = ds;
			}
		}

		virtual public string GetDefaultValue(DataSet dataSchema, DataRowView rowColumn)
		{
			throw new NotImplementedException();
		}
		virtual public TTypeEnum GetNativeType(DataSet dataSchema, DataRowView rowColumn)
		{
			throw new NotImplementedException();
		}
		virtual public TypeCode GetDataType(DataSet dataSchema, DataRowView rowColumn)
		{
			throw new NotImplementedException();
		}
		virtual public int GetMaxLength(DataSet dataSchema, DataRowView rowColumn)
		{
			throw new NotImplementedException();
		}
		virtual public bool HasMaxLength(DataSet dataSchema, DataRowView rowColumn)
		{
			throw new NotImplementedException();
		}
		virtual public string GetPrimaryKey(DataSet dataSchema, DataRowView rowColumn)
		{
			throw new NotImplementedException();
		}
	}
}
