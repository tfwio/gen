/*
 * User: xo
 * Date: 6/17/2017
 * Time: 11:33 PM
 */
using System;
namespace System
{
	public class SimpleProgressEventArgs : EventArgs
	{
		public double? NumericResult {
			get;
			set;
		}

    public string StringResult {
      get;
      set;
    }

    public string SmallStringResult {
      get;
      set;
    }

		public string ErrorResult {
			get;
			set;
		}
	}
}








