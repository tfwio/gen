/* oOo * 11/14/2007 : 10:22 PM */
using System;

namespace System
{
	/// <summary>
	/// All of these message types correlate to the System.ConsoleColor Enum values.
	/// </summary>
	public enum logT
	{
		n = ConsoleColor.White,
		w = MessageType.Default,
		b = ConsoleColor.Blue,
		c = ConsoleColor.Cyan,
		g = ConsoleColor.Green,
		y = ConsoleColor.Yellow,
		wrn = MessageType.Yellow,
		m = ConsoleColor.Magenta,
		err = ConsoleColor.Red,
	}
	/// <summary>
	/// All of these message types correlate to the System.ConsoleColor Enum values.
	/// </summary>
	public enum MessageType
	{
		Default = ConsoleColor.White,
		White = MessageType.Default,
		Blue = ConsoleColor.Blue,
		Cyan = ConsoleColor.Cyan,
		Green = ConsoleColor.Green,
		Yellow = ConsoleColor.Yellow,
		Warn = MessageType.Yellow,
		Magenta = ConsoleColor.Magenta,
		Error = ConsoleColor.Red,
	}
}
