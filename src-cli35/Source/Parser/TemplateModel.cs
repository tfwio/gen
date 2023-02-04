using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Generator.Elements;
using Generator.Core.Markup;
using Global = System.Cor3.last_addon;
using GenConfig = Generator.Export.Intrinsic.IDbConfiguration4;
namespace Generator.Parser
{

  public class TemplateModel
  {
    internal GenConfig ConfigMemory;
    internal DatabaseElement  DB;
    internal TableElement     T;
    internal TableTemplate    TT;
    
    public string TemplateName, DatabaseName, TableName, TemplateContentReformat;
    public string TemplateBody { get; set; }
    public string TemplateContent { get; set; }
    
    void LoadConfig(GenConfig config)
    {
      ConfigMemory    = config;
      DatabaseName    = config.SelectedTable.Parent?.Name ?? "[Unknown]";
      TableName       = config.SelectedTable.Name ?? "[Unknown]";
      TemplateBody    = config.SelectedTemplate.ElementTemplate;
      TemplateContent = config.SelectedTemplate.ItemsTemplate;
    }
    
    /// full-initialize table-template.
    internal void PrepareReplaceValues()
    {
      TemplateContentReformat = T.ReplaceValues( TemplateContent );
    }
    /// prepare table-level template
    internal void PrepareReformat()
    {
      TemplateContentReformat = T.Reformat( TemplateContent ); // what's the table we're dealing with here?
    }
    
    public void SetView(DataViewLink link)
    {
      DB = ConfigMemory.SelectedCollection.Databases.FirstOrDefault(xdb => xdb.Name == link.Database);
      T = (TableElement)DB.Items.FirstOrDefault(t => T.Name == link.Table);
    }
    
    /// <summary>
    /// Set the datbase and table name for the view.
    /// </summary>
    /// <param name="name"></param>
    /// <remarks>
    /// if anybody is going to walk out of the context of a database and into the context
    /// of a database-collection, well --- this would be our guy.
    /// </remarks>
    public void SetView()
    {
      DB = ConfigMemory.SelectedCollection.Databases.FirstOrDefault(dbx=>dbx.Name== T.View.Database);
      T = DB.Items.FirstOrDefault(t => t.Name == T.View.Table);
    }
    
    /// <summary>Set DB as config-selected-db and T to the supplied 'tableName'.</summary>
    public void SetView(string tableName)
    {
      DB = ConfigMemory.SelectedDatabase;
      T = DB.Items.FirstOrDefault(t => t.Name == tableName);
    }
    
    public void SetTemplate(string alias)
    {
      TT = ConfigMemory.Templates[alias];
      TemplateBody = TT.ElementTemplate;
      TemplateContent = TT.ItemsTemplate;
      // why do this? //T.View = config.SelectedView;
      
    }
    
    static public TemplateModel LoadSelection(GenConfig config)
    {
      var view = new TemplateModel();
      view.LoadConfig(config);
      
      view.DB = config.SelectedCollection.Databases // should we  expand the context even further?
        .FirstOrDefault(db => db.Name == config.SelectedTable.Name);
      view.T  = config.SelectedTable;
      view.TT = config.SelectedTemplate;
      
      return view;
    }
    
    static public TemplateModel LoadView(GenConfig config)
    {
      var view   = LoadSelection(config);
      
      string sdb = config.SelectedView.Database, stb=config.SelectedView.Table;
      view.DB    = config.SelectedCollection.Databases.FirstOrDefault(db => db.Name == config.SelectedView.Database);
      view.T     = (TableElement)view.DB.Items.FirstOrDefault(t => t.Name == stb);
      
      if (TemplateFactory.CheckForError(view.DB, view.T, true)) // until we have something better
        throw new Exception(Gen.Strings.MsgDatabaseOrTableNullError_Exception);
      
      view.T.View     = config.SelectedView;
      
      return view;
    }
  }
}


