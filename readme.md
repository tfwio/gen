**Generator** is a template driven code generation program written in csharp.


A command-line tool which uses a xml-based template and data-scheme
to generate code ready for reading and writing to a given database (such as SQLite).

Documentation on template authoring and data-scheme XML is to come, but for now some tags are explained in [.src-lib/Schematics/Template-Tags.md](.src-lib/Schematics/Template-Tags.md)

*There is also (shoddy) UI for helping author such templates and data-schemes but the
licensing of its dependencies conflict, so no releases are provided.**


2023-02-03 **incarnation phase 3**

looks like this was a little defunct some things were left hanging.


**Console Application**: gen.exe

- created a new "gen.exe" app using `CommandLineParser` which had one primary feature:
  - if 0 params are called when loading gen.exe, attempt to load a `.gen` file (if present) as default operation: the configuration file is pointing to a generator-configuration and template and database/table to apply to generating a particular file.
- now its looking like I've gotten the gen.exe app working with all its command-line argument/parameter(s) working but ***broke the main feature***.


**WPF Prototype** App


There is also a WPF app that kind of might work as an editor but this idea will probably be scrapped for better.

Will document more on this as it comes back to life.

