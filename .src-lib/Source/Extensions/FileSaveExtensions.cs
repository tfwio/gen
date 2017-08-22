/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 2/10/2013
 * Time: 1:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace System
{
	static public class FileUtiltiyExtension
	{
		#if WPF4
		static public string GetFileName(this Microsoft.Win32.FileDialog dialog)
		{
			bool? result = dialog.ShowDialog();
			return result.HasValue && result.Value ? dialog.FileName : (string)null;
		}
		#endif
		/// <summary>
		/// Copies fileName to a time-stamped file in fmt: 'yyyyMMddHHmm'.
		/// <para>returns the generated file name weather or not the
		/// file is copied.</para>
		/// <para>We only back up the file if the input file exists.</para>
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		static public string BackupFile(this string fileName)
		{
			string bname = fileName.GenerateDatedFileName();
			if (System.IO.File.Exists(fileName))
			{
				System.IO.File.Copy(fileName,bname);
			}
			return bname;
		}
		static public string GenerateDatedFileName(this string fileName)
		{
//			return fileName.GenerateFileName(DateTime.Now.ToString("yyyyMMddHHmm"));
			return fileName.GenerateFileName(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
		}
		static public string GenerateFileName(this string filename, string append)
		{
			System.IO.FileInfo file = new System.IO.FileInfo(filename);
			string returned = file.FullName;
			string fname = file.Name;
			string val = string.Empty;
			if (file.Exists)
			{
				DateTime dt = new DateTime();
				List<string> list = new List<string>(fname.Split('-'));
				bool result = DateTime.TryParse(list[list.Count-1], out dt);
				val = result ?
					fname.Replace(list[list.Count-1],"").Trim('-',' '):
					fname;
			}
			return filename
				.Replace(file.Extension,string.Concat("-",append,file.Extension))
				;
		}
	}
}
