/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.Windows.Forms;
namespace Generator.Elements
{
	public partial class SimpleQueryElement
	{
		#if TREEV
		public void ToTree(TreeNode node)
		{
			TreeNode element = node.Nodes.Add(Name);
			element.Tag = this;
		}
	#endif
	}
}




