/*
 * Created by SharpDevelop.
 * User: oIo
 * Date: 2/2/2011
 * Time: 2:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;

using AvalonDock;
using AvalonEdit.Sample;
using Generator.AvalonEdit.Helpers;
using Generator.Classes;
using Generator.Dock;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Indentation;

namespace Generator.AvalonEdit.CompletionExtension
{
	// THIS CLASS IS NOT USED!!!!
	static class CompletionExtension
	{
		#region Data Template Constants
		const string ac_data  =
			"DbType;FriendlyName";
		const string ac_table =
			"TableName;TableNameC;" +
			"TableCleanName;TableCleanNameC;" +
			"TableNameClean;tablenameclean;TableNameCClean;" +
			"TableType;tabletype";
		const string ac_field =
			"FieldValues;FieldValues,Cdf;FieldValuesNK;FieldValuesNK,Cdf;" +
			"PKDataNameC;PKCleanName;PKCleanName,Nodash;" +
			"PKFriendlyName;PKFriendlyNameC";
		const string ac_pktyp =
			"PKDataName;PKDataType;PKDataTypeNative;PKDescription";
		const string ac_adapt =
			"AdapterNs;AdapterT;AdapterNsT;" +
			"CommandNs;CommandT;CommandNsT;" +
			"ConnectionT;ConnectionNsT;ConnectionNs;" +
			"ReaderNslReaderT;ReaderNsT";
		const string ac_prime =
			"PK;pk;PrimaryKey";
		// note that FriendlyName is doubled here
		// as it's first found also on line 30
		const string ac_types =
			"CleanName;FriendlyName;FriendlyNameC;" +
			"DataType;datatype;DataTypeNative;datatypenative;FlashDataType;" +
			"DataName;dataname;DataNameC;" +
			"FormatString;" +
			"max;MaxLMAX;nmax;smax;MaxLength;" +
			"IsString;IsBool;IsNum;" +
			"Native;" +
			"UseFormat;IsNullable;" +
			"FieldIndex;" +
			"Description;" +
			"DefaultValue";
		const string ac_erratum = "Date;Time;DateTime";
		static readonly string ac_001 = string.Concat(
			ac_data,";",
			ac_table,";",
			ac_field,";",
			ac_prime,";",
			ac_adapt,";",
			ac_pktyp);
		static readonly string ac_002 = string.Concat(
			ac_erratum,";",
			ac_types,";",
			ac_table,";",
			ac_prime,";",
			ac_adapt);
		const string fmt_field = "$({0})";
		#endregion

		static public void GetStrategyFromName(this TextEditor editor, IEditorContext control)
		{
			
			FoldingManager.Uninstall(control.FoldingManager);
			if (editor.SyntaxHighlighting == null) {}//control.FoldingStrategy = null;
			else
			{
				switch (editor.SyntaxHighlighting.Name) {
					case "XML":
						control.FoldingStrategy = new XmlFoldingStrategy();
						editor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						break;
					case "C#":
					case "C++":
					case "PHP":
					case "Java":
						editor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(editor.Options);
						control.FoldingStrategy = new CSharpPragmaRegionFoldingStrategy();
						break;
					default:
						editor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						control.FoldingStrategy = null;
						break;
				}
			}
			if (control.FoldingStrategy != null) {
				if (control.FoldingManager == null)
					control.FoldingManager = FoldingManager.Install(editor.TextArea);
				control.FoldingStrategy.UpdateFoldings(control.FoldingManager, editor.Document);
			} else {
				
				if (control.FoldingManager != null) {
					FoldingManager.Uninstall(control.FoldingManager);
					control.FoldingManager = null;
				}
				
			}
		}
		static public void GetStrategyFromName2(this TextEditor editor, IEditorContext control)
		{
			FoldingManager.Uninstall(control.FoldingManager);
			AbstractFoldingStrategy NewFoldingStrategy  = null;
			IIndentationStrategy NewIndentationStrategy = null;

			if (editor.SyntaxHighlighting == null) {}//control.FoldingStrategy = null;
			else
			{
				switch (editor.SyntaxHighlighting.Name) {
					case "XML":
						NewFoldingStrategy = new XmlFoldingStrategy();
						NewIndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						break;
					case "C#":
					case "C++":
					case "PHP":
					case "Java":
						NewFoldingStrategy = new CSharpPragmaRegionFoldingStrategy();
						NewIndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy();
						break;
					default:
						NewFoldingStrategy = new CSharpPragmaRegionFoldingStrategy();
						NewIndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						break;
				}
			}
			//control.FoldingStrategy is not assigned
			//control.FoldingStrategy,editor.TextArea.IndentationStrategy
			if (NewFoldingStrategy != null) {
				
				control.FoldingStrategy = NewFoldingStrategy;
				if (control.FoldingManager == null) control.FoldingManager = FoldingManager.Install(editor.TextArea);
				
				control.FoldingStrategy.UpdateFoldings(control.FoldingManager, editor.Document);
				
			} else {
				
				if (control.FoldingManager != null) {
					FoldingManager.Uninstall(control.FoldingManager);
					control.FoldingManager = null;
				}
				
			}
		}
		static public void UiSetAutoComplete(this TempateEditorDocument editorControl, TextEditor control)
		{
			string[] array = null;
			// get and sot completion data to array
			if (editorControl.Factory.SelectionType == TemplateType.TableTemplate) array = ac_001.Split(';');
			else if (editorControl.Factory.SelectionType == TemplateType.FieldTemplate) array = ac_002.Split(';');
			//
			Array.Sort(array);
			//
			editorControl.CompletionWindow = new CompletionWindow(editorControl.avalonTextEditor.TextArea);
			IList<ICompletionData> data = editorControl.CompletionWindow.CompletionList.CompletionData;
			foreach (string value in array)
				data.Add(
					new MyCompletionData(
						string.Format(fmt_field,value),
						"default_group"
					));
			editorControl.CompletionWindow.Show();
			editorControl.CompletionWindow.Closed += delegate { editorControl.CompletionWindow = null; };
			//
			Array.Clear(array,0,array.Length);
			array = null;
		}
		static public void UiSetupAutoCompleteLists(this TempateEditorDocument editorControl)
		{
			editorControl.UiSetAutoComplete(editorControl.avalonTextEditor);
		}

	}
}
