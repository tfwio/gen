/*
 * oIo ? 12/6/2010 ? 12:59 PM
 */
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.CSharp;

namespace Cor3.Data.CodeDom
{
	
	class CompilationUtil
	{

		#region const
		const string t_lib_flag = "/t:library";
		const string block_usage = @"
	using System;
	using System.Xml;
	using System.Data;
	using System.Windows.Forms;
	using System.Drawing;";
		const string block = @"
	// 
	//
	$(UsageBlock)
	
	namespace $(NamespaceName)
	{
	public class $(ClassName)
	{
		public object $(FunctionName)()
		{
	$(CodeBlock)
		}
	}
	}
	";
		#endregion

		static public string StandardBlock { get { return block.Replace("$(UsageBlock)",block_usage); } }

		/// <summary>
		/// <para>? kim.david.hauser</para>
		/// <para>http://www.codeproject.com/KB/cs/evalcscode.aspx</para>
		/// </summary>
		static public Assembly GenerateAssembly(
			string sCSCode,
			string nsName,
			string className,
			string funName) {
			
			string source = StandardBlock
				.Replace("$(NamespaceName)",nsName)
				.Replace("$(ClassName)",className)
				.Replace("$(FunctionName)",funName)
				.Replace("$(CodeBlock)",sCSCode);

			CSharpCodeProvider c = new CSharpCodeProvider();
			#pragma warning disable 618
			ICodeCompiler icc = c.CreateCompiler();
			#pragma warning restore 618

			CompilerParameters cp = new CompilerParameters();
			cp.CompilerOptions = t_lib_flag;
			cp.GenerateInMemory = true;

			CompilerResults cr = icc.CompileAssemblyFromSource(cp,source);
			if( cr.Errors.Count > 0 ){
				MessageBox.Show(
					"ERROR: " + cr.Errors[0].ErrorText,
					"Error evaluating cs code",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error );
				return null;
			}

			System.Reflection.Assembly a = cr.CompiledAssembly;
			return a;

		}

		// Set by a function that requires it.
		static string OutputExecutable = null;

		static CompilerParameters CompilerParams
		{
			get
			{
				CompilerParameters cp = new CompilerParameters();
				cp.GenerateExecutable = true;
				cp.OutputAssembly = OutputExecutable;//exeName;
				cp.GenerateInMemory = false;
				cp.TreatWarningsAsErrors = false;
				return cp;
			}
		}

		static public CodeDomProvider GetProvider(FileInfo sourceFile)
		{
			if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".CS")
				return CodeDomProvider.CreateProvider("CSharp");
			else if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".VB")
				return CodeDomProvider.CreateProvider("VisualBasic");
			else
				Console.WriteLine("Source file must have a .cs or .vb extension");
			return null;
		}
		
		/// <summary>
		/// See: OutputExecutable and CompilerParameters
		/// </summary>
		static public bool CompileExecutable(String sourceName)
		{
			bool compileOk = false;
			FileInfo sourceFile = new FileInfo(sourceName);
			CodeDomProvider provider = GetProvider(sourceFile);
			if (provider != null)
			{
				// sets our static CompilerParams ExecutablePath
				String OutputExecutable = String.Format(
					@"{0}\{1}.exe",
					System.Environment.CurrentDirectory,
					sourceFile.Name.Replace(".", "_"));

				CompilerParameters cp = CompilerParams;
				
				CompilerResults cr = provider
					.CompileAssemblyFromFile(cp,sourceName);

				if(cr.Errors.Count > 0)
				{
					Console.WriteLine(
						"Errors building {0} into {1}",
						sourceName, cr.PathToAssembly);
					foreach(CompilerError ce in cr.Errors)
					{
						Console.WriteLine("  {0}", ce.ToString());
						Console.WriteLine();
					}
				}
				else {
					// Display a successful compilation message.
					Console.WriteLine(
						"Source {0} built into {1} successfully.",
						sourceName, cr.PathToAssembly);
				}

				// Return the results of the compilation.
				if (cr.Errors.Count > 0) compileOk = false;
				else compileOk = true;
			}
			return compileOk;
		}
	}



}
