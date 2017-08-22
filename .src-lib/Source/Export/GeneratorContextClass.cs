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
#if WPF4
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Generator.Elements;
using Generator.Core.Markup;

namespace Generator.Export
{
public class GeneratorContextClass<TUIControl> : Generator.Export.Intrinsic.IGeneratorContext where TUIControl:Window
{
internal readonly TUIControl win;
internal Generator.Export.Intrinsic.IFactory iconfig = null;
//		public bool UseNamespaces { get;set; }
	
#region Database Context
public DatabaseCollection DataCollection { get; set; }
public ObservableCollection<DatabaseElement> Databases { get; set; }
public ObservableCollection<TableElement> Tables { get; set; }
public ObservableCollection<FieldElement> Fields { get; set; }
#endregion
	
#region Template Context
public TemplateCollection TemplateCollection { get; set; }
public ObservableCollection<string> TemplateGroups { get; set; }
public ObservableCollection<TableTemplate> Templates { get; set; }
#endregion
	
public string AssemblySectionName { get; set; }
public ObservableCollection<ReferenceAssemblyElement> AssemblyReferences { get; set; }
	
	
public GeneratorContextClass(TUIControl win)
{
	this.win = win;
}
}
}
#endif