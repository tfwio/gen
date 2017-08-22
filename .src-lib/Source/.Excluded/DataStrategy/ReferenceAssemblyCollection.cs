/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.Runtime.Hosting;
using System.Windows.Forms;
using System.Xml.Serialization;

#endregion
namespace Generator.Core.Entities
{
	public class ReferenceAssemblyCollection
	{
		string name;
		[XmlAttribute] public string Name { get { return name; } set { name = value; } }
	
		List<ReferenceAssemblyElement> list;
		[XmlElement("AssemblyReference")]
		public List<ReferenceAssemblyElement> Assemblies { get { return list; } set { list = value; } }
		public ReferenceAssemblyCollection(string sectionName)
		{
			name = sectionName;
		}
		public ReferenceAssemblyCollection()
		{
		}
		public ReferenceAssemblyCollection(TreeNode tn)
		{
			if (!(tn.Tag is ReferenceAssemblyCollection)) throw new ArgumentException();
			ReferenceAssemblyCollection aref = tn.Tag as ReferenceAssemblyCollection;
			list = new List<ReferenceAssemblyElement>();
			foreach (TreeNode tnode in tn.Nodes)
			{
				object bref = tnode.Tag;
				if (bref is ReferenceAssemblyElement) list.Add(bref as ReferenceAssemblyElement);
			}
		}
		public TreeNode ToNode()
		{
			TreeNode refnode = new TreeNode("Assemblies");
			refnode.Name = DatabaseCollection.ref_asm_node;
			refnode.Tag = this;
			refnode.ImageKey = refnode.SelectedImageKey = ImageKeyNames.Assembly;
			foreach (ReferenceAssemblyElement elm in Assemblies)
			{
				refnode.Nodes.Add(elm.ToNode());
			}
			return refnode;
		}
	}
}
