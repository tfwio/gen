/* oio : 3/9/2014 10:25 PM */
using System;
using System.Collections.Generic;
using System.Cor3.Data;
using System.Cor3.Data.Engine;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Win32;
using GeneratorTool.SQLiteUtil;

namespace GeneratorTool.Views
{
  /// <summary>
  /// Interaction logic for UserControl1.xaml
  /// </summary>
  public partial class SQLiteView : UserControl
  {
    
    static public readonly ICommand DbFileLoadCommand = new RoutedUICommand(){ InputGestures={ new KeyGesture(Key.O,ModifierKeys.Control) } };
    static public readonly ICommand DbFileCreateCommand = new RoutedUICommand();
    
    static public readonly ICommand SqlExecuteSelectCommand = new RoutedUICommand(){ InputGestures={ new KeyGesture(Key.F5) } };
    static public readonly ICommand SqlExecuteInsertCommand = new RoutedUICommand(){ InputGestures={ new KeyGesture(Key.F4) } };
    
    static public readonly RoutedUICommand ToggleSqlCommand = new RoutedUICommand(){ InputGestures={ new KeyGesture(Key.F2) } };
    static public readonly RoutedUICommand ToggleViewCommand = new RoutedUICommand(){ InputGestures={ new KeyGesture(Key.F3) } };
    
    static SQLFileLoader sqlFile = new SQLFileLoader();
    static DataFileLoader database = new DataFileLoader();
    
    DataSet data;
    
    public static bool PopError() {
      var v = HasSqlExecuteError|HasSqlSelectException;
      HasSqlExecuteError = false;
      HasSqlSelectException = false;
      return v;
    }
    
    public SQLiteView()
    {
      InitializeComponent();
      
      CommandBindings.Add(new CommandBinding (ToggleSqlCommand,(e,r)=>{ tabs.SelectedItem =  tabSql; tabSql.Focus(); edit.Focus(); }));
      CommandBindings.Add(new CommandBinding (ToggleViewCommand,(e,r)=>{ tabs.SelectedItem = tabView; btnNonQuery.Focus(); }));
      //    FIXME:SQLiteView() needs not to switch views to data on error.
      CommandBindings.Add(new CommandBinding (
        SqlExecuteSelectCommand,(e,r)=>{
          Event_ExecuteSelectQuery(e,r);
          if (!PopError()) tabs.SelectedItem = tabView;
          btnNonQuery.Focus();
        }));
      CommandBindings.Add(new CommandBinding (
        SqlExecuteInsertCommand,(e,r)=>{
          Event_ExecuteNonQuery(e,r);
          if (!PopError()) tabs.SelectedItem = tabView;
          btnNonQuery.Focus();
        }));
      tabs.ItemContainerStyle = new Style(){Setters={new Setter(UIElement.VisibilityProperty, Visibility.Collapsed)}};
    }
    public override void BeginInit()
    {
      base.BeginInit();
      Console.WriteLine("SQLiteView.BeginInit");
      sqlFile.UpdateBindings(this);
    }
    protected override void OnInitialized(EventArgs e)
    {
      base.OnInitialized(e);
      Console.WriteLine("SQLiteView.OnInitialized");
    }
    
    #region database commands
    int Adapt(SQLiteDataAdapter A, DataSet D, string tablename)
    {
      return A.Fill(D,tablename);
    }
    SQLiteDataAdapter DBSelect(DbOp op, string query, SQLiteConnection connection)
    {
      return new SQLiteDataAdapter(){ SelectCommand = new SQLiteCommand(query,connection) };
    }
    SQLiteDataAdapter DBInsert(DbOp op, string query, SQLiteConnection connection)
    {
      return new SQLiteDataAdapter(){ InsertCommand = new SQLiteCommand(query,connection) };
    }
    
    static void ThrowException(Exception error, bool selectError=false, bool queryError=false)
    {
      HasSqlSelectException=selectError;
      var t = string.Empty;
      if (selectError) t = $"SQL SELECT Query Exception";
      if (queryError) t = $"SQL EXECUTE Exception";
      var v = new FirstFloor.ModernUI.Windows.Controls.ModernDialog
      {
        Title = $"{t} (pewpew lazers rule)",
        Content = error.Message
      };
      //v.OkButton.Click += new RoutedEventHandler((o,a)=>{ MessageBox.Show("ok was clicked"); });
      v.Buttons = new Button[] { v.OkButton, v.CancelButton };
      var r = v.ShowDialog();
    }
    
    void Event_ExecuteNonQuery(object sender, RoutedEventArgs args)
    {
      if (!database.IsLoaded) return;
      using (var db = new SQLiteDb(database.DataFile))
      {
        try { data = db.Insert(edit.Text,DBInsert); }
        catch (Exception error) { ThrowException(error,queryError:true); }
      }
    }
    
    static bool HasSqlSelectException = false;
    static bool HasSqlExecuteError = false;
    
    void Event_ExecuteSelectQuery(object sender, RoutedEventArgs args)
    {
      if (!database.IsLoaded) return;
      using (var db = new SQLiteDb(database.DataFile))
      {
        try {
          data = db.Select("mytable", edit.Text, DBSelect,Adapt);
          if (data.Tables.Count > 0) grid.ItemsSource = data.Tables["mytable"].DefaultView;
        }
        catch (Exception error) { ThrowException(error,selectError:true) ; }
      }
    }
    void Event_CreateDb(object sender, RoutedEventArgs args)
    {
      database.Create();
      if (!database.IsLoaded)
      {
        return;
      }
    }
    void Event_OpenDb(object sender, RoutedEventArgs args)
    {
      database.Load();
      if (!database.IsLoaded)
      {
        return;
      }
    }
    void Event_OpenSQL(object sender, RoutedEventArgs args)
    {
      sqlFile.Load();
      if (!sqlFile.IsLoaded)
      {
        return;
      }
      edit.Load(sqlFile.SqlFile);
    }
    #endregion
  }
}