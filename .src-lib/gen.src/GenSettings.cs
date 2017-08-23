/* oio * 8/2/2014 * Time: 2:03 PM
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Generator.Core.Markup;
using Generator.Elements;
namespace GeneratorApp
{
	public class GenSettings
	{
		public string ReplacementTag { get; set; }
		public string TemplateName { get; set; }
		public string TableName { get; set; }
		public string DatabaseName { get; set; }
		public bool HasConfigFile { get { return (FileConfig != null); } }
		public bool HasSchemaAndTemplate { get { return (FileSchema != null) & (FileTemplates != null); } } 
		
		public FileInfo FileConfig { get; set; }
		public FileInfo FileTemplates { get; set; }
		public FileInfo FileSchema { get; set; }
		public FileInfo JsonConfig { get; set; }
		
		public FileInfo FileOut { get; set; }
		public FileInfo FileIn { get; set; }
	}
}


