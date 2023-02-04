/* oio * 8/2/2014 * Time: 2:03 PM */
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CommandLine;
using CommandLine.Text;
using Generator.Elements;
using GeneratorApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace GeneratorApp
{
  static class Actions {
    // needle for database = ":"
    static void check_database(this string haystack, string needle, string title, string message, Func<bool> condition, ConsoleColor neutral= ConsoleColor.Yellow)
    {
      Logger.Log(condition() ? neutral : ConsoleColor.Red, title, message);
    }
    static public void Generate(
      string file_database,
      string file_template,
      string database,
      string template,
      string output
      )
    {
      var conf = new GeneratorConfig() { datafile= file_database, templatefile=file_template, };

    }

    // this is a continuation, part 2.
    static public void Generate(
      GeneratorReader reader,
      string dbname,
      string tablename,
      string template,
      string output)
    {
      if (false) Console.WriteLine("Created Reader");

      Logger.Error(
        ConsoleColor.Green,
        "lodaded: datafile", "file={0}, absolute={1}, conf-dir-name={2}",
        reader.Model.Configuration.datafile,
        System.IO.Path.IsPathRooted(reader.Model.Configuration.datafile),
        System.IO.Path.GetDirectoryName(reader.Model.FileName)
      );

      if (System.IO.File.Exists(reader.Model.FileName))
      {
        var origin = System.IO.Path.GetDirectoryName(reader.Model.FileName);

        if (!System.IO.Path.IsPathRooted(reader.Model.Configuration.datafile)){
          reader.Model.Configuration.datafile = System.IO.Path.GetFullPath(System.IO.Path.Combine(origin,reader.Model.Configuration.datafile));
          Logger.Error(ConsoleColor.Magenta,"dat", reader.Model.Configuration.datafile);
        }
        if (!System.IO.Path.IsPathRooted(reader.Model.Configuration.templatefile)){
          reader.Model.Configuration.templatefile = System.IO.Path.GetFullPath(System.IO.Path.Combine(origin,reader.Model.Configuration.templatefile));
          Logger.Error(ConsoleColor.Magenta,"tpl", reader.Model.Configuration.templatefile);
        }
      }
      
      reader.Initialize();
      
      var generated = reader.Generate(
        reader.Model.Databases[dbname][tablename],
        reader.Model.Templates[template]);
      if (output == null){
        Console.Out.Write(generated);
      }
      else {
        System.IO.File.WriteAllText(output, generated);
      }
    }
    /// <summary>
    /// in the future  it would probably be better to use tokens
    /// or something to split our input a bit more wisely.
    /// 
    /// E.G.
    /// 
    /// "table=tablename:database=dbname:template=tplName"
    /// </summary>
    /// <param name="config"></param>
    /// <param name="template"></param>
    /// <param name="database"></param>
    /// <param name="output"></param>
    static public void Generate(
        string config,
        string database,
        string template,
        string output)
    {
      string dbname=null, tablename=null;

      string file_db=null, file_tpl=null;
      if (config.Contains(":"))
      {
        var values = config.Split(':');
        file_db = values[0].Trim();
        file_tpl = values[1].Trim();
        config = null;
      }

      if (database.Contains(":")){
        var values = database.Split(':');
        dbname = values[0].Trim();
        tablename = values[1].Trim();
        database = null;
      }


      if (false)
      {
        database.check_database(database, "check", "complex database", () => database.Contains(":"));
        Console.WriteLine(); // ---
        Logger.LogB("check-config", config??"[Null]");
        Console.WriteLine(); // ---
        Logger.LogB("check-database", database);
        Logger.LogB("check-template", template);
        Console.WriteLine(); // ---
        Logger.LogB("check-dbname", dbname);
        Logger.LogB("check-tablename", tablename);
        Console.WriteLine();
        Console.WriteLine("Creating Reader");
      }

      var config_exists = System.IO.File.Exists(config);

      // Logger.Log(
      //   config_exists ? ConsoleColor.Green : ConsoleColor.Red,
      //   "N",
      //   "Checking for existence of CONF file"
      // );

      // 1. see if we have xdata/xtpl
      // 2. see if we have a conf
      GeneratorReader reader = ! config_exists
        ? new GeneratorReader() { Model= new GeneratorModel(file_db, file_tpl) }
        : new GeneratorReader() { Model= config != null ? new GeneratorModel(config) : new GeneratorModel(database, template) };
      Generate(reader, dbname, tablename, template, output);

    }
  }
  //[JsonArray(AllowNullItems=true)]
  public class GeneratorJsonTask {
    // [JsonProperty]
    public string database { get; set; }
    public string template { get; set; }
    [JsonProperty("replace-file")] public string replaceFile { get; set; }
    [JsonProperty("replace-token")] public string replaceToken { get; set; }
    public string output { get; set; }
  }
  public class JsonConfig {
    public string schema    { get; set; }
    public string databases { get; set; }
    public string templates { get; set; }
    public List<GeneratorJsonTask> gen { get; set; }
  }
  class Options
  {
    [Option("gconf", HelpText="path to generator templates file (*.generator-config)")]
    public string GeneratorConfigFile { get; set; }
    
    [Option('t', "tpl", HelpText="path to generator templates file (*.xtpl)")]
    public string TemplateFile { get; set; }
    
    [Option('s', "scheme", HelpText="path to generator schema file (*.xdata)")]
    public string Schema { get; set; }
    
    [Option('T', "tpl-id", HelpText="string name of the template")]
    public string TemplateName { get; set; }
    
    [Option('D', "db", HelpText="string name of the DataSet specifying both the database and table names separated by colon where multiple table-names can be separated by comma.")]
    public string DataSetName { get; set; }

    [Option('i', "inc", HelpText = "path to input file used as a search-replace source.")]
    public string IncludeFile { get; set; }

    [Option("tag", HelpText = "tag used as search-replace needle in the transclude-file.")]
    public string IncludeTag { get; set; }

    [Option('o', "out", HelpText="(explicit) path to generated output file-name.")]
    public string ExplicitOutputFile { get; set; }

    [Value(0, MetaName = "Output File", HelpText = "(implicit) path to generated output file.")]
    public string ImplicitOutputFile { get; set; }

  }
  
  internal sealed class Program
  {
    static void CheckVariable(object option, string message, object value){
      if (option==null) Logger.Error(ConsoleColor.Red, "N", $"{message} = {value??"Null"}");
      else Logger.Error(ConsoleColor.Green, "Y", $"{message} = {value??"Null"}");
    }
    static void RunOpts(Options options)
    {
      if (true)
      {
        CheckVariable(options.GeneratorConfigFile, "GeneratorConfigFile", options.GeneratorConfigFile);
        CheckVariable(options.TemplateFile, "TemplateFile", options.TemplateFile);
        CheckVariable(options.Schema, "Schema", options.Schema);
        CheckVariable(options.ImplicitOutputFile, "ImplicitOutputFile", options.ImplicitOutputFile);
        CheckVariable(options.ExplicitOutputFile, "ExplicitOutputFile", options.ExplicitOutputFile);
        CheckVariable(options.IncludeFile, "IncludeFile", options.IncludeFile);
        CheckVariable(options.IncludeTag, "IncludeTag", options.IncludeTag);
      }
    }
    
    static void RunError(IEnumerable<Error> errors)
    {
      foreach (var e in errors)
      {
        Console.WriteLine($"- error: {e.Tag} ... {e.ToString()}.");
      }
    }
    /// <summary>
    /// first checks to see if we have a .gen file.
    /// if so, then processes arguments from the given json file.
    /// <br/><br/>
    /// basically, this was my first attempt at using CommandLineParser.
    /// <br/><br/>
    /// this is the primary feature.
    /// <br/><br/>
    /// we take a `.gen` file in the directory gen.exe was called
    /// and process it as JSON.
    /// </summary>
    static void DoDefault(){
      
      // we should have a ignore-color setting perhaps?
      Logger.Error(ConsoleColor.Green, "Executing", ".gen");

      var fileContent = System.IO.File.ReadAllText(".gen");
      // exception?
      
      var json_conf = JsonConvert.DeserializeObject<JsonConfig>(fileContent);
      // exception?

      Logger.Log(ConsoleColor.Green, "schema", json_conf.schema??"[Null]");
      // schema-file?
      if (!(json_conf.schema != null && System.IO.File.Exists(json_conf.schema)))
      {
        Logger.Error(ConsoleColor.Green, "gen-config", System.IO.Path.GetFullPath(json_conf.schema));
      }
      else
      {
        Logger.Error(ConsoleColor.Red, "SCHEMA", "no schema!");
      }

      // adequate inputs?
      var ContentErrors = new List<Tuple<int, string, string>>();
      foreach (var node in json_conf.gen){
        if (node.output==null){
          var fmt1 = string.Format("Expected `\"output\" : \"somefile.ext\"` to template=\"{0}\" db=\"{1}\" is missing.", node.template, node.database);
          ContentErrors.Add(new Tuple<int, string, string>(100, "Error", fmt1));
        }
      }
      foreach (var error in ContentErrors){
        Logger.Error(ConsoleColor.Red, error.Item2, error.Item3);
      }
      
      foreach (var error in ContentErrors)
      {
        switch(error.Item1){
            case 100: continue;
            default: return;
        }
      }
      
      foreach (var node in json_conf.gen)
      {
        try
        {
          Logger.Error(
            ConsoleColor.Magenta,
            "db: {0}, tpl: {1}, o: {2}",
            node.database??"none",
            node.template??"None",
            node.output??"None"
           );
          Actions.Generate(json_conf.schema, node.database, node.template, node.output);
        }
        catch (Exception error)
        {
          Logger.Error(ConsoleColor.Red, "ERROR", error.ToString());
        }
      }
    }
    /// <summary>Program entry point.</summary>
    [STAThread]
    private static void Main(string[] args)
    {

      // check for a local ".gen" file (no title, just `.gen`)
      if (args.Length==0 && System.IO.File.Exists(".gen")){
        DoDefault();
        return;
      }

      // run help if there are no arguments, and there is no .gen file
      if (args.Length == 0) {
        Parser.Default.ParseArguments<Options>(new string[]{"--help"});
        return;
      }

      //var settings = new ParserSettings();
      var results = Parser.Default.ParseArguments<Options>(args)
        .WithParsed<Options>(RunOpts)
        .WithNotParsed<Options>(RunError)
        ;
      bool
        has_conf = results.Value.GeneratorConfigFile==null,
        has_schema = results.Value.Schema!=null,
        has_template = results.Value.TemplateFile!=null,
        has_config_files = has_schema && has_template;


      if (!(has_conf || has_config_files))
      {
        Logger.LogR("NO GOOD", "!"); // Null
        Console.WriteLine($"error count: {results.Errors.Count()}");
        Console.WriteLine(); // ---
        Logger.LogB("GeneratorConfigFile", results.Value.GeneratorConfigFile ?? "Null"); // Null
        Console.WriteLine(); // ---
        Logger.LogB("Schema", results.Value.Schema??"Null"); // ./Shematics/my.xdata
        Logger.LogB("TemplateFile", results.Value.TemplateFile??"Null"); // ./Schematics/mt.xtpl
        Console.WriteLine(); // ---
        Logger.LogB("DataSetName", results.Value.DataSetName??"Null"); // Calibre:books
        Logger.LogB("TemplateName", results.Value.TemplateName ?? "Null"); // js.calibre-models
        Console.WriteLine(); // ---
        Logger.LogB("ExplicitOutputFile", results.Value.ExplicitOutputFile??"Null"); // fn
        Logger.LogB("ImplicitOutputFile", results.Value.ImplicitOutputFile??"Null"); // fn
        Console.WriteLine(); // ---
      }


      Actions.Generate(
        results.Value.GeneratorConfigFile??$"{results.Value.Schema}:{results.Value.TemplateFile}",
        results.Value.DataSetName,
        results.Value.TemplateName,
        results.Value.ExplicitOutputFile??results.Value.ImplicitOutputFile);
      // .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
      // .WithNotParsed<Options>((errs) => HandleParseError(errs));
    }
  }
}
