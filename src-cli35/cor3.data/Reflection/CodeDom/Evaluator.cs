/*
 * oIo ? 12/3/2010 ? 3:04 PM
 */
using System;
using System.CodeDom.Compiler;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Microsoft.CSharp;

namespace System
{
	/// <summary>
	/// Description of Evaluator.
	/// </summary>
	public class Evaluator
	{

		static public void Test(string domain)
		{
			AppDomainSetup ads = new AppDomainSetup();
			AppDomain ad = AppDomain.CreateDomain(domain,null,ads);
			lock (ad) // the lock probably isn't really necessary.
			{
				// perform any necessary calls ?
			}
			//			FieldInfo fi;
			//			fi.IsLiteral
			AppDomain.Unload(ad);
		}
		
		static public object ExecuteAssembly(
			Assembly asm,
			string typeName, string funName,
			params object[] parameters)
		{
			object o = asm.CreateInstance(typeName);
			Type t = o.GetType();
			MethodInfo mi = t.GetMethod(funName);
			object s = mi.Invoke(o, null);
			return s;
		}
		
		
		#region Old
		// Eval > Evaluates C# sourcelanguage
		
		/// <summary>
		/// <para>? kim.david.hauser</para>
		/// <para>http://www.codeproject.com/KB/cs/evalcscode.aspx</para>
		/// <para>This is the original ?Eval? function Renamed, or commented out.</para>
		/// </summary>
		static object OldEval(string sCSCode) {
			
			CSharpCodeProvider c = new CSharpCodeProvider();
			#pragma warning disable 618
			ICodeCompiler icc = c.CreateCompiler();
			#pragma warning restore 618
			CompilerParameters cp = new CompilerParameters();
			
			cp.ReferencedAssemblies.Add("system.dll");
			cp.ReferencedAssemblies.Add("system.xml.dll");
			cp.ReferencedAssemblies.Add("system.data.dll");
			cp.ReferencedAssemblies.Add("system.windows.forms.dll");
			cp.ReferencedAssemblies.Add("system.drawing.dll");
			
			cp.CompilerOptions = "/t:library";
			cp.GenerateInMemory = true;
			
			StringBuilder sb = new StringBuilder("");
			sb.Append("using System;\n" );
			sb.Append("using System.Xml;\n");
			sb.Append("using System.Data;\n");
			sb.Append("using System.Data.SqlClient;\n");
			sb.Append("using System.Windows.Forms;\n");
			sb.Append("using System.Drawing;\n");
			
			sb.Append("namespace CSCodeEvaler{ \n");
			sb.Append("public class CSCodeEvaler{ \n");
			sb.Append("public object EvalCode(){\n");
			sb.Append("return "+sCSCode+"; \n");
			sb.Append("} \n");
			sb.Append("} \n");
			sb.Append("}\n");
			
			CompilerResults cr = icc.CompileAssemblyFromSource(cp, sb.ToString());
			if( cr.Errors.Count > 0 ){
				MessageBox.Show("ERROR: " + cr.Errors[0].ErrorText,
				                "Error evaluating cs code", MessageBoxButtons.OK,
				                MessageBoxIcon.Error );
				return null;
			}
			
			System.Reflection.Assembly a = cr.CompiledAssembly;
			object o = a.CreateInstance("CSCodeEvaler.CSCodeEvaler");
			
			Type t = o.GetType();
			MethodInfo mi = t.GetMethod("EvalCode");
			
			object s = mi.Invoke(o, null);
			return s;
			
		}
		#endregion
		
	}
}
