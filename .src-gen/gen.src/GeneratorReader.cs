/* oio * 8/2/2014 * Time: 2:03 PM */
using System;
using System.IO;
using Generator;
using Generator.Core.Markup;
using Generator.Elements;
namespace GeneratorApp
{
	public interface IGeneratorReader
	{
		Action InitializeCompleteAction { get; set; }
		Action LoadCompleteAction { get; set; }
		Action SaveCompleteAction { get; set; }
		void Initialize();
	}
	public interface IGeneratorWriter
	{
		Action InitializeCompleteAction { get; set; }
		Action LoadCompleteAction { get; set; }
		Action SaveCompleteAction { get; set; }
		void Initialize();
	}
	public class GeneratorJsonWriter
	{
		public DatabaseCollection DataConfiguration { get;set; }
		public TemplateCollection Templates { get;set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class GeneratorReader : IGeneratorReader
	{
		public GeneratorModel Model { get; set; }

		#region (static) ICommand
		public Action InitializeCompleteAction { get; set; }

		public Action LoadCompleteAction { get; set; }

		public Action SaveCompleteAction { get; set; }

		public Action<string> TemplateGeneratedAction { get; set; }

//		CommandBindingCollection CommandBindings {
//			get;
//			set;
//		}
//
//		static public readonly ICommand InitializeConfigurationCommand = new RoutedUICommand() {
//			Text = "Initialize generator configuration-file."
//		};
//
//		static public readonly ICommand LoadConfigurationCommand = new RoutedUICommand() {
//			Text = "Load generator configuration-file.",
//			InputGestures =  {
//				new KeyGesture(Key.O, ModifierKeys.Control)
//			}
//		};
//
//		static public readonly ICommand SaveConfigurationCommand = new RoutedUICommand() {
//			Text = "Save generator configuration-file.",
//			InputGestures =  {
//				new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
//			}
//		};
//
//		static public readonly ICommand SaveConfigurationAsCommand = new RoutedUICommand() {
//			Text = "Save generator configuration-file (as).",
//			InputGestures =  {
//				new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
//			}
//		};

		#endregion
		#region readonly / constants
		const string filter = "Generator Configuration|*.generator-config|Xml Document (generator-config)|*.xml";

		#if !NCORE
		readonly OpenFileDialog ofd = new OpenFileDialog { Filter = filter };
		readonly SaveFileDialog sfd = new SaveFileDialog { Filter = filter };
    #endif
    
		#endregion
		#region Method: string Generate, IDbConfiguration4 GetConfig
		public Generator.Export.Intrinsic.IDbConfiguration4 GetConfig(TableElement table, TableTemplate template)
		{
			return new TemplateManager() {
				SelectedCollection = Model.Databases,
				SelectedDatabase = table == null ? null : table.Parent,
				SelectedTable = table,
				Templates = Model.Templates,
				SelectedTemplate = template,
				SelectedTemplateGroup = template == null ? null : template.Group,
			};
		}

		string GeneratedTemplate { get; set; }

		public string Generate(TableElement tableName, TableTemplate templateName)
		{
			var config = GetConfig(tableName, templateName);
			GeneratedTemplate = Model.Databases.ConvertInput(config, config.SelectedTable.Name);
			if (TemplateGeneratedAction != null)
				TemplateGeneratedAction.Invoke(GeneratedTemplate);
			config = null;
			return GeneratedTemplate;
		}

		#endregion
//		void ConfigLoad()
//		{
//			Model.FileName = ofd.FileName;
//			Model.Configuration = GeneratorConfig.Load(Model.FileName);
//			InitializeConfiguration(this, null);
//		}
		#region RoutedEvents
		/// <summary>
		/// Primary Generator Instance
		/// </summary>
		public void Initialize()
		{
			InitializeConfiguration(null,null);
    }
    /// <summary>
    /// Primary Generator Instance
    /// </summary>
    void InitializeConfiguration(object o, /*Routed*/EventArgs a)
		{
      FileInfo fifer = new FileInfo(Model.FileName);

      var datafile = (Model.Configuration.datafile.Length >=2)
                   && (Model.Configuration.datafile[1] == ':')
                    ? Model.Configuration.datafile
                    : Path.Combine(fifer.Directory.FullName, Model.Configuration.datafile);
      Model.Databases = DatabaseCollection.Load(datafile);

      // if we have something like {'c',':','/',...} or in other
      // words, a drive-letter and path, then we know not to use
      // "relative-path" mode.
      //
      // its become painfully obvious to me how old some of this
      // code and concept is/was.
      var tmplfile = (Model.Configuration.templatefile.Length >=2)
                   && (Model.Configuration.templatefile[1] == ':')
                    ? Model.Configuration.templatefile
                    : Path.Combine(fifer.Directory.FullName, Model.Configuration.templatefile);
      Model.Templates = TemplateCollection.Load(tmplfile);
      fifer = null;

			Model.Databases.Rechild(); // why is this necessary?

			// we need some error handling here

      if (InitializeCompleteAction != null) InitializeCompleteAction.Invoke();

		}
		#endregion
	}
}


