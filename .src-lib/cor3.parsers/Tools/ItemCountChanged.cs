/*
 * 

 * oio * 04/22/2012 * 14:16
 
 */
using System;

namespace System.Cor3.Parsers.Tools
{
	public class ItemCountChanged : EventArgs
	{
		public int ItemCount { get;set; }
		public int CurrentItem { get;set; }
		public ItemCountChanged(int max, int value)
		{
			ItemCount = max;
			CurrentItem = value;
		}
	}
}
