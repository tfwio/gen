/* oio : 1/21/2014 9:33 AM */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Generator.Elements;

namespace GeneratorTool.Views
{
	interface IBindCommands
	{
		void UpdateBindings(UserControl window);
	}
	// based on modern-ui CommandBase?
	abstract public class BasicCommand : ICommand
	{
	  public Predicate<object> Cando { get; set; }
	  public Action<object>    Do    { get; set; }
	  
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		
		public void OnCanExecuteChanged() { CommandManager.InvalidateRequerySuggested(); }
		
		public virtual bool CanExecute(object parameter) { return true; }
//		public virtual bool CanExecute(object parameter) { return if (Cando == null) (if (Do!=null) Do(parameter) : true): Cando(parameter); }
		
		public void Execute(object parameter) { if (!this.CanExecute(parameter)) { return; } this.OnExecute(parameter); }
		
		protected abstract void OnExecute(object parameter);
	}
	/// <summary>
	/// The content here was taken from a table-copy command.
	/// It is merely temporary.
	/// </summary>
	public class DatabaseCreateCommand : BasicCommand
	{
		public MoxiView View { get; set; }
	  
		public override bool CanExecute(object parameter)
		{
      return View != null &&
        View.Model != null &&
        View.Model.ClipboardItem != null &&
        View.Model.ClipboardItem is TableElement;
		}

		protected override void OnExecute(object parameter)
		{
			var table = parameter as TableElement;
			if (table == null) {
				ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
				return;
			}
			//	else ModernDialog.ShowMessage(parameter.ToString(), "Good!", MessageBoxButton.OK);
      var parent = table.Parent;
			int index = parent.Children.IndexOf(table);
			var elm = new TableElement(View.Model.ClipboardItem as TableElement);
			//			elm.Name = string.Format("{0} (copy)", elm.Name);
			elm.Parent = parent;
			parent.Insert(index, elm);
			View.RefreshDataTree(parent);
		}
	}
	public class FieldCopyCmd : BasicCommand
	{
		public MoxiView View { get; set; }
		protected override void OnExecute(object parameter)
		{
			var field = parameter as FieldElement;
			if (field == null) {
				ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
				return;
			}
			var parent = field.Parent;
			View.Model.ClipboardItem = FieldElement.Clone(field);
		}
	}
	public class FieldCutCmd : BasicCommand
	{
		public MoxiView View { get; set; }
		protected override void OnExecute(object parameter)
		{
			var field = parameter as FieldElement;
			if (field == null) {
				ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
				return;
			}
			var parent = field.Parent;
			View.Model.ClipboardItem = FieldElement.Clone(field);
			parent.Fields.Remove(field);
			parent.Fields = parent.Fields;
		}
	}
	
	public class FieldPasteAboveCmd : BasicCommand
	{
		public MoxiView View { get; set; }
		public override bool CanExecute(object parameter)
		{
			if (View == null || View.Model == null || View.Model.ClipboardItem == null) return false;
			return View.Model.ClipboardItem is FieldElement;
		}
		protected override void OnExecute(object parameter)
		{
			var field = parameter as FieldElement;
			if (field == null) {
				ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
				return;
			}
			var parent = field.Parent;
			int index = parent.Fields.IndexOf(field);
			var elm = FieldElement.Clone(View.Model.ClipboardItem as FieldElement);
//			elm.DataName = string.Format("{0} (copy)", elm.DataName);
			elm.Parent = parent;
			parent.Fields.Insert(index, elm);
			parent.Fields = parent.Fields;
		}
	}
	public class FieldPasteBelowCmd : BasicCommand
	{
		public MoxiView View { get; set; }
		public override bool CanExecute(object parameter)
		{
			if (View==null || View.Model==null || View.Model.ClipboardItem==null) return false;
			return View.Model.ClipboardItem is FieldElement;
		}
		protected override void OnExecute(object parameter)
		{
			var field = parameter as FieldElement;
			if (field == null) {
				ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
				return;
			}
			var parent = field.Parent;
			int index = parent.Fields.IndexOf(field)+1;
			var elm = FieldElement.Clone(View.Model.ClipboardItem as FieldElement);
			elm.Parent = parent;
//			elm.DataName = string.Format("{0} (copy)",elm.DataName);
			parent.Fields.Insert(index, elm);
			parent.Fields = parent.Fields;
		}
	}
	public class GeneratorCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;
		public bool CanExecute(object parameter)
		{
			if (CanExecuteChanged!=null) CanExecuteChanged(this, new EventArgs());
			return true;
		}
		virtual public void Execute(object parameter)
		{
		}
	}
  public class MuiMessageCommand : BasicCommand
  {
    static public readonly MuiMessageCommand ShowMessageCommand = new MuiMessageCommand{};
    public string TestParameter { get; set; }
    protected override void OnExecute(object parameter) {
      ModernDialog.ShowMessage(string.Format("{{ Null={0}, Value: {1} }}", parameter==null, parameter),"Message Title",MessageBoxButton.OK);
    }
  }
  // TODO: implement command execution
  public class MyCommand : CommandBase
  {
    public ICommand Command { get; set; }
    protected override void OnExecute(object parameter)
    {
      if (Command != null && Command.CanExecute(parameter))
        Command.Execute(parameter);
    }
  }
  public class TableCopyCmd : BasicCommand
  {
    public MoxiView View { get; set; }
    protected override void OnExecute(object parameter)
    {
      var table = parameter as TableElement;
      if (table == null) {
        ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
        return;
      }
      View.Model.ClipboardItem = new TableElement(table);
    }
  }
  public class TableCutCmd : BasicCommand
  {
    public MoxiView View { get; set; }
    protected override void OnExecute(object parameter)
    {
      var table = parameter as TableElement;
      if (table == null) {
        ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
        return;
      }
      var parent = table.Parent;
      View.Model.ClipboardItem = new TableElement(table);
      parent.Remove(table);
      table.Parent = null;
      View.RefreshDataTree(parent);
    }
  }
  /// <summary>
  /// FIXME
  /// It appears this command is not working.
  /// </summary>
  public class TableCreateCmd : BasicCommand
  {
    public MoxiView View { get; set; }
    public override bool CanExecute(object parameter)
    {
      if (View == null || View.Model == null) return false;
      return parameter is DatabaseElement;
    }
    protected override void OnExecute(object parameter)
    {
      var database = parameter as DatabaseElement;
      if (database == null) {
        ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
        return;
      }
      var parent = database.Parent;
      var table = new TableElement(){
        Parent=database,
        BaseClass=null,
        DbType="SQLite",
        Description=null,
        Inherits=null,
        Name="New Table",
        Fields = new List<FieldElement>()
      };
//			table.items = new List<FieldElement>();
      table.Fields.Add(
        new FieldElement(){
          Parent=table,
          DataName="id",
          DataTypeNative="Int64",
          DataType="INTEGER"
        });
      table.PrimaryKey = "id";
      database.Children.Add(table);
      View.RefreshDataTree(database);
    }
  }
  public class TablePasteAboveCmd : BasicCommand
  {
    public MoxiView View { get; set; }
    public override bool CanExecute(object parameter)
    {
      if (View == null || View.Model == null || View.Model.ClipboardItem == null) return false;
      return View.Model.ClipboardItem is TableElement;
    }

    protected override void OnExecute(object parameter)
    {
      var table = parameter as TableElement;
      if (table == null) {
        ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
        return;
      }
      //			else ModernDialog.ShowMessage(parameter.ToString(), "Good!", MessageBoxButton.OK);
      var parent = table.Parent;
      int index = parent.Children.IndexOf(table);
      var elm = new TableElement(View.Model.ClipboardItem as TableElement);
//			elm.Name = string.Format("{0} (copy)", elm.Name);
      elm.Parent = parent;
      parent.Insert(index, elm);
      View.RefreshDataTree(parent);
    }
  }
  public class TablePasteBelowCmd : BasicCommand
  {
    public MoxiView View { get; set; }
    public override bool CanExecute(object parameter)
    {
      if (View == null || View.Model == null || View.Model.ClipboardItem == null) return false;
      return View.Model.ClipboardItem is TableElement;
    }
    
    protected override void OnExecute(object parameter)
    {
      var table = parameter as TableElement;
      var parent = table.Parent;
      if (table == null) {
        ModernDialog.ShowMessage("no field detected.", "error", MessageBoxButton.OK);
        return;
      }
      int index = parent.Children.IndexOf(table)+1;
      var elm = new TableElement(View.Model.ClipboardItem as TableElement);
//			elm.Name = string.Format("{0} (copy)", elm.Name);
      elm.Parent = parent;
      parent.Insert(index, elm);
      View.RefreshDataTree(parent);
    }
  }
}
