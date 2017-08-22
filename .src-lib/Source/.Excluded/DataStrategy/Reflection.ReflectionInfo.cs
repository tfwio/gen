/*
 * oIo ? 1/2/2011 ? 7:30 PM
 */
#region Using
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Generator.Core.Entities;
using EventInfo = System.Reflection.EventInfo;
using MemberInfo = System.Reflection.MemberInfo;
using MethodInfo = System.Reflection.MethodInfo;
using PropertyInfo = System.Reflection.PropertyInfo;

#endregion

namespace System.Cor3.Reflection
{
	/// <summary>
	/// Description of ReflectionInfo.
	/// </summary>
	public class ReflectionInfo
	{
		
		#region File List Helpers
		//cbAssembly.DataSource = System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies();
		// lists sibling files
		static public List<FileInfo> GetFileList(FileInfo f, params string[] searchPattern)
		{
			List<FileInfo> info = new List<FileInfo>();
			foreach (string sp in searchPattern)
				info.AddRange(f.Directory.GetFiles(sp));
			return info;
		}

		// list files
		static public List<FileInfo> GetFileList(DirectoryInfo d, params string[] searchPattern)
		{
			List<FileInfo> info = new List<FileInfo>();
			foreach (string sp in searchPattern)
				info.AddRange(d.GetFiles(sp));
			return info;
		}
		#endregion

		#region TypeInfo, TreeNode
		static public void ParentTypeInfo(Type t, TreeNode node)
		{
			if (t.BaseType == null) return;
			TreeNode node0 = node.Nodes.Add("Base");
			TreeNode node1 = node0;
			Type t0 = t;
			string parentnames = t0.Name;
			List<string> names = new List<string>();
			while ((t0 = t0.BaseType)!=null)
			{
				names.Add(CleanTypeName(t0));
				node1 = node1.Nodes.Add(CleanTypeName(t0));
//				node1.ToolTipText = TypeInfoString(t0);
			}
			node0.Text = string.Join(":", names.ToArray());
		}
		static public void TypeInfoImage(Type t, TreeNode node1)
		{
			node1.SelectedImageKey = node1.ImageKey = ImageKeyNames.Class;
		}
		static public void NodeImage(Type t, TreeNode node1)
		{
			switch (t.MemberType)
			{
			case System.Reflection.MemberTypes.Method: node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Method; break;
			case System.Reflection.MemberTypes.Field: node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Field; break;
			case System.Reflection.MemberTypes.NestedType: node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Obj; break;
			case System.Reflection.MemberTypes.Property: node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Field; break;
			case System.Reflection.MemberTypes.TypeInfo: TypeInfoImage(t,node1); break;
			case System.Reflection.MemberTypes.Event: node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Delegate; break;
			case System.Reflection.MemberTypes.Custom: node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Memo; break;
			case System.Reflection.MemberTypes.Constructor: node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Ctor; break;
			}
			if (t.IsEnum) node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Enumeration;
			else if (t.IsClass) node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Class;
			else if (t.IsInterface) node1.ImageKey = node1.SelectedImageKey = ImageKeyNames.Framework;
		}
		#endregion

		static public bool HasAttribute(System.Reflection.MethodAttributes ma, System.Reflection.MethodInfo m)
		{
			int a = (int)m.Attributes, b = (int)ma;
			return (a & b) == b;
		}
		static public string HasAttribute(System.Reflection.MethodAttributes ma, System.Reflection.MethodInfo m, string input)
		{
			return (HasAttribute(ma,m)) ? input : string.Empty;
		}

		static public List<string> ParameterNames(System.Reflection.MethodInfo m)
		{
			List<string> paramnames = new List<string>();
			foreach (System.Reflection.ParameterInfo pi in m.GetParameters())
			{
				paramnames.Add(string.Format("{0} {1}",CleanTypeName(pi.ParameterType),pi.Name));
			}
			return paramnames;
		}
		
		#region Generic info
		static public List<string> GetGenericProperty(System.Reflection.MethodInfo m)
		{
			List<string> list = new List<string>();
			foreach (Type t in m.GetGenericArguments())
				list.Add(string.Format(
					"{0}",
					t.Name
				));
			return list;
		}

		static public string GenericStr(System.Reflection.MethodInfo m)
		{
			List<string> list = new List<string>();
			foreach (Type t in m.GetGenericArguments()) list.Add(string.Format("{0}",t.Name));
			return string.Format("<{0}>",string.Join(",",list.ToArray()));
		}
		static public string GenericStr(Type m)
		{
			List<string> list = new List<string>();
			foreach (Type t in m.GetGenericArguments())
				list.Add(string.Format("{0}",CleanTypeName(t)));
			return string.Format("<{0}>",string.Join(",",list.ToArray()));
		}
		static public string GenericName(Type t, bool typename)
		{
			if (t ==null) return string.Empty;
			List<string> list = new List<string>();
			if (t.IsGenericTypeDefinition)
			{
				foreach (Type i in t.GetGenericArguments())
				{
					list.Add(i.Name);
				}
			}
			return string.Concat(
				typename ? t.Name+": ": string.Empty,
				string.Join(",",list.ToArray())
			);
		}
		static public string GenericsInfoString(Type t)
		{
			if (!t.IsGenericParameter) return string.Empty;
			List<string> list = new List<string>();
			if (t.ContainsGenericParameters)
			{
				foreach (Type o in t.GetGenericParameterConstraints())
					list.Add(o.Name);
//				foreach (Type o in t.GetGenericTypeDefinition())
//					list.Add(o.Name);
				foreach (Type o in t.GetGenericArguments())
					list.Add(o.Name);
			}
			return string.Join("‹",list.ToArray());
		}
		#endregion
		static string CleanTypeName(Type t)
		{
			string tname = t.Name;
			if (t.Name.IndexOf('`')>=0)
			{
				tname = string.Concat(
					t.Name.Substring(0,t.Name.IndexOf('`')),
					GenericStr(t)
				);
			}
			return tname;
		}
		static public TreeNode TypeGetInfo(TreeView treeView1, string ns, List<TreeNode> parent, Type t)
		{
//			treeView1.Sorted = true;
			string ns1 = ns==null ? string.Empty: ns;
			string ns0 = t.Namespace == null ? string.Empty : t.Namespace;
			if (t==null) MessageBox.Show("The type was NULL!",string.Concat(ns,t.AssemblyQualifiedName));
			if (ns0.CompareTo(ns1)!=0) return null;
			string tname = CleanTypeName(t);
			TreeNode node1 = new TreeNode(tname);
			parent.Add(node1);
			ParentTypeInfo(t,node1);
			NodeImage(t,node1);
			node1.Tag = t;
			node1.ToolTipText = TypeInfoString(t);
			string[] nn = new string[]{
				"interfaces","events","methods","properties"
			};
			TreeNode	tniface		= node1.Nodes.Add(nn[0],nn[0]),
						tnevents	= node1.Nodes.Add(nn[1],nn[1]),
						methods		= node1.Nodes.Add(nn[2],nn[2]),
						properties	= node1.Nodes.Add(nn[3],nn[3]);
			
			foreach (Type m in t.GetInterfaces()) GetInterfaces(tniface,m);
			if (tniface.Nodes.Count==0) tniface.Remove();
			
			foreach (EventInfo e in t.GetEvents()) GetEvents(tnevents,e);
			if (tnevents.Nodes.Count==0) tnevents.Remove();
			
			foreach (MethodInfo m in t.GetMethods()) GetMethodNode(methods,m);
			if (methods.Nodes.Count==0) methods.Remove();
			
			foreach (PropertyInfo m in t.GetProperties()) GetProperties(properties,m);
			if (properties.Nodes.Count==0) properties.Remove();
			
			if (t.IsEnum) GetEnumTypeValues(ns0,node1,t);
			return node1;
		}

		#region Get SubNode
		static public void GetMethodNode(TreeNode methods, System.Reflection.MethodInfo m)
		{
			if (HasAttribute(System.Reflection.MethodAttributes.SpecialName,m)) return;
			List<string> paramnames = ParameterNames(m), generics = GetGenericProperty(m);
			TreeNode method = null;
			if (!(m.DeclaringType==null)) {
				if (!methods.Nodes.ContainsKey(m.DeclaringType.Name))
				{
					methods.Nodes.Add(CleanTypeName(m.DeclaringType))
						.Name = m.DeclaringType.Name;
				}
				method = methods.Nodes[m.DeclaringType.Name].Nodes.Add(
					string.Format("{1}:{0}",CleanTypeName(m.ReturnType),m.Name));
			}
			else
			{
				method = methods.Nodes.Add(string.Format("{1}",CleanTypeName(m.ReturnType),m.Name));
			}
			method.ImageKey = method.SelectedImageKey = ImageKeyNames.Function;
			method.Text = string.Format(
				"{0}{1} {2}{3}({4})",
				string.Concat(
					m.IsVirtual?"virtual ":string.Empty,
					m.IsPublic?"public ":string.Empty,
					m.IsPrivate?"private ":string.Empty,
					m.IsFamily?"(family) ":string.Empty,
					m.IsStatic?"static ":string.Empty
				),
				string.Concat(
					CleanTypeName(m.ReturnParameter.ParameterType)
				),
				m.Name,
				string.Concat(
					m.IsGenericMethod ? string.Format("<{0}>",string.Join(",",generics.ToArray())):string.Empty
				),
				string.Join(", ",paramnames.ToArray())
			);
			method.Tag = m;
		}
		static public void GetProperties(TreeNode properties, System.Reflection.PropertyInfo m)
		{
//			m.GetGener
			
			string itemname = string.Format(
				"{0} {1} {{{2}}}",
				string.Concat(
					CleanTypeName(m.PropertyType)," "
				),
				
				m.Name,
				string.Concat(m.CanRead ? "get;":"",m.CanWrite ? "set;":"")
			);
			if (m.PropertyType.IsGenericTypeDefinition)
			{
//				if (m.PropertyType.UnderlyingSystemType.
				itemname = string.Concat(itemname,GenericStr(m.PropertyType));
			}
			TreeNode property = null;
			if (!(m.DeclaringType==null)) {
				if (!properties.Nodes.ContainsKey(m.DeclaringType.Name))
				{
					properties.Nodes.Add(CleanTypeName(m.DeclaringType))
						.Name = m.DeclaringType.Name;
				}
				property = properties.Nodes[m.DeclaringType.Name].Nodes.Add(
					string.Format("{1}:{0}",CleanTypeName(m.PropertyType),m.Name));
			}
			else
			{
				property = properties.Nodes.Add(string.Format("{1}:{0}",m.PropertyType.Name,m.Name));
			}
			foreach (System.Reflection.CustomAttributeData o in System.Reflection.CustomAttributeData.GetCustomAttributes(m))
			{
				property.Nodes.Add(string.Format("{0}",o));
			}
//			TreeNode property = properties.Nodes.Add();
			property.ToolTipText = itemname;
			property.ImageKey = property.SelectedImageKey = ImageKeyNames.Field;
			property.Tag = m;
		}
		static public void GetInterfaces(TreeNode properties, Type m)
		{
			TreeNode property = null;
			property = new TreeNode(CleanTypeName(m));
			foreach (System.Reflection.CustomAttributeData o in System.Reflection.CustomAttributeData.GetCustomAttributes(m))
			{
				property.Nodes.Add(string.Format("{0}",o));
			}
			property.ToolTipText = CleanTypeName(m.UnderlyingSystemType);
			property.ImageKey = property.SelectedImageKey = ImageKeyNames.Field;
			property.Tag = m;
			if (property.Nodes.Count > 0) properties.Nodes.Add(property);
		}
		static public void GetEvents(TreeNode properties, System.Reflection.EventInfo e)
		{
			string itemname = string.Format(
				"{0} {1} [{2}]",
				CleanTypeName(e.EventHandlerType),
				e.Name,
				string.Join("|",new string[]{
				            	e.IsMulticast ? "multicast":"",
				            	e.IsSpecialName ? "special-name":""
				            })
			);
			TreeNode property = null;
			if (!(e.DeclaringType==null)) {
				if (!properties.Nodes.ContainsKey(e.DeclaringType.Name))
				{
					properties.Nodes.Add(CleanTypeName(e.DeclaringType))
						.Name = e.DeclaringType.Name;
				}
				property = properties.Nodes[e.DeclaringType.Name].Nodes.Add(
					string.Format("{1}:{0}",CleanTypeName(e.EventHandlerType),e.Name));
			}
			else
			{
				property = properties.Nodes.Add(
					string.Format(
						"{1}:{0}",CleanTypeName(e.EventHandlerType),e.Name));
			}
//			if (properties.Nodes.Count <= 0) properties.Remove();
			property.ToolTipText = itemname;
			property.ImageKey = property.SelectedImageKey = ImageKeyNames.Delegate;
			property.Tag = e;
		}
		static public void GetEnumTypeValues(string ns, TreeNode node, Type t)
		{
			TreeNode values = new TreeNode("Values");
			if (t.IsEnum) foreach (System.Reflection.FieldInfo f in t.GetFields())
			{
				values.Nodes.Add(f.Name)
					.ToolTipText = CleanTypeName(f.FieldType);
			}
			node.Nodes.Add(values);
		}
		#endregion

		static public System.Reflection.Assembly ListNamespaces(
			TreeView treeView1,
			ReferenceAssemblyElement refasm)
		{
			System.Reflection.Assembly asm = refasm.Assembly;
			List<Type> Types = new List<Type>(asm.GetExportedTypes());
			List<TreeNode> Nodes = new List<TreeNode>();
			treeView1.Nodes.Clear();
			foreach (string ns in Mirror.GetNamespaces(asm))
			{
				string names = ns==null?string.Empty:ns;
				DictionaryList<string,TreeNode> NodeSet = new DictionaryList<string, TreeNode>();
				NodeSet.CreateKey("interface");
				NodeSet.CreateKey("enum");
				NodeSet.CreateKey("other");
				NodeSet.CreateKey("ValueTypes");
				TreeNode node0 = new TreeNode(ns);
				node0.Tag = names;
				
				foreach(Type t in Types)
				{
					if (t.IsInterface) TypeGetInfo(treeView1,names, NodeSet["interface"], t);
					else if (t.IsEnum) TypeGetInfo(treeView1,names, NodeSet["enum"], t);
					else if (t.IsValueType)
					{
						TreeNode tn = TypeGetInfo(treeView1,names, NodeSet["ValueTypes"], t);
						if (tn!=null) tn.ImageKey = tn.SelectedImageKey = ImageKeyNames.Field;
					}
					else TypeGetInfo(treeView1,names,NodeSet["other"],t);
				}
				if (NodeSet["interface"].Count>0) foreach (TreeNode tn in NodeSet["interface"]) node0.Nodes.Add(tn);
				else NodeSet.Remove("interface");
				if (NodeSet["enum"].Count>0) foreach (TreeNode tn in NodeSet["enum"]) node0.Nodes.Add(tn);
				else NodeSet.Remove("enum");
				if (NodeSet["other"].Count>0) foreach (TreeNode tn in NodeSet["other"]) node0.Nodes.Add(tn);
				else NodeSet.Remove("other");
				if (NodeSet["ValueTypes"].Count>0) foreach (TreeNode tn in NodeSet["ValueTypes"]) node0.Nodes.Add(tn);
				else NodeSet.Remove("ValueTypes");

				if ((ns==null)||(ns==string.Empty)) treeView1.Nodes.Add(node0);
				else Nodes.Add(node0);
			}
			
			treeView1.Nodes.AddRange(Nodes.ToArray()); Nodes.Clear();
			return asm;
		}

		static public string TypeInfoString(Type t)
		{
			return string.Format(
				"{0} {1} {2} ({3})",
				string.Concat(
					t.IsSerializable?"serializable ":string.Empty,
					t.IsPrimitive?"primitive ":string.Empty,
					t.IsPublic?"public ":string.Empty,
					t.IsSealed?"sealed ":string.Empty,
					t.IsAbstract?"abstract ":string.Empty,
					t.IsAnsiClass?"Ansi Class ":string.Empty,
					t.IsArray?"Array ":string.Empty
				),
				string.Concat(
					t.IsEnum?"Enum ":string.Empty,
					t.IsClass?"Class ":string.Empty,
					t.IsInterface?"Interface ":string.Empty
				),
				t.MemberType,
				string.Concat(
					t.IsGenericParameter?" IsGenericParameter":string.Empty,
					t.IsGenericType?" IsGenericType":string.Empty,
					t.IsGenericTypeDefinition?" IsGenericTypeDefinition":string.Empty,
					
					t.IsByRef?" IsByRef":string.Empty,
					t.IsAutoClass?" IsAutoClass":string.Empty,
					t.IsAutoLayout?" IsAutoLayout":string.Empty,
					t.IsCOMObject?" IsCOMObject":string.Empty,
					t.IsContextful?" IsContextful":string.Empty,
					t.IsExplicitLayout?" IsExplicitLayout":string.Empty,
					
					t.IsImport?" IsImport":string.Empty,
					
					t.IsLayoutSequential?" IsLayoutSequential":string.Empty,
					t.IsMarshalByRef?" IsMarshalByRef":string.Empty,
					
					t.IsNested?" IsNested":string.Empty,
					t.IsNestedAssembly?" IsNestedAssembly":string.Empty,
					t.IsNestedFamANDAssem?" IsNestedFamANDAssem":string.Empty,
					t.IsNestedFamily?" IsNestedFamily":string.Empty,
					t.IsNestedFamORAssem?" IsNestedFamORAssem":string.Empty,
					t.IsNestedPrivate?" IsNestedPrivate":string.Empty,
					t.IsNestedPublic?" IsNestedPublic":string.Empty,
					
					t.IsPointer?" IsPointer":string.Empty,
					t.IsNotPublic?" IsNotPublic":string.Empty,
					// !
					t.IsSpecialName?" IsSpecialName":string.Empty,
					t.IsUnicodeClass?" IsUnicodeClass":string.Empty,
					// !
					t.IsValueType?" IsValueType":string.Empty,
					t.IsVisible?" IsVisible":string.Empty
				)
			);
		}
	}
	public struct MemberTypes
	{
		static readonly public int All = (int)System.Reflection.MemberTypes.All;
		static readonly public int Constructor = (int)System.Reflection.MemberTypes.Constructor;
		static readonly public int Custom = (int)System.Reflection.MemberTypes.Custom;
		static readonly public int Event = (int)System.Reflection.MemberTypes.Event;
		static readonly public int Field = (int)System.Reflection.MemberTypes.Field;
		static readonly public int Method = (int)System.Reflection.MemberTypes.Method;
		static readonly public int NestedType = (int)System.Reflection.MemberTypes.NestedType;
		static readonly public int Property = (int)System.Reflection.MemberTypes.Property;
		static readonly public int TypeInfo = (int)System.Reflection.MemberTypes.TypeInfo;
	}
}
