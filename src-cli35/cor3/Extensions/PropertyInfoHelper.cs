/* tfwxo * 1/19/2016 * 2:10 AM */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System
{
  static public class PropertyInfoHelper
  {
    /// <param name="inherit">true to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events; see Remarks.</param>
    static public T GetCustomAttribute<T>(this PropertyInfo prop, bool inherit)
      where T:Attribute // class is nullable
    {
      var a = prop.GetCustomAttributes(typeof(T), false);
      return a.Length == 1 ? (T)a[0] : (T)null;
    }
    
    static public string GetStringValue(this object TargetClass, string PropertyName, string nullValue="")
    {
      Type t = null;
      PropertyInfo i = null;
      object val = null;
      try {
        t = TargetClass.GetType();
        i = t.GetProperty(PropertyName);
        val = i.GetValue(TargetClass,null);
      } catch {
        Console.WriteLine("Couldn't get a valid value for '{0}'", i.Name);
      }
      return string.Format("{0}", val ?? nullValue);
    }
    
    static public bool SetInstanceValue(this object TargetClass, string PropertyName, object value)
    {
      var type = TargetClass.GetType();
      var prop = type.GetProperty(PropertyName);
      var props = type.GetProperties();
      try { prop.SetValue( TargetClass, value, null); }
      catch { return false; }
      return true;
    }
    
    static public string GetFormatStringValue(this object TargetClass, string PropertyName, string format="{0}", string nullValue="")
    {
      var t = TargetClass.GetType();
      var i = t.GetProperty(PropertyName);
      var v = i.GetValue(TargetClass,null);
      return string.Format("{0}", v ?? nullValue);
    }

    static public string GetCategory(this PropertyInfo p, string defaultCategory="global")
    {
      var l = p.GetCustomAttributes(false).ToList();
      foreach (var item in l)
      {
        var categoryAttribute = item as CategoryAttribute;
        if (categoryAttribute != null)
        {
          var cat = categoryAttribute;
          return cat.Category;
        }
      }
      return defaultCategory;
    }
  }
}
