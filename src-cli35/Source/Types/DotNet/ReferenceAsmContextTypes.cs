/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */

#region Using
using System;
#endregion

namespace Generator.Elements.Types
{
	[Flags] public enum ReferenceAsmContextTypes
	{
		//GACFromRegistry,
		Reflection = 1,
		Runtime = 2,
		Current = 4,
		Isolated = 8,
		
		CurrentRuntime = Current|Runtime,
		CurrentReflection = Current|Reflection,
		IsolatedRuntime = Isolated|Runtime,
		IsolatedReflection = Isolated|Reflection,
		
		LoadedDll = 16,
		LoadedExe = 32,
		LoadedModule = 64,
		LoadedAssemblyName = 128,
		
	}
}
