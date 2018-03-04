/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using Generator.Assets;
#if TREEV
using System.Windows.Forms;
#endif
namespace Generator.Elements
{
	public partial class QueryElement
	{
		#if TREEV
		public void ToTree(TreeNode root)
		{
			TreeNode node = root.Nodes.Add(name);
			node.Name = name;
			node.Tag = this;
			node.SelectedImageKey = node.ImageKey = ImageKeyNames.SqlQuery;
		}

		static public QueryElement FromNode(TreeNode node)
		{
			QueryElement q = node.Tag as QueryElement;
			if (!q.name.Equals(node.Text)) {
				q.name = node.Text;
			}
			return q;
		}
		#endif
	}
}


