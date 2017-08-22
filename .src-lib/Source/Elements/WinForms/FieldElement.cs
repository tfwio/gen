using System;
using System.Linq;
using System.Windows.Forms;
namespace Generator.Elements
{
	public partial class FieldElement
	{
		#if TREEV
		public TreeNode ToNode()
		{
			var tn = new TreeNode(DataName);
			tn.Name = DataName;
			tn.ToolTipText = ToolTip;
			// the image correlates with the PanelTableEditor (or whatever it's called)
			tn.SelectedImageKey = tn.ImageKey = "field";
			tn.Tag = this;
			return tn;
		}

		public FieldElement(TreeNode tn)
		{
			if (tn.Tag is FieldElement) {
				var fe = tn.Tag as FieldElement;
				IsArray = fe.IsArray;
				DataName = fe.DataName;
				DataType = fe.DataType;
				DataTypeNative = fe.DataTypeNative;
				DefaultValue = fe.DefaultValue;
				Description = fe.Description;
				FormatString = fe.FormatString;
				IsNullable = fe.IsNullable;
				MaxLength = fe.MaxLength;
				UseFormat = fe.UseFormat;
				FormType = fe.FormType;
				Tags = fe.Tags;
				this.BlockAction = fe.BlockAction;
				this.CodeBlock = fe.CodeBlock;
			}
		}
	#endif
	}
}




