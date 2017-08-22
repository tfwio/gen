// - 20170728 added another DIC and serializer method.
// - copyright 20160119-20170728 tfwroble [gmail]
namespace System.IO
{
  using System;
  using System.Linq;
  using System.Collections.Generic;

  /// <summary>
  /// Ignore IniSerialization on a given Property.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  public class IgnoreAttribute : Attribute
  {
  }
  
  /// <summary>
  /// Ignore IniSerialization on a given Property.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  public class IniKeyAttribute : Attribute
  {
    public string Alias { get; set; }
    public string Group { get; set; }
    /// <summary>
    /// hasn't been implemented yet.
    /// </summary>
    public string Default { get; set; }
    public bool Ignore { get; set; }
  }
  
  /// <summary>
  /// The Serialization semantic works for string values only as only string values
  /// are stored to ini files, not to mention simplicity.
  /// 
  /// 
  /// if you would like to implement other types of values, you would be responsible
  /// for parsing complex values and the various number types, etc.
  /// </summary>
  public class IniCollection : Dictionary<string,Dictionary<string,string>>
  {
    public IniCollection(params string[] groups) : base() { if (groups.Length > 0) InitializeGroups(groups); }
    
    public IniCollection(IniCollection collection) : base(collection) { }
    
    public IniCollection(FileInfo file) : this(StaticLoad(file.FullName)) {}
    
    public IniCollection(object type_instance) : this() { FromInstance(type_instance); }
    
    internal void FromInstance(object input_o, string default_category=null)
    {
      var input_t = input_o.GetType();
      var WritableProperties = input_t.GetProperties().ToList();
      foreach (var i in WritableProperties)
      {
        string group_name = null, key_name=i.Name;
        var ign_attr = i.GetCustomAttribute<IgnoreAttribute>(false);
        var ali_attr = i.GetCustomAttribute<IniKeyAttribute>(false);
        
        if (ign_attr!=null) continue;
        if (ali_attr!=null && ali_attr.Ignore) continue;
        
        if (!string.IsNullOrEmpty(ali_attr.Alias)) key_name = ali_attr.Alias;
        group_name = ali_attr != null ? ali_attr.Group : i.Name;
        
        if (string.IsNullOrEmpty(group_name)) throw new Exception("Ini-file contained a 'headless' key/value (no group).");
        
        if (!ContainsKey(group_name)) InitializeGroups(group_name.Trim());
        
        this[group_name].Add(key_name,input_o.GetStringValue(i.Name, ""));
      }
    }
    
    public void ToInstance(object class_instance)
    {
      var class_type = class_instance.GetType();
      var WritableProperties = class_type.GetProperties().ToList();
      foreach (var i in WritableProperties)
      {
        string group_name = null, key_name=i.Name;
        
        var ign_attr = i.GetCustomAttribute<IgnoreAttribute>(false);
        var ali_attr = i.GetCustomAttribute<IniKeyAttribute>(false);
        
        if (ign_attr != null) continue;
        if (ali_attr != null && ali_attr.Ignore) continue;
        if (ali_attr != null) { group_name = ali_attr.Group; }// Group is a manditory field
        if (ali_attr != null && !string.IsNullOrEmpty(ali_attr.Alias)) { key_name = ali_attr.Alias; }
        if (string.IsNullOrEmpty(group_name)) throw new Exception("Ini-file contained a 'headless' key/value (no group).");
        var has = HasGroupAndKey(group_name, key_name);
        if (has)
        {
          if (!class_instance.SetInstanceValue(i.Name, this[group_name,key_name]))
            throw new Exception("Error writing instance value");
        }
      }
    }
    
    internal void InitializeGroups(params string[] groups) { foreach (var g in groups) this.Add(g, new Dictionary<string,string>()); }
    
    public void Write(string iniFile)
    {
      Write(iniFile.GetFileInfo(false));
    }
    public void Write(FileInfo file)
    {
      if (file.Exists) file.Delete();
      using (var writer = new StreamWriter(file.FullName, false, System.Text.Encoding.UTF8))
      {
        foreach (var k in this.Keys)
        {
          writer.WriteLine("[{0}]",k);
          writer.WriteLine("");
          foreach (var kv in this[k])
          {
            writer.WriteLine("{0} = {1}", kv.Key, kv.Value);
            writer.WriteLine("");
          }
        }
      }
    }
    
    bool HasGroupAndKey(string pGroup, string pKey) { return this[pGroup].ContainsKey(pKey); }
    
    public string this[string kGroup, string kKey]
    {
      get { return HasGroupAndKey(kGroup,kKey) ? this[kGroup][kKey] : null; }
      set { if (ContainsKey(kGroup)) this[kGroup].Add(kKey,value); }
    }
    
    static public IniCollection StaticLoad(string file)
    {
      var Dic = new IniCollection();
      if (!File.Exists(file)) return null;
      var data = File.ReadAllLines(file, Text.Encoding.UTF8);
      string k1 = "", k2 = "";
      foreach (var line in data)
      {
        int i = -1;
        var start = line.Trim(' ');
        if (string.IsNullOrEmpty(start)) continue;
        if (start[0]==';') continue;
        if (start[0]=='#') continue;
        if (start[0]=='[')
        {
          k1 = start.Trim('[',']');
          if (!Dic.ContainsKey(k1)) Dic.InitializeGroups(k1);
          continue;
        }
        else if ((i=start.IndexOf('=')) >= 0)
        {
          k2 = start.Substring(0,i);
          i++;
          var v = start.Substring(i,start.Length-i);
          Dic[k1].Add(k2.Trim(),v.Trim());
        }
      }
      data = null;
      return Dic;
    }
    
  }
  
  /// <summary>
  /// it could get a bit confusing that I have two dictionaries for the same purpose.
  /// both appear to work, one seems to be simpler than the other (dic&lt;string,dic&lt;str,str>>)
  /// but in reality they both do the same thing.
  /// 
  /// There was a little parser hiccup which forced me to rewrite anew which told me that
  /// I wasn't trimming the key and that I didn't have to re-write anything...
  /// 
  /// and now I have two frigging collection cases both of which happen to be used.
  /// 
  /// 
  /// this collection is used in <see cref="IniReader"/> and that is all.
  /// 
  /// IniReader also happens to be obsolete.
  /// </summary>
  public class IniDic : Dictionary<string,List<Keyed>> {
    
    public IniDic(params string[] groupNames) : base() { InitializeGroupNames(groupNames); }
    
    public void InitializeGroupNames(params string[] groupNames)
    {
      foreach (var g in groupNames) this.Add(g, new List<Keyed>());
    }
    
    public string this[string kGroup, string kKey]
    {
      get { return ContainsKey(kGroup) ? this[kGroup].First(f=>f.Name==kKey).Value : null; }
      set { if (ContainsKey(kGroup)) { this[kGroup].Add(new Keyed(kKey,value)); } }
    }
  }
  /// <summary>
  /// <c>Dictionary&lt;string,List&lt;Pair>>;</c> would be our primary container for all settings where the Key
  /// defines our group and each value is a (custom) keyvalue pair type of thing.
  /// </summary>
  /// <seealso cref="Pair"/>
  class IniReader : IDisposable
  {
    #region IDisposable implementation

    public void Dispose()
    {
      Dic.Clear();
    }

    #endregion
    
    public bool ContainsKey(string Key)
    {
      return Dic.ContainsKey(Key);
    }
    
    readonly IniDic Dic = new IniDic();

    /// <summary>
    /// Returns a formatted 'ini' Key/Value string such as
    /// "Key=Value"
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="Name">Name.</param>
    /// <param name="Value">Value.</param>
    static string KeyStr(string Name, object Value)
    {
      return string.Format("{0}={1}",Name,Value);
    }

    // 
    // Self Referencing Dictionary Lookups
    // ===========================================================
    
    [IgnoreAttribute]
    public string this[string k1, string k2] {
      get { return Dic == null ? null : this[k1]?.Find(p => p.Name == k2)?.Value; }
    }
    
    [IgnoreAttribute]
    public List<Keyed> this[string k1] {
      get { return Dic.ContainsKey(k1) ? Dic[k1] : null; }
    }

    // 
    // Get Value Methods
    // ===========================================================

    // getInt
    public int GetInt32(string k1, string k2, int defaultValue = 0)
    {
      var v = this[k1,k2];
      int result = defaultValue;
      return v == null ? defaultValue : int.TryParse(v, out result) ? result : defaultValue;
    }

    // getFloat
    public float GetFloat(string k1, string k2, float nullValue = 0.0f)
    {
      var v = this[k1,k2];
      float result = nullValue;
      return (v == null) ? nullValue : float.TryParse(v, out result) ? result : nullValue;
    }

    // getString
    public string GetChars(string k1, string k2, string nullValue=null)
    {
      return this[k1,k2] ?? nullValue;
    }
    public string GetChars(Keyed Key, string nullValue=null)
    {
      return this[Key.Name,Key.Value] ?? nullValue;
    }

    public void Load(string file)
    {
      if (!File.Exists(file)) return;
      Dic.Clear();
      var data = File.ReadAllLines(file, Text.Encoding.UTF8);
      string k1 = "", k2 = "";
      foreach (var line in data)
      {
        int i = -1;
        var start = line.Trim(' ');
        if (string.IsNullOrEmpty(start)) continue;
        if (start[0]==';') continue;
        if (start[0]=='#') continue;
        if (start[0]=='[')
        {
          k1 = start.Trim('[',']');
          Dic.Add(k1,new List<Keyed>());
          //Debug.Print("Key: {0} — Added",k1);
          continue;
        }
        else if ((i=start.IndexOf('=')) >= 0)
        {
          k2 = start.Substring(0,i);
          i++;
          var v = start.Substring(i,start.Length-i);
          Dic[k1].Add(new Keyed(k2,v));
          //Debug.Print("Sub: {0} — {1}",k2, v);
        }
      }
      data = null;
    }

  }

  /// <summary>
  /// this is a very general INI WRITER used for manually wrting values (IE: writing your own code)
  /// to an INI file.
  /// 
  /// 
  /// - <c><see cref="Write">Write()</see></c> appends a blank line.
  /// 
  /// - <c><see cref="Write(string)">Write(string)</see></c> Append a group.
  /// 
  /// - <c><see cref="Write(string,object)">Write(string)</see></c> Append a Key and Value to the last written group.
  /// In this case, the object is automatically converted using ToString() via a call to <c>String.Format(...)</c>
  /// as defined in <see cref="KeyStr(string, object)"/>.
  /// </summary>
  class IniWriter : IDisposable
  {
    Stream       Stream { get; set; }
    StreamWriter Writer { get; set; }
    #if __WIN
    public string NewLine { get; set; } = "\r\n";
    #else
    public string NewLine { get; set; } = "\n";
    #endif
    #region .ctor

    public IniWriter(string outputFile)
    {
      if (File.Exists (outputFile)) File.Delete (outputFile);
      Stream = new FileStream(outputFile, FileMode.OpenOrCreate);
      Writer = new StreamWriter(Stream);
      Writer.NewLine = NewLine;
      IsDisposed = false;
    }
    public IniWriter() : this(new MemoryStream())
    {
    }
    public IniWriter(Stream stream)
    {
      Stream = stream;
      Writer = new StreamWriter(Stream);
      IsDisposed = false;
    }
    ~IniWriter()
    {
      if (!IsDisposed)
        Dispose ();
    }

    #endregion

    /// <summary>
    /// Write a Key. If the key isn't surrounded in square-braces,
    /// it will be [wrapped] in them.
    /// </summary>
    /// <param name="Key">Key.</param>
    public void Write(string Key)
    {
      Writer.WriteLine(string.Format(Key[0]=='[' ? "{0}" : "[{0}]",Key));
    }

    /// <summary>
    /// Writes such as "Key=Value".
    /// </summary>
    /// <param name="Key">Key.</param>
    /// <param name="Value">Value.</param>
    public void Write(string Key, object Value)
    {
      Writer.WriteLine (KeyStr (Key,Value));
    }

    /// <summary>
    /// Writes a line-break using
    /// </summary>
    public void Write()
    {
      Writer.WriteLine ();
    }

    string KeyStr(string Name, object Value)
    {
      return string.Format("{0}={1}",Name,Value);
    }

    #region IDisposable implementation
    bool IsDisposed = false;
    public void Dispose ()
    {
      if (Writer!=null) {
        try {
          Writer.Close();
        } catch {
        }
        try {
          Writer.Dispose();
        } catch {
        } finally {
          Writer = null;
        }
      }
      if (Stream!=null) {
        try {
          Stream.Close();
        } catch {
        }
        try {
          Stream.Dispose();
        } catch {
        } finally {
          Writer = null;
        }
      }
      IsDisposed = true;
    }
    #endregion

  }

}

