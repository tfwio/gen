/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
#if TREEV || false
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace Generator.Elements
{
	public partial class TableElement
	{
		// .ctor
		public TableElement(TreeNode node)
		{
			Name = node.Text;
			if (items == null)
				items = new List<FieldElement>();
			if (node.Tag is TableElement) {
				TableElement te = node.Tag as TableElement;
				DbType = te.DbType;
				BaseClass = te.BaseClass;
				Name = te.Name;
				PrimaryKey = te.PrimaryKey;
			}
			foreach (TreeNode tn in node.Nodes) {
				if (tn.Tag is FieldElement)
					items.Add(new FieldElement(tn));
			}
		}
		public TreeNode ToNode()
		{
			var tn = new TreeNode(Name);
			tn.Name = Name;
			// the image correlates with the PanelTableEditor (or whatever it's called)
			tn.SelectedImageKey = tn.ImageKey = "table";
			tn.Tag = this;
			foreach (FieldElement fe in items)
				tn.Nodes.Add(fe.ToNode());
			return tn;
		}

		public void ToTree(TreeNode tn)
		{
			var tblelement = tn.Nodes.Add(Name);
			tblelement.Name = this.Name;
			//			tblelement.BaseClass = this.BaseClass;
			// the image correlates with the PanelTableEditor (or whatever it's called)
			tblelement.SelectedImageKey = tblelement.ImageKey = "table";
			foreach (FieldElement item in items)
				tblelement.Nodes.Add(item.ToNode());
		}
	}
}
#endif


