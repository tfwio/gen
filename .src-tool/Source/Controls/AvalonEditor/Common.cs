/*
 * User: oio
 * Date: 12/21/2011
 * Time: 1:16 AM
 */

#region Using
using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.Indentation.CSharp;

#endregion

namespace GeneratorTool.Controls
{
	static class Common
	{
		internal static readonly IHighlightingDefinition CSS2 = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("CSS2");
		
		/// <summary>
		/// Get a formatted string for a given file size.
		/// This helper method is known to have come from
		/// the samples provided by Awesomium SDK, though
		/// my understanding of this method came from dealing with
		/// resizing/repartitioning Virtual Hard Disk (VHD) files.
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
	    public static string GetFileSize( this FileInfo file )
	        {
	            long bytes = file.Length;
	
	            if ( bytes >= 1073741824 )
	            {
	                Decimal size = Decimal.Divide( bytes, 1073741824 );
	                return String.Format( "{0:##.##} GB", size );
	            }
	            else if ( bytes >= 1048576 )
	            {
	                Decimal size = Decimal.Divide( bytes, 1048576 );
	                return String.Format( "{0:##.##} MB", size );
	            }
	            else if ( bytes >= 1024 )
	            {
	                Decimal size = Decimal.Divide( bytes, 1024 );
	                return String.Format( "{0:##.##} KB", size );
	            }
	            else if ( bytes > 0 & bytes < 1024 )
	            {
	                Decimal size = bytes;
	                return String.Format( "{0:##.##} Bytes", size );
	            }
	            else
	            {
	                return "0 Bytes";
	            }
	        }
	        
		#region File To Base64 String
		/// <summary>
		/// Converts a file to base-64 string.
		/// This is an indirect call to the overload using formatting-options.None
		/// and concealing file-read exception.
		/// </summary>
		/// <param name="filePath">The input path</param>
		/// <returns>A base64 string or Null if there is an (File-Read) exception.</returns>
		internal static string ToBase64String(string filePath)
		{
			return ToBase64String(filePath,Base64FormattingOptions.None,false);
		}
		/// <summary>
		/// Converts a file to base-64 string.
		/// </summary>
		/// <param name="filePath">The input path</param>
		/// <param name="options">Base64FormattingOptions</param>
		/// <param name="exceptionOnError">If true, throws the exception -or- if false, returns null.</param>
		/// <returns>A base64 string or Null.</returns>
		internal static string ToBase64String(string filePath, Base64FormattingOptions options, bool exceptionOnError)
		{
			byte[] buffer = null;
			string returnvalue = null;
			System.IO.FileInfo fi = new FileInfo(filePath);
			if (fi.Length >= int.MaxValue) throw new FileLoadException("File is too big for the memory buffer");
			fi = null;
			buffer = File.ReadAllBytes(filePath);
			try { returnvalue = Convert.ToBase64String(buffer,Base64FormattingOptions.None); }
			catch { if (exceptionOnError) throw; }
			Array.Clear(buffer,0,buffer.Length); // this probably isn't necessary.
			buffer = null;
			return returnvalue;
		}
		#endregion
		
		/*
		 * In this section we have a bunch of file-extensions as used by the template
		 * loader/exporter.
		 * 
		 * These loaders (open/save file dialog) are either cloned from visualeditor
		 * project, or are similar.
		 * 
		 * None of this seems to have use in this project.
		 */
		#region String Const
		/// <summary>
		/// Documents
		/// </summary>
		internal const string xml_root = "documents";
		/// <summary>
		/// Document
		/// </summary>
		internal const string xml_item = "document";
		/// <summary>
		/// Path
		/// </summary>
		internal const string xml_type = "type";
		internal const string xml_file = "path";
		internal const string xml_curr = "isActive";
		internal const string file_ext_css = ".css";
		internal const string file_ext_tpl = ".tpl";
		internal const string filter_files = "Cascading Style-Sheet|*.css|" +
			"Asp.NET (aspx,ascx,master)|*.aspx;*.ascx;*.master|" +
			"CSharp (cs)|*.cs|" +
			"Template File|*.tpl|" +
			"All Files|*";
		internal const string filter_docs = "Document Collection|*.docs";
	
		#endregion
	
		#region Helper Methods
		/// <summary>
		/// Get the file-name of a template file without the *.tpl extension.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		internal static string GetFileOrTplExtension(string fileName)
		{
			if (System.IO.Path.GetExtension(fileName)==".tpl")
				return System.IO.Path.GetExtension(fileName.Replace(".tpl",""));
			return System.IO.Path.GetExtension(fileName);
		}
		/// <summary>
		/// Determine weather or not the extension ends with '.tpl'.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		internal static bool IsTemplateExtension(string fileName)
		{
			return System.IO.Path.GetExtension(fileName)==".tpl";
		}
		#endregion
	}
}
