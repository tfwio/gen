using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using Generator.Elements.Basic;
using Generator.Resources;
namespace Generator.Elements
{
	public partial class DataViewElement
	{
		#region public: TreeView, TreeNode (Helpers)
		#if TREEV
		public void ToTree(TreeNode tn)
		{
			tn.Nodes.Add(ToNode());
		}

		public TreeNode ToNode()
		{
			var node = new TreeNode(this.Name) {
				Name = this.Name,
				Tag = this,
				ImageKey = ImageKeyNames.View,
				SelectedImageKey = ImageKeyNames.View
			};
			foreach (DataViewLink link in this.LinkItems) {
				node.Nodes.Add(new TreeNode(link.Table) {
					ImageKey = ImageKeyNames.ViewLink,
					SelectedImageKey = ImageKeyNames.ViewLink
				});
			}
			return node;
		}
	#endif
	#endregion
	}
}


