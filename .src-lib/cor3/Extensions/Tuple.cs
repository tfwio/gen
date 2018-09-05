using System;
using System.Collections.Generic;
using System.Linq;
// This file exists so that the Commandline utility library can be compiled in net3.5
// Extension Method string.IsNullOrWhitespace

// --------------------------------------------------------------------------------------------
// for net35 here is a really frigging simple impl of Tuple<,,>, Tuple<,> and Tuple.new<,?,?>
// in addition to a simple Lazy<> impl from SO.
// --------------------------------------------------------------------------------------------
// Tuple
// https://stackoverflow.com/questions/7120845/equivalent-of-tuple-net-4-for-net-framework-3-5#7120902
// Lazy
// https://stackoverflow.com/questions/3207580/implementation-of-lazyt-for-net-3-5#3207743
// Info on Lazy<>
// https://docs.microsoft.com/en-us/dotnet/framework/performance/lazy-initialization
// https://docs.microsoft.com/en-us/dotnet/api/system.lazy-1?redirectedfrom=MSDN&view=netframework-4.7.2
// --------------------------------------------------------------------------------------------
namespace System.Collections.Generic
{
  public class Tuple<T1, T2>
  {
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Tuple(T1 first, T2 second)
    {
      First = first;
      Second = second;
    }
  }
  public class Tuple<T1, T2, T3>
  {
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    public T3 Third { get; private set; }
    internal Tuple(T1 first, T2 second, T3 third)
    {
      First = first;
      Second = second;
      Third = third;
    }
  }

  public static class Tuple
  {
    public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
    {
      var tuple = new Tuple<T1, T2>(first, second);
      return tuple;
    }
    public static Tuple<T1, T2, T3> New<T1, T2, T3>(T1 first, T2 second, T3 third)
    {
      var tuple = new Tuple<T1, T2, T3>(first, second, third);
      return tuple;
    }
  }

}
namespace System {
  
  static public class StringUtil
  {
    public static bool IsNullOrWhiteSpace(this string value)
    {
      if (value == null) return true;
      return string.IsNullOrEmpty(value.Trim());
    }
  }
  
  /// <summary>
  /// Provides support for lazy initialization.
  /// </summary>
  /// <typeparam name="T">Specifies the type of object that is being lazily initialized.</typeparam>
  public sealed class Lazy<T>
  {
    private readonly object padlock = new object();
    private readonly Func<T> createValue;
    private bool isValueCreated;
    private T value;

    /// <summary>
    /// Gets the lazily initialized value of the current Lazy{T} instance.
    /// </summary>
    public T Value
    {
      get
      {
        if (!isValueCreated)
        {
          lock (padlock)
          {
            if (!isValueCreated)
            {
              value = createValue();
              isValueCreated = true;
            }
          }
        }
        return value;
      }
    }

    /// <summary>
    /// Gets a value that indicates whether a value has been created for this Lazy{T} instance.
    /// </summary>
    public bool IsValueCreated
    {
      get
      {
        lock (padlock)
        {
          return isValueCreated;
        }
      }
    }


    /// <summary>
    /// Initializes a new instance of the Lazy{T} class.
    /// </summary>
    /// <param name="createValue">The delegate that produces the value when it is needed.</param>
    public Lazy(Func<T> createValue)
    {
      if (createValue == null) throw new ArgumentNullException("createValue");

      this.createValue = createValue;
    }


    /// <summary>
    /// Creates and returns a string representation of the Lazy{T}.Value.
    /// </summary>
    /// <returns>The string representation of the Lazy{T}.Value property.</returns>
    public override string ToString()
    {
      return Value.ToString();
    }
  }

}