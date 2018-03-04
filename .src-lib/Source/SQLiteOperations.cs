#region Using
/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 07/18/2011
 * Time: 07:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Cor3.Data;
using System.Cor3.Data.Context;
using System.Cor3.Data.Engine;
using System.Data;
using System.Data.SQLite;
using System.IO;

#if WPF4
using System.Windows;
using Microsoft.Win32;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
#elif !NCORE
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
#endif

using Generator.Elements;
using Generator.Elements.Types;
using Generator.Core.Markup;
#endregion
namespace Generator
{
  /// <summary>
  /// Description of SQLiteOperations.
  /// </summary>
  public static class SQLiteOperations
  {
    #if WPF4 || !NCORE
    static public SaveFileDialog sfd = new SaveFileDialog();
    static public OpenFileDialog ofd = new OpenFileDialog();
    #endif
    
    static public Dictionary<string,DataColumn> db3(this FileInfo sqlite3_db, string table_name)
    {
      var dic = new Dictionary<string,DataColumn>();
      using (var db = new SQLiteQuery(sqlite3_db.FullName))
        using(var xo = db.ExecuteSelect("Select all from [$tablename$];",table_name))
      {
        foreach (DataRow r in xo.Tables[table_name].DefaultView)
        {
          for (int j = 0, rItemArrayLength = r.ItemArray.Length; j < rItemArrayLength; j++) {
            var i = r.ItemArray[j];
            dic.Add(xo.Tables[table_name].Columns[j].ColumnName, xo.Tables[table_name].Columns[j]);
          }
          break;
        }
      }
      return dic;
    }
    static public Dictionary<string,string> GetFieldNames(this FileInfo sqlite3_db, string table_name)
    {
      var dic = new Dictionary<string,string>();
      using (var db = new SQLiteQuery(sqlite3_db.FullName))
        using(var xo = db.ExecuteSelect("Select all from [$tablename$];",table_name))
      {
        foreach (DataRow r in xo.Tables[table_name].DefaultView)
        {
          for (int j = 0, rItemArrayLength = r.ItemArray.Length; j < rItemArrayLength; j++) {
            var i = r.ItemArray[j];
            dic.Add(xo.Tables[table_name].Columns[j].ColumnName, xo.Tables[table_name].Columns[j].DataType.Name);
          }
          break;
        }
      }
      return dic;
    }
    
    /// 0131
    static string ƒ(this string input, params object[] value) { return string.Format(input, value); }
    
    
    static public string Db2Code(this FileInfo f, string t)
    {
      string result = string.Empty;
      var dic = f.db3(t);
      foreach (var key in dic.Keys) result += "      public {0} {1} { get; set; }".ƒ(dic[key].ToString(), key);
      result += "    public class {0}\n  {\n{1}\n}".ƒ(result);
      return result;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db3"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <example>
    /// We're trying to create something like...
    /// <code>
    /// &lt;Database ConnectionType="SQLite" Name="AudioFileTagging">
    ///    &lt;Table Name="Tag" PrimaryKey="id" DbType="SQLite">
    ///      &lt;Field Tags="" DataType="INTEGER" DataTypeNative="Int64" DataName="id" IsArray="false" DefaultValue="DBNull.Value" BlockAction="" CodeBlock="id" FormType="HIDDEN" />
    ///      &lt;Field Tags="" DataType="VARCHAR" DataTypeNative="String" DataName="Artist" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="VARCHAR" DataTypeNative="String" DataName="Title" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="VARCHAR" DataTypeNative="String" DataName="Album" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="VARCHAR" DataTypeNative="String" DataName="AlbumArtist" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="VARCHAR" DataTypeNative="String" DataName="Composer" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="INTEGER" DataTypeNative="Int32" DataName="Year" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="VARCHAR" DataTypeNative="String" DataName="Copyright" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="INTEGER" DataTypeNative="Int32" DataName="TrackID" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="INTEGER" DataTypeNative="Int32" DataName="TrackCount" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="INTEGER" DataTypeNative="Int32" DataName="DiscID" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///      &lt;Field Tags="" DataType="INTEGER" DataTypeNative="Int32" DataName="DiscCount" IsArray="false" DefaultValue="DBNull.Value" CodeBlock="" />
    ///    &lt;/Table>
    ///  &lt;/Database.
    /// </code>
    /// </example>
//    static public string Db2XmlCode(this FileInfo f, string t)
//    {
//      var db = new TableElement(){
//        Name=t,
//        DbType="SQLite",
//        Fields=new List<FieldElement>(){
//        }
//      };
//      var types = f.db3(t);
//      var field = f.db3(t);
//      foreach (var key in dic.Keys)
//      {
//        db.Fields.Add(
//          new FieldElement(){
//            DataTypeNative=types[key].,
//            DataType=field[key]
//          });
//      }
//      return string.Empty;
//    }
    
    #region Generator Template
    /// <summary>
    /// Drops and recreates a SQLite Templates configuration
    /// within a particular SQLite Database. The new configuration
    /// is empty.
    /// </summary>
    /// <param name="databaseFile"></param>
    static public void GeneratorTemplatesDestroy(string databaseFile, bool andCreate)
    {
      #region Queries
      string query_drop_templates_table = @"DROP TABLE [generator-templates];";
      string query_create_templates_table = @"
	CREATE TABLE ""generator-templates"" (
			[id]				INTEGER PRIMARY KEY AUTOINCREMENT, pid INTEGER,
			[crd]				DATETIME DEFAULT NULL,
			[mod]				DATETIME DEFAULT NULL,
			summary				LONGVARCHAR DEFAULT NULL,
			mmd					LONGVARCHAR DEFAULT NULL,
			'Name'				VARCHAR DEFAULT NULL,
			'Alias'				VARCHAR DEFAULT NULL,
			'Group'				VARCHAR DEFAULT NULL,
			'SyntaxLanguage'	VARCHAR DEFAULT NULL,
			'Tags'				VARCHAR DEFAULT NULL,
			'ItemsTemplate'		VARCHAR DEFAULT NULL,
			'ElementTemplate'	VARCHAR DEFAULT NULL,
			'file-project'		LONGVARCHAR DEFAULT NULL,
			'file-type'			LONGVARCHAR DEFAULT NULL,
			'file-name'			LONGVARCHAR DEFAULT NULL
		);";
      #endregion
      using (SQLiteDb db = new SQLiteDb(databaseFile))
      {
        using (SQLiteConnection C = db.Connection)
        {
          using (SQLiteDataAdapter A = new SQLiteDataAdapter(string.Empty,C))
          {
            C.Open();
            A.InsertCommand = new SQLiteCommand(query_drop_templates_table,C);
            try { A.InsertCommand.ExecuteNonQuery(); }
            catch (Exception exc) { ErrorMessage.Show("Error executing the DROP query."); }//exc.ToString()
            if (andCreate)
            {
              A.InsertCommand = new SQLiteCommand(query_create_templates_table,C);
              try { A.InsertCommand.ExecuteNonQuery(); }
              catch (Exception exc) { ErrorMessage.Show("Error executing the CREATE query."); }//exc.ToString()
            }
            C.Close();
          }
        }
      }
    }
#if !NCORE
    static public void GeneratorTemplatesCreate(string databaseFile)
    {
      ofd.Filter = "Xml-Template Configuration|*.xml|All files|*";
      #if WPF4
      if (!ofd.ShowDialog().Value) return;
      #else
      if (ofd.ShowDialog()!=DialogResult.OK) return;
      #endif
      GeneratorTemplatesCreate(databaseFile,ofd.FileName);
    }
#endif
    static public void GeneratorTemplatesCreate(string databaseFile, string xmlTemplateFile)
    {
      TemplateCollection dc = TemplateCollection.Load(xmlTemplateFile);
      #region Query
      string cr_x = @"
insert into [generator-templates] (
	[Name],[Group],[Alias],ElementTemplate,ItemsTemplate,SyntaxLanguage
) values (
	@Name,@Group,@Alias,@ElementTemplate,@ItemsTemplate,@SyntaxLanguage
);";
      #endregion
      string query = string.Concat(cr_x);
      using (SQLiteDb db = new SQLiteDb(databaseFile))
      {
        using (SQLiteConnection C = db.Connection)
        {
          using (SQLiteDataAdapter A = new SQLiteDataAdapter(string.Empty,C))
          {
            C.Open();
            foreach (TableTemplate tpl in dc.Templates)
            {
              A.InsertCommand = new SQLiteCommand(query,C);
              A.InsertCommand.Parameters.AddWithValue("@Name",tpl.Name);
              A.InsertCommand.Parameters.AddWithValue("@ElementTemplate",tpl.ElementTemplate);
              A.InsertCommand.Parameters.AddWithValue("@ItemsTemplate",tpl.ItemsTemplate);
              A.InsertCommand.Parameters.AddWithValue("@Alias",tpl.Alias);
              A.InsertCommand.Parameters.AddWithValue("@Group",tpl.Group);
              A.InsertCommand.Parameters.AddWithValue("@SyntaxLanguage",tpl.SyntaxLanguage);
              A.InsertCommand.Parameters.AddWithValue("@Tags",tpl.Tags);
              try { A.InsertCommand.ExecuteNonQuery(); }
              catch (Exception exc) { ErrorMessage.Show(exc.ToString()); }
            }
            C.Close();
          }
        }
      }
    }
    #endregion
    /**
     * seems to have little to no use here.
     * ———————————————————————————————————
     * If it is to be useful, this function needs to be re-written.
     */
    #region ExportSQLiteCreateSnippit
#if !NCORE
    /// <summary>
    /// Uses a SaveFileDialog to get a file and continues to the overload.
    /// </summary>
    static public void ExportSQLiteCreateSnippit()
    {
      sfd.Filter = "SQL|*.sql|*.text|*.text|All files|*";
      #if WPF4
      if (!sfd.ShowDialog().Value) return;
      #else
      if (sfd.ShowDialog()!=DialogResult.OK) return;
      #endif
      ExportSQLiteCreateSnippit(sfd.FileName);
    }
    /// <summary>
    /// This method moved along with the others.
    /// I believe that it generally clones the table structures from one table to sql table definitions.
    /// 
    /// 
    /// it probably would be better to use SQLite3.exe to do this kind of thing.
    /// </summary>
    /// <remarks>
    /// Another reason to ignore this function is it's use of the notably unstable SQLiteContext class—which doesn't seem to bother assigning a file to the loaded and queried database.
    /// </remarks>
    static public void ExportSQLiteCreateSnippit(string sqlFile)
    {
      List<string> list = new List<string>();
      
      string output = string.Empty;
      using (var c = new System.Cor3.Data.Context.SQLiteContext())
      {
        c.Select("select * from sqlite_master");
        using (DataView v = c.Data.GetDataView(0))
        {
          foreach (DataRowView row in v)
          {
            list.Add(string.Format("# {0}",row["name"]));
            list.Add("");
            list.Add(row["sql"].ToString());
            list.Add("");
          }
        }
      }
      try { File.WriteAllText(sqlFile,string.Join("\r\n",list.ToArray()),System.Text.Encoding.UTF8); }
      catch (Exception e) { throw new Exception("There was an error writing to the file.",e); }
    }
#endif
    #endregion

    #region XmlToDatabaseConfiguration
#if !NCORE
    /// <summary>
    /// 1. Starts off by prompting you for a database-configuration file (*.xdata/xml)
    /// 
    /// 2. Continues to operate by calling XmlToDatabaseConfiguration.
    /// </summary>
    /// <param name="databaseFile">the sqlite database to manage.</param>
    static public void XmlToDbConfig_LoadXml(string databaseFile)
    {
      ofd.Filter = "xdata|*.xdata|xml|*.xml|all files|*";
      #if WPF4
      if (!ofd.ShowDialog().Value) return;
      #else
      if (ofd.ShowDialog()!=DialogResult.OK) return;
      #endif
      XmlToDatabaseConfiguration(ofd.FileName,databaseFile);
    }

    /// <summary>
    /// <para>1. Starts off by prompting you for a SQLite database.</para>
    /// <para>2. Continues to operate by calling XmlToDatabaseConfiguration.</para>
    /// </summary>
    /// <param name="xmlDatabaseConfiguration">the Xml database-config file-input.</param>
    static public void XmlToDbConfig_LoadSQLiteDb(string xmlDatabaseConfiguration)
    {
      ofd.Filter = "SQLite Database File (db,xdb,sqlite)|*.db;*.xdb;*.sqlite|All Files|*";
      #if WPF4
      if (!ofd.ShowDialog().Value) return;
      #else
      if (ofd.ShowDialog()!=DialogResult.OK) return;
      #endif
      XmlToDatabaseConfiguration(xmlDatabaseConfiguration,ofd.FileName);
    }
#endif

    /// <summary>
    /// <para>1. Iterate through the databases and tables inserting them as it goes.</para>
    /// <para>
    /// Data contained in the database file is destroyed/replaced if there were
    /// any database configuration elements.
    /// </para>
    /// <para>
    /// TODO: Support will be added to append (even a duplicate) data-configs to an
    /// existing configuration.
    /// </para>
    /// </summary>
    /// <param name="xmlDataConfig"></param>
    /// <param name="databaseFile"></param>
    static public void XmlToDatabaseConfiguration(string xmlDataConfig, string databaseFile)
    {
      // we need to ensure that the tables exist.
      DatabaseCollection c = DatabaseCollection.Load(xmlDataConfig);
      
      int countdb = 0, counttable = 0, countfield = 0;
      foreach (DatabaseElement elmDb in c.Databases)
      {
        string query = null;
        using (SQLiteDb db = new SQLiteDb(databaseFile))
        {
          using (SQLiteConnection C = db.Connection)
          {
            query = "insert into [gen-data-db] ([crd],[mod],[Name],[PrimaryKey],[ConnectionType]) values(@crd,@mod,@Name,@PrimaryKey,@ConnectionType);";
            using (SQLiteDataAdapter A = new SQLiteDataAdapter(string.Empty,C))
            {
              C.Open();
              A.InsertCommand = new SQLiteCommand(query,C);
              A.InsertCommand.Parameters.AddWithValue("@crd",DateTime.Now);
              A.InsertCommand.Parameters.AddWithValue("@mod",DateTime.Now);
              A.InsertCommand.Parameters.AddWithValue("@Name",elmDb.Name);
              A.InsertCommand.Parameters.AddWithValue("@PrimaryKey",elmDb.PrimaryKey);
              A.InsertCommand.Parameters.AddWithValue("@ConnectionType",elmDb.ConnectionType== ConnectionType.None?"":elmDb.ConnectionType.ToString());
              try {
                A.InsertCommand.ExecuteNonQuery();
                countdb++;
              }
              catch (Exception exc)
              {
                System.Diagnostics.Debug.Assert(false,exc.ToString());
              }
              
              C.Close();
            }
            // we can iterate again
            foreach (TableElement tpl in elmDb.Items)
            {
              using (SQLiteDataAdapter A = new SQLiteDataAdapter(string.Empty,C))
              {
                query = "insert into [gen-data-table] ([pid],[crd],[mod],[Name],[DbType],[Description],[PrimaryKey]) values(@pid,@crd,@mod,@Name,@DbType,@Description,@PrimaryKey);";
                C.Open();
                A.InsertCommand = new SQLiteCommand(query,C);
                A.InsertCommand.Parameters.AddWithValue("@crd",DateTime.Now);
                A.InsertCommand.Parameters.AddWithValue("@mod",DateTime.Now);
                A.InsertCommand.Parameters.AddWithValue("@pid",countdb);
                A.InsertCommand.Parameters.AddWithValue("@Name",tpl.Name);
                A.InsertCommand.Parameters.AddWithValue("@DbType",tpl.DbType=="Unspecified"?"":tpl.DbType.ToString());
                A.InsertCommand.Parameters.AddWithValue("@Description",tpl.Description);
                A.InsertCommand.Parameters.AddWithValue("@PrimaryKey",tpl.PrimaryKey);
                try {
                  A.InsertCommand.ExecuteNonQuery();
                  counttable++;
                }
                catch (Exception exc)
                {
                  System.Diagnostics.Debug.Assert(false,exc.ToString());
//									if (MessageBox.Show(exc.ToString(),"table")== MessageBoxResult.OK) return; ;
                }
                C.Close();
              }
              foreach (FieldElement fld in tpl.Fields)
              {
                query = @"
				insert into [gen-data-field] (
					[crd],
					[mod],
					[pid],
					[BlockAction],
					[CodeBlock],
					[DataName],
					[Name],
					[DataType],
					[DataTypeNative],
					[DefaultValue],
					[Description],
					[FormatString],
					[FormType],
					[IsNullable],
					[MaxLength],
					[Tags],
					[UseFormat]
				) VALUES (
					@crd,
					@mod,
					@pid,
					@BlockAction,
					@CodeBlock,
					@DataName,
					@DataName,
					@DataType,
					@DataTypenative,
					@DefaultValue,
					@Description,
					@FormatString,
					@FormType,
					@IsNullable,
					@MaxLength,
					@Tags,
					@UseFormat);";
                using (SQLiteDataAdapter A = new SQLiteDataAdapter(string.Empty,C))
                {
                  C.Open();
                  A.InsertCommand = new SQLiteCommand(query,C);
                  A.InsertCommand.Parameters.AddWithValue("@crd",DateTime.Now);
                  A.InsertCommand.Parameters.AddWithValue("@mod",DateTime.Now);
                  A.InsertCommand.Parameters.AddWithValue("@pid",counttable);
                  A.InsertCommand.Parameters.AddWithValue("@BlockAction",fld.BlockAction);
                  A.InsertCommand.Parameters.AddWithValue("@CodeBlock",fld.CodeBlock);
                  A.InsertCommand.Parameters.AddWithValue("@DataName",fld.DataName);
                  A.InsertCommand.Parameters.AddWithValue("@DataType",fld.DataType);
                  A.InsertCommand.Parameters.AddWithValue("@DataTypeNative",fld.DataTypeNative);
                  A.InsertCommand.Parameters.AddWithValue("@DefaultValue",fld.DefaultValue);
                  A.InsertCommand.Parameters.AddWithValue("@Description",fld.Description);
                  A.InsertCommand.Parameters.AddWithValue("@FormatString",fld.FormatString);
                  A.InsertCommand.Parameters.AddWithValue("@FormType",fld.FormType);
                  A.InsertCommand.Parameters.AddWithValue("@IsNullable",fld.IsNullable);
                  A.InsertCommand.Parameters.AddWithValue("@MaxLength",fld.MaxLength);
                  A.InsertCommand.Parameters.AddWithValue("@Tags",fld.Tags);
                  // A.InsertCommand.Parameters.AddWithValue("@ToolTip",fld.ToolTip);
                  A.InsertCommand.Parameters.AddWithValue("@UseFormat",fld.UseFormat);
                  // A.InsertCommand.Parameters.AddWithValue("@DataName",fld.);
                  try {
                    A.InsertCommand.ExecuteNonQuery();
                    countfield++;
                  }
                  catch (Exception exc)
                  {
                    System.Diagnostics.Debug.Assert(false,exc.ToString());
//										if (MessageBox.Show(exc.ToString(),"gen-data-field")== MessageBoxResult.OK) return; ;
                  }
                  C.Close();
                }
              }
            }
          }
        }
      }
    }
    #endregion

    // 
    #region GeneratorDataCollectionDestroy
    /// <summary>
    /// This overload calls the overload forcing creation
    /// of new tables once the tables are dropped.
    /// </summary>
    static public void DropDataConfigurationTable(string databaseFile)
    {
      DropDataConfigurationTable(databaseFile,true);
    }
    /// <summary>
    /// The SQLite Database is cleared and optionally the table structure
    /// is re-created.
    /// </summary>
    /// <param name="databaseFile"></param>
    /// <param name="andRecreate"></param>
    static public void DropDataConfigurationTable(string databaseFile, bool andRecreate)
    {
      string queryDrop = @"
		drop table [gen-data-field];
		drop table [gen-data-table];
		drop table [gen-data-db];",
      query = @"
	CREATE TABLE ""gen-data-field"" (
			id					INTEGER PRIMARY KEY AUTOINCREMENT,
			pid					INTEGER,
			'crd'				DATETIME DEFAULT NULL,
			'mod'				DATETIME DEFAULT NULL,
			summary				LONGVARCHAR DEFAULT NULL,
			mmd					LONGVARCHAR DEFAULT NULL,
			'Name'				VARCHAR DEFAULT NULL,
			'BlockAction'		VARCHAR DEFAULT NULL,
			'CodeBlock'			LONGVARCHAR DEFAULT NULL,
			'DataName'			VARCHAR DEFAULT NULL,
			'DataType'			VARCHAR DEFAULT NULL,
			DataTypeNative		VARCHAR DEFAULT NULL,
			DefaultValue		VARCHAR DEFAULT NULL,
			Description			VARCHAR DEFAULT NULL,
			FormatString		LONGVARCHAR DEFAULT NULL,
			FormType			VARCHAR DEFAULT NULL,
			IsNullable			BOOLEAN,
			MaxLength			INTEGER,
			UseFormat			BOOLEAN DEFAULT 0,
			Tags				VARCHAR DEFAULT NULL
		);
	CREATE TABLE ""gen-data-table"" (
			id					INTEGER PRIMARY KEY AUTOINCREMENT,
			pid					INTEGER,
			'crd'				DATETIME DEFAULT NULL,
			'mod'				DATETIME DEFAULT NULL,
			summary				LONGVARCHAR DEFAULT NULL,
			mmd					LONGVARCHAR DEFAULT NULL,
			'Name'				VARCHAR DEFAULT NULL,
			'Description'		VARCHAR DEFAULT NULL,
			'PrimaryKey'		VARCHAR DEFAULT NULL,
			'DbType'			VARCHAR DEFAULT NULL
		);
	CREATE TABLE ""gen-data-db"" (
			id					INTEGER PRIMARY KEY AUTOINCREMENT,
			pid					INTEGER,
			'crd'				DATETIME DEFAULT NULL,
			'mod'				DATETIME DEFAULT NULL,
			summary				LONGVARCHAR DEFAULT NULL,
			mmd					LONGVARCHAR DEFAULT NULL,
			'Name'				VARCHAR DEFAULT NULL,
			'PrimaryKey'		VARCHAR DEFAULT NULL,
			'ConnectionType'	VARCHAR DEFAULT NULL
		);
	";
      using (SQLiteDb db = new SQLiteDb(databaseFile))
      {
        using (SQLiteConnection C = db.Connection)
        {
          //query = "insert into [gen-data-db] ([Name],[PrimaryKey],[ConnectionType]) values(@Name,@PrimaryKey,@ConnectionType);";
          using (SQLiteDataAdapter A = new SQLiteDataAdapter(queryDrop,C))
          {
            C.Open();
            try
            {
              A.SelectCommand.ExecuteNonQuery();
            }
            catch
            {
              
            }
            if (andRecreate)
            {
              
              A.SelectCommand = new SQLiteCommand(query,C);
              try
              {
                A.SelectCommand.ExecuteNonQuery();
              }
              catch
              {
              }
            }
            C.Close();
            
          }
        }
      }
    }
    #endregion
    
    // empty
    #region GeneratorDataCollectionCreate
    #if !NCORE
    // FIXME: THIS DOES NOTHING?!
    /// <summary>
    /// this is a template method—as in that it is generally an empty method.
    /// </summary>
    static public void GeneratorDataCollectionCreate()
    {
      sfd.Filter = "xconfig|*.xconfig|xml|*.xml|all files|*";
      #if WPF4
      if (!sfd.ShowDialog().Value) return;
      #elif !NCORE
      if (sfd.ShowDialog()!=DialogResult.OK) return;
      #endif
      
      List<string> list = new List<string>();
      
      string fname = sfd.FileName;
      DatabaseCollection c = DatabaseCollection.Load(fname);
      int dbid = 1,tid=1;
      foreach (DatabaseElement elmDb in c.Databases)
      {
        using (var ct = new System.Cor3.Data.Context.SQLiteContext())
        {
          foreach (TableElement elmT in elmDb.Items)
          {
            foreach (FieldElement elmF in elmT.Fields)
            {
              
            }
            tid++;
          }
        }
        dbid++;
      }
      
      //			string output = string.Empty;
      //			using (SQLiteContext c0 = new SQLiteContext())
      //			{
      //				c.Select("select * from sqlite_master");
      //				using (DataView v = c.Data.GetDataView(0))
      //				{
      //					foreach (DataRowView row in v)
      //					{
      //						list.Add(string.Format("# {0}",row["name"]));
      //						list.Add("");
      //						list.Add(row["sql"].ToString());
      //						list.Add("");
      //					}
      //				}
      //			}
    }
    #endif
    #endregion
  }
}
