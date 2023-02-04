#region User/License
//       2/10/2011 * 9:52 PM
// oio * 8/19/2012 * 5:55 PM

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

using Generator.Elements;
using Generator.Core.Markup;
using Generator.Parser;

namespace Generator.Export.Intrinsic
{
	/// <summary>
	/// The IFactory interface suggests a connection to a user-interface
	/// for our application.
	/// </summary>
	public interface IFactory {
//		System.Windows.Forms.TreeNode SelectedTreeNode { get; }

		bool HasSelectedDatabase { get; }
		bool HasSelectedTable { get; }
		bool HasSelectedField { get; }
		
		bool HasSelectionForField { get; }
		
		bool IsSelectedFieldPrimary { get; }
		
		//template elements
		DataTable ItemsTable { get; set; }
		DataRowView SelectedTemplateRow { get; set; }
		
		//more template elements
		TemplateCollection Templates { get; set; }
		
		TableTemplate SelectedTemplate { get; }
		
		TemplateType SelectionType { get; set; }
		
		ITemplateSelection TemplateInstance { get; }
		
		string SelectedTemplateRowGroup { get; set; }
		string SelectedTemplateGroup { get; set; }
		
		#if WPF4
		//using System.Collections.ObjectModel;
		ObservableCollection<string> GroupNames { get; set; }
		#else
		//using System.Collections.Generic;
		List<string> GroupNames { get; set; }
		#endif
		
		// database elements
		DatabaseCollection SelectedCollection { get; set; }
		DatabaseElement SelectedDatabase { get; set; }
		TableElement SelectedTable { get; set; }
		FieldElement SelectedField { get; set; }
	}

}
