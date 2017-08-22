/* oio * 01/21/2014 * Time: 09:09 */
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Generator;
using Generator.Core.Markup;
namespace GeneratorTool
{
	public class GeneratorUIModel
	{
		public object ClipboardItem {
			get { return clipboardItem; }
			set { clipboardItem = value; }
		} object clipboardItem = null;
		
		public TemplateManager TemplateContext { get; set; }

		public TemplateManager LastFactory { get; set; }

		public ViewMode LastViewMode {
			get { return lastViewMode; }
			set { lastViewMode = value; }
		} ViewMode lastViewMode = ViewMode.TemplateTable;

		public string ViewText {
			get { return viewText; } set { viewText = value; }
		} string viewText = null;
		

		public object LastTemplate { get; set; }

		public object LastSelectedObject { get; set; }

		public object LastSelectedView { get; set; }

		public GeneratorReader Reader { get; set; }

		public ObservableCollection<TableTemplate> TemplateGroups { get; set; }
	}
}




