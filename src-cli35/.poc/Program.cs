/* oio * 8/2/2014 * Time: 2:03 PM */
using System;

namespace GeneratorApp
{
  /// <summary>Class with program entry point.</summary>
  internal sealed class Program
  {
    void TestParse()
    {
      System.TypeCode tc1 = TypeCode.Byte;
      System.TypeCode tc2;
      tc1.ToString().TryParse<System.TypeCode>(out tc2);
      
      Logger.LogG("Testing TryParse",$"{tc1} and {tc2}\n");
      
      var a = tc1.HasFlag(TypeCode.Int32);
      Logger.LogG("Testing HasFlag TypeCode.Byte = TypeCode.Int32", $"HasFlag = {a}\n");
      a = tc1.HasFlag(TypeCode.Byte);
      Logger.LogG("Testing HasFlag TypeCode.Byte = TypeCode.Byte", $"HasFlag = {a}\n");
//      var r = tc1.
//      Logger.LogG("Testing TryParse(Enum.Name, out...)",$"");
      
      
      Console.Write("Press a Key to continue\n");
      
      
      
      Console.ReadKey();
      return;
    }
    /// <summary>Program entry point.</summary>
    [STAThread]
    private static void Main(string[] args)
    {
      GeneratorApplication app;
      if (args.Length==0)
      {
        Console.WriteLine("Nothing to frigging do?");
        app = GeneratorApplication.Create("--help");
        Console.ReadKey();
        return;
      }
      app = new GeneratorApplication(args);
    }
  }
}
