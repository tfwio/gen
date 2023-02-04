/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#region Using
using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
#endregion

namespace Generator.Backend
{
	public class QueryElement : DataMapElement
	{
		[XmlAttribute] public string name;
		[XmlAttribute] public string source;
		[XmlAttribute] public string context;
		[XmlAttribute] public string mode;
		[XmlElement] public string sql;
		
		#region FORMS TreeNode
//		#if FORMS
		public void ToTree(TreeNode root)
		{
			TreeNode node = root.Nodes.Add(name);
			node.Name = name;
			node.Tag = this;
			node.SelectedImageKey = node.ImageKey =
				ImageKeyNames.SqlQuery;
		}
		static public QueryElement FromNode(TreeNode node)
		{
			QueryElement q = node.Tag as QueryElement;
			if (!q.name.Equals(node.Text))
			{
				q.name = node.Text;
			}
			return q;
		}
//		#endif
		#endregion

		public QueryElement()
		{
			
		}
		public QueryElement(string name, string sql)
		{
			this.name	= name;
			this.sql	= sql;
		}
		public QueryElement(string name, string sql, string source, string context)
			: this(name,sql)
		{
			this.source = source;
			this.context = context;
		}
		
		
	}
}
