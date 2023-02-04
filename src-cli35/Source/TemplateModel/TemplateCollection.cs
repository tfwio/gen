/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 5:39 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Generator.Parser;
#endregion

namespace Generator.Core.Markup
{
	[XmlRoot("TemplateCollection")]//,Namespace="http://w3.tfw.co/xmlns/2011/templates"
	public class TemplateCollection : SerializableClass<TemplateCollection> //, INotifyPropertyChanged
	{
		static public List<string> GetGroupNames(TemplateCollection collection)
		{
			var groupnames = new List<string>();
			foreach (TableTemplate template in collection.templates)
			{
				if (groupnames.Contains(template.Group)) continue;
				groupnames.Add(template.Group);
			}
			return groupnames;
		}
		public class TemplateGroup
		{
			public string  GroupName { get; set; }
			public IEnumerable<TableTemplate> Templates { get; set; }
			
			static TemplateGroup GetGroup(TemplateCollection templates, string groupOn) {
				return new TemplateGroup { GroupName=groupOn, Templates=templates.Templates.Where(tpl=>tpl.Group==groupOn) };
			}
			
			static public List<TemplateGroup> GetTemplates(TemplateCollection templates) {
				var items = GetGroupNames(templates);
				var list = new List<TemplateGroup>();
				items.Sort();
				foreach (string g in items)
					list.Add(new TemplateGroup { GroupName=g, Templates=templates.Templates.Where(tpl=>tpl.Group==g).OrderBy(tpl=>tpl.Alias) });
				return list;
			}
		}
		List<TemplateGroup> GroupedItems { get; set; }
		
		public List<TemplateGroup> GetGrouping()
		{
			return TemplateGroup.GetTemplates(this);
		}
		
		List<string> usingNamespace = new List<string>();
		[Category("Assembly")]
		public string[] UsingNamespace { get { return usingNamespace.ToArray(); } set { usingNamespace = new List<string>(value); } }
		
		List<string> referenceAssembly = new List<string>();
		[Category("Assembly")]
		public string[] ReferenceAssembly { get { return referenceAssembly.ToArray(); } set { referenceAssembly = new List<string>(value); } }
		
		List<TableTemplate> templates = new List<TableTemplate>();
		public List<TableTemplate> Templates { get { return templates; } set { templates = value; } }
		
		[XmlIgnore] public TableTemplate this[string Alias] { get { return FindAlias(Alias); } }
		[XmlIgnore] public TableTemplate this[FieldMatch match] { get { return FindAlias(match.TemplateAlias); } }
		
		
		public TableTemplate FindAlias(string Alias) {
			foreach (TableTemplate tpl in this.Templates)
			{
				if (tpl.Alias==null) continue;
				if (tpl.Alias==string.Empty) continue;
				if (tpl.Alias.Trim()==Alias.Trim())
					return tpl;
			}
			return null;
		}
		
		public void GetTableValues(DataTable table)
		{
			Templates.Clear();
			foreach (DataRow row in table.Rows)
			{
				templates.Add(new TableTemplate(row));
			}
		}
		
		public void ColumnDefaults(DataTable table)
		{
			table.Columns.Add("Name",typeof(string));
			table.Columns.Add("Alias",typeof(string));
			table.Columns.Add("Group",typeof(string));
			table.Columns.Add("Tags",typeof(string));
			table.Columns.Add("SyntaxLanguage",typeof(string));
			table.Columns.Add("ClassName",typeof(string));
			table.Columns.Add(res.elmTpl,typeof(string));
			table.Columns.Add(res.itmTpl,typeof(string));
		}
		
		public DataSet GetData()
		{
			var table = new DataTable("Templates");
			var ds = new DataSet{ Tables = { table } };
			ColumnDefaults(table);
			foreach (TableTemplate tt in Templates) tt.ToTable(table);
			return ds;
		}
		
		public void Add(TableTemplate item)
		{
			Templates.Add(item);
		}
		
		public TemplateCollection(TemplateCollection tplBase, DataTable table)
		{
			UsingNamespace = tplBase.UsingNamespace;
			ReferenceAssembly = tplBase.ReferenceAssembly;
			this.GetTableValues(table);
		}
		public TemplateCollection()
		{
		}
	}
}
