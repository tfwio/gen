/* oio * 01/21/2014 * Time: 09:09
 */
using System;
using System.Linq;
namespace GeneratorTool
{
   /// why not add <c>flags?</c>
  [Flags] public enum ViewMode
	{
		Undefined = 0,
		Database,
		DataView,
		Table,
		Field,
		TemplateTable,
		TemplateField,
		TemplatePreview
	}
}




