using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

using Generator.Elements;
using Generator.Core.Markup;
using Generator.Export;
using Generator.Parser;

#if WPF4
using EventType = System.Windows.RoutedEventArgs;
#else

using EventType = System.EventArgs;
#endif
namespace Generator
{
	/// <summary>
	/// Note that Windows.Forms.TreeNode is referenced in correlation with
	/// most data-elements.
	/// </summary>
	public class TemplateManager : IDbConfiguration4, IFactory
	{
		static int tcount = 0;
		static int __dbIncr = 0;
		const int __tbIncr = 0;
		
		#if WPF4
		
		#region Commands: Add Field
		static public readonly ICommand CmdCombineTableTool = new RoutedCommand();
		static public readonly ICommand AddByteAction = new RoutedCommand();
		static public readonly ICommand AddInt32Action = new RoutedCommand();
		static public readonly ICommand AddBigintAction = new RoutedCommand();
		static public readonly ICommand AddNVarChar50Action = new RoutedCommand();
		static public readonly ICommand AddNVarCharAction = new RoutedCommand();
		static public readonly ICommand AddDecimalAction = new RoutedCommand();
		static public readonly ICommand AddFloatAction = new RoutedCommand();
		static public readonly ICommand AddDateTimeAction = new RoutedCommand();
		static public readonly ICommand AddBoolAction = new RoutedCommand();
		#endregion
		#region Commands: Template
		static public readonly ICommand CmdTemplateStateHistoryForward = new RoutedCommand();
		static public readonly ICommand CmdTemplateStateHistoryBackward = new RoutedCommand();
		static public readonly ICommand CmdTemplateSaveCurrent = new RoutedCommand();
		static public readonly ICommand CmdTemplateExportCurrent = new RoutedCommand();
		#if WPF4
		static public readonly ICommand EditorSaveCurrentCommand = new RoutedCommand();
		static public readonly InputBinding EditorSaveCurrent = new KeyBinding(EditorSaveCurrentCommand,Key.S,ModifierKeys.Control);
		#endif
		#endregion
		#region Commands: Database
		
		static public readonly ICommand CmdSelectedDatabase = new RoutedCommand();
		static public readonly ICommand CmdSelectedTable = new RoutedCommand();
		static public readonly ICommand CmdSelectedField = new RoutedCommand();
		
		// TODO: Not implemented: ICommand CmdCreateDatabaseConfiguration
		static public readonly ICommand CmdCreateDatabaseConfiguration = new RoutedCommand();
		static public readonly ICommand CmdNewDatabaseConfiguration = new RoutedCommand();
		
		static public readonly ICommand CmdSaveCurrentTemplate = new RoutedCommand();
		#endregion
		
		#endif
		
		#region Event: UpdateDatabaseContextRequest
		/// <summary></summary>
		internal event EventHandler UpdateDatabaseContextRequest;

		/// <summary></summary>
		internal protected void OnUpdateDatabaseContextRequest()
		{
			Logger.LogC("TemplateManager","OnUpdateDatabaseContextRequest");
			if (UpdateDatabaseContextRequest != null) {
				UpdateDatabaseContextRequest(this, EventArgs.Empty);
			}
		}

		/// <summary></summary>
		internal event EventHandler UpdateTemplateContextRequest;

		/// <summary></summary>
		internal protected void OnUpdateTemplateContextRequest()
		{
			Logger.LogC("TemplateManager","OnUpdateTemplateContextRequest");
			if (UpdateTemplateContextRequest != null) {
				UpdateTemplateContextRequest(this, EventArgs.Empty);
			}
		}
		#endregion
		
		#region Selection: Query
		
		public bool HasQuery { get { return SelectedQuery != null; } }
		public QueryElement selectedQuery = null;
		public QueryElement SelectedQuery { get { return selectedQuery; } set { selectedQuery = value; } }
		/// <summary>
		/// Generate default elements for a empty configuration file.
		/// </summary>
		/// <returns></returns>
		virtual public DatabaseCollection CreateConfig()
		{
			var dc	= new DatabaseCollection();
			var dbelm	= new DatabaseElement(string.Format(Messages.Node_New_Database_Element,__dbIncr));
			dc.Queries				= new List<QueryElement>();
			dc.Databases			= new List<DatabaseElement>(){ dbelm };
			__dbIncr++;
			
			dbelm.Items				= new List<TableElement>();
			dbelm.Views				= new List<DataViewElement>();
			var te					= new TableElement();
			te.Fields				= new List<FieldElement>();
			te.Name					= string.Format(Messages.Node_New_Table_Element, __tbIncr);
			
			dbelm.Items.Add(te);
			SelectedCollection = dc;
			return dc;
		}
		#endregion

		#region Selection: Templates
		
		//
		// Template Generator Helper
		// —————————————————————————
		
		public virtual string SelectedTemplateGroup { get; set; }
		public string LastSelectedTemplate { get; private set; }
 
		
		#if WPF4
		public ObservableCollection<string> GroupNames { get; set; }
		#else
		public List<string> GroupNames { get; set; }
		#endif
		
		#endregion
		
		//[Bindable]
		public string SelectedTemplateRowGroup
		{
			get { return (string)SelectedTemplateRow["Group"]; }
			set { SelectedTemplateRow["Group"] = value; }
		}
		
		/// <summary>Converts the selected template and returns the value.</summary>
		internal protected string CurrentDataTemplate
		{
			get
			{
				LastSelectedTemplate = this.SelectedTable == null ? "\"NO TABLE\"" : this.SelectedTable.Name;
				return this.SelectedCollection.ConvertInput( this, LastSelectedTemplate );
			}
		}
		
		// 
		// Template Context
		// ----------------
		
		public TemplateCollection Templates { get;set; }
		
		/// <summary>gets the selected template.  Sets the template however note that this is not inherited when passing through <see cref="ITemplateSelection.SelectedTemplate" /></summary>
		public TableTemplate SelectedTemplate { get; set; }
		
		#region SelectionType
		
		/// <summary>Get or Set the type of <see cref="Generator.Core.Types.TemplateType" />. Possible values are used to determine what part of the template is beging edited or previewed.</summary>
		/// <remarks>If this value isn't present will surely break just about any procedure dependant on the selected template.</remarks>
		public TemplateType SelectionType {
			get { return selectionType; }
			set {
				selectionType = value;
				if (SelectionTypeChanged != null) {
					SelectionTypeChanged(this, EventArgs.Empty);
				}
			}
		} TemplateType selectionType;
		
		public event EventHandler SelectionTypeChanged;

		#endregion
		
		//
		// Explicit (concealed) Interface Members
		// --------------------------------------
		
		/// <summary>IFactory instance implementation.</summary>
		ITemplateSelection IFactory.TemplateInstance { get { return this; } }
		
		/// <summary>ITemplateSelection instance implementation.</summary>
		ITemplateSelection ITemplateSelection.TemplateInstance { get { return this; } }
		
		//
		// Template DataTable & DataRowView
		// --------------------------------------
		
		/// <summary>DataRowView Selection of a specific Template Item</summary>
		public DataRowView SelectedTemplateRow { get; set; }
		
		/// <summary>DataRowView Selection of a specific Template Item</summary>
		public DataView SelectedGroupView { get; set; }
		
		/// <summary>A DataTable used for storing, sorting, managing, selecting and editing templates.</summary>
		public DataTable ItemsTable { get; set; }
		
		internal bool CanSaveTemplatesProperty
		{
			get
			{
				if (Templates==null) return false;
				if (Templates.FileLoadedOrSaved==null) return false;
				return Templates.FileLoadedOrSaved != string.Empty;
			}
		}
		
		#if WPF4
		/// <summary>Enables Template groups and filters.
		/// See RefreshGroups (method is probably renamed) method body.
		/// </summary>
		public ObservableCollection<string> TemplateGroups { get; set; }
		#else
		public List<string> TemplateGroups { get; set; }
		#endif
		
		#region Selection: Database
		public DatabaseCollection	SelectedCollection { get; set; }
		public bool IsSelectedFieldPrimary { get { return (HasSelectedField | HasSelectedDatabase) && SelectedField.DataName == SelectedDatabase.Name; } }
		
		public bool HasSelectedDatabase { get { return SelectedDatabase != null; } }
		public bool HasSelectedTable { get { return SelectedTable != null; } }
		public bool HasSelectedField { get { return SelectedField != null; } }
		public bool HasSelectedView { get { return SelectedView != null; } }
		public bool HasSelectionForField { get { return HasSelectedDatabase && HasSelectedTable&&HasSelectedField; } }
		
		public DatabaseElement		SelectedDatabase { get; set; }
		public TableElement			SelectedTable { get; set; }
		public DataViewElement		SelectedView { get; set; }
		public DataViewLink			SelectedLink { get; set; }
		public FieldElement			SelectedField { get; set; }
		public IDatabaseCollection	Instance { get { return this; } }
		#endregion
		
		#region Obsolete: FromITemplateSelection
		
		/// <summary>
		/// This methode seems to be Obsolete,
		/// </summary>
		/// <param name="selection"></param>
		void FromITemplateSelection(ITemplateSelection selection)
		{
			SelectedTemplate = selection.SelectedTemplate;
			Templates = selection.Templates;
		}
		
		#endregion
		
		#region Action: AddNewTemplate
		
		public void AddNewTemplateHandler(object sender, EventType args)
		{
			this.AddNewTemplateAction();
		}
		
		public void AddNewTemplateAction()
		{
			++tcount;
			Logger.LogY("TemplateContext","AddNewTemplate");
			var tt = new TableTemplate();
			tt.Alias = string.Format("(New{0})", tcount);
			tt.Group = SelectedTemplateGroup;
			tt.ElementTemplate = string.Empty;
			tt.ItemsTemplate = string.Empty;
			tt.ToTable(ItemsTable);
			const string templateContext = "TemplateContext";
			const string str = "              : {0} : {1}";
			Logger.LogY(templateContext, str, SelectedTemplateGroup, this);
		}
		
		#endregion
		
		/// <summary>.ctor</summary>
		public TemplateManager()
		{
		}

	}
}
