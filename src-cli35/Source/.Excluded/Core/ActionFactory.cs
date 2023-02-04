/*
 * User: oIo
 * Date: 2/5/2011 – 10:00 PM
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using Generator.Elements.Types;
namespace Generator
{
	/// <summary>
	/// More of a working test for a set of globally available type-references.
	/// does this do anything?
	/// </summary>
	public class ActionFactory
	{
		static List<T> EnumerateViewTypes<T>(Assembly containedAssembly)
			where T:class
		{
			List<T> types = new List<T>();
			foreach (Type t in containedAssembly.GetTypes())
				if (t.BaseType == typeof(T))
					types.Add(Activator.CreateInstance(t) as T);
			return types;
		}
		static public List<object> EnumeratedActions
		{
			get
			{
				List<object> list = new List<object>();
				
				list.Add("— WebFormTypes —");
				foreach (object o in TypeCode.GetValues(typeof(WebFormTypes))) list.Add(o);
				
				list.Add("— WebElementTypes —");
				foreach (object o in TypeCode.GetValues(typeof(WebElementType))) list.Add(o);
				
				list.Add("— TypeCode —");
				foreach (object o in TypeCode.GetValues(typeof(TypeCode))) list.Add(o);
				
				list.Add("— ActionTypes —");
				foreach (object o in ActionTypes.GetValues(typeof(ActionTypes))) list.Add(o);
				
				GetTypesFromAsm(typeof(ExtJsFieldType).Assembly, ref list);
				GetTypesFromAsm(System.Reflection.Assembly.GetEntryAssembly(), ref list);
				/*
				list.Add("— ExtNet v1.6 Field —");
				foreach (object o in ExtJsTypeHelper.FieldType) list.Add(o);
				
				list.Add("— ExtNet v1.6 Record Field —");
				foreach (object o in ExtJsTypeHelper.RecordFieldType) list.Add(o);
				 */
				
				return list;
			}
		}
		static void GetTypesFromAsm(Assembly asm, ref List<object> list)
		{
			List<EnumProvider> tlist = EnumerateViewTypes<EnumProvider>(asm);
			foreach (IEnumProvider provider in tlist)
			{
				IEnumProvider p = provider;
				list.Add(string.Format("[ {0} ]",provider.Name));
				foreach (object o in provider.Types)
					list.Add(o);
				p = null;
			}
			tlist.Clear();
			tlist = null;
			GC.Collect();
		}
	}
}
