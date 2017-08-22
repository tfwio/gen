/* oio : 03/10/2014 00:34 */
using System;
using System.Reflection;
using System.Xml;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;

namespace GeneratorTool
{
	/// <summary>
	/// Description of utils.
	/// </summary>
	public static class AvalonEditorUtils
	{
		#region Load Resource
		
		static public void LoadXshdRes(string name, string xshdResource, params string[] extensions)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			using (System.IO.Stream s = asm.GetManifestResourceStream(xshdResource))
			{
				IHighlightingDefinition isql;
				using (XmlReader reader = new XmlTextReader(s)) isql =
					HighlightingLoader.Load(reader, HighlightingManager.Instance);
				HighlightingManager.Instance.RegisterHighlighting(name,extensions,isql);
			}
		}
//		static void LoadRtfResource(string name, string xshdResource, params string[] extensions)
//		{
//			Assembly asm = Assembly.GetExecutingAssembly();
//			using (System.IO.Stream s = asm.GetManifestResourceStream(xshdResource))
//			{
//				IHighlightingDefinition isql;
//				using (XmlReader reader = new XmlTextReader(s)) isql =
//					HighlightingLoader.Load(reader, HighlightingManager.Instance);
//				HighlightingManager.Instance.RegisterHighlighting(name,extensions,isql);
//			}
//		}
		#endregion
	}
}
