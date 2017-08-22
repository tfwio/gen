/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Generator.Core.Entities.Types;
using System.Cor3.Reflection;
using System.Runtime.Hosting;
using System.Windows.Forms;
using System.Xml.Serialization;
#endregion

namespace Generator.Core.Entities
{
	/// <summary>
	/// <para>• (ReadOnly) Name</para>
	/// <para>• AssemblyName — indirectly contains the Name property.</para>
	/// <para>• ApplicationBase</para>
	/// <para>• AssemblyFileLocation</para>
	/// </summary>
	public class ReferenceAssemblyElement
	{
		string name = string.Empty,appBase = string.Empty,assemblyFileName = string.Empty;
		
		const string notdefined= "(assembly not defined)";
		#region readonly
		[XmlIgnore] public string Name { get { return name==null?notdefined:name==string.Empty?notdefined:name; } set { name=value; } }

		public bool HasAsmName { get { return !((name==null)||(name==string.Empty)); } }
		public bool AssemblyFileExists { get { return assemblyFileName==null ? false : System.IO.File.Exists(assemblyFileName); } }
		#endregion
		#region Properties
		[XmlAttribute] public string ApplicationBase { get { return appBase; } set { appBase = value; } }
		[XmlAttribute("AssemblyFileLocation")] public string AssemblyFileLocation { get { return assemblyFileName; } set { assemblyFileName = value; Autoload(); } }
		#endregion

		Assembly assembly = null;
		[XmlIgnore] public Assembly Assembly { get { return assembly; } protected set { assembly = value; } }
		
		public bool AssemblyIsLoaded { get { return !(assembly==null); } }
		
		ReferenceAsmContextTypes context = ReferenceAsmContextTypes.Reflection;
		[DefaultValueAttribute(ReferenceAsmContextTypes.Reflection)]
		[XmlIgnore] public ReferenceAsmContextTypes Context { get { return context; } set { context = value; } }
		
		protected void Autoload()
		{
			FileInfo fi = new System.IO.FileInfo(this.AssemblyFileLocation);
			AppDomainSetup setup = ReferenceAppDomain.AppDomainSetupAlt;
			ApplicationBase = fi.Directory.FullName;//System.AppDomain.CurrentDomain.BaseDirectory;
			setup.ShadowCopyFiles = "true";
			setup.ApplicationBase = fi.Directory.FullName;
			if ((setup.PrivateBinPath!=null) && !setup.PrivateBinPath.Contains(fi.Directory.FullName))
				setup.PrivateBinPath = string.Concat(setup.PrivateBinPath==null?"":setup.PrivateBinPath,fi.Directory.FullName,";");;
//			AppDomain separate = ReferenceAppDomain.CreateAppDomain(
//				ReferenceAppDomain.DefaultName,setup);
//			foreach (System.Reflection.Assembly asmb in separate.ReflectionOnlyGetAssemblies())
//			{
//				Global.statG("asmb {0}",setup.PrivateBinPath);
//				Global.statG("asmb {0}",asmb.GetName());
//				Global.statY("{0}",asmb.Location);
//			}
            try {
                Assembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(fi.FullName);
            }
            catch {
                
            }
//			separate.ActivationContext.();
//			if (System.IO.File.Exists(AssemblyFileLocation)&& !(AssemblyIsLoaded))
//				assembly =
//					separate.Load(fi.Name);
			// 			appBase = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			// to tell that it's loaded or not – for runtime (not xml serialization)
			if (fi.Extension==".dll") Context |= ReferenceAsmContextTypes.LoadedDll;
			else if (fi.Extension==".exe") Context |= ReferenceAsmContextTypes.LoadedExe;
			Name = Assembly.FullName.Split(',')[0];
		}
		
		public ReferenceAssemblyElement() // for xml serialization
		{
		}
		
		public ReferenceAssemblyElement(TreeNode node)
		{
			if (node.Tag is ReferenceAssemblyElement)
			{
				ReferenceAssemblyElement nodeTag  = node.Tag as ReferenceAssemblyElement;
				if (nodeTag.ApplicationBase!=null && nodeTag.ApplicationBase != string.Empty) ApplicationBase = nodeTag.ApplicationBase;
				if (nodeTag.AssemblyFileExists) AssemblyFileLocation = nodeTag.AssemblyFileLocation;
				else return;
				if (!AssemblyIsLoaded) { Autoload(); }
				
			}
			else throw new ArgumentException();
		}
		public ReferenceAssemblyElement(FileInfo fi) // for xml serialization
		{
			Context = ReferenceAsmContextTypes.CurrentReflection;
			Assembly = Assembly.ReflectionOnlyLoadFrom(AssemblyFileLocation = fi.FullName);
			Autoload();
		}
		
		public void ToTree(TreeNode node)
		{
			node.Nodes.Add(ToNode());
		}
		public TreeNode ToNode()
		{
			TreeNode tn;
			if (this.assembly==null) 
			{
//				Global.statR("ToNode: the assembly wasn't loaded");
			}
			if (Name==null) tn = new TreeNode("… ( asm ) …");
			else tn = new TreeNode(Name);
			if (AssemblyIsLoaded) tn.ToolTipText = Assembly.FullName;
			tn.SelectedImageKey = tn.ImageKey = "asm";
			tn.Tag = this;
			return tn;
		}
	}

}
