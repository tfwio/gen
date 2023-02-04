/*
 * User: xo
 * Date: 8/13/2017
 * Time: 1:20 PM
 */
using System;
using System.Threading;
using System.Windows.Forms;

//using NLog=System.Diagnostics.Debug;//.net 3.5 isn't supporting this
using NLog = System.Console;

// this may be just a bit of overkill for handling an event from a thread,
// but then again overkill is good.

namespace System
{
  /// <summary>
  /// Start — create a new thread and start it.
  /// 
  /// 
  /// Stop — destroy the thread (safe while running)
  ///
  /// 
  /// Run — used as ThreadStart (method) called by the thread.
  /// 
  /// 
  /// Cancel — The troll on the thread-loop/bridge says: "you may pass" (and the thread runs its course and aborts automatically)
  /// 
  /// 
  /// Done — [for you to override] indirectly calls <see cref="Stop"/>.
  /// You absolutely MUST call the base method BEFORE any other action in order for this to work properly though.
  /// 
  /// This method is called at the end of the thread-loop.
  /// 
  /// 
  /// # Safety on destruction of the Context (Ctx)
  /// 
  /// 
  /// Its important that you cancel the thread before the <see cref="Syu.Ctx"/> is destroyed...
  /// assuming that you utilize the included thread-safe form update mechanism...
  /// 
  /// From the encapsulating form's <see cref="Form.Dispose(bool)"/> method, you should
  /// likely call <see cref="Stop()"/>.
  /// 
  /// If Not, this is a trap for placing in your <see cref="Go"/> override if you don't handle the forms <see cref="Form.Dispose(bool)"/> override.
  /// 
  /// <code>
  /// Exception e1; // class-level
  /// 
  /// // where a1 is
  /// try { a1.Go(); }
  /// catch (ObjectDisposedException e2) { e1 = e2; }
  /// catch { throw; }
  /// finally {
  ///   if (e1 != null){
  ///     System.Diagnostics.Debug.WriteLine("Handle in the main form to prevent {0}", e1.ToString());
  ///     e1 = null;
  ///   }
  /// }
  /// </code>
  /// </summary>
  public class Clew //: Moc, It //where t:class
  {
    ~Clew() { Stop(); }


    public event EventHandler ThreadStopped;
    protected virtual void OnThreadStopped()
    {
      NLog.WriteLine("<{0}> {1}", GetType().Name, "Thread is stopped");
      var handler = ThreadStopped;
      if (handler != null)
        handler(this, EventArgs.Empty);
    }

    /// <summary></summary>
    public void Prioritize(ThreadPriority priority) { mP = priority; if (mT != null) mT.Priority = mP; }

    /// <summary></summary>
    public void SetInterval(int ms) { timetorun = ms; }

    /// <summary></summary>
    public void Start() { if (mT != null) { mT.Abort(); mT.Join(); mT = null; } mT = new Thread(Run) { Priority = mP }; mT.Start(); }

    //void Loop() { lock (mL) { while (isTimerActive) { Thread.Sleep(timetorun); Go(); } Done(); } }

    /// <summary>
    /// If you want a loop, we have the isActive var to work with you can use the following override snippit.
    /// 
    /// 
    /// Override to simplify.
    /// 
    /// See: <see cref="SetInterval()"/> &amp; <see cref="Cancel()"/>
    /// </summary>
    /// <code>
    /// /// current-code looks like...
    /// if (useTimer) lock (mL) { while (isTimerActive) { Thread.Sleep(timetorun); Go(); } Done(); }
    /// else lock (mL) { Go(); Done(); }
    /// 
    /// /// &lt;inheritdoc/>
    /// protected override void Run() { lock (mL) { while (isTimerActive) { Thread.Sleep(timetorun); Go(); } Done(); } }
    /// </code>
    virtual protected void Run()
    {
      NLog.WriteLine("<{0}> {1}", GetType().Name, "Run: Locking thread and running");
      if (useTimer) lock (mL) { while (isTimerActive) { Thread.Sleep(timetorun); Go(); } Done(); }
      else lock (mL) { Go(); Done(); }
    }

    /// <summary></summary>
    internal void Suspend() { if (mT != null) mT.Suspend(); }

    /// <summary></summary>
    internal void Resume() { if (mT != null) mT.Resume(); }

    /// <summary>
    /// Kills the thread and once this is done, <see cref="OnThreadStopped"/>
    /// is triggered.
    /// 
    /// If your EventHandler destroys this class, you might want to be careful
    /// in your <see cref="Done"/> overload, not to trigger before any post processing
    /// has been done.
    /// </summary>
    public void Stop() { if (mT != null) { mT.Abort(); mT.Join(); mT = null; } OnThreadStopped(); }

    /// <summary></summary>
    public void Cancel() { isTimerActive = false; }

    /// <summary>
    /// Kills the thread and once this is done, <see cref="OnThreadStopped"/>
    /// is triggered.
    /// 
    /// <pre>
    /// protected void Done()
    /// {
    ///   // clean up some stuff...
    ///   base.Done(); // The owner may destroy this class on this call.
    /// }
    /// </pre>
    /// 
    /// 
    /// This is an indirect call to Stop(), which triggers <see cref="OnThreadStopped"/>.
    /// </summary>
    virtual protected void Done() { Stop(); }

    /// <summary>
    /// This is called during each thread interval.
    /// 
    /// 
    /// Here, you are responsible for providing any work that needs to be done including scheduling
    /// event notifiations (you might have implemented) and such.
    /// </summary>
    virtual protected void Go() { }


    //    protected IList<TList> items;
    //    protected Gogo<t> Gadget;
    protected bool isTimerActive = true;
    readonly protected object mL = new object();
    protected int timetorun = 100;
    protected Thread mT;
    public bool useTimer=false;
    ThreadPriority mP = ThreadPriority.Normal;
  }


  //GetSomeLength mGsl;
  //mGsl = new GetSomeLength(this, ()=> {});
  public class ClewPost : Clew
  {
    ITere post;
    Fay go;
    System.Windows.Forms.Control Ctx;
    
    public ClewPost(System.Windows.Forms.Control ctx, Fay primaryAction, Fay postAction)
    {
      Ctx = ctx;
      // I forgot I didn't want this running on the ui-thread..
      go = primaryAction;
      // on the ui thread.
      post = Tere.Create(ctx, postAction);
    }
    
    protected override void Go()
    {
      go();
    }
    protected override void Done()
    {
      post.Go();
      base.Done();
    }
    
  }

  /// thread control
  //public interface It { void Start(); void Stop(); }

  public delegate void Fay();
  public delegate void Fay<t>(t o);
  public delegate void Fay<t, tt>(t o, tt oo);
  //public delegate void Gogo<t,tt,ttt>();
  //public interface IMoo<t,tt,ttt> : IMooBasic { Gogo<t,tt,ttt> Gadget { get; } }
  //public interface IMoo<t,tt> : IMooBasic { Gogo<t,tt> Gadget { get; } }
  public interface ITere<t, tt> { Control Ctx { get; } void Go(t o, tt oo); Fay<t, tt> Ta { get; } bool UseContext { get; } }
  public interface ITere<t> { Control Ctx { get; } void Go(t o); Fay<t> Ta { get; } bool UseContext { get; } }
  public interface ITere { Control Ctx { get; } void Go(); Fay Ta { get; } bool UseContext { get; } }
  #if dotnet_v3_5 // or greater
  static class __g__
  {
    static public ITere       Create      (this Control ctx, Fay act, bool forceDirect=false) { return new Tere{ Ctx=ctx, Ta=act, ForceDirect=forceDirect }; }
    static public ITere<t>    Create<t>   (this Control ctx, Fay<t> act)    { return new Tere<t>{ Ctx=ctx, Ta=act }; }
    static public ITere<t,tt> Create<t,tt>(this Control ctx, Fay<t,tt> act) { return new Tere<t,tt>{ Ctx=ctx, Ta=act }; }
  }
  #endif
  /// <summary>
  /// Base class for Tere
  /// </summary>
  public class Syu {
    /// <summary>
    /// Ignore the form context and just use the action (if set to true)
    /// </summary>
    public bool ForceDirect { get; set; } = false;
    /// <summary>Check Control!=null and Control.InvokeRequired==true (allowing ForceDirect to override all of this, of course).</summary>
    public bool FormFollowsFunction { get { return !(ForceDirect && Ctx==null); } }
    public bool UseContext { get { return FormFollowsFunction && Ctx.InvokeRequired; } }
    public Control Ctx { get; set; }
  }
  /// <summary>
  /// This just calls some form action from some thread.
  /// </summary>
  public class Tere : Syu, ITere
  {
    public Fay Ta { get; set; }
    public void Go() {
      if (UseContext) Ctx.Invoke(Ta); else Ta();
    }

    static public ITere Create(Control ctx, Fay act, bool forceDirect = false) { return new Tere { Ctx = ctx, Ta = act, ForceDirect = forceDirect }; }
  }
  /// <summary>
  /// This just calls some form action from some thread.
  /// </summary>
  public class Tere<t> : Syu, ITere<t>
  {
    public Fay<t> Ta { get; set; }
    public void Go(t o) { if (UseContext) Ctx.Invoke(Ta,o); else Ta(o); }
  }
  /// <summary>
  /// This just calls some form action from some thread.
  /// </summary>
  public class Tere<t,tt> : Syu, ITere<t,tt>
  {
    public Fay<t,tt> Ta { get; set; }
    public void Go(t o, tt oo) { if (UseContext) Ctx.Invoke(Ta, o, oo); else Ta(o,oo); }
  }
  
}




