/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 07/18/2011
 * Time: 07:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Collections.Generic;
using System.Cor3.Parsers;
using System.Data;
using System.Text.RegularExpressions;

using Generator.Core.Schemas;
using Generator.Core.Types;
using Generator.Parser;
using Generator.Extensions;

#endregion

namespace Generator.Parser
{

	/// <summary>
	/// Description of Parser. (don't forget to map DEFAULT VALUE)
	/// TODO: This code is to be moved to System.Cor3.Data
	/// </summary>
	static public class SqlTemplateParser
	{
		// note that in-stead of using the following method, it might
		// proove interesting (and easier to comply with) to use a XmlWriter.
		#region Constants xml: DatabaseCollection, Database, Table, Field
		// Schema=""%TABLE_SCHEMA%""
		// Type=""%TABLE_TYPE%""

		#region TODO: FEATURED (CONDITIONAL) ACTION
		static readonly string[] checkAccess = Strings.Schema_Access_Fields.Split(',');

		// static bool ignoreAceSys = true;
		static bool IsNotAceSystem(string tablename)
		{
			List<string> ilist = new List<string>(checkAccess);
			foreach (string table in checkAccess)
				if (table==tablename) return false;
			return true;
		}

		#endregion

		#endregion

		#region Parse SQL Template And Field
/// <summary>
		/// If any commands are present within the input string, then the command portion is
		/// replaced with the results of the command.  Note that this is to work like a template.
		/// <para>
		/// • We're dealing with schema-info in the dataset and must convert from the schema info to
		/// a XML string of data that can be used as a DatabaseCollection with tables and fields.
		/// </para>
		/// <para>
		/// • The idea is to support SqlServer and Access databases (using OLE) natively
		/// </para>
		/// <remarks>
		/// I believe that this method is to parse a data-schema into a DataConfig.
		/// </remarks>
		/// </summary>
		/// <param name="input">is a string which might contain commands</param>
		/// <param name="ds">The DataSet containing relevant Schema information (Tables, Columns &amp; DataType tables are required)</param>
		/// <returns>All recognized tags are replaced with the command values.</returns>
		static public string ParseSqlTemplate(string input, DataSet ds)
		{
			// Starting out with a clone of our input.
			string output = string.Copy(input);
			// Loop through and find all the matches
			foreach (Match m in input.getmatches())
			{
				var list = new List<string>();
				var tm = new tempmatch(m);

				if (tm.MethodsArray.Length != 3) continue;
				if ((tm.MethodsArray[0]=="list") && (tm.MethodsArray[1]=="columns"))
				{
					System.Windows.Forms.MessageBox.Show("listing cols");
					foreach (DataRowView row in ds.Tables[Strings.Schema_Columns].DefaultView)
					{
						if (row["TABLE_NAME"].ToString().ToLower().Equals(tm.MethodsArray[2].ToLower()))
							list.Add(string.Format("{0}",row["COLUMN_NAME"]));
					}
				}
				else if ((tm.MethodsArray[0]=="list") && (tm.MethodsArray[1]=="tables"))
				{
					System.Windows.Forms.MessageBox.Show("listing tables");
					foreach (DataRowView row in ds.Tables[Strings.Schema_Tables].DefaultView)
					{
						string tname = row["TABLE_NAME"].ToString();
						if (ds.DataSetName==SchemaExtension.ole_ace12)
							if (!IsNotAceSystem(tname)) list.Add(string.Format("{0}",tname));
						else if (row["TABLE_CATALOG"].ToString().ToLower().Equals(tm.MethodsArray[2].ToLower()))
							list.Add(string.Format("{0}",tname));
					}
				}
				// PRIMARY SECTION
				else if ((tm.MethodsArray[0]=="show") && (tm.MethodsArray[1]=="tables"))
				{
					System.Windows.Forms.MessageBox.Show("show tables");
					//%TABLE_CATALOG%,%TABLE_NAME%,%COLUMN_NAME%,%IS_NULLABLE%
					//%TABLE_SCHEMA%,%TABLE_NAME%,%TABLE_TYPE%
					foreach (DataRowView row in ds.Tables[Strings.Schema_Tables].DefaultView)
					{
						if (IsNotAceSystem(row["TABLE_NAME"].ToString()))
							FieldElementLineFromRow(ds,row,list,tm);
					}
				}
				
				string noob = string.Join("",list.ToArray());
				output = output.Replace(tm.Range.Substring(input),noob);
			}
			return Strings.Xml_DatabaseCollection.Replace(
				"{inner-content}",
				Strings.Xml_Database.Replace("{inner-content}",output)
			);
//			list.Add(tm.Range.Substring(input),string.Format("{0}",ds.Tables[tm.Params]));
		}

		/// <summary>
		/// This is the most important part of converting data from a schema to a DatabaseElement.
		/// It acts on a row that represents a table.
		/// </summary>
		/// <remarks>
		/// Used in ParseSqlTemplate; This only seems to work for SqlServer.
		/// </remarks>
		/// <param name="ds"></param>
		/// <param name="row">Schema: “Tables”</param>
		/// <param name="list"></param>
		/// <param name="tm"></param>
		static public void FieldElementLineFromRow(DataSet ds, DataRowView row, IList<string> list, tempmatch tm)
		{
			if (ds.DataSetName==SchemaExtension.ole_ace12)
			{
				// we need to know the data types;
//				var customers = CompiledQuery.Compile(
//					(ds.Tables["DATA_TYPES"] context, string filterCountry) =>
				//                        from c in context.Customers
				//                        where c.Orders.Count > 5
				//                        select c);

				string newport = Strings.Xml_Table
					.ReplaceFieldsT(ds,row,"TABLE_NAME","TABLE_SCHEMA","TABLE_TYPE")
					.Replace("{inner-content}","".FilterColumns(ds,row["TABLE_NAME"].ToString(),true));
				list.Add(newport);
			}
			// if this is the table that we're looking for
			else if (row["TABLE_CATALOG"].ToString().ToLower().Equals(tm.MethodsArray[2].ToLower()))
			{
				string newport = Strings.Tbl_TableElement
					.ReplaceFieldsT(ds,row,"TABLE_NAME","TABLE_SCHEMA","TABLE_TYPE")
					.Replace("{inner-content}","".FilterColumns(ds,row["TABLE_NAME"].ToString()));
				list.Add(newport);
			}
			// this is not sound: a default action
			else list.Add(
				"\n{tcatalog}, {tschema}, {tname}"
				.REPLACE(
					new REPLACEMENT("{tcatalog}",row.GetString("TABLE_CATALOG")),
					new REPLACEMENT("{tschema}",row.GetString("TABLE_SCHEMA")),
					new REPLACEMENT("{tname}",row.GetString("TABLE_NAME"))
				)
			);
		}

		#endregion
		
		#region Filter Columns
		static public string FilterColumns(this string input, DataSet ds, string table)
		{
			return input.FilterColumns(ds,table,false);
		}

		/// <summary>
		/// Act on schema-Columns-row.
		/// The hard part is getting the Max-Length and Two DataTypes from correlation to
		/// DataTypes table.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="ds"></param>
		/// <param name="table"></param>
		/// <returns></returns>
		static public string FilterColumns(this string input, DataSet ds, string table, bool ace)
		{
			List<string> list = new List<string>();
			string fmt = "";
			if (string.IsNullOrEmpty(fmt)) fmt = Strings.Tbl_FieldElement;
			using (SchemaStateViewBase view = new SchemaStateViewBase(ds,table))
			{
//				System.Windows.MessageBox.Show("how many?",view.viewCols.Count.ToString());
				foreach (DataRowView row in view.viewCols)
				{
					string nstr = fmt.ReplaceFieldsC(ds,row,"TABLE_NAME","COLUMN_NAME","ORDINAL_POSITION","IS_NULLABLE","NATIVE_TYPE","MAX_LENGTH");
					if (ace)
					{
//						AccessDataTypes adt = (AccessDataTypes)int.Parse(row["DATA_TYPE"].ToString());
						nstr = nstr
							.Replace("%DATA_TYPE%",row.AceStrAceType("DATA_TYPE"))
							.Replace("%NATIVE_TYPE%",row.AceStrTypeCode("DATA_TYPE"))
							.Replace("%CHARACTER_MAXIMUM_LENGTH%","-1");
//						switch (adt)
//						{
//							case AccessDataTypes.AutoIncr:
//							case AccessDataTypes.Currency:
//							case AccessDataTypes.DateTime:
						////							case AccessDataTypes.Number:
//							case AccessDataTypes.Ole:
//							case AccessDataTypes.YesNo:
//								nstr = nstr.Replace("%CHARACTER_MAXIMUM_LENGTH%","-1");
//								break;
//							case AccessDataTypes.Hyperlink:
						////							case AccessDataTypes.Memo:
						////							case AccessDataTypes.Text:
//								nstr = nstr.Replace("%CHARACTER_MAXIMUM_LENGTH%","-1");
//								break;
//							default:
//								nstr = nstr.Replace("%CHARACTER_MAXIMUM_LENGTH%","-1");
//								break;
//						}
					}
					
					list.Add(nstr);//%TABLE_NAME%,%COLUMN_NAME%,%ORDINAL_POSITION%,%IS_NULLABLE%,%DATA_TYPE%
				}
			}
			return string.Join("",list.ToArray());
		}

		#endregion
		
		/// <summary>
		/// Gets a DataType row for a specific schema-column-row
		/// </summary>
		/// <param name="row">Provided from a call to GetSchema and is the Columns table-row.</param>
		/// <param name="ds">A DataSet provided by calling GetSchema()</param>
		/// <returns>DataType-Row for a specific Column-row</returns>
		static public DataRowView SchemaDataType(DataRowView row, DataSet ds)
		{
			DataTable dataTypes = ds.Tables[Strings.Schema_DataTypes];
			int typeSrc = (int)row["DATA_TYPE"];
			foreach (DataRowView r in dataTypes.DefaultView)
			{
				int typeId = (int)(short)r["NativeDataType"];
				if (typeSrc==typeId)
				{
					return r;
				}
			}
			return null;
		}

		#region ReplaceFieldsT,ReplaceFieldsC,ReplaceFields
		/// <summary>
		/// This is an overload of ReplaceFields taylored for Tables.
		/// </summary>
		static public string ReplaceFieldsT(this string input, DataSet ds, DataRowView row, params string[] fields)
		{
			return input.ReplaceFields(true,ds,row,fields);
		}
		/// <summary>
		/// This is an overload of ReplaceFields taylored for Columns.
		/// </summary>
		static public string ReplaceFieldsC(this string input, DataSet ds, DataRowView row, params string[] fields)
		{
			return input.ReplaceFields(false,ds,row,fields);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="isTable"></param>
		/// <param name="ds"></param>
		/// <param name="row"></param>
		/// <param name="fields"></param>
		/// <returns></returns>
		static public string ReplaceFields(this string input, bool isTable, DataSet ds, DataRowView row, params string[] fields)
		{
			DataRowView dataType = null;
			if (!isTable) {
				dataType = SchemaDataType(row,ds);
				if (row["CHARACTER_MAXIMUM_LENGTH"]==DBNull.Value)
				{ // nada
				} else {
				}
			}
			// data_type, character_maximum_length
			string output = string.Copy(input);
			foreach (string field in fields)
			{
				//<Field DataType="AutoNumber" DataName="id" MaxLength="-1" UseFormat="false" IsNullable="false" />
				//CHARACTER_MAXIMUM_LENGTH(col)
				//DATA_TYPE(col)
				//COLUMN_HASDEFAULT (bool)
				//COLUMN_DEFAULT
				//DATA_TYPE
				if (row.Row.Table.Columns.Contains(field))
				{
					if (field.ToLower().Equals("is_nullable"))
					{
						output = output.Replace(
							"%{tag}%".Replace("{tag}",field),
							string.Format("{0}",row[field].ToString().ToBool()));
					}
					else if (field.ToLower().Equals("data_type"))
					{
						if (!isTable) // of course we wouldn't be here if this was a table—and all
						{
							output = output.Replace("%{tag}%".Replace("{tag}",field),row.GetString(field));
						}
					}
					else if (field.ToLower().Equals("native_type"))
					{
						
					}
					else if (field.ToLower().Equals("character_maximum_length"))
					{
						output = output.Replace(
							"%{tag}%".Replace("{tag}",field),
							string.Format("{0}",row[field]==DBNull.Value? -1: row[field]));
					}
					else
					{
						output = output.ReplaceField(row,field);
					}
				}
			}
			return output;
		}
		#endregion

		// The following methods are completely un-necessary.
		
		#region parse (depreceated)
		/// <summary>
		/// this function is now ignored.  It was the first function written in this class
		/// that was used for testing.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static string parse(string input)
		{
			List<string> list = new List<string>();
			list.Add("we have some text");
			foreach (Match m in input.getmatches())
			{
				list.Add( string.Concat("0: ",m.MFormat(0,MatchExtension.mformat1,',')) );
				TextRange.FromMatch(m);
			}
			return string.Join("\r\n",list.ToArray());
		}
		#endregion
		#region FilterType: Not Used
		// the row has to be from a Schema: "Columns"
		// the idea is to get a NativeDataType (dotnet) or a Ole-Specific type such as for SqlServer or Access.
		// Each kind of data-provider is going to have their own data-types and convensions, so it would be best to
		// keep it simple and leave the details for manual customization later.
		// this function is designed to work on getting a type for Access (SqlServer will come later)
		static public string FilterType(this DataRowView row, DataSet ds, string getField)
		{
			int type = (int)row["DATA_TYPE"];
			int chal = (int)row["CHARACTER_MAXIMUM_LENGTH"];
			
			foreach (DataRowView rv in ds.FindTable(Strings.Schema_DataTypes).DefaultView)
			{
				//CHARACTER_MAXIMUM_LENGTH(col)
				//DATA_TYPE(col)
				DataRowView dataTypeRow = SchemaDataType(rv,ds);
				if (dataTypeRow!=null) return row.GetString(getField);
				else goto ender;
			}
		ender:
			return "unknown";
		}
		#endregion

	}

}
