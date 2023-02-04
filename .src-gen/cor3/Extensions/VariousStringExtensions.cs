/*
 * User: xo
 * Date: 6/17/2017
 * Time: 11:33 PM
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace System
{
  
  static class VariousStringExtensions
  {
    /// Create Operators (Oprands) --- probably isn't used
    static public string Oprand(this string name, string oprand, string value, string separator="") { return string.Concat(name,oprand,value,separator); }
    
    static public string HtmlBracketA(this string input) { return input.Wrap("<",">"); }
    static public string HtmlBracketB(this string input) { return input.Wrap("</",">"); }
    static public string HtmlBracketC(this string input) { return input.Wrap("<"," />"); }
    
    static public string HtmlTag(this string input, string tag, bool is_std=true) { return string.Concat(is_std ? tag.HtmlBracketA() : string.Empty,input, is_std ? tag.HtmlBracketB() : tag.HtmlBracketC()); }
    
    static public string Wrap(this string input, string wrap) { return string.Concat(wrap,input,wrap); }
    static public string Wrap(this string input, string pre, string post) { return string.Concat(pre,input,post); }

    static public string ReplaceExtension(this FileSystemInfo fileinfo, string extension) { return fileinfo == null ? string.Empty : fileinfo.FullName.Replace(fileinfo.Extension, extension); }
    
    // parser helpers -----------------------------------------------
    
    const string chars_to_escape=@"=;#\";
    
    static public string DecodeAvString(this string input)
    {
      int i=0;
      string result = input;
      while (i < 4)
      {
        string nc = chars_to_escape[i].ToString();
        string nt = "\\"+nc;
        if (input.Contains(nt)) result.Replace(nt,nc);
        i++;
      }
      return result;
    }
    static public string EncodeAvString(this string input)
    {
      int i=0;
      string result = input;
      while (i < 4)
      {
        string nc = chars_to_escape[i].ToString();
        string nt = "\\"+nc;
        if (input.Contains(nc)) result.Replace(nc,nt);
        i++;
      }
      return result;
    }
    
    // tagging helpers ----------------------------------------------
    
    /// Spit and Trim.
    static public string[] SplitSemiColon(this string input) { return input.SplitBy(";"); }
    /// Spit and Trim.
    static public string[] SplitComma(this string input) { return input.SplitBy(","); }
    /// Spit and Trim.
    static public string[] SplitBy(this string input, string strToSplitBy, bool removeEmpties=true)
    {
      var para1 = new string[]{strToSplitBy};
      var array = string.IsNullOrEmpty(input) ? null : input.Split(para1, removeEmpties ? StringSplitOptions.RemoveEmptyEntries: StringSplitOptions.None);
      for (int i = 0; i < array.Length; i++) array[i] = array[i].Trim();
      return array;
    }
    
    /// Spit and Trim (to list).
    static public List<string> SplitByList(this string input, string strToSplitBy, bool removeEmpties) { return new List<string>(input.SplitBy(strToSplitBy,removeEmpties)); }
    
    static public string JoinSemiColon(this string[] input) { return input.JoinBy("; "); }
    static public string JoinBy(this string[] input, string joinBy) { return string.Join(joinBy, input); }
    static public string NullNotEmpty(this string input) { return string.IsNullOrEmpty(input) ? null : input; }
    static public string EmptyNotNull(this string input) { return input ?? string.Empty; }
    static public bool NullOrEmpty(this string input) { return string.IsNullOrEmpty(input); }
    
    // -------------------------------------------------
    // comment-helpers ----------------------------------------------
    // : windows text-box improperly shows unix line-endings so we need to apply/reverse them
    // : and will store them with just a \n as opposed to \r\n.
    
    static public string NormalizeEolUni(this string input)
    {
      return input.Replace("\r\n","\n");
    }
    static public string NormalizeEolWin(this string input)
    {
      return input.NormalizeEolUni().Replace("\n","\r\n");
    }

    // for using the dictionary containing FFmpeg commands
    
    // WTF?  I don't remem
    static public string rmake(this Dictionary<string,string> dic, string tag, params string[] me)
    {
      int len = 0;
      var result = dic[tag];
      while (++len % 2 == 1 && len < me.Length)
      {
        result = result.Replace(me[len-1], me[len]);
      }
      return result;
    }
    // WTF?  I don't remem
    static public string me(this string input, params string[] me)
    {
      int c = me.Length;
      int i = 0;
      string result = input;
      while (++i < c)
      {
        if (i %2 == 1) result = result.Replace(me[i-1], me[i]);
      }
      return result;
    }
    
    static public string MetaLine(this string title, string filter, params object[] values)
    {
      return string.Format("{0} = {1}", title.HtmlTag("b"), string.Format(filter, values));
    }
    
  }
  
}

