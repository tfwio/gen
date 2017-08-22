using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Generator.Export;
using Generator.Parser;
using Generator;
using Generator.Resources;
using System.Windows.Forms;
namespace Generator.Elements
{
	public partial class DatabaseCollectionForms
	{
		#region TreeView Support
		#if TREEV
		public void ToTree(TreeView tn, bool append)
		{
			if (!append)
				tn.Nodes.Clear();
			if (Databases == null)
				return;
			var node = new TreeNode("Database Collection");
			var queries = new TreeNode(queryContainer.ToString());
			queries.ImageKey = queries.SelectedImageKey = queries.Name = queries.Text;
			queries.Tag = queryContainer;
			node.Tag = this;
			// the image correlates with the PanelTableEditor (or whatever it's called)
			node.SelectedImageKey = node.ImageKey = ImageKeyNames.Databases;
			#if ASMREF
						if (Assemblies!=null) node.Nodes.Add(Assemblies.ToNode());
			#endif
			if (Databases != null)
				foreach (DatabaseElement telm in Databases)
					telm.ToTree(node);
			//			foreach (DatabaseElement telm in Databases) telm.ToTree(node);
			if (Queries != null)
				foreach (QueryElement telm in Queries)
					telm.ToTree(queries);
			tn.Nodes.Add(node);
		}

		public DatabaseCollectionForms(TreeView tv)
		{
			base.useNamespaces = true;
			this.Databases = new List<DatabaseElement>();
			this.Queries = new List<QueryElement>();
			foreach (TreeNode node in tv.Nodes) {
				System.Windows.Forms.MessageBox.Show(node.Tag.ToString());
				if (node.Tag is DatabaseCollection) {
					TreeNode[] nodes = tv.Nodes.Find(ref_asm_node, true);
					bool hasit = nodes.Length > 0;
					Global.statR("DC");
					if (hasit) {
						MessageBox.Show("we have assembly entries");
						TreeNode found = nodes[0];
					}
					DatabaseCollection dc = node.Tag as DatabaseCollection;
					Global.statR("we're moving past the continue statement at DatabaseCollection level");
					foreach (TreeNode node1 in node.Nodes) {
						if (node1.Tag is DatabaseElement) {
							Databases.Add(new DatabaseElement(node1));
						}
						else
							if (node1.Tag == (queryContainer)) {
								foreach (TreeNode qnode in node1.Nodes) {
									QueryElement element = QueryElement.FromNode(qnode);
									Queries.Add(element);
								}
							}
							else
								if (node1.Tag is QueryElement) {
									Queries.Add(QueryElement.FromNode(node1));
								}
						#if ASMREF
												else if (node1.Tag is ReferenceAssemblyCollection) {
							Assemblies = node1.Tag as ReferenceAssemblyCollection;
						}
						#endif
					}
				}
			}
		}
	#endif
	#endregion
	}
}


