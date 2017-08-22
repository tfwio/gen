/* oOo * 11/14/2007 : 10:22 PM */
using System;
using System.ComponentModel.Design.Serialization;
using System.Xml.Serialization;

using DialogResult = System.Windows.Forms.DialogResult;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;

namespace System.IO
{

  /* oOo * 11/15/2007 : 5:53 AM */
  /**
   * IGNORE THIS CLASS... Skip down to the next SerializableClass<T>.
   * my intention is to call a open and save function when the program launches
   * and closes.  all i would need to do is send the required data such as a built
   * in configuration that would initialize a window to it's previous posision.
   * so you would Open(), and then Add(Component,'Setting',bool) as the bool value
   * would be set to false for items that are not serializable.
   **/
  public class Serial
  {
    static public void SerializeXml(string filepath,Type T,object o, XmlSerializerNamespaces xmlnss)
    {
      XmlSerializer xser = new XmlSerializer(T);
      {
        
        if (File.Exists(filepath)) File.Delete(filepath);
        using (FileStream fs = new FileStream(filepath,FileMode.OpenOrCreate))
          xser.Serialize(fs,o,xmlnss);
      }
    }
    static public void SerializeXml(string filepath,Type T,object o)
    {
      XmlSerializer xser = new XmlSerializer(T);
      {
        if (File.Exists(filepath)) File.Delete(filepath);
        using (FileStream fs = new FileStream(filepath,FileMode.OpenOrCreate))
        {
          xser.Serialize(fs,o);
        }
      }
    }
    static public void SerializeXml(string filepath,Type T,object o, XmlAttributeOverrides xao)
    {
      XmlSerializer xser;
      xser = new XmlSerializer(T,xao);
      if (File.Exists(filepath)) File.Delete(filepath);
      using (FileStream fs = new FileStream(filepath,FileMode.OpenOrCreate))
        xser.Serialize(fs,o);
    }
    static public string SerializeString(Type T,object o)
    {
      XmlSerializer xser;
      string str = null;
      using (MemoryStream ms = new MemoryStream())
      {
        xser = new XmlSerializer(T);
        xser.Serialize(ms,o);
        str = System.Text.Encoding.Default.GetString(ms.ToArray());
      }
      return str;
    }
    static public MemoryStream SerializeMemoryStream<T>
      (/*object o, System.Text.Encoding encoding*/ object o)
      where T:class, new()
    {
      XmlSerializer xser = new XmlSerializer(typeof(T));
      MemoryStream ms = new MemoryStream();
      xser.Serialize(ms,o);
      xser = null;
      return ms;
    }
    const string msg_deserializationError = "{0}\r\nPress Ok to ignore the Exception,\r\nCancel to throw the exception.";
    const string msg_deserializationErrorTitle = "There was an error reading the XML file…";
    static public T DeSerialize<T>(string file)
    {
      return DeSerialize<T>(file,false);
    }
    static public T DeSerialize<T>(string file, bool useMsgbox)
    {
      XmlSerializer xser = new XmlSerializer(typeof(T));
      object result = null;
      using (FileStream fs = File.Open(file,FileMode.OpenOrCreate))
      {
        try {
          result = xser.Deserialize(fs);
        } catch (Exception error) {
          DialogResult dr = DialogResult.None;
          #if TRACE
          Console.Error.Write("deserialization error");
          #endif
          
          if (useMsgbox) dr = MessageBox.Show(
            string.Format(msg_deserializationError,error.Message),
            msg_deserializationErrorTitle,MessageBoxButtons.OKCancel
           );
          if (useMsgbox && (dr==DialogResult.Cancel) ) { throw error; }
        } finally {
          
        }
      }
      return (T)result;
    }
    static public T DeSerialize<T>(string file, XmlAttributeOverrides xao)
    {
      XmlSerializer xser = new XmlSerializer(typeof(T),xao);
      FileStream fs = File.Open(file,FileMode.Open);
      T obj = (T)xser.Deserialize(fs);
      fs.Dispose();
      fs = null;
      return obj;
    }
    static public object DeSerialize(string file, Type T)
    {
      XmlSerializer xser = new XmlSerializer(T);
      FileStream fs = File.Open(file,FileMode.Open);
      object obj = (object)xser.Deserialize(fs);
      fs.Dispose();
      fs = null;
      return obj;
    }
    static public T DeSerialize<T>(Stream fs)
    {
      XmlSerializer xser = new XmlSerializer(typeof(T));
      T obj = (T)xser.Deserialize(fs);
      return obj;
    }
#if controlutil
    /// Not working
    static public void SerializeCode(SerializationStore store)
    {
      string fname = ControlUtil.FSave("Any|*");
      if (fname!=string.Empty)
        using (FileStream fs = new FileStream(fname,FileMode.OpenOrCreate))
          store.Save(fs);
    }
#endif
  }
  public partial class SerializableClass<T>
    where T:SerializableClass<T>, new()
  {
    virtual public XmlSerializerNamespaces SerializableNamespaces
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    /// <summary>
    /// separated by semi-colon.
    /// </summary>
    public string FileDialogSelectedFiles {
      get { return fileDialogSelectedFiles; }
      set { fileDialogSelectedFiles = value; }
    } string fileDialogSelectedFiles;
    
    virtual public bool UseNamespaces
    {
      get { return useNamespaces; }
    } internal protected bool useNamespaces = false;
    
    [XmlIgnore] public string FileLoadedOrSaved = string.Empty;
    
    [XmlIgnore] virtual protected string FileFilter
    {
      get { return fileFilter; }
      set { fileFilter = value; }
    } protected string fileFilter = "Xml File|*.xml|All Files|*.*"; //ControlUtil.XmlFile;
    #if controlutil

    virtual public void Save(T obj)
    {
      string fname = ControlUtil.FSave(FileFilter);
      if (fname==string.Empty) return;
      Save(fname,obj);
    }
    #endif
    
    virtual public void Save(string fname,T obj)
    {
      obj.FileLoadedOrSaved = fname;
      Serial.SerializeXml(fname,typeof(T),obj);
    }
    virtual public Stream SaveStream()
    {
      return Serial.SerializeMemoryStream<T>(this);
    }

    /// <summary>Gets a</summary>
    static public T LoadBase64(string fname)
    {
      T obj;
      string dataStr = File.ReadAllText(fname);
      byte[] data = Convert.FromBase64String(dataStr);
      using (MemoryStream ms = new MemoryStream(data))
      {
        XmlSerializer xs = new XmlSerializer(typeof(T));
        obj = (T)xs.Deserialize(ms);
      }
      Array.Clear(data,0,data.Length);
      dataStr = null; data = null;
      return obj;
    }
    #if controlutil
    virtual public void SaveBase64()
    {
      string fname = ControlUtil.FSave(FileFilter);
      if (fname==string.Empty) return;
      SaveBase64(fname);
    }
    #endif
    virtual public void SaveBase64(string fname)
    {
      byte[] data;
      using (MemoryStream ms = SaveStream() as MemoryStream)
        data = ms.ToArray();
      if (data.Length==0) return;
      string dataString = Convert.ToBase64String(data,Base64FormattingOptions.InsertLineBreaks);
      File.WriteAllText(fname,dataString);
      Array.Clear(data,0,data.Length);
      dataString = null; data = null;
    }
#if controlutil
    static public T Load()
    {
      T nclass = new T();
      string filter = (nclass as SerializableClass<T>).FileFilter;
      nclass = null;
      string fname = ControlUtil.FGet(filter);
      if (fname==string.Empty) return null;
      return Load(fname);
    }
#endif
    static public T Load(string fname)
    {
      T obj = null;
      try { obj = Serial.DeSerialize<T>(fname); } catch{  }
      if (obj!=null) obj.FileLoadedOrSaved = fname;
      return obj;
    }
    static public void Load(string fname, T obj)
    {
      try { obj = Serial.DeSerialize<T>(fname); } catch{  }
      if (obj!=null) obj.FileLoadedOrSaved = fname;
    }
  }
}
