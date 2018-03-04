#if TREEV
using System;
using System.Linq;
using System.Windows.Forms;
namespace Generator.Elements
{
	public partial class ReferenceAssemblyElement
	{

		public ReferenceAssemblyElement(TreeNode node)
		{
			if (node.Tag is ReferenceAssemblyElement) {
				ReferenceAssemblyElement nodeTag = node.Tag as ReferenceAssemblyElement;
				if (nodeTag.ApplicationBase != null && nodeTag.ApplicationBase != string.Empty)
					ApplicationBase = nodeTag.ApplicationBase;
				if (nodeTag.AssemblyFileExists)
					AssemblyFileLocation = nodeTag.AssemblyFileLocation;
				else
					return;
				if (!AssemblyIsLoaded) {
					Autoload();
				}
			}
			else
				throw new ArgumentException();
		}

		public void ToTree(TreeNode node)
		{
			node.Nodes.Add(ToNode());
		}

		public TreeNode ToNode()
		{
			TreeNode tn;
			if (this.assembly == null) {
				//				Global.statR("ToNode: the assembly wasn't loaded");
			}
			if (Name == null)
				tn = new TreeNode("… ( asm ) …");
			else
				tn = new TreeNode(Name);
			if (AssemblyIsLoaded)
				tn.ToolTipText = Assembly.FullName;
			tn.SelectedImageKey = tn.ImageKey = "asm";
			tn.Tag = this;
			return tn;
		}
	}
}
#endif




