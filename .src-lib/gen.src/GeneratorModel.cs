/* oio * 8/2/2014 * Time: 2:03 PM
 */
using System;
using System.ComponentModel;
using Generator.Core.Markup;
using Generator.Elements;
namespace GeneratorApp
{
	public class GeneratorModel : INotifyPropertyChanged
	{
		public GeneratorModel(string configFile)
		{
			Configuration = GeneratorConfig.Load(configFile);
		}
		public GeneratorModel(string fileSchematic, string fileTemplates)
		{
			Configuration = new GeneratorConfig(){datafile=fileSchematic,templatefile=fileTemplates};
		}
		
		public string FileName {
			get { return fileName; }
			set { fileName = value; OnPropertyChanged("FileName"); }
		} string fileName;

		public GeneratorConfig Configuration {
			get { return configuration; }
			set { configuration = value; OnPropertyChanged("Configuration"); }
		} GeneratorConfig configuration;

		public DatabaseCollection Databases {
			get { return databases; }
			set { databases = value; OnPropertyChanged("Databases"); }
		} DatabaseCollection databases;

		public TemplateCollection Templates {
			get { return templates; }
			set { templates = value; OnPropertyChanged("Templates"); }
		} TemplateCollection templates;

		void GetTemplate(string groupKey)
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		virtual protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}




