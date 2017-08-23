<!-- title:generator
subtitle: csharp generator library
date: 20170822
author: some dude
 -->

**Generator** is a template driven code generation program written in csharp.

There are generally two versions of this here

1. [.src-lib](.src-lib) — [`.src-lib/Gen.Tool.csproj`](.src-lib/Gen.Tool.csproj) and  [`.src-lib/Gen.Lib.csproj`](.src-lib/Gen.Lib.csproj) are identical except that the library version doesn't have the workings of the command-line tool version.
2. [.src-tool](.src-tool) — `[.src-tool/Gen.App.csproj](.src-tool/Gen.App.csproj)` is a shallow WIP wrapper of the general functionality expected.

Since some of the source licenses conflict a bit, we don't release binaries of the ui-app until thats resolved.


**TOOL REFERENCE ASSEMBLIES**

Newtonsoft.JSON, System.Data.SQLite

**APP REFERENCE ASSEMBLIES**

data: System.Data.SQLite.Core, Newtonsoft.Json  
ui: AvalonDock, AvalonDock.Themes,  FirstFloor.ModernUI  
sharp develop: ICSharpCode.AvalonEdit v5.0.1.0,  ICSharpCode.SharpDevelop, ICSharpCode.SharpDevelop.Widgets v4.3.3.9664

