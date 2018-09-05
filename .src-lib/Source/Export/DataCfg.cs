using System;
using System.Collections.Generic;
using System.Diagnostics;

using Generator.Core;
using Generator.Elements;
using Generator.Core.Markup;
using SqlDbType = System.Data.SqlDbType;
/*
 * This is designed to be imported into another project as an include.
 */
namespace Generator.Export
{
	// =========================================================
	// This is both used internally and must be included externally.
	// =========================================================
	class DataCfg : Generator.Export.Intrinsic.IDataConfig
	{
		public bool HasTemplate(string alias)
		{
			return Templates.FindAlias(alias) != null;
		}
		// x
		public DatabaseCollection Databases { get; set; }
		public DatabaseElement Database { get; set; }
		public TableElement Table { get;set; }
		// y
		public TemplateCollection Templates { get;set; }
		public TableTemplate Template { get;set; }
		
		static public string Parse(
			DatabaseCollection databases,
			DatabaseElement database,
			TemplateCollection templates,
			TableElement table,
			TableTemplate template
		)
		{
			return Parse(
				new DataCfg(){
					Databases  = databases,
					Database = database,
					Table = table,
					Templates = templates,
					Template = template
				});
		}
		/// <summary>
		/// A consolidated version of ConvertInput.
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		static public string Parse(Generator.Export.Intrinsic.IDataConfig config)
		{
			// TableElement tableElementbackup = config.Table;
			string tableOut = Parse(config.Databases, config.Table, config.Template);
			List<QuickMatch> list;
			
			while (0 != (list = TemplateReferenceUtil.GetReferences(tableOut)).Count)
			{
				// we could restore the table if it was changed
				QuickMatch match0 = list[0];
				
				if (!list[0].HasParams) /* error no parameters  */ continue;
				string newOut = string.Empty;
				// TableTemplate tbltmpl = null;
				
				if (!match0.HasMultipleParams) { // single parameter-match
					
					bool checker = config.HasTemplate(list[0].Params[0]);
					Debug.Assert(checker,string.Format("Template {0} not found! if you continue, the generated content will have errors.",match0.Params[0]));
					if (checker)
					{
						config.Template = config.Templates[list[0].Params[0]];
						newOut = Parse( config );
						tableOut = tableOut.Replace(list[0].FullString, newOut );
					}
					
				} else { // Multiple param-matches
					
					for(int i=1; i < match0.Params.Length; i++)
					{
						bool checker = config.HasTemplate(match0.Params[0]);
						Debug.Assert(checker,string.Format("Template {0} not found! if you continue, the generated content will have errors.",match0.Params[0]));
						if (checker)
						{
							config.Template = config.Templates[match0.Params[0]];
							config.Table = config.Database[match0.Params[i]];
							newOut += Parse( config );
						}
					}
					tableOut = tableOut.Replace(list[0].FullString,newOut);
				}
			}
			return tableOut;
		}
		static public string Parse(Generator.Export.Intrinsic.IDataConfig config, string template)
		{
			config.Template = config.Templates[template];
			return Parse(config);
		}
		static public string Parse(DatabaseCollection collection, TableElement table, TableTemplate template)
		{
			return Parse(
				table,
				template.ElementTemplate,
				template.ItemsTemplate);
		}
		static public string Parse(TableElement table, string tableTemplate, string fieldTemplate)
		{
			// FIXME: Log Error
			if (table==null) return tableTemplate;
			Logger.LogM("template factory","Converting {0}",table.Name);
			bool noKey		= ParserHelper.FieldReferencesKey(tableTemplate);
			string tableOut	= ParserHelper.ReplaceValues(table,tableTemplate);
			string fieldOut	= table.Reformat(fieldTemplate);
	
			List<string> paramStrings = ParserHelper.GetParamStrings(table,fieldOut,noKey);
			tableOut = ParserHelper.ReplaceFieldValues(tableOut, paramStrings.ToArray());
			paramStrings.Clear(); paramStrings = null;
	
			return tableOut;
		}
	}
}
