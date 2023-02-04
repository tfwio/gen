/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.IO;
using System.Xml.Serialization;

namespace Generator.Elements
{
	[XmlRoot("config")]
	public class GeneratorConfig : SerializableClass<GeneratorConfig>
	{
		[XmlElement("file-data")] public string datafile;
		[XmlElement("file-template")] public string templatefile;
		[XmlElement("file-ex")] public string expressionfile;
		
		public class configuration
		{
			[XmlElement()] public string selectTable;
			[XmlElement] public string selectTemplate;
			[XmlElement] public string selectExpressions;
			public configuration()
			{
			}
		}
		[XmlElement("selection")] public configuration selection;
		
		public GeneratorConfig()
		{
			base.fileFilter = "generator-config|*.generator-config;";
		}
		public GeneratorConfig(Generator.Export.Intrinsic.IDbConfiguration4 win)
		{
			if (win.SelectedCollection!=null)
				datafile = win.SelectedCollection.FileLoadedOrSaved;
			if (win.Templates!=null)
				templatefile = win.Templates.FileLoadedOrSaved;
		}
	}
}
