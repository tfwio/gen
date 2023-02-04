#region Using directives
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
#endregion

#if isgentool
[assembly: AssemblyTitle("Generator")]
[assembly: AssemblyProduct("gen")]
#elif LIBGEN
[assembly: InternalsVisibleTo(@"gen, PublicKey=0024000004800000940000000602000000240000525341310004000001000100371770f6f7a9d2c986847d5a45617f5812baf14ed86f161b8f41d09871b330e8e275eea3bce14604821d2dab3e081e1430c028c7663be4093db14eebaf9fdcbd31bb467454062ea851b53a2e2ea9c242ccbbae11c87904123e8eb05364022994cd76e8e370eb4b4605dd039d5fe9e841bab8f510ad754c1aab2c6ffe0b438cbc")]
#else
[assembly: AssemblyTitle("Gen")]
[assembly: AssemblyProduct("Gen")]
[assembly: InternalsVisibleTo(@"GeneratorUI, PublicKey=0024000004800000940000000602000000240000525341310004000001000100371770f6f7a9d2c986847d5a45617f5812baf14ed86f161b8f41d09871b330e8e275eea3bce14604821d2dab3e081e1430c028c7663be4093db14eebaf9fdcbd31bb467454062ea851b53a2e2ea9c242ccbbae11c87904123e8eb05364022994cd76e8e370eb4b4605dd039d5fe9e841bab8f510ad754c1aab2c6ffe0b438cbc")]
#endif

[assembly: AssemblyCopyright("Copyright 2012-2018 github.com/tfwio")]
[assembly: ComVisible(false)]
[assembly: AssemblyDescription(@"Source code generation by way of a XML table definition file (schema) and a template-configuration file (templates).")]
[assembly: AssemblyVersion("1.1.0")]

internal static class res
{
	internal const string elmTpl = "ElementTemplate";
	internal const string itmTpl = "ItemsTemplate";
}

//  mer-ka-ba: living light vehicle or being

