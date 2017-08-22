//---------------------------------------------------------------------------
// 
// File: HtmlParser.cs
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// Description: Parser for Html-to-Xaml converter
//
//---------------------------------------------------------------------------

using System;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
// StringBuilder
// important TODOS: 

namespace System.Cor3.Parsers.Html
{

	public class CharacterEntityConversions : DICT<string,string>
	{
		static public readonly CharacterEntityConversions Conversions = new CharacterEntityConversions();
		// SO ITSANEXAMPLE;_
		//http://webdesign.about.com/od/localization/l/blhtmlcodes-ascii.htm
		public CharacterEntityConversions()
		{
			this.Add("…","&#133;");
			this.Add("‘","&#145;");
			this.Add("’","&#146;");
			this.Add("“","&#147;");
			this.Add("”","&#148;");
			this.Add("–","&#150;");
			this.Add("—","&#151;");
			this.Add("«","&#171;");
			this.Add("¬","&#172;");
			this.Add("®","&#174;");
			this.Add("¯","&#175;");
			this.Add("°","&#176;");
			this.Add("±","&#177;");
			XLog.WriteC("entities loaded\n","");
	//		ConversionChars.Add("","&#;");
		}
	}
}
