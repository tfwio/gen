/* oio * 8/2/2014 * Time: 2:03 PM */
using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using GeneratorApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace GeneratorApp
{
  static class Actions {
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
    static public void Generate(string config, string database, string template, string output){
      string dbname=null, tablename=null;
      if (database.Contains(":")){
        var values = database.Split(':');
        dbname = values[0].Trim();
        tablename = values[1].Trim();
      }
      
      if (config != null) {
        Logger.Error(ConsoleColor.Green, "Generator-Config", "config exists");
        
      } else {
        Logger.Error(ConsoleColor.Red, "Generator-Config", "NO config exists");
      }
      var reader = new GeneratorReader()
      {
        
        Model=config != null ?
          new GeneratorModel(config) :
          new GeneratorModel(database,template)
          
      };
      
      if (System.IO.File.Exists(config))
      {
        Logger.Error(ConsoleColor.Magenta,
                     "datafile", "file={0}, absolute={1}, conf-dir={2}",
                     reader.Model.Configuration.datafile,
                     System.IO.Path.IsPathRooted(reader.Model.Configuration.datafile),
                     System.IO.Path.GetDirectoryName(config)
                    );
        
        var origin = System.IO.Path.GetDirectoryName(config);
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
        //        Console.ReadKey();
      }
      else {
        System.IO.File.WriteAllText(output, generated);
      }
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
    
    [Option('o', "out", HelpText="(explicit) path to generated output file-name.")]
    public string ExplicitOutputFile { get; set; }
    
    [Value(0, MetaName="Output File", HelpText="(implicit) path to generated output file.")]
    public string ImplicitOutputFile { get; set; }
    
    // [Usage(ApplicationAlias = "yourapp")]
    // public static IEnumerable<Example> Examples {
    //   get {
    //     yield return new Example("Normal scenario", new Options { GeneratorConfigFile = "file.generator-config", OutputFile = "out.bin" });
    //     //        yield return new Example("Logging warnings", UnParserSettings.WithGroupSwitchesOnly() , new Options { InputFile = "file.bin", LogWarning = true });
    //     //        yield return new Example("Logging errors", new[] { UnParserSettings.WithGroupSwitchesOnly(), UnParserSettings.WithUseEqualTokenOnly() }, new Options { InputFile = "file.bin", LogError = true });
    //   }
    // }

    // [Option('r', "read", Required = true, HelpText = "Input files to be processed.")]
    // public IEnumerable<string> InputFiles { get; set; }

    // Omitting long name, defaults to name of property, ie "--verbose"
    // [Option(Default = false, HelpText = "Prints all messages to standard output.")]
    // public bool Verbose { get; set; }
    
    // [Value(0, MetaName = "offset", HelpText = "File offset.")]
    // public long? Offset { get; set; }
  }
  
  internal sealed class Program
  {
    static void CheckVariable(object option, string message){
      if (option==null) Logger.Error(ConsoleColor.Red, "N", $"no {message}");
      else Logger.Error(ConsoleColor.Green, "Y", $"have {message}");
    }
    static void RunOpts(Options options)
    {
      CheckVariable(options.GeneratorConfigFile, "generator config file");
      CheckVariable(options.TemplateFile, "templates file");
      CheckVariable(options.Schema, "schema file");
      CheckVariable(options.ImplicitOutputFile, "out file");
      CheckVariable(options.ExplicitOutputFile, "explicit out file");
    }
    
    static void RunError(IEnumerable<Error> errors)
    {
      foreach (var e in errors)
      {
        Console.WriteLine($"- error: {e.Tag} ... {e.ToString()}.");
      }
    }
    static void DoDefault(){
      
      // we should have a ignore-color setting perhaps?
      Logger.Error(ConsoleColor.Green, "Executing", ".gen");
      var fileContent = System.IO.File.ReadAllText(".gen");
      // exception?
      
      var content = JsonConvert.DeserializeObject<JsonConfig>(fileContent);
      // exception?
      
      // schema-file?
      if (!(content.schema != null && System.IO.File.Exists(content.schema)))
      {
        Logger.Error(ConsoleColor.Red, "SCHEMA", "no schema!");
      }
      else
      {
        Logger.Error(ConsoleColor.Green, "gen-config", System.IO.Path.GetFullPath(content.schema));
      }
      // adequate inputs?
      var ContentErrors = new List<Tuple<int, string, string>>();
      foreach (var node in content.gen){
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
      
      foreach (var node in content.gen)
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
          Actions.Generate(content.schema, node.database, node.template, node.output);
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
      if (args.Length==0 && System.IO.File.Exists(".gen")){
        DoDefault();
      }
      else if (args.Length == 0) {
        Parser.Default.ParseArguments<Options>(new string[]{"--help"});
        return;
      }
      else {
        var settings = new ParserSettings();
        Parser.Default.ParseArguments<Options>(args)
          .WithParsed<Options>(RunOpts)
          .WithNotParsed<Options>(RunError)
          ;
      }
      // .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
      // .WithNotParsed<Options>((errs) => HandleParseError(errs));
    }
  }
}
