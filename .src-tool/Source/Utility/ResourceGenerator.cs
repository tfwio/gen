using System;
using System.Windows.Controls;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Resources;
using System.Resources.Tools;

namespace GeneratorTool.Views
{
	//https://github.com/icsharpcode/SharpDevelop/blob/c1a7a7626de04a239494ffe8545dce2be96a89c2/src/AddIns/DisplayBindings/ResourceEditor/Project/Src/ResourceCodeGeneratorTool.cs
	static class StaticResourceGenerator
	{
		static public void SaveResource(WriterTplModel mdl, string fileName, bool dlgConfirm = false, string ns = "System")
		{
			var RG = new ResourceGenerator(){Model=mdl,FilePathInput=fileName,ShowConfirmationDialog=dlgConfirm, OutputNamespace=ns};
			RG.SaveResource();
			RG = null;
		}

	}
	//https://github.com/icsharpcode/SharpDevelop/blob/c1a7a7626de04a239494ffe8545dce2be96a89c2/src/AddIns/DisplayBindings/ResourceEditor/Project/Src/ResourceCodeGeneratorTool.cs
	class ResourceGenerator
	{
		const string ResxFilter = "Resource File (*.resx)|*.resx";
		static readonly SaveFileDialog SaveResxFileDialog = new SaveFileDialog() { Filter=ResxFilter }; 
		static readonly OpenFileDialog LoadResxFileDialog = new OpenFileDialog() { Filter=ResxFilter };
		static readonly string DefaultOutputNamespace = "System";
		
		public WriterTplModel Model { get;set; }
		public string OutputNamespace { get;set; }
		public string GeneratedCodeNamespace { get;set; }
		public string FilePathInput { get;set; }
		public string FilePathOutput { get;set; }
		public string FilePath { get;set; }
		public bool ShowConfirmationDialog {
			get { return showMessage; } set { showMessage = value; }
		} bool showMessage = false;
		public bool IsGenPublic {
			get { return genPublic; } set { genPublic = value; }
		} bool genPublic = false;
		
		public bool? CsDesigner(string content, string title="generating..."){
			var dlg = new ModernDialog(){ Content=content,Title=title };
			var rc = new RelayCommand((x) => { dlg.DialogResult = true; dlg.Close(); });
			dlg.Buttons = new[]{ dlg.CancelButton,new Button(){ Content = "okay", Command = rc, IsDefault = true}};
			return dlg.ShowDialog();
		}
		
		/// <summary>
		/// <para>FilePathInput to *.resx</para>
		/// <para>OutputFileName to *.Designer.cs</para>
		/// <para>where OutputFileName is generated from a provided FilePathInput</para>
		/// </summary>
		public void SaveResource()
		{
			
			if (string.IsNullOrEmpty(OutputNamespace))
			{
				OutputNamespace = DefaultOutputNamespace;
				//				throw new ArgumentClass("OutputNamespace can not be null");
			}
			if (string.IsNullOrEmpty(FilePathInput))
			{
				var value = SaveResxFileDialog.ShowDialog();
				if (!(value.HasValue && value.Value)) return ;
				FilePathInput = SaveResxFileDialog.FileName;
				//				OutputNamespace = DefaultOutputNamespace;
//				throw new ArgumentException("FilePathInput can not be null");
			}
			if (string.IsNullOrEmpty(FilePathInput))
			{
				//				OutputNamespace = DefaultOutputNamespace;
				//				throw new ArgumentClass("FilePathInput can not be null");
			}
			
			var f = new FileInfo(FilePathInput);
			if (!f.Exists) return;
			FilePathOutput = Path.Combine(f.Directory.FullName, string.Concat(Path.GetFileNameWithoutExtension(f.Name),".Designer.cs"));
			
			WriteResx();
			
			var result = ShowConfirmationDialog ? CsDesigner(FilePathOutput) : true;
			
			if ((result.HasValue && result.Value))
			{
				WriteCsDesigner();
			}
		}
		
		void WriteResx()
		{
			const string resxTitleString = "{0}.{1}";
			using (var resx = new ResXResourceWriter(FilePathInput))
			{
				resx.AddResource("TemplateGroups",Model.GroupNames);
				foreach (var groupName in Model.GroupNames)
				{
					Model.GetRows(groupName);
					foreach (var element in Model.rows)
					{
						string templateName = element.Title;
						if (element.Container!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Container"),element.Container);
						if (element.Foot!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Foot"),element.Foot);
						if (element.Groupfoot!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Groupfoot"),element.Groupfoot);
						if (element.Grouphead!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Grouphead"),element.Grouphead);
						if (element.Head!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Head"),element.Head);
						if (element.Note!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Note"),element.Note);
						if (element.Row!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Row"),element.Row);
						if (element.Table!=null) resx.AddResource(string.Format(resxTitleString,templateName,"Table"),element.Table);
					}
				}
			}
		}
		bool IsValidInput()
		{
			if (string.IsNullOrEmpty(FilePathInput)) throw new ArgumentException("FilePathInput can not be null");
			var f = new FileInfo(FilePathInput);
			bool valid = string.Equals(f.Extension, ".resx", StringComparison.OrdinalIgnoreCase);
			return valid;
		}
		
		IResourceReader ReaderForInput()
		{
			ResXResourceReader reader = new ResXResourceReader(FilePathInput);
			((ResXResourceReader)reader).BasePath = Path.GetDirectoryName(FilePathInput);
			return reader;
		}
		public void WriteCsDesigner()
		{
			if (string.IsNullOrEmpty(FilePathInput)) throw new ArgumentException("FilePathInput can not be null");
			if (string.IsNullOrEmpty(GeneratedCodeNamespace)) GeneratedCodeNamespace = OutputNamespace;
			var f = new FileInfo(FilePathInput);
			
			using (IResourceReader reader = IsValidInput() ? ReaderForInput() : new ResourceReader(FilePathInput))
			{
				var resources = new Hashtable();
				foreach (DictionaryEntry de in reader) resources.Add(de.Key, de.Value);
				string[] unmatchable = null;
				using (CodeDomProvider csprovider = CodeDomProvider.CreateProvider("CSharp"))
				using (TextWriter writer = new StreamWriter(FilePathOutput))
				{
					CodeCompileUnit ccu = StronglyTypedResourceBuilder.Create(
						resources,        // resourceList
						Path.GetFileNameWithoutExtension(FilePathInput), // baseName
						GeneratedCodeNamespace, // generatedCodeNamespace
						OutputNamespace, // resourcesNamespace
						csprovider, // codeProvider
						IsGenPublic,             // internal class
						out unmatchable);
					csprovider.GenerateCodeFromCompileUnit(ccu,writer,null);
				}
			}
			//			foreach (string s in unmatchable) {
			//				context.MessageView.AppendLine(String.Format(System.Globalization.CultureInfo.CurrentCulture, ResourceService.GetString("ResourceEditor.ResourceCodeGeneratorTool.CouldNotGenerateResourceProperty"), s));
			//			}
		}
	}
}


