/*
 * oio * 4/24/2012 * 10:11 AM
 */
#region Using
using System;
#endregion
namespace GeneratorTool.Controls
{
	/// <summary>
	/// This was simply a test.  Not implemented
	/// </summary>
	public interface ISyntaxRecognitionStrategy
	{
		string FileName { get; }
		string FileExtension { get; }
		string FileOrTemplateExtension { get; }
		bool IsTemplateFile { get; }
	}
}
