/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.Windows.Forms;
namespace Generator.Elements
{
	public partial class TableElementForms
	{
#if TREEV
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
#endif
	}
}


