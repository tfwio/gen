using System;
using System.Collections.Generic;
using Generator.Assets;

#if TREEV
using System.Windows.Forms;
#endif
namespace Generator.Elements
{
	#if TREEV
	public partial class DatabaseElement
	{
		/// <summary>
		/// Constructor for a Windows.Forms UI (TreeView.Node)
		/// </summary>
		/// <param name="tn"></param>
		public DatabaseElement(TreeNode tn)
		{
			if (items == null) items = new List<TableElement>();
			if (views == null) views = new List<DataViewElement>();
			Name = tn.Text;
			if (tn.Tag is DatabaseElement)
			{
				var dt = tn.Tag as DatabaseElement;
				ConnectionType = dt.ConnectionType;
				PrimaryKey = dt.PrimaryKey;
				//				PrimaryKey = dt.PrimaryKey;
			}
			foreach (TreeNode node in tn.Nodes)
			{
				if (node.Tag is TableElement) items.Add(new TableElement(node));
				//				if (Views == null) views = new List<DataViewElement>();
				else if (node.Tag is DataViewElement) {
					views.Add(new DataViewElement(node));
				}
			}
		}
		public void ToTree(TreeView tv)
		{
			foreach (TableElement telm in Items)
				tv.Nodes.Add(telm.ToNode());
			//			foreach (DataViewElement telm in Views) tv.Nodes.Add(telm.ToNode());
		}
	
		public void ToTree(TreeNode tn)
		{
			tn.Nodes.Add(ToNode());
		}
	
		public TreeNode ToNode()
		{
			var node = new TreeNode(Name);
			node.Name = Name;
			// the image correlates with the PanelTableEditor (or whatever it's called)
			node.SelectedImageKey = node.ImageKey = ImageKeyNames.Database;
			node.Tag = this;
			foreach (TableElement telm in Items)
				node.Nodes.Add(telm.ToNode());
			//			foreach (DataViewElement telm in Views) node.Nodes.Add(telm.ToNode());
			return node;
		}
	}
	#endif
}


