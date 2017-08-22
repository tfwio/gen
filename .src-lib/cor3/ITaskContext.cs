/*
 * User: xo
 * Date: 6/17/2017
 * Time: 11:33 PM
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NLog = System.Console;
namespace System
{
  /// <summary>
  /// this class is generally obsolete.
  /// 
  /// Its put to use in <see cref="IniReader"/> and the dictionary implementation using it.
  /// </summary>
  public class Keyed
  {
    public string Name { get; set; }
    public string Value { get; set; }
    public Keyed() { }
    public Keyed(string pItemKey, string pItemValue) { Name=pItemKey; Value=pItemValue; }
  }
  class ITaskContext
  {
    public IVisualModel UserFace { get; set; }
    public IList<ITask> Commands { get; set; }
    
    // we would commonly get the first input file with
    public FileInfo Input  { get; set; }
    public FileInfo Output { get; set; }

    public void Execute()
    {
      UserFace.LockUI();
      foreach (var task in Commands)
      {
      }
      UserFace.UnlockUI();
    }
    
    public Action Cancel  { get; set; }

  }
  
  public interface IControlContainer
  {
    bool KeyInvoke(string key);
    // via control or form
    Control[] Controls { get; }
    string this[string Key] { get; }
  }
  
  interface IVisualModel : IControlContainer // Subset of Control
  {
    /// Called before first TaskContext.Execute
    void LockUI();
    
    /// Called in response to all tasks being complete.
    void UnlockUI();
    
    /// after LockUI before Execute(Tasks)
    void BeginUndeterminalProcess(int taskIndex);
    /// after LockUI before Execute(Tasks)
    void BeginDeterminalProgress(int taskIndex);
    
    // before complete
    void EndProgress();
    /// Long messages would go to a console output-like pane or control
    void LogProgressLongMessage(string progress);
    /// Short messages may be shown in window-title, window-status-bar or perhaps a label or something.
    void LogProgressShortMessage(string progress);
    
    /// Shows a number in a short message
    void LogProgressPercent(double progress);
    
    /// LogProgress_StringFormat
    string LogProgress_StringFormat { get; set; }
    
    int NumberOfTasks { get; set; }
    
    event EventHandler IndicateProgress;
  }
  
  static class CommandExtent
  {
    // these strings should be accessible to the main user-interface control (or window)
    // via the this[string Key] accessor.
    internal const string
      PictureStartKey="picture-start"
      , PictureLengthKey="picture-length"
      , InputFileKey = "input"
      , OutputFileKey = "output"
      ;
    static public ITask CoverJpeg = new ITask(){
      CommandTemplate=@"-hide_banner -y -i ""$input$"" -ss $picture-start$ -t $picture-length$ ""$output$""",
      CanShowProgress=false, IsManaged=false, OutputFileExtension=".jpg",
      Arguments=new Dictionary<string, string>{ {"input", null}, {"output", null}, {"start", null}, {"length", null}, },
      SetArguments = (t,v) => {
        t.Arguments[PictureStartKey]  = v[PictureStartKey];
        t.Arguments[PictureLengthKey] = v[PictureLengthKey];
      }
    };
  }
  
  class ITask
  {
    public ITask(){
    }
    //public ITask(FileInfo input) { SetInOutFiles(input,(FileInfo)null); }
    
    // to be defined by instance
    internal Dictionary<string,string> Arguments  { get; set; }
    
    // manditory!
    internal Action<ITask,IVisualModel> SetArguments { get; set; }
    
    /// Triggered when the task is complete.
    public event EventHandler TaskIsComplete;
    public void OnTaskIsComplete(){ var hpp = TaskIsComplete; if (hpp!=null) hpp(this,EventArgs.Empty); }
    
    public ITaskContext Context { get; set; }
    
    //public Type TypeInstance { get { return typeof(ITask); } } // might be useful
    
    public string CommandTemplate     { get; set; }
    public string CommandText         { get; set; }
    
    public void InitializeCommand()
    {
      Arguments[CommandExtent.InputFileKey]  = Context.Input.FullName;
      Arguments[CommandExtent.OutputFileKey] = Context.Output.FullName;
      SetArguments(this,Context.UserFace);
      CommandText = CommandTemplate;
      
      foreach (var k in Arguments) CommandText = CommandText.Replace(k.Key.Wrap("$"),k.Value);
    }
    
    public string OutputFileExtension { get; set; }
    
    public IList<string> CleanupArtifactsPre { get; set; }  = new List<string>();
    public IList<string> CleanupArtifactsPost { get; set; } = new List<string>();

    /// for example, we might check to see if a particular file is locked.
    public Func<ITaskContext, bool>[] Check { get; set; }

    public Action<ITask>[] Execute { get; set; }
    public Action<ITask>[] PreClean { get; set; }
    public Action<ITask>[] PostClean { get; set; }

    /// if <see cref="CanShowProgress"/> is set to true, we trigger this little event here.
    public event EventHandler<SimpleProgressEventArgs> HasProgress;
    public void OnProgress(){ var hpp = HasProgress; if (hpp!=null) hpp(this,new SimpleProgressEventArgs()); }

    public event EventHandler <SimpleProgressEventArgs>TaskHasError;

    /// If set to false, then this is undeterminable otherwise its determinable (sends percent progress complete or not).
    public bool CanShowProgress { get; set; } = false;

    /// the task will either be a <see cref="IsProcess"/>, or <see cref="IsManaged"/>
    public bool IsProcess { get; set; } = false;

    /// the task will either be a <see cref="IsProcess"/>, or <see cref="IsManaged"/>
    public bool IsManaged { get {return !IsProcess; } set { IsProcess = !value; } }
  }
}




