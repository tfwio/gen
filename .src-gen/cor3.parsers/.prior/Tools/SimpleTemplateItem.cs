/*
 * oio * 04/22/2012 * 14:16
 */
using System;

namespace System.Cor3.Parsers.Tools
{
	public class SimpleTemplateItem
	{
		public string FileInput {get;set;}
		public string FileOutput {get;set;}
		
		public SimpleTemplateItem() { }
		/// <summary>
		/// Assumes to strip the extension '.tpl'
		/// </summary>
		/// <param name="fileInput">The Input File</param>
		public SimpleTemplateItem(string fileInput) : this(fileInput,".tpl") { }
		/// <summary>
		/// Sets FileInput, and strips 'stripExtension' to FileOutput.
		/// </summary>
		/// <param name="fileInput">The Input File</param>
		/// <param name="stripExtension">The Extension to strip: applied to FileOutput.</param>
		public SimpleTemplateItem(string fileInput, string stripExtension) { this.FileInput = fileInput; this.FileOutput = fileInput.Replace(stripExtension,string.Empty); }
		
		internal string Replace(string toReplace, string toReplaceWith) { return Replace(toReplace,toReplaceWith,System.Text.Encoding.UTF8,false); }
		internal string Replace(string toReplace, string toReplaceWith, System.Text.Encoding encoding) { return Replace(toReplace,toReplaceWith,encoding,false); }
		internal string Replace(string oldValue, string newValue, System.Text.Encoding encoding, bool IgnoreException) { string text = null; try { text = System.IO.File.ReadAllText(this.FileInput, encoding); } catch (Exception exception) { if (!IgnoreException) throw exception; } return text.Replace(oldValue,newValue); }
		
	}
}
