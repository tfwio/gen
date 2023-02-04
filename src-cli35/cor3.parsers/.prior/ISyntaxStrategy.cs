/*
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Cor3.Parsers
{
	/// <summary>
	/// We need to do two things here: #1 Provide more support such as
	/// (IRange) GetNext, (IRange) GetPrevious and so fourth. #2 we need
	/// to provide more anonimity.
	/// </summary>
	public interface ISyntaxStrategy: IDisposable
	{
		void ResetBuffer(string text);
		void Parse();
//		IEnumerable<ITextRange> FragmentsFromOffset(int offset);
//		IEnumerable<ITextRange> FragmentsFromSection(TextRange range);
		int GetRowOffset(int offset);
		int GetColumnOffset(int offset);
		string Text { get; set; }
		string FileName { get; }
	}
}
