using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Generator.Elements;
using Generator.Core.Markup;
using Generator;
using Global=System.Cor3.last_addon;

namespace Generator.Parser
{
	static public class TemplateFactory
	{
		
		#region NO
		#if NO
		#region Not Used
		static public DictionaryList<string,string> GroupedKeys = new DictionaryList<string,string>();
		/// not yet
		static public string CategorizedField(string fieldname)
		{
			return null;
		}
		#endregion
		
		public static List<string> GetParamStrings(DataViewElement view, TableElement table, string fieldTemplate, bool noKey)
		{
			List<string> paramStrings = new List<string>();
			List<string> viewFields = new List<string>(view.Fields.Split(','));
			int counter=0;
			
			foreach (string item in viewFields) // otherwise Table.Fields
			{
				FieldElement field = FieldElement.Clone(table[item]);
				string fieldGen = fieldTemplate;
				bool isPrimary = table.PrimaryKey==field["DataName"].ToString();
				if (field==null) {
					
					continue;
				}
				if (noKey & isPrimary) continue;
				
				// take care of table-values within the field-template
				fieldGen = fieldGen
					.ReplaceP("FieldIndex",counter++)
					.ReplaceP("IsPrimary",isPrimary)
					.ReplaceP("PrimaryKey",table.PrimaryKey);
				
				// continue to replace field-values
				fieldGen = field.Replace(
					fieldGen,
					delegate(DICT<string,object> dic){
						//
						dic.Add("FieldName",field.DataName);
						field.DataName = string.IsNullOrEmpty(view.Alias) ? string.Empty : view.Alias + "." + dic["DataName"];
						dic["DataName"] = field.DataName;
						dic["dataname"] = field.DataName.ToLower();//
						dic["DataNameC"] = field.DataName.ToStringCapitolize();
						dic["CleanName,Nodash"] = field.DataName.Clean();
						dic["FriendlyName"] = field.DataName.Clean();
						dic["CleanName"] = field.DataName.Replace("-","_");
						dic["FriendlyNameC"] = field.DataName.Clean().ToStringCapitolize();
					});
				paramStrings.Add(fieldGen);
			}
			return paramStrings;
		}
		
		public static List<string> GetParamStrings(DataViewLink link, TableElement table, string fieldTemplate, bool noKey)
		{
			List<string> paramStrings = new List<string>();
			List<string> linkFields = new List<string>(link.Fields.Split(','));
			int counter=0;
			foreach (string item in linkFields) // otherwise Table.Fields
			{
				FieldElement field =	FieldElement.Clone(table[item]);
				string fieldGen =		fieldTemplate;
				bool isPrimary =		table.PrimaryKey==field["DataName"].ToString();
				
				if (field==null)
				{
					throw new ArgumentException(
						string.Format(
							"No field named ‘{0}’ found in table ‘{1}’.",
							item,
							table.Name
						)
					);
				}
				
				if (noKey & isPrimary) continue;
				
				// take care of table-values within the field-template
				fieldGen = fieldGen
					.ReplaceP("FieldIndex",counter++)
					.ReplaceP("IsPrimary",isPrimary)
					.ReplaceP("PrimaryKey",table.PrimaryKey);
				
				// continue to replace field-values
				fieldGen = field.Replace(
					fieldGen,
					delegate(DICT<string,object> dic)
					{
						//
						dic.Add("FieldName",field.DataName);
						field.DataName = string.IsNullOrEmpty(link.Alias) ? string.Empty : link.Alias + "." + dic["DataName"];
						dic["DataName"] =			field.DataName;
						dic["dataname"] =			field.DataName.ToLower();//
						dic["DataNameC"] =			field.DataName.ToStringCapitolize();
						dic["CleanName,Nodash"] =	field.DataName.Clean();
						dic["FriendlyName"] =		field.DataName.Clean();
						dic["CleanName"] =			field.DataName.Replace("-","_");
						dic["FriendlyNameC"] =		field.DataName.Clean().ToStringCapitolize();
					}
				);
				
				paramStrings.Add(fieldGen);
				
			}
			return paramStrings;
		}
		
		/// <summary>
		/// Find all occurances of Table or Field Template-Tag references.
		/// </summary>
		static FieldMatch GetMatches(string tableTemplate)
		{
			Logger.LogG("template-fac","gotmatches");
			
			MatchCollection mc = regex_fieldTemplateTag.Matches(tableTemplate);
			if (mc==null) return null;
			FieldMatch fm = null;
			if (mc.Count >= 1) { fm = new FieldMatch(mc); }
			return fm;
		}
		
		#endif
		#endregion
		
		#region (static/constant)
		
		public const string fmt_field = "$({0})";
		
		static int counter=0;
		
		static readonly Dictionary<NsTypes,string> AC001 = new Dictionary<NsTypes,string>(){
			{ NsTypes.Global, Gen.Strings.Factory_AcErratum },
			{ NsTypes.TableTypes, string.Concat(
				Gen.Strings.Factory_AcTable,";",
				Gen.Strings.Factory_AcPrime) },
			{ NsTypes.AdapterTypes,
				Gen.Strings.Factory_AcAdapt },
			{ NsTypes.DatabaseTypes,
				Gen.Strings.Factory_AcData },
			{ NsTypes.FieldTypes, string.Concat(
				Gen.Strings.Factory_AcField,";",
				Gen.Strings.Factory_AcTypes,";",
				Gen.Strings.Factory_AcPkType) },
		};
		static readonly Dictionary<NsTypes,string> AC002 = new Dictionary<NsTypes,string>(){
			{ NsTypes.Global,Gen.Strings.Factory_AcErratum },
			{ NsTypes.TableTypes, string.Concat(
				Gen.Strings.Factory_AcTable,";",
				Gen.Strings.Factory_AcTypes,";",
				Gen.Strings.Factory_AcPrime) },
			{ NsTypes.AdapterTypes,
				Gen.Strings.Factory_AcAdapt },
		};
		
		static public readonly Dictionary<NsTypes,string> Ac01 = AC001;
		static public readonly Dictionary<NsTypes,string> Ac02 = AC002;

		static public string[] Group1(NsTypes groupn) { return Ac01[groupn].Split(';'); }
		static public string[] Group2(NsTypes groupn) { return Ac02[groupn].Split(';'); }

		static readonly Regex regex_fieldTemplateTag = new Regex(Gen.Strings.Regex_Field_Template_Tag,RegexOptions.Multiline);

		#endregion
		
		#region Check For Error
		static bool CheckForError(DatabaseElement db,
		                          TableElement table,
		                          bool showMessageBox=false,
		                          bool ignoreException=true)
		{
			bool hasError = false;
			if (hasError = (db==null || table==null))
			{
				if (showMessageBox) System.Windows.Forms.MessageBox.Show(
					Gen.Strings.MsgDatabaseOrTableNullError_Title,
					Gen.Strings.MsgDatabaseOrTableNullError_Message
				);
				if (!ignoreException) throw new ArgumentException (Gen.Strings.MsgDatabaseOrTableNullError_Exception);
			}
			return hasError;
		}
		static bool CheckForError(TableElement table,
		                          bool showMessageBox=false,
		                          bool ignoreException=true)
		{
			bool hasError = false;
			if (hasError = (table==null))
			{
				if (showMessageBox) System.Windows.Forms.MessageBox.Show(
					Gen.Strings.MsgDatabaseOrTableNullError_Title,
					Gen.Strings.MsgDatabaseOrTableNullError_Message
				);
				if (!ignoreException) throw new ArgumentException (Gen.Strings.MsgDatabaseOrTableNullError_Exception);
			}
			return hasError;
		}
		#endregion
		
		#region Convert
		
		static public string ConvertTable(Generator.Export.Intrinsic.IDbConfiguration4 tps)
		{
			return ConvertInput(tps,true);
		}
		
		
		/// <summary>
		/// #1 Starting Point
		/// <para>
		/// The selection must have at the least a table and selected template.
		/// Converts a selection (table or view) provided by IDbConfiguration4.
		/// </para>
		/// </summary>
		/// <param name="tps">IDbConfiguration4; (selection)</param>
		/// <param name="newVersion">This isn't used.</param>
		/// <returns>parsed string result.</returns>
		static public string ConvertInput(Generator.Export.Intrinsic.IDbConfiguration4 tps, bool newVersion)
		{
			string tableOut = string.Empty;
			
			DatabaseElement database = null;
			TableElement table = tps.SelectedTable;
			TableTemplate template = tps.SelectedTemplate;
			
			if (table!=null)
			{
				// set the table
				table = tps.SelectedTable;
				
				// Get the template and do first layer of parsing
				tableOut = ConvertInput2(
					tps,
					table,
					template.ElementTemplate,
					template.ItemsTemplate,
					false
				);
				
				Logger.Warn("template factory","Listing Items");
				
				// holds a list of template-tag matches.
				List<QuickMatch> list;
				
				// Check for recurring templates to parse.
				while (0 != (list = TemplateReferenceUtil.GetReferences(tableOut)).Count)
				{
					tableOut = ConvertInputPart(tps,table,list[0],tableOut);
				}
			}
			
			#region IF View IS SELECTION
			else if (tps.SelectedView!=null)
			{
				string sdb=	tps.SelectedView.Database, stb=tps.SelectedView.Table;
				
				database =	tps.SelectedCollection.Databases.FirstOrDefault(db => db.Name == sdb);
				table = (TableElement)database.Items.FirstOrDefault(t => t.Name == stb);
				
				if (CheckForError(database,table,true))
				{
					return Gen.Strings.MsgDatabaseOrTableNullError_Exception;
				}
				
				table.View = tps.SelectedView;
				tableOut = ConvertInput2(
					tps,
					table,
					tps.SelectedTemplate.ElementTemplate,
					tps.SelectedTemplate.ItemsTemplate,
					false
				);
				table.View = null;
				
				#region Null Message
				if (database==null || table==null)
				{
					System.Windows.Forms.MessageBox.Show("Error finding database or table for view-link","Exiting generation...");
					return "Error finding database or table for view.";
				}
				#endregion
				
				Logger.Warn( "template factory" , "Listing Items" );
				
				List<QuickMatch> list;
				while (0 != (list = TemplateReferenceUtil.GetReferences( tableOut )).Count)
				{
					QuickMatch match0 = list[0];
					
					#region Parameter Check/Logging
					
					Logger.LogC( "\ttemplate factory" , "{0}", match0.Value );
					
					if (!list[0].HasParams)
					{
						Logger.LogC( "TemplateFactory.ConvertInput ERROR" , "No Params" );
						continue;
					}
					#endregion
					
					TableTemplate tbltmpl=null;
					
					if (!match0.HasMultipleParams)
					{
						#region Single Table Reference
						
						tbltmpl = tps.Templates[list[0].Params[0]];
						
						if (tbltmpl==null)
						{
							Logger.Warn("TemplateFactory.ConvertInput ERROR","Tag: ‘{0}’ value.",list[0].Name);
							continue; // return tableOut;
						}
						
						TableTemplate tpl=tps.Templates[list[0].Params[0]];
						table.View = tps.SelectedView;
						string newOut = ConvertInput2( tps, table, tpl.ElementTemplate, tpl.ItemsTemplate, false );
						tableOut = tableOut.Replace(list[0].FullString,newOut);
						table.View = null;
						Logger.LogM("TemplateFactory.ConvertInput","{0}",match0.Params[0]);
						
						#endregion
					}
					else if (match0.Name=="Directory")
					{
						#region Directory

						if (System.IO.Directory.Exists(match0.Value))
						{
							List<string> listf = new List<string>();
							foreach (string dir in System.IO.Directory.GetDirectories(match0.Value))
							{
								listf.Add(dir);
							}
							tableOut = tableOut.Replace(match0.FullString,string.Join(",",listf.ToArray()));
						}
						
						#endregion
					}
					else
					{
						#region Main Parser Section
						Logger.LogC("template factory","Match0.Value = “{0}”",match0.Value);
						Logger.LogC("\tfactory",".Name = “{0}”",match0.Name);
						System.Cor3.ConsoleColorC.statC("{0}",match0.Params[0]);
						string newOut = string.Empty;
						for(int i=1; i < match0.Params.Length; i++)
						{
							System.Cor3.ConsoleColorC.statG("table: “{0}”", match0.Params[i]);
							TableTemplate tpl = tps.Templates[match0.Params[0]];
							TableElement tbl = tps.SelectedDatabase[match0.Params[i]];
							table.View = tps.SelectedView;
							newOut += string.Format("{0}",ConvertInput2( tps, tbl, tpl.ElementTemplate, tpl.ItemsTemplate, false ));
							table.View = null;
						}
						tableOut = tableOut.Replace(list[0].FullString,newOut);
						#endregion
					}
				}
			}
			#endregion
			
			return tableOut;
		}
		
		/// changed: we are not just calling continue to break out of a loop;  No loop;
		/// Now we're returning the input 'tableIn'.
		static public string ConvertInputPart(Generator.Export.Intrinsic.IDbConfiguration4 tps, TableElement table, QuickMatch match, string tableIn)
		{
			string tableOut = tableIn;
//			QuickMatch match0 = list[0];
			Logger.LogC("\ttemplate factory","{0}",match.Value);
			
			if (!match.HasParams) {
				Logger.LogC("TemplateFactory.ConvertInput ERROR","No Params");
				return tableOut/*continue*/;
			}
			
			TableTemplate tbltmpl=null;
			
			if (!match.HasMultipleParams)
			{
				#region Single Table Reference
				
				#region Warn if null
				// this is wrong.
				// See above how we assign the tbltpl to null before checking here!
				if (tbltmpl==null) {
					Logger.Warn(
						Gen.Strings.LogTemplateFactoryErr_Title,
						Gen.Strings.LogTemplateFactoryErr_Filter,
						match. Name
					);
					return tableOut;
				}
				#endregion
				
				// Get our template
				tbltmpl = tps.Templates[match.Params[0]];
				
				// get our input
				string newOut = ConvertInput2( tps, table, tbltmpl.ElementTemplate, tbltmpl.ItemsTemplate, false );
				
				// replace the template tag with the parsed content.
				tableOut = tableOut.Replace( match.FullString, newOut );
				
				Logger.LogM( Gen.Strings.LogFactoryConvert_Title, "{0}", match.Params[0] );
				
				#endregion
			}
			// the Directory Element is not parsed.
			else if (match.Name=="Directory")
			{
				#region Directory
				
				if (System.IO.Directory.Exists(match.Value))
				{
					List<string> listf = new List<string>();
					foreach (string dir in System.IO.Directory.GetDirectories(match.Value))
					{
						listf.Add(dir);
					}
					tableOut = tableOut.Replace(match.FullString,string.Join(",",listf.ToArray()));
				}
				
				#endregion
			}
			// This is the main parser loop.
			else
			{
				// If we are here, we have a complex template;
				// That is, our template references multiple tables.
				#region Main Parser Section
				
//				switch (match0.Name) { default: Global.statC("{0}",match0.Params[0]); break; }
				Logger.LogC(Gen.Strings.LogTemplateInfo_Title,Gen.Strings.LogTemplateInfo_Format,match.Value,match.Name,match.Params[0]);
				
				string newOut = string.Empty;
				
				for(int i=1; i < match.Params.Length; i++)
				{
					Global.statG(Gen.Strings.LogTable_Format, match.Params[i]);
					TableTemplate tpl = tps.Templates[match.Params[0]];
					TableElement tbl = tps.SelectedDatabase[match.Params[i]];
					newOut += ConvertInput2(tps,tbl,tpl.ElementTemplate,tpl.ItemsTemplate,false);
				}
				tableOut = tableOut.Replace( match.FullString, newOut );
				#endregion
			}
			
			return tableOut;
			
		}
		
		/// <summary>
		/// Parser Pass #2:
		/// <para>
		/// This is the parser's initialization point.
		/// </para>
		/// <para>Here, we process the given template given a particular table.</para>
		/// <para>The parser process repeats using this method for each requested table/template
		/// provided by the given template until there are no templates left to parse.</para>
		/// </summary>
		/// <param name = "tps"></param>
		/// <param name="table"></param>
		/// <param name="tableTemplate"></param>
		/// <param name="fieldTemplate"></param>
		/// <param name="isForView"></param>
		/// <returns></returns>
		static public string ConvertInput2(Generator.Export.Intrinsic.IDbConfiguration4 tps,
		                                   TableElement table,
		                                   string tableTemplate,
		                                   string fieldTemplate,
		                                   bool isForView)
		{
			// UNDONE: Log Error
			if (table==null) return tableTemplate;
			
			Logger.LogM( "template factory" , "Converting {0}" , table.Name );
			
			bool   noKey	= tableTemplate.Contains("FieldValuesNK");
			// replace common tableTemplate values.
			string tableOut	= table.ReplaceValues( tableTemplate );
			
			List<string> paramStrings = GetParamStrings( tps, table, fieldTemplate, noKey );
			
			tableOut = ReplaceFieldValues( tableOut, paramStrings.ToArray() );
			
			paramStrings.Clear();
			paramStrings = null;
			
			return tableOut;
		}
		
		static string ReplaceFieldValues(string input,
		                                          params string[] values)
		{
			string cFieldOut = string.Join(",",values);
			string fieldOut = string.Join(string.Empty,values);
			
			return input
				.ReplaceP("FieldValues",fieldOut)
				.ReplaceP("FieldValuesNK",fieldOut)
				.ReplaceP("FieldValues,Cdf",cFieldOut)
				.ReplaceP("FieldValuesNK,Cdf",cFieldOut);
		}
		
		// primary (used)
		/// <summary>
		/// Parser Pass #2.1
		/// <para>
		/// FieldTemplate is parsed for each field in the table and returned
		/// in a list where each parsed field is an item.
		/// </para>
		/// </summary>
		/// <param name = "tps"></param>
		/// <param name="table"></param>
		/// <param name="fieldTemplate"></param>
		/// <param name="noKey"></param>
		/// <returns>
		/// A list of parsed table-fields given fieldTemplate.
		/// </returns>
		public static List<string> GetParamStrings(Generator.Export.Intrinsic.IDbConfiguration4 tps,
		                                           TableElement table,
		                                           string fieldTemplate,
		                                           bool noKey)
		{
			var paramStrings = new List<string>();
			string temp = null, sdb=null, stb=null;
			// what's the table we're dealing with here?
			string fieldOut = table.Reformat( fieldTemplate );
			counter=0;
			if (table.View==null && table.Link==null) // we leave the link here so that templates can be generated per link.
			{
				foreach (FieldElement field in table.Fields)
				{
					temp = GetParam(field,table,fieldOut,noKey);
					if (!string.IsNullOrEmpty(temp)) paramStrings.Add(temp);
				}
			}
			else if (table.View!=null)
			{
				sdb = table.View.Database;
				stb = table.View.Table;
				DatabaseElement db = tps.SelectedCollection.Databases.FirstOrDefault(xdb => xdb.Name == sdb);
				var tbl = (TableElement)db.Items.FirstOrDefault(t => t.Name == stb);
				if (CheckForError(tbl)) Logger.Warn("GetParamStrings","Table \"{0}\" wasn't found");
				fieldOut = tbl.ReplaceValues( fieldTemplate );
				foreach (FieldElement field in tbl.Fields)
				{
					field.View = table.View;
					temp = GetParam(field,table,fieldOut,noKey);
					bool hasField = table.View.HasField(table,field,true);
					if (hasField && !string.IsNullOrEmpty(temp)) paramStrings.Add(temp);
					field.View = null;
				}
				tbl = null;
				foreach (DataViewLink link in table.View.LinkItems)
				{
					sdb = link.Database;
					stb = link.Table;
					db = tps.SelectedCollection.Databases.FirstOrDefault(xdb => xdb.Name == sdb);
					tbl = (TableElement)db.Items.FirstOrDefault(t => t.Name == stb);
					if (CheckForError(tbl)) Logger.Warn("GetParamStrings","Table \"{0}\" wasn't found");
					Logger.LogG("Found table","{0}",tbl.Name);
					fieldOut = tbl.ReplaceValues( fieldTemplate );
					Logger.LogG("Template","{0}",fieldOut);
					foreach (FieldElement field in tbl.Fields)
					{
						field.View = table.View;
						field.Link = link;
						temp = GetParam(field,table,fieldOut,noKey);
						bool hasField = link.HasField(table,field,true);
						if (hasField && !string.IsNullOrEmpty(temp)) paramStrings.Add(temp);
						field.View = null;
						field.Link = null;
					}
					tbl = null;
				}
				
			}
			return paramStrings;
		}
		
		static string GetParam(FieldElement field,
		                       TableElement table,
		                       string fieldTemplate,
		                       bool noKey)
		{
			// Copy the template-string for parsing.
			string fieldGen = string.Copy(fieldTemplate);
			// Check if this is the primary-key
			bool isPrimary = table.PrimaryKey==field["DataName"].ToString();
			// Skip primary key field if requested (noKey)
			if (noKey & isPrimary) return null;
			fieldGen = fieldGen
				.ReplaceP("FieldIndex",counter++)
				.ReplaceP("IsPrimary",isPrimary)
				.ReplaceP("PrimaryKey",table.PrimaryKey);
			// parse
			fieldGen = field.Replace(fieldGen);
			return fieldGen; //paramStrings.Add(fieldGen);
		}
		
		#endregion
		
	}
}
