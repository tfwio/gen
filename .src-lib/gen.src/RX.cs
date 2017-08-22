/* oio * 8/2/2014 * Time: 2:03 PM
 */
using System;

namespace GeneratorApp
{
	class RX
	{
		public const string HELPSTRING = @"
	Generator Command Appliaction
	==========================================================
	
	There are two modes of operation in this application.
	
	Generator-Configuration-File Basis
	----------------------------------------------------------
	
	Here, a generator-configuraion file as its basis.  Here, you supply a path to your configuration-file and supply a name of the template you intend to use and a name for the generation process. 
	
	note: round and square braces of course are not used in the command
	
	gen -gcfg ([gen-cfg] | -it [path] -is [path]) -tbln [table-name] -tpln [template-name] -i (file) -o (file)
	
	  -gcfg: (file)   Generator Configuration File
	
	if no generator-config, then supply the following two files...
	  
	  -it: (file) input template config *.xtpl
	  -is: (file) input data-schematic config *.xdata
	  -ij: (file) input data-schematic config *.json|*.gen
                  - typically, a file named '.gen' would be created for this.
                  - additionally, we use something like nodejs with a ""gen_cfg"" sub-directory.
	
	the following pertain to information contained in configurations.
	
    -db:
	  -dbn:   (string) The name of the database your specified table is in.
    -t:
    -table:
	  -tbln: (string) The name of the database->table
	  
	  -tpl:
	  -tpln: (string) The name of the template.
	
	options
	
	  -i  (file) input file containing (see -r) input template.
	  -o  (file) output file.
	  
	  -r  (string) a tag that is replaced in the (see -i) input file.
	      This tag is used as the caret position for write output of
	      the (re-) generation.
	
	Via this mode, the input file would contain a quick data-config and
	also the Template and possibly even specify the output-file.
";
		public const string TEST_CONFIG = @"
Configuration  = {config}
OutputFile     = {output}
TemplateName   = {template}
db.table       = {db}.{table}
";
	}
}
