// Copyright (c) 2005-2013 tfwroble
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// oio 3/7/2012 11:19 PM
using System;
using System.Collections.Generic;
using System.Cor3.Data.Context;
using System.Cor3.Data.Engine;
using System.Data;
using System.Data.SQLite;
namespace System.Cor3.Data.Settings
{
	public class SQLiteSettings<TEventArgs> : SQLiteSettings where TEventArgs:EventArgs
	{
		/*
		class SettingAction : TAction<SQLiteSettings<TEvent>,TEvent> {
			public SettingAction(SQLiteSettings<TEvent> settings) : base(settings) { }
		}
		readonly SettingAction InitializeAction, GetIntAction, GetStringAction, SetStringAction;
		 */
		public SQLiteSettings(string filePath) : base(filePath)
		{
		}
	}
	public class SQLiteSettings
	{
		readonly string SettingsFile;
		static public string ApplicationName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }
		private readonly string ApplicationDirectory;
		string AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().FullName; } }
		private SQLiteSettingsContext NewContext { get { return new SQLiteSettings.SQLiteSettingsContext(SettingsFile); } }
		
		public setting this[string nam, string grp, string typ]
		{
			get { return GetValue(new setting(){Name=nam, Group=grp, SType=typ}); }
			//set { SetValue(new setting(){ Name=nam, Group=grp, SType=typ }); }
		}
		public setting this[string nam]
		{
			get { return this[nam,null,null]; }
//			set { SetValue(new setting(){ Name=nam, }); }
		}
		
		public setting ForceValue(setting value) { return SetValue(value,true); }

		#region CTX
		/// <summary>
		/// Used for a ONE-TIME operation.
		/// </summary>
		public class CTX : IDisposable
		{
			readonly SQLiteSettings C;
			readonly SQLiteSettings SC;
			readonly StatementType EC;
			protected delegate void CBAction(SQLiteSettingsContext c, SQLiteDb db, SQLiteDb.CBRowParam cba, SQLiteDb.CBParams cbp);
			protected delegate void CBParams(SQLiteDb.CBDataFill cbf, SQLiteDb.CBRowParam cba, SQLiteDb.CBParams cbp);
			protected void Act1(SQLiteSettings c, SQLiteDb db, SQLiteDb.CBRowParam cba, SQLiteDb.CBParams cbp)
			{
			}
			public void Update(SQLiteDb.CBParams cb)
			{
				using (SQLiteSettingsContext C=SC.NewContext) using (SQLiteDb db = new SQLiteDb(SC.SettingsFile))
				{
//					this.Act1(C,db,defaultaction,cb);
				}
			}
			public CTX(SQLiteSettings ss, StatementType st)
			{
				SC = ss; EC = st;
			}
			void IDisposable.Dispose() { if (C!=null) (C as IDisposable).Dispose(); }
		}
		#endregion

		public void InitializeData()
		{
//			using (Context=NewContext)
		}
		
		#region Setters
		public setting SetValue(setting value) { return SetValue(value,false); }
		public setting SetValue(setting value, bool force)
		{
			setting v = null;
			using (Context = NewContext) v = Context.SetValue(value,force);
			return value = v;
		}
		#endregion
		#region Getters
		public setting GetValue(string name, string grp)
		{
			return GetValue(new setting(){ Name=name, Group=grp});
		}
		public setting GetValue(setting value)
		{
			setting v = null;
			using (Context = new SQLiteSettings.SQLiteSettingsContext(SettingsFile))
			{
				v = Context.GetValue(value);
			}
			return v;
		}
		#endregion
		#region Properties
		public int GetNumberOfSettings()
		{
			int count =-1;
			using (Context = new SQLiteSettingsContext(this.SettingsFile))
			{
				Logger.LogM("SQLiteSettings","GetNumberOfSettings");
//				Logger.LogM("querying settings",this.Context.SELECT);
//				Context.Initialize();
				if (Context.data.Tables[0]==null)
				{
					Logger.Warn("SQLiteSettings","No Table");
					return -1;
				}
				if (Context.data.Tables[0].Rows==null)
				{
					Logger.Warn("SQLiteSettings","No Table-Rows");
					return -1;
				}
				count = Context.data.Tables[0].Rows.Count;
			}
			return count;
		}
		#endregion
		#region Enum: Settings Location
		public enum SettingsLocation
		{
			LocalAppSettings,
			MyDocuments,
		}
		#endregion

		#region Setting
		
		public class setting
		{
			const string KeyNamesAttr = "id,name,type,value,desc,grp,gid";
			public string this[string Key]
			{
				get
				{
					switch (Key)
					{
							case "id": return ID.ToString();
							case "name": return Name;
							case "value": return Value;
							case "desc": return Description;
							case "grp": return Group;
							case "gid": return GroupId.ToString();
							default: throw new KeyNotFoundException();
					}
				}
				set
				{
					switch (Key)
					{
							case "id": ID = int.Parse(value); break;
							case "name": Name = value; break;
							case "value": Value = value; break;
							case "desc": Description = value; break;
							case "grp": Group = value; break;
							case "gid": GroupId = int.Parse(value); break;
							default: throw new KeyNotFoundException();
					}
				}
			}
			
			public Byte? ToByte { get { byte i = 0; return Byte.TryParse(Value,out i) ? (byte?)i : null; } }
			public SByte? ToSByte { get { sbyte i = 0; return SByte.TryParse(Value,out i) ? (sbyte?)i : null; } }
			public Int32? ToInt32 { get { int i = 0; return int.TryParse(Value,out i) ? (int?)i : null; } }
			public UInt32? ToUInt32 { get { uint i = 0; return uint.TryParse(Value,out i) ? (uint?)i : null; } }
			public Double? ToDouble { get { double i = 0; return Double.TryParse(Value,out i) ? (double?)i : null; } }
			public Single? ToFloat { get { float i = 0; return float.TryParse(Value,out i) ? (float?)i : null; } }
			public Decimal? ToDecimal { get { decimal i = 0; return Decimal.TryParse(Value,out i) ? (decimal?)i : null ; } }
			public Boolean? ToBool { get { bool i = false; return Boolean.TryParse(Value,out i) ? (Boolean?)i : null; } }
			
			public int? ID = null;
			public string Name = null;
			public string SType = null;
			public string Value = null;
			public string Group = null;
			public int? GroupId = null;
			public string Description = null;
			
			public SQLiteDataAdapter AdapterInsert(DbOp op, string Query, SQLiteConnection c){
				SQLiteDataAdapter a = new SQLiteDataAdapter();
//				a.SelectCommand = new SQLiteCommand("SELECT 'return something so we don''t have an empty table.' [result];",c);
				a.InsertCommand = new SQLiteCommand(Query,c);
				this.ParameterizeUpdate(a.InsertCommand);
				return a;
			}
			public SQLiteDataAdapter AdapterUpdate(DbOp op, string Query, SQLiteConnection c){
				SQLiteDataAdapter a = new SQLiteDataAdapter();
//				a.SelectCommand = new SQLiteCommand("SELECT 'return something so we don''t have an empty table.' [result];",c);
				a.UpdateCommand = new SQLiteCommand(Query,c);
				this.ParameterizeUpdate(a.UpdateCommand);
				return a;
			}
			public int FillOperation(SQLiteDataAdapter A, DataSet D, string tablename)
			{
//				A.Fill(D,tablename);
				return 0;
			}
			public setting()
			{
			}
			public setting(DataRowView row)
			{
				int tempint = -1;
				bool temphasint = int.TryParse(row["id"].ToString(), out tempint);
				if (temphasint) this.ID = tempint;
				this.Name = row.Row.Field<string>("name");
				this.SType = row.Row.Field<string>("type");
				this.Description = row.Row.Field<string>("desc");
				this.Value = row.Row.Field<string>("value");
				this.Group = row.Row.Field<string>("grp");
				temphasint = int.TryParse(row["gid"].ToString(),out tempint);
				if (temphasint) this.GroupId = tempint;
				Logger.LogY("setting: row","id={0}, Name={1}, value={2}",row["id"], row["name"], row["value"]);
				Logger.LogY("SETTING",     "id={0}, Name={1}, value={2}",this.ID, this.Name, this.Value);
			}
			public SQLiteCommand ParameterizeInsert(SQLiteCommand command)
			{
				command.Parameters.AddWithValue("@name",this.Name);
				command.Parameters.AddWithValue("@type",this.Group);
				command.Parameters.AddWithValue("@value",this.Value);
				command.Parameters.AddWithValue("@desc",this.Description);
				command.Parameters.AddWithValue("@grp",this.Group);
				command.Parameters.AddWithValue("@gid",this.GroupId);
				return command;
			}
			public SQLiteCommand ParameterizeUpdate(SQLiteCommand command)
			{
				command.Parameters.AddWithValue("@xid",this.ID);
				command.Parameters.AddWithValue("@name",this.Name);
				command.Parameters.AddWithValue("@type",this.Group);
				command.Parameters.AddWithValue("@value",this.Value);
				command.Parameters.AddWithValue("@desc",this.Description);
				command.Parameters.AddWithValue("@grp",this.Group);
				command.Parameters.AddWithValue("@gid",this.GroupId);
				Logger.LogG("ParameterizeUpdate: ID", this.ID.ToString());
				Logger.LogG("ParameterizeUpdate: Value", this.Value);
				return command;
			}
		}
		#endregion

		#region SettingsContext
		SQLiteSettingsContext Context = null;
		/// <summary>
		/// we're not using this class typically.
		/// <para>
		/// In this case we're not interested in loading all of the content at all.
		/// we just want a setting or cluster of settings.
		/// </para>
		/// </summary>
		public class SQLiteSettingsContext : System.Cor3.Data.Context.SQLiteContext
		{
			#region const
			const string query_settingreset = @"drop table if exists [settings];
	create table [settings] (
	    [id]    integer primary key autoincrement, [type] varchar DEFAULT ""string"",
	    [gid]   integer DEFAULT -1, [grp]   varchar DEFAULT ""global"",
	    [name]  varchar, [desc]  varchar, [value] varchar
	);
	insert into settings (name,desc,value) values(
	    ""application"",
	    ""The owner application"",
	    ""$appname""
	);
	select * from settings;*/";
			#endregion

			#region GetValue
			public setting GetValue(setting value) { return GetValue(value.Name, value.Group); }
			public setting GetValue(SQLiteDb db, setting value) { return GetValue(db,value.Name, value.SType, value.Group); }
			public setting GetValue(string name, string grp)
			{
				setting s = null;
				using (SQLiteDb db = new SQLiteDb(datafile)) s = GetValue(db,name,null,grp);
				return s;
			}
			public setting GetValue(SQLiteDb db, string name, string typ, string grp)
			{
				AdapterSelectAction = delegate(DbOp op, string Query, SQLiteConnection c){
					SQLiteDataAdapter a = new SQLiteDataAdapter(Query, c);
					a.SelectCommand.Parameters.AddWithValue("@name",name);
					if (grp!=null) a.SelectCommand.Parameters.AddWithValue("@grp",grp);
					if (typ!=null) a.SelectCommand.Parameters.AddWithValue("@type",typ);
					return a;
				};
				string q = @"select * from [settings] where [name] = @name;";
				if (typ!=null) q.Replace(";"," and [type] = @type;");
				if (grp!=null) q.Replace(";"," and [grp] = @grp;");
				
				setting s = null;
				
				using (DataSet ds = db.Select(Context.TableName,q, AdapterSelectAction, FillSelect))
				{
					if (ds.Tables[0].Rows.Count == 1) s = new setting(ds.Tables[0].DefaultView[0]);
					else Logger.Warn("GetValue: ERROR","NOTHING RETURNED!");
				}
				return s;
			}
			#endregion
			#region SetValue
			public setting SetValue(setting value) { return SetValue(value,false); }
			public setting SetValue(setting value, bool force)
			{
				setting o = value;
				using (SQLiteDb db = new SQLiteDb(datafile)) o = SetValue(db,value,force);
				return o;
			}
			
			public setting SetValue(SQLiteDb db, setting value) { return SetValue(db,value,false); }
			public setting SetValue(SQLiteDb db, setting value, bool force)
			{
				// first check if the value is present
				setting s = GetValue(db,value.Name,value.SType,value.Group);
				bool hasValue = s!=null;
				if (force)
				{
					s.Value = value.Value;
					Logger.LogG("Force Value: ID", s.ID.ToString());
					Logger.LogG("Force Value", s.Value);
				}
				
				string q = hasValue ? @"UPDATE [settings] SET [name] = @name, [type] = @type, [value] = @value, [desc] = @desc, [grp] = @grp, [gid] = @gid WHERE [id] = @xid;" : @"INSERT INTO [settings] ( [name],[type],[value],[desc],[grp],[gid] ) VALUES ( @name, @type, @value, @desc, @grp, @gid );";
				// our insert statement
				if (hasValue) {
					using (DataSet ds = db.Update(Context.TableName, q, s.AdapterUpdate, s.FillOperation))
					{
						if (ds.Tables.Count==0) {}
//						else
						// if (ds.Tables[0].Rows.Count == 1) s = new setting(ds.Tables[Context.TableName].DefaultView[0]);
					}
				} else {
					using (DataSet ds = db.Insert(Context.TableName, q, s.AdapterInsert, s.FillOperation))
					{
//						if (ds.Tables.Count==0) MessageBox.Show(q, "there wasn't a table!");
//						else
						//if (ds.Tables[0].Rows.Count == 1) s = new setting(ds.Tables[Context.TableName].DefaultView[0]);
					}
				}
				return s;
			}

			#endregion
			#region DeleteValue
			public void DeleteValue(setting value)
			{
				Delete("delete from [settings] where [id] = @id".Replace("@id",value["id"]));
			}
			#endregion

			#region InitializeDataFile
			public void InitializeDataFile()
			{
				using (SQLiteDb db = new SQLiteDb(datafile))
					using (
						DataSet ds = db.Insert(
							query_settingreset.Replace("@appname",ApplicationName),
							delegate(
								DbOp op,
								string q,
								SQLiteConnection c
							)
							{
								SQLiteDataAdapter a = new SQLiteDataAdapter();
								a.InsertCommand = new SQLiteCommand(q,c);
								return a;
							})) { ; }
			}
//			public SQLiteDataAdapter InitializeDataFileCallback
			#endregion

			public override void Initialize() {
				Logger.LogM("SQLiteSettingsContext","Initialize");
				Logger.LogM("querying settings",this.Context.SELECT);
				this.Select(this.Context.SELECT);
			}
			
			public SQLiteSettingsContext(string settingfile) : base(getContext(settingfile))
			{
			}

			static QueryContextInfo getContext(string file)
			{
				return new QueryContextInfo(){
					TableName = "settings",
					TablePk = "id",
					TableTitle = "name",
					TableContent = "value",
					TableFile = file,
					TableFieldsAttribute = "name,desc,value,grp,gid",
					SqlSort = "name"
				};
			}
		}

		#endregion
		
		public SQLiteSettings(string filePath)
		{
			this.SettingsFile = filePath;
			//Context = new SQLiteSettingsContext(this.SettingsFile = filePath);
		}
	}
}
