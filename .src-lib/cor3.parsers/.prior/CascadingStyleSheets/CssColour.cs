/*
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 */
using System;

namespace System.Cor3.Parsers.CascadingStyleSheets
{
	public class CssColour
	{
		#region Color
		static public readonly string simpleColor = @"transparent
	black,#000000,0,0,0
	silver,#C0C0C0,192,192,192
	gray,#808080,128,128,128
	white,#FFFFFF,255,255,255
	maroon,#800000,128,0,0
	red,#FF0000,255,0,0
	purple,#800080,128,0,128
	fuchsia,#FF00FF,255,0,255
	green,#008000,0,128,0
	lime,#00FF00,0,255,0
	olive,#808000,128,128,0
	yellow,#FFFF00,255,255,0
	navy,#000080,0,0,128
	blue,#0000FF,0,0,255
	teal,#008080,0,128,128
	aqua,#00FFFF,0,255,255";
		public enum KnownCssColour {
			/// <summary>aliceblue : F0F8FF or new Color.FromArgb(1,240,248,255)< /summary>
			aliceblue,
			/// <summary>antiquewhite : FAEBD7 or new Color.FromArgb(1,250,235,215)< /summary>
			antiquewhite,
			/// <summary>aqua : 00FFFF or new Color.FromArgb(1,0,255,255)< /summary>
			aqua,
			/// <summary>aquamarine : 7FFFD4 or new Color.FromArgb(1,127,255,212)< /summary>
			aquamarine,
			/// <summary>azure : F0FFFF or new Color.FromArgb(1,240,255,255)< /summary>
			azure,
			/// <summary>beige : F5F5DC or new Color.FromArgb(1,245,245,220)< /summary>
			beige,
			/// <summary>bisque : FFE4C4 or new Color.FromArgb(1,255,228,196)< /summary>
			bisque,
			/// <summary>black : 000000 or new Color.FromArgb(1,0,0,0)< /summary>
			black,
			/// <summary>blanchedalmond : FFEBCD or new Color.FromArgb(1,255,235,205)< /summary>
			blanchedalmond,
			/// <summary>blue : 0000FF or new Color.FromArgb(1,0,0,255)< /summary>
			blue,
			/// <summary>blueviolet : 8A2BE2 or new Color.FromArgb(1,138,43,226)< /summary>
			blueviolet,
			/// <summary>brown : A52A2A or new Color.FromArgb(1,165,42,42)< /summary>
			brown,
			/// <summary>burlywood : DEB887 or new Color.FromArgb(1,222,184,135)< /summary>
			burlywood,
			/// <summary>cadetblue : 5F9EA0 or new Color.FromArgb(1,95,158,160)< /summary>
			cadetblue,
			/// <summary>chartreuse : 7FFF00 or new Color.FromArgb(1,127,255,0)< /summary>
			chartreuse,
			/// <summary>chocolate : D2691E or new Color.FromArgb(1,210,105,30)< /summary>
			chocolate,
			/// <summary>coral : FF7F50 or new Color.FromArgb(1,255,127,80)< /summary>
			coral,
			/// <summary>cornflowerblue : 6495ED or new Color.FromArgb(1,100,149,237)< /summary>
			cornflowerblue,
			/// <summary>cornsilk : FFF8DC or new Color.FromArgb(1,255,248,220)< /summary>
			cornsilk,
			/// <summary>crimson : DC143C or new Color.FromArgb(1,220,20,60)< /summary>
			crimson,
			/// <summary>cyan : 00FFFF or new Color.FromArgb(1,0,255,255)< /summary>
			cyan,
			/// <summary>darkblue : 00008B or new Color.FromArgb(1,0,0,139)< /summary>
			darkblue,
			/// <summary>darkcyan : 008B8B or new Color.FromArgb(1,0,139,139)< /summary>
			darkcyan,
			/// <summary>darkgoldenrod : B8860B or new Color.FromArgb(1,184,134,11)< /summary>
			darkgoldenrod,
			/// <summary>darkgray : A9A9A9 or new Color.FromArgb(1,169,169,169)< /summary>
			darkgray,
			/// <summary>darkgreen : 006400 or new Color.FromArgb(1,0,100,0)< /summary>
			darkgreen,
			/// <summary>darkgrey : A9A9A9 or new Color.FromArgb(1,169,169,169)< /summary>
			darkgrey,
			/// <summary>darkkhaki : BDB76B or new Color.FromArgb(1,189,183,107)< /summary>
			darkkhaki,
			/// <summary>darkmagenta : 8B008B or new Color.FromArgb(1,139,0,139)< /summary>
			darkmagenta,
			/// <summary>darkolivegreen : 556B2F or new Color.FromArgb(1,85,107,47)< /summary>
			darkolivegreen,
			/// <summary>darkorange : FF8C00 or new Color.FromArgb(1,255,140,0)< /summary>
			darkorange,
			/// <summary>darkorchid : 9932CC or new Color.FromArgb(1,153,50,204)< /summary>
			darkorchid,
			/// <summary>darkred : 8B0000 or new Color.FromArgb(1,139,0,0)< /summary>
			darkred,
			/// <summary>darksalmon : E9967A or new Color.FromArgb(1,233,150,122)< /summary>
			darksalmon,
			/// <summary>darkseagreen : 8FBC8F or new Color.FromArgb(1,143,188,143)< /summary>
			darkseagreen,
			/// <summary>darkslateblue : 483D8B or new Color.FromArgb(1,72,61,139)< /summary>
			darkslateblue,
			/// <summary>darkslategray : 2F4F4F or new Color.FromArgb(1,47,79,79)< /summary>
			darkslategray,
			/// <summary>darkslategrey : 2F4F4F or new Color.FromArgb(1,47,79,79)< /summary>
			darkslategrey,
			/// <summary>darkturquoise : 00CED1 or new Color.FromArgb(1,0,206,209)< /summary>
			darkturquoise,
			/// <summary>darkviolet : 9400D3 or new Color.FromArgb(1,148,0,211)< /summary>
			darkviolet,
			/// <summary>deeppink : FF1493 or new Color.FromArgb(1,255,20,147)< /summary>
			deeppink,
			/// <summary>deepskyblue : 00BFFF or new Color.FromArgb(1,0,191,255)< /summary>
			deepskyblue,
			/// <summary>dimgray : 696969 or new Color.FromArgb(1,105,105,105)< /summary>
			dimgray,
			/// <summary>dimgrey : 696969 or new Color.FromArgb(1,105,105,105)< /summary>
			dimgrey,
			/// <summary>dodgerblue : 1E90FF or new Color.FromArgb(1,30,144,255)< /summary>
			dodgerblue,
			/// <summary>firebrick : B22222 or new Color.FromArgb(1,178,34,34)< /summary>
			firebrick,
			/// <summary>floralwhite : FFFAF0 or new Color.FromArgb(1,255,250,240)< /summary>
			floralwhite,
			/// <summary>forestgreen : 228B22 or new Color.FromArgb(1,34,139,34)< /summary>
			forestgreen,
			/// <summary>fuchsia : FF00FF or new Color.FromArgb(1,255,0,255)< /summary>
			fuchsia,
			/// <summary>gainsboro : DCDCDC or new Color.FromArgb(1,220,220,220)< /summary>
			gainsboro,
			/// <summary>ghostwhite : F8F8FF or new Color.FromArgb(1,248,248,255)< /summary>
			ghostwhite,
			/// <summary>gold : FFD700 or new Color.FromArgb(1,255,215,0)< /summary>
			gold,
			/// <summary>goldenrod : DAA520 or new Color.FromArgb(1,218,165,32)< /summary>
			goldenrod,
			/// <summary>gray : 808080 or new Color.FromArgb(1,128,128,128)< /summary>
			gray,
			/// <summary>green : 008000 or new Color.FromArgb(1,0,128,0)< /summary>
			green,
			/// <summary>greenyellow : ADFF2F or new Color.FromArgb(1,173,255,47)< /summary>
			greenyellow,
			/// <summary>grey : 808080 or new Color.FromArgb(1,128,128,128)< /summary>
			grey,
			/// <summary>honeydew : F0FFF0 or new Color.FromArgb(1,240,255,240)< /summary>
			honeydew,
			/// <summary>hotpink : FF69B4 or new Color.FromArgb(1,255,105,180)< /summary>
			hotpink,
			/// <summary>indianred : CD5C5C or new Color.FromArgb(1,205,92,92)< /summary>
			indianred,
			/// <summary>indigo : 4B0082 or new Color.FromArgb(1,75,0,130)< /summary>
			indigo,
			/// <summary>ivory : FFFFF0 or new Color.FromArgb(1,255,255,240)< /summary>
			ivory,
			/// <summary>khaki : F0E68C or new Color.FromArgb(1,240,230,140)< /summary>
			khaki,
			/// <summary>lavender : E6E6FA or new Color.FromArgb(1,230,230,250)< /summary>
			lavender,
			/// <summary>lavenderblush : FFF0F5 or new Color.FromArgb(1,255,240,245)< /summary>
			lavenderblush,
			/// <summary>lawngreen : 7CFC00 or new Color.FromArgb(1,124,252,0)< /summary>
			lawngreen,
			/// <summary>lemonchiffon : FFFACD or new Color.FromArgb(1,255,250,205)< /summary>
			lemonchiffon,
			/// <summary>lightblue : ADD8E6 or new Color.FromArgb(1,173,216,230)< /summary>
			lightblue,
			/// <summary>lightcoral : F08080 or new Color.FromArgb(1,240,128,128)< /summary>
			lightcoral,
			/// <summary>lightcyan : E0FFFF or new Color.FromArgb(1,224,255,255)< /summary>
			lightcyan,
			/// <summary>lightgoldenrodyellow : FAFAD2 or new Color.FromArgb(1,250,250,210)< /summary>
			lightgoldenrodyellow,
			/// <summary>lightgray : D3D3D3 or new Color.FromArgb(1,211,211,211)< /summary>
			lightgray,
			/// <summary>lightgreen : 90EE90 or new Color.FromArgb(1,144,238,144)< /summary>
			lightgreen,
			/// <summary>lightgrey : D3D3D3 or new Color.FromArgb(1,211,211,211)< /summary>
			lightgrey,
			/// <summary>lightpink : FFB6C1 or new Color.FromArgb(1,255,182,193)< /summary>
			lightpink,
			/// <summary>lightsalmon : FFA07A or new Color.FromArgb(1,255,160,122)< /summary>
			lightsalmon,
			/// <summary>lightseagreen : 20B2AA or new Color.FromArgb(1,32,178,170)< /summary>
			lightseagreen,
			/// <summary>lightskyblue : 87CEFA or new Color.FromArgb(1,135,206,250)< /summary>
			lightskyblue,
			/// <summary>lightslategray : 778899 or new Color.FromArgb(1,119,136,153)< /summary>
			lightslategray,
			/// <summary>lightslategrey : 778899 or new Color.FromArgb(1,119,136,153)< /summary>
			lightslategrey,
			/// <summary>lightsteelblue : B0C4DE or new Color.FromArgb(1,176,196,222)< /summary>
			lightsteelblue,
			/// <summary>lightyellow : FFFFE0 or new Color.FromArgb(1,255,255,224)< /summary>
			lightyellow,
			/// <summary>lime : 00FF00 or new Color.FromArgb(1,0,255,0)< /summary>
			lime,
			/// <summary>limegreen : 32CD32 or new Color.FromArgb(1,50,205,50)< /summary>
			limegreen,
			/// <summary>linen : FAF0E6 or new Color.FromArgb(1,250,240,230)< /summary>
			linen,
			/// <summary>magenta : FF00FF or new Color.FromArgb(1,255,0,255)< /summary>
			magenta,
			/// <summary>maroon : 800000 or new Color.FromArgb(1,128,0,0)< /summary>
			maroon,
			/// <summary>mediumaquamarine : 66CDAA or new Color.FromArgb(1,102,205,170)< /summary>
			mediumaquamarine,
			/// <summary>mediumblue : 0000CD or new Color.FromArgb(1,0,0,205)< /summary>
			mediumblue,
			/// <summary>mediumorchid : BA55D3 or new Color.FromArgb(1,186,85,211)< /summary>
			mediumorchid,
			/// <summary>mediumpurple : 9370DB or new Color.FromArgb(1,147,112,219)< /summary>
			mediumpurple,
			/// <summary>mediumseagreen : 3CB371 or new Color.FromArgb(1,60,179,113)< /summary>
			mediumseagreen,
			/// <summary>mediumslateblue : 7B68EE or new Color.FromArgb(1,123,104,238)< /summary>
			mediumslateblue,
			/// <summary>mediumspringgreen : 00FA9A or new Color.FromArgb(1,0,250,154)< /summary>
			mediumspringgreen,
			/// <summary>mediumturquoise : 48D1CC or new Color.FromArgb(1,72,209,204)< /summary>
			mediumturquoise,
			/// <summary>mediumvioletred : C71585 or new Color.FromArgb(1,199,21,133)< /summary>
			mediumvioletred,
			/// <summary>midnightblue : 191970 or new Color.FromArgb(1,25,25,112)< /summary>
			midnightblue,
			/// <summary>mintcream : F5FFFA or new Color.FromArgb(1,245,255,250)< /summary>
			mintcream,
			/// <summary>mistyrose : FFE4E1 or new Color.FromArgb(1,255,228,225)< /summary>
			mistyrose,
			/// <summary>moccasin : FFE4B5 or new Color.FromArgb(1,255,228,181)< /summary>
			moccasin,
			/// <summary>navajowhite : FFDEAD or new Color.FromArgb(1,255,222,173)< /summary>
			navajowhite,
			/// <summary>navy : 000080 or new Color.FromArgb(1,0,0,128)< /summary>
			navy,
			/// <summary>oldlace : FDF5E6 or new Color.FromArgb(1,253,245,230)< /summary>
			oldlace,
			/// <summary>olive : 808000 or new Color.FromArgb(1,128,128,0)< /summary>
			olive,
			/// <summary>olivedrab : 6B8E23 or new Color.FromArgb(1,107,142,35)< /summary>
			olivedrab,
			/// <summary>orange : FFA500 or new Color.FromArgb(1,255,165,0)< /summary>
			orange,
			/// <summary>orangered : FF4500 or new Color.FromArgb(1,255,69,0)< /summary>
			orangered,
			/// <summary>orchid : DA70D6 or new Color.FromArgb(1,218,112,214)< /summary>
			orchid,
			/// <summary>palegoldenrod : EEE8AA or new Color.FromArgb(1,238,232,170)< /summary>
			palegoldenrod,
			/// <summary>palegreen : 98FB98 or new Color.FromArgb(1,152,251,152)< /summary>
			palegreen,
			/// <summary>paleturquoise : AFEEEE or new Color.FromArgb(1,175,238,238)< /summary>
			paleturquoise,
			/// <summary>palevioletred : DB7093 or new Color.FromArgb(1,219,112,147)< /summary>
			palevioletred,
			/// <summary>papayawhip : FFEFD5 or new Color.FromArgb(1,255,239,213)< /summary>
			papayawhip,
			/// <summary>peachpuff : FFDAB9 or new Color.FromArgb(1,255,218,185)< /summary>
			peachpuff,
			/// <summary>peru : CD853F or new Color.FromArgb(1,205,133,63)< /summary>
			peru,
			/// <summary>pink : FFC0CB or new Color.FromArgb(1,255,192,203)< /summary>
			pink,
			/// <summary>plum : DDA0DD or new Color.FromArgb(1,221,160,221)< /summary>
			plum,
			/// <summary>powderblue : B0E0E6 or new Color.FromArgb(1,176,224,230)< /summary>
			powderblue,
			/// <summary>purple : 800080 or new Color.FromArgb(1,128,0,128)< /summary>
			purple,
			/// <summary>red : FF0000 or new Color.FromArgb(1,255,0,0)< /summary>
			red,
			/// <summary>rosybrown : BC8F8F or new Color.FromArgb(1,188,143,143)< /summary>
			rosybrown,
			/// <summary>royalblue : 4169E1 or new Color.FromArgb(1,65,105,225)< /summary>
			royalblue,
			/// <summary>saddlebrown : 8B4513 or new Color.FromArgb(1,139,69,19)< /summary>
			saddlebrown,
			/// <summary>salmon : FA8072 or new Color.FromArgb(1,250,128,114)< /summary>
			salmon,
			/// <summary>sandybrown : F4A460 or new Color.FromArgb(1,244,164,96)< /summary>
			sandybrown,
			/// <summary>seagreen : 2E8B57 or new Color.FromArgb(1,46,139,87)< /summary>
			seagreen,
			/// <summary>seashell : FFF5EE or new Color.FromArgb(1,255,245,238)< /summary>
			seashell,
			/// <summary>sienna : A0522D or new Color.FromArgb(1,160,82,45)< /summary>
			sienna,
			/// <summary>silver : C0C0C0 or new Color.FromArgb(1,192,192,192)< /summary>
			silver,
			/// <summary>skyblue : 87CEEB or new Color.FromArgb(1,135,206,235)< /summary>
			skyblue,
			/// <summary>slateblue : 6A5ACD or new Color.FromArgb(1,106,90,205)< /summary>
			slateblue,
			/// <summary>slategray : 708090 or new Color.FromArgb(1,112,128,144)< /summary>
			slategray,
			/// <summary>slategrey : 708090 or new Color.FromArgb(1,112,128,144)< /summary>
			slategrey,
			/// <summary>snow : FFFAFA or new Color.FromArgb(1,255,250,250)< /summary>
			snow,
			/// <summary>springgreen : 00FF7F or new Color.FromArgb(1,0,255,127)< /summary>
			springgreen,
			/// <summary>steelblue : 4682B4 or new Color.FromArgb(1,70,130,180)< /summary>
			steelblue,
			/// <summary>tan : D2B48C or new Color.FromArgb(1,210,180,140)< /summary>
			tan,
			/// <summary>teal : 008080 or new Color.FromArgb(1,0,128,128)< /summary>
			teal,
			/// <summary>thistle : D8BFD8 or new Color.FromArgb(1,216,191,216)< /summary>
			thistle,
			/// <summary>tomato : FF6347 or new Color.FromArgb(1,255,99,71)< /summary>
			tomato,
			/// <summary>turquoise : 40E0D0 or new Color.FromArgb(1,64,224,208)< /summary>
			turquoise,
			/// <summary>violet : EE82EE or new Color.FromArgb(1,238,130,238)< /summary>
			violet,
			/// <summary>wheat : F5DEB3 or new Color.FromArgb(1,245,222,179)< /summary>
			wheat,
			/// <summary>white : FFFFFF or new Color.FromArgb(1,255,255,255)< /summary>
			white,
			/// <summary>whitesmoke : F5F5F5 or new Color.FromArgb(1,245,245,245)< /summary>
			whitesmoke,
			/// <summary>yellow : FFFF00 or new Color.FromArgb(1,255,255,0)< /summary>
			yellow,
			/// <summary>yellowgreen : 9ACD32 or new Color.FromArgb(1,154,205,50)< /summary>
			yellowgreen,
		}
		const string colorstr = @"
				aliceblue	#F0F8FF	240,248,255
				antiquewhite	#FAEBD7	250,235,215
				aqua	#00FFFF	0,255,255
				aquamarine	#7FFFD4	127,255,212
				azure	#F0FFFF	240,255,255
				beige	#F5F5DC	245,245,220
				bisque	#FFE4C4	255,228,196
				black	#000000	0,0,0
				blanchedalmond	#FFEBCD	255,235,205
				blue	#0000FF	0,0,255
				blueviolet	#8A2BE2	138,43,226
				brown	#A52A2A	165,42,42
				burlywood	#DEB887	222,184,135
				cadetblue	#5F9EA0	95,158,160
				chartreuse	#7FFF00	127,255,0
				chocolate	#D2691E	210,105,30
				coral	#FF7F50	255,127,80
				cornflowerblue	#6495ED	100,149,237
				cornsilk	#FFF8DC	255,248,220
				crimson	#DC143C	220,20,60
				cyan	#00FFFF	0,255,255
				darkblue	#00008B	0,0,139
				darkcyan	#008B8B	0,139,139
				darkgoldenrod	#B8860B	184,134,11
				darkgray	#A9A9A9	169,169,169
				darkgreen	#006400	0,100,0
				darkgrey	#A9A9A9	169,169,169
				darkkhaki	#BDB76B	189,183,107
				darkmagenta	#8B008B	139,0,139
				darkolivegreen	#556B2F	85,107,47
				darkorange	#FF8C00	255,140,0
				darkorchid	#9932CC	153,50,204
				darkred	#8B0000	139,0,0
				darksalmon	#E9967A	233,150,122
				darkseagreen	#8FBC8F	143,188,143
				darkslateblue	#483D8B	72,61,139
				darkslategray	#2F4F4F	47,79,79
				darkslategrey	#2F4F4F	47,79,79
				darkturquoise	#00CED1	0,206,209
				darkviolet	#9400D3	148,0,211
				deeppink	#FF1493	255,20,147
				deepskyblue	#00BFFF	0,191,255
				dimgray	#696969	105,105,105
				dimgrey	#696969	105,105,105
				dodgerblue	#1E90FF	30,144,255
				firebrick	#B22222	178,34,34
				floralwhite	#FFFAF0	255,250,240
				forestgreen	#228B22	34,139,34
				fuchsia	#FF00FF	255,0,255
				gainsboro	#DCDCDC	220,220,220
				ghostwhite	#F8F8FF	248,248,255
				gold	#FFD700	255,215,0
				goldenrod	#DAA520	218,165,32
				gray	#808080	128,128,128
				green	#008000	0,128,0
				greenyellow	#ADFF2F	173,255,47
				grey	#808080	128,128,128
				honeydew	#F0FFF0	240,255,240
				hotpink	#FF69B4	255,105,180
				indianred	#CD5C5C	205,92,92
				indigo	#4B0082	75,0,130
				ivory	#FFFFF0	255,255,240
				khaki	#F0E68C	240,230,140
				lavender	#E6E6FA	230,230,250
				lavenderblush	#FFF0F5	255,240,245
				lawngreen	#7CFC00	124,252,0
				lemonchiffon	#FFFACD	255,250,205
				lightblue	#ADD8E6	173,216,230
				lightcoral	#F08080	240,128,128
				lightcyan	#E0FFFF	224,255,255
				lightgoldenrodyellow	#FAFAD2	250,250,210
				lightgray	#D3D3D3	211,211,211
				lightgreen	#90EE90	144,238,144
				lightgrey	#D3D3D3	211,211,211
				lightpink	#FFB6C1	255,182,193
				lightsalmon	#FFA07A	255,160,122
				lightseagreen	#20B2AA	32,178,170
				lightskyblue	#87CEFA	135,206,250
				lightslategray	#778899	119,136,153
				lightslategrey	#778899	119,136,153
				lightsteelblue	#B0C4DE	176,196,222
				lightyellow	#FFFFE0	255,255,224
				lime	#00FF00	0,255,0
				limegreen	#32CD32	50,205,50
				linen	#FAF0E6	250,240,230
				magenta	#FF00FF	255,0,255
				maroon	#800000	128,0,0
				mediumaquamarine	#66CDAA	102,205,170
				mediumblue	#0000CD	0,0,205
				mediumorchid	#BA55D3	186,85,211
				mediumpurple	#9370DB	147,112,219
				mediumseagreen	#3CB371	60,179,113
				mediumslateblue	#7B68EE	123,104,238
				mediumspringgreen	#00FA9A	0,250,154
				mediumturquoise	#48D1CC	72,209,204
				mediumvioletred	#C71585	199,21,133
				midnightblue	#191970	25,25,112
				mintcream	#F5FFFA	245,255,250
				mistyrose	#FFE4E1	255,228,225
				moccasin	#FFE4B5	255,228,181
				navajowhite	#FFDEAD	255,222,173
				navy	#000080	0,0,128
				oldlace	#FDF5E6	253,245,230
				olive	#808000	128,128,0
				olivedrab	#6B8E23	107,142,35
				orange	#FFA500	255,165,0
				orangered	#FF4500	255,69,0
				orchid	#DA70D6	218,112,214
				palegoldenrod	#EEE8AA	238,232,170
				palegreen	#98FB98	152,251,152
				paleturquoise	#AFEEEE	175,238,238
				palevioletred	#DB7093	219,112,147
				papayawhip	#FFEFD5	255,239,213
				peachpuff	#FFDAB9	255,218,185
				peru	#CD853F	205,133,63
				pink	#FFC0CB	255,192,203
				plum	#DDA0DD	221,160,221
				powderblue	#B0E0E6	176,224,230
				purple	#800080	128,0,128
				red	#FF0000	255,0,0
				rosybrown	#BC8F8F	188,143,143
				royalblue	#4169E1	65,105,225
				saddlebrown	#8B4513	139,69,19
				salmon	#FA8072	250,128,114
				sandybrown	#F4A460	244,164,96
				seagreen	#2E8B57	46,139,87
				seashell	#FFF5EE	255,245,238
				sienna	#A0522D	160,82,45
				silver	#C0C0C0	192,192,192
				skyblue	#87CEEB	135,206,235
				slateblue	#6A5ACD	106,90,205
				slategray	#708090	112,128,144
				slategrey	#708090	112,128,144
				snow	#FFFAFA	255,250,250
				springgreen	#00FF7F	0,255,127
				steelblue	#4682B4	70,130,180
				tan	#D2B48C	210,180,140
				teal	#008080	0,128,128
				thistle	#D8BFD8	216,191,216
				tomato	#FF6347	255,99,71
				turquoise	#40E0D0	64,224,208
				violet	#EE82EE	238,130,238
				wheat	#F5DEB3	245,222,179
				white	#FFFFFF	255,255,255
				whitesmoke	#F5F5F5	245,245,245
				yellow	#FFFF00	255,255,0
				yellowgreen	#9ACD32	154,205,50
				";
		
		/**
		 * http://www.w3.org/TR/2011/REC-css3-color-20110607/#svg-color
		 */
	
		#endregion
		#region ColorNameValues
		/// <summary>CSS Color 'aliceblue'</summary>
		public const string css_color_aliceblue = "aliceblue";
		/// <summary>CSS Color 'aliceblue'</summary>
		public const int css_color_aliceblue_h = 0xF0F8FF;
		/// <summary>CSS Color 'antiquewhite'</summary>
		public const string css_color_antiquewhite = "antiquewhite";
		/// <summary>CSS Color 'antiquewhite'</summary>
		public const int css_color_antiquewhite_h = 0xFAEBD7;
		/// <summary>CSS Color 'aqua'</summary>
		public const string css_color_aqua = "aqua";
		/// <summary>CSS Color 'aqua'</summary>
		public const int css_color_aqua_h = 0x00FFFF;
		/// <summary>CSS Color 'aquamarine'</summary>
		public const string css_color_aquamarine = "aquamarine";
		/// <summary>CSS Color 'aquamarine'</summary>
		public const int css_color_aquamarine_h = 0x7FFFD4;
		/// <summary>CSS Color 'azure'</summary>
		public const string css_color_azure = "azure";
		/// <summary>CSS Color 'azure'</summary>
		public const int css_color_azure_h = 0xF0FFFF;
		/// <summary>CSS Color 'beige'</summary>
		public const string css_color_beige = "beige";
		/// <summary>CSS Color 'beige'</summary>
		public const int css_color_beige_h = 0xF5F5DC;
		/// <summary>CSS Color 'bisque'</summary>
		public const string css_color_bisque = "bisque";
		/// <summary>CSS Color 'bisque'</summary>
		public const int css_color_bisque_h = 0xFFE4C4;
		/// <summary>CSS Color 'black'</summary>
		public const string css_color_black = "black";
		/// <summary>CSS Color 'black'</summary>
		public const int css_color_black_h = 0x000000;
		/// <summary>CSS Color 'blanchedalmond'</summary>
		public const string css_color_blanchedalmond = "blanchedalmond";
		/// <summary>CSS Color 'blanchedalmond'</summary>
		public const int css_color_blanchedalmond_h = 0xFFEBCD;
		/// <summary>CSS Color 'blue'</summary>
		public const string css_color_blue = "blue";
		/// <summary>CSS Color 'blue'</summary>
		public const int css_color_blue_h = 0x0000FF;
		/// <summary>CSS Color 'blueviolet'</summary>
		public const string css_color_blueviolet = "blueviolet";
		/// <summary>CSS Color 'blueviolet'</summary>
		public const int css_color_blueviolet_h = 0x8A2BE2;
		/// <summary>CSS Color 'brown'</summary>
		public const string css_color_brown = "brown";
		/// <summary>CSS Color 'brown'</summary>
		public const int css_color_brown_h = 0xA52A2A;
		/// <summary>CSS Color 'burlywood'</summary>
		public const string css_color_burlywood = "burlywood";
		/// <summary>CSS Color 'burlywood'</summary>
		public const int css_color_burlywood_h = 0xDEB887;
		/// <summary>CSS Color 'cadetblue'</summary>
		public const string css_color_cadetblue = "cadetblue";
		/// <summary>CSS Color 'cadetblue'</summary>
		public const int css_color_cadetblue_h = 0x5F9EA0;
		/// <summary>CSS Color 'chartreuse'</summary>
		public const string css_color_chartreuse = "chartreuse";
		/// <summary>CSS Color 'chartreuse'</summary>
		public const int css_color_chartreuse_h = 0x7FFF00;
		/// <summary>CSS Color 'chocolate'</summary>
		public const string css_color_chocolate = "chocolate";
		/// <summary>CSS Color 'chocolate'</summary>
		public const int css_color_chocolate_h = 0xD2691E;
		/// <summary>CSS Color 'coral'</summary>
		public const string css_color_coral = "coral";
		/// <summary>CSS Color 'coral'</summary>
		public const int css_color_coral_h = 0xFF7F50;
		/// <summary>CSS Color 'cornflowerblue'</summary>
		public const string css_color_cornflowerblue = "cornflowerblue";
		/// <summary>CSS Color 'cornflowerblue'</summary>
		public const int css_color_cornflowerblue_h = 0x6495ED;
		/// <summary>CSS Color 'cornsilk'</summary>
		public const string css_color_cornsilk = "cornsilk";
		/// <summary>CSS Color 'cornsilk'</summary>
		public const int css_color_cornsilk_h = 0xFFF8DC;
		/// <summary>CSS Color 'crimson'</summary>
		public const string css_color_crimson = "crimson";
		/// <summary>CSS Color 'crimson'</summary>
		public const int css_color_crimson_h = 0xDC143C;
		/// <summary>CSS Color 'cyan'</summary>
		public const string css_color_cyan = "cyan";
		/// <summary>CSS Color 'cyan'</summary>
		public const int css_color_cyan_h = 0x00FFFF;
		/// <summary>CSS Color 'darkblue'</summary>
		public const string css_color_darkblue = "darkblue";
		/// <summary>CSS Color 'darkblue'</summary>
		public const int css_color_darkblue_h = 0x00008B;
		/// <summary>CSS Color 'darkcyan'</summary>
		public const string css_color_darkcyan = "darkcyan";
		/// <summary>CSS Color 'darkcyan'</summary>
		public const int css_color_darkcyan_h = 0x008B8B;
		/// <summary>CSS Color 'darkgoldenrod'</summary>
		public const string css_color_darkgoldenrod = "darkgoldenrod";
		/// <summary>CSS Color 'darkgoldenrod'</summary>
		public const int css_color_darkgoldenrod_h = 0xB8860B;
		/// <summary>CSS Color 'darkgray'</summary>
		public const string css_color_darkgray = "darkgray";
		/// <summary>CSS Color 'darkgray'</summary>
		public const int css_color_darkgray_h = 0xA9A9A9;
		/// <summary>CSS Color 'darkgreen'</summary>
		public const string css_color_darkgreen = "darkgreen";
		/// <summary>CSS Color 'darkgreen'</summary>
		public const int css_color_darkgreen_h = 0x006400;
		/// <summary>CSS Color 'darkgrey'</summary>
		public const string css_color_darkgrey = "darkgrey";
		/// <summary>CSS Color 'darkgrey'</summary>
		public const int css_color_darkgrey_h = 0xA9A9A9;
		/// <summary>CSS Color 'darkkhaki'</summary>
		public const string css_color_darkkhaki = "darkkhaki";
		/// <summary>CSS Color 'darkkhaki'</summary>
		public const int css_color_darkkhaki_h = 0xBDB76B;
		/// <summary>CSS Color 'darkmagenta'</summary>
		public const string css_color_darkmagenta = "darkmagenta";
		/// <summary>CSS Color 'darkmagenta'</summary>
		public const int css_color_darkmagenta_h = 0x8B008B;
		/// <summary>CSS Color 'darkolivegreen'</summary>
		public const string css_color_darkolivegreen = "darkolivegreen";
		/// <summary>CSS Color 'darkolivegreen'</summary>
		public const int css_color_darkolivegreen_h = 0x556B2F;
		/// <summary>CSS Color 'darkorange'</summary>
		public const string css_color_darkorange = "darkorange";
		/// <summary>CSS Color 'darkorange'</summary>
		public const int css_color_darkorange_h = 0xFF8C00;
		/// <summary>CSS Color 'darkorchid'</summary>
		public const string css_color_darkorchid = "darkorchid";
		/// <summary>CSS Color 'darkorchid'</summary>
		public const int css_color_darkorchid_h = 0x9932CC;
		/// <summary>CSS Color 'darkred'</summary>
		public const string css_color_darkred = "darkred";
		/// <summary>CSS Color 'darkred'</summary>
		public const int css_color_darkred_h = 0x8B0000;
		/// <summary>CSS Color 'darksalmon'</summary>
		public const string css_color_darksalmon = "darksalmon";
		/// <summary>CSS Color 'darksalmon'</summary>
		public const int css_color_darksalmon_h = 0xE9967A;
		/// <summary>CSS Color 'darkseagreen'</summary>
		public const string css_color_darkseagreen = "darkseagreen";
		/// <summary>CSS Color 'darkseagreen'</summary>
		public const int css_color_darkseagreen_h = 0x8FBC8F;
		/// <summary>CSS Color 'darkslateblue'</summary>
		public const string css_color_darkslateblue = "darkslateblue";
		/// <summary>CSS Color 'darkslateblue'</summary>
		public const int css_color_darkslateblue_h = 0x483D8B;
		/// <summary>CSS Color 'darkslategray'</summary>
		public const string css_color_darkslategray = "darkslategray";
		/// <summary>CSS Color 'darkslategray'</summary>
		public const int css_color_darkslategray_h = 0x2F4F4F;
		/// <summary>CSS Color 'darkslategrey'</summary>
		public const string css_color_darkslategrey = "darkslategrey";
		/// <summary>CSS Color 'darkslategrey'</summary>
		public const int css_color_darkslategrey_h = 0x2F4F4F;
		/// <summary>CSS Color 'darkturquoise'</summary>
		public const string css_color_darkturquoise = "darkturquoise";
		/// <summary>CSS Color 'darkturquoise'</summary>
		public const int css_color_darkturquoise_h = 0x00CED1;
		/// <summary>CSS Color 'darkviolet'</summary>
		public const string css_color_darkviolet = "darkviolet";
		/// <summary>CSS Color 'darkviolet'</summary>
		public const int css_color_darkviolet_h = 0x9400D3;
		/// <summary>CSS Color 'deeppink'</summary>
		public const string css_color_deeppink = "deeppink";
		/// <summary>CSS Color 'deeppink'</summary>
		public const int css_color_deeppink_h = 0xFF1493;
		/// <summary>CSS Color 'deepskyblue'</summary>
		public const string css_color_deepskyblue = "deepskyblue";
		/// <summary>CSS Color 'deepskyblue'</summary>
		public const int css_color_deepskyblue_h = 0x00BFFF;
		/// <summary>CSS Color 'dimgray'</summary>
		public const string css_color_dimgray = "dimgray";
		/// <summary>CSS Color 'dimgray'</summary>
		public const int css_color_dimgray_h = 0x696969;
		/// <summary>CSS Color 'dimgrey'</summary>
		public const string css_color_dimgrey = "dimgrey";
		/// <summary>CSS Color 'dimgrey'</summary>
		public const int css_color_dimgrey_h = 0x696969;
		/// <summary>CSS Color 'dodgerblue'</summary>
		public const string css_color_dodgerblue = "dodgerblue";
		/// <summary>CSS Color 'dodgerblue'</summary>
		public const int css_color_dodgerblue_h = 0x1E90FF;
		/// <summary>CSS Color 'firebrick'</summary>
		public const string css_color_firebrick = "firebrick";
		/// <summary>CSS Color 'firebrick'</summary>
		public const int css_color_firebrick_h = 0xB22222;
		/// <summary>CSS Color 'floralwhite'</summary>
		public const string css_color_floralwhite = "floralwhite";
		/// <summary>CSS Color 'floralwhite'</summary>
		public const int css_color_floralwhite_h = 0xFFFAF0;
		/// <summary>CSS Color 'forestgreen'</summary>
		public const string css_color_forestgreen = "forestgreen";
		/// <summary>CSS Color 'forestgreen'</summary>
		public const int css_color_forestgreen_h = 0x228B22;
		/// <summary>CSS Color 'fuchsia'</summary>
		public const string css_color_fuchsia = "fuchsia";
		/// <summary>CSS Color 'fuchsia'</summary>
		public const int css_color_fuchsia_h = 0xFF00FF;
		/// <summary>CSS Color 'gainsboro'</summary>
		public const string css_color_gainsboro = "gainsboro";
		/// <summary>CSS Color 'gainsboro'</summary>
		public const int css_color_gainsboro_h = 0xDCDCDC;
		/// <summary>CSS Color 'ghostwhite'</summary>
		public const string css_color_ghostwhite = "ghostwhite";
		/// <summary>CSS Color 'ghostwhite'</summary>
		public const int css_color_ghostwhite_h = 0xF8F8FF;
		/// <summary>CSS Color 'gold'</summary>
		public const string css_color_gold = "gold";
		/// <summary>CSS Color 'gold'</summary>
		public const int css_color_gold_h = 0xFFD700;
		/// <summary>CSS Color 'goldenrod'</summary>
		public const string css_color_goldenrod = "goldenrod";
		/// <summary>CSS Color 'goldenrod'</summary>
		public const int css_color_goldenrod_h = 0xDAA520;
		/// <summary>CSS Color 'gray'</summary>
		public const string css_color_gray = "gray";
		/// <summary>CSS Color 'gray'</summary>
		public const int css_color_gray_h = 0x808080;
		/// <summary>CSS Color 'green'</summary>
		public const string css_color_green = "green";
		/// <summary>CSS Color 'green'</summary>
		public const int css_color_green_h = 0x008000;
		/// <summary>CSS Color 'greenyellow'</summary>
		public const string css_color_greenyellow = "greenyellow";
		/// <summary>CSS Color 'greenyellow'</summary>
		public const int css_color_greenyellow_h = 0xADFF2F;
		/// <summary>CSS Color 'grey'</summary>
		public const string css_color_grey = "grey";
		/// <summary>CSS Color 'grey'</summary>
		public const int css_color_grey_h = 0x808080;
		/// <summary>CSS Color 'honeydew'</summary>
		public const string css_color_honeydew = "honeydew";
		/// <summary>CSS Color 'honeydew'</summary>
		public const int css_color_honeydew_h = 0xF0FFF0;
		/// <summary>CSS Color 'hotpink'</summary>
		public const string css_color_hotpink = "hotpink";
		/// <summary>CSS Color 'hotpink'</summary>
		public const int css_color_hotpink_h = 0xFF69B4;
		/// <summary>CSS Color 'indianred'</summary>
		public const string css_color_indianred = "indianred";
		/// <summary>CSS Color 'indianred'</summary>
		public const int css_color_indianred_h = 0xCD5C5C;
		/// <summary>CSS Color 'indigo'</summary>
		public const string css_color_indigo = "indigo";
		/// <summary>CSS Color 'indigo'</summary>
		public const int css_color_indigo_h = 0x4B0082;
		/// <summary>CSS Color 'ivory'</summary>
		public const string css_color_ivory = "ivory";
		/// <summary>CSS Color 'ivory'</summary>
		public const int css_color_ivory_h = 0xFFFFF0;
		/// <summary>CSS Color 'khaki'</summary>
		public const string css_color_khaki = "khaki";
		/// <summary>CSS Color 'khaki'</summary>
		public const int css_color_khaki_h = 0xF0E68C;
		/// <summary>CSS Color 'lavender'</summary>
		public const string css_color_lavender = "lavender";
		/// <summary>CSS Color 'lavender'</summary>
		public const int css_color_lavender_h = 0xE6E6FA;
		/// <summary>CSS Color 'lavenderblush'</summary>
		public const string css_color_lavenderblush = "lavenderblush";
		/// <summary>CSS Color 'lavenderblush'</summary>
		public const int css_color_lavenderblush_h = 0xFFF0F5;
		/// <summary>CSS Color 'lawngreen'</summary>
		public const string css_color_lawngreen = "lawngreen";
		/// <summary>CSS Color 'lawngreen'</summary>
		public const int css_color_lawngreen_h = 0x7CFC00;
		/// <summary>CSS Color 'lemonchiffon'</summary>
		public const string css_color_lemonchiffon = "lemonchiffon";
		/// <summary>CSS Color 'lemonchiffon'</summary>
		public const int css_color_lemonchiffon_h = 0xFFFACD;
		/// <summary>CSS Color 'lightblue'</summary>
		public const string css_color_lightblue = "lightblue";
		/// <summary>CSS Color 'lightblue'</summary>
		public const int css_color_lightblue_h = 0xADD8E6;
		/// <summary>CSS Color 'lightcoral'</summary>
		public const string css_color_lightcoral = "lightcoral";
		/// <summary>CSS Color 'lightcoral'</summary>
		public const int css_color_lightcoral_h = 0xF08080;
		/// <summary>CSS Color 'lightcyan'</summary>
		public const string css_color_lightcyan = "lightcyan";
		/// <summary>CSS Color 'lightcyan'</summary>
		public const int css_color_lightcyan_h = 0xE0FFFF;
		/// <summary>CSS Color 'lightgoldenrodyellow'</summary>
		public const string css_color_lightgoldenrodyellow = "lightgoldenrodyellow";
		/// <summary>CSS Color 'lightgoldenrodyellow'</summary>
		public const int css_color_lightgoldenrodyellow_h = 0xFAFAD2;
		/// <summary>CSS Color 'lightgray'</summary>
		public const string css_color_lightgray = "lightgray";
		/// <summary>CSS Color 'lightgray'</summary>
		public const int css_color_lightgray_h = 0xD3D3D3;
		/// <summary>CSS Color 'lightgreen'</summary>
		public const string css_color_lightgreen = "lightgreen";
		/// <summary>CSS Color 'lightgreen'</summary>
		public const int css_color_lightgreen_h = 0x90EE90;
		/// <summary>CSS Color 'lightgrey'</summary>
		public const string css_color_lightgrey = "lightgrey";
		/// <summary>CSS Color 'lightgrey'</summary>
		public const int css_color_lightgrey_h = 0xD3D3D3;
		/// <summary>CSS Color 'lightpink'</summary>
		public const string css_color_lightpink = "lightpink";
		/// <summary>CSS Color 'lightpink'</summary>
		public const int css_color_lightpink_h = 0xFFB6C1;
		/// <summary>CSS Color 'lightsalmon'</summary>
		public const string css_color_lightsalmon = "lightsalmon";
		/// <summary>CSS Color 'lightsalmon'</summary>
		public const int css_color_lightsalmon_h = 0xFFA07A;
		/// <summary>CSS Color 'lightseagreen'</summary>
		public const string css_color_lightseagreen = "lightseagreen";
		/// <summary>CSS Color 'lightseagreen'</summary>
		public const int css_color_lightseagreen_h = 0x20B2AA;
		/// <summary>CSS Color 'lightskyblue'</summary>
		public const string css_color_lightskyblue = "lightskyblue";
		/// <summary>CSS Color 'lightskyblue'</summary>
		public const int css_color_lightskyblue_h = 0x87CEFA;
		/// <summary>CSS Color 'lightslategray'</summary>
		public const string css_color_lightslategray = "lightslategray";
		/// <summary>CSS Color 'lightslategray'</summary>
		public const int css_color_lightslategray_h = 0x778899;
		/// <summary>CSS Color 'lightslategrey'</summary>
		public const string css_color_lightslategrey = "lightslategrey";
		/// <summary>CSS Color 'lightslategrey'</summary>
		public const int css_color_lightslategrey_h = 0x778899;
		/// <summary>CSS Color 'lightsteelblue'</summary>
		public const string css_color_lightsteelblue = "lightsteelblue";
		/// <summary>CSS Color 'lightsteelblue'</summary>
		public const int css_color_lightsteelblue_h = 0xB0C4DE;
		/// <summary>CSS Color 'lightyellow'</summary>
		public const string css_color_lightyellow = "lightyellow";
		/// <summary>CSS Color 'lightyellow'</summary>
		public const int css_color_lightyellow_h = 0xFFFFE0;
		/// <summary>CSS Color 'lime'</summary>
		public const string css_color_lime = "lime";
		/// <summary>CSS Color 'lime'</summary>
		public const int css_color_lime_h = 0x00FF00;
		/// <summary>CSS Color 'limegreen'</summary>
		public const string css_color_limegreen = "limegreen";
		/// <summary>CSS Color 'limegreen'</summary>
		public const int css_color_limegreen_h = 0x32CD32;
		/// <summary>CSS Color 'linen'</summary>
		public const string css_color_linen = "linen";
		/// <summary>CSS Color 'linen'</summary>
		public const int css_color_linen_h = 0xFAF0E6;
		/// <summary>CSS Color 'magenta'</summary>
		public const string css_color_magenta = "magenta";
		/// <summary>CSS Color 'magenta'</summary>
		public const int css_color_magenta_h = 0xFF00FF;
		/// <summary>CSS Color 'maroon'</summary>
		public const string css_color_maroon = "maroon";
		/// <summary>CSS Color 'maroon'</summary>
		public const int css_color_maroon_h = 0x800000;
		/// <summary>CSS Color 'mediumaquamarine'</summary>
		public const string css_color_mediumaquamarine = "mediumaquamarine";
		/// <summary>CSS Color 'mediumaquamarine'</summary>
		public const int css_color_mediumaquamarine_h = 0x66CDAA;
		/// <summary>CSS Color 'mediumblue'</summary>
		public const string css_color_mediumblue = "mediumblue";
		/// <summary>CSS Color 'mediumblue'</summary>
		public const int css_color_mediumblue_h = 0x0000CD;
		/// <summary>CSS Color 'mediumorchid'</summary>
		public const string css_color_mediumorchid = "mediumorchid";
		/// <summary>CSS Color 'mediumorchid'</summary>
		public const int css_color_mediumorchid_h = 0xBA55D3;
		/// <summary>CSS Color 'mediumpurple'</summary>
		public const string css_color_mediumpurple = "mediumpurple";
		/// <summary>CSS Color 'mediumpurple'</summary>
		public const int css_color_mediumpurple_h = 0x9370DB;
		/// <summary>CSS Color 'mediumseagreen'</summary>
		public const string css_color_mediumseagreen = "mediumseagreen";
		/// <summary>CSS Color 'mediumseagreen'</summary>
		public const int css_color_mediumseagreen_h = 0x3CB371;
		/// <summary>CSS Color 'mediumslateblue'</summary>
		public const string css_color_mediumslateblue = "mediumslateblue";
		/// <summary>CSS Color 'mediumslateblue'</summary>
		public const int css_color_mediumslateblue_h = 0x7B68EE;
		/// <summary>CSS Color 'mediumspringgreen'</summary>
		public const string css_color_mediumspringgreen = "mediumspringgreen";
		/// <summary>CSS Color 'mediumspringgreen'</summary>
		public const int css_color_mediumspringgreen_h = 0x00FA9A;
		/// <summary>CSS Color 'mediumturquoise'</summary>
		public const string css_color_mediumturquoise = "mediumturquoise";
		/// <summary>CSS Color 'mediumturquoise'</summary>
		public const int css_color_mediumturquoise_h = 0x48D1CC;
		/// <summary>CSS Color 'mediumvioletred'</summary>
		public const string css_color_mediumvioletred = "mediumvioletred";
		/// <summary>CSS Color 'mediumvioletred'</summary>
		public const int css_color_mediumvioletred_h = 0xC71585;
		/// <summary>CSS Color 'midnightblue'</summary>
		public const string css_color_midnightblue = "midnightblue";
		/// <summary>CSS Color 'midnightblue'</summary>
		public const int css_color_midnightblue_h = 0x191970;
		/// <summary>CSS Color 'mintcream'</summary>
		public const string css_color_mintcream = "mintcream";
		/// <summary>CSS Color 'mintcream'</summary>
		public const int css_color_mintcream_h = 0xF5FFFA;
		/// <summary>CSS Color 'mistyrose'</summary>
		public const string css_color_mistyrose = "mistyrose";
		/// <summary>CSS Color 'mistyrose'</summary>
		public const int css_color_mistyrose_h = 0xFFE4E1;
		/// <summary>CSS Color 'moccasin'</summary>
		public const string css_color_moccasin = "moccasin";
		/// <summary>CSS Color 'moccasin'</summary>
		public const int css_color_moccasin_h = 0xFFE4B5;
		/// <summary>CSS Color 'navajowhite'</summary>
		public const string css_color_navajowhite = "navajowhite";
		/// <summary>CSS Color 'navajowhite'</summary>
		public const int css_color_navajowhite_h = 0xFFDEAD;
		/// <summary>CSS Color 'navy'</summary>
		public const string css_color_navy = "navy";
		/// <summary>CSS Color 'navy'</summary>
		public const int css_color_navy_h = 0x000080;
		/// <summary>CSS Color 'oldlace'</summary>
		public const string css_color_oldlace = "oldlace";
		/// <summary>CSS Color 'oldlace'</summary>
		public const int css_color_oldlace_h = 0xFDF5E6;
		/// <summary>CSS Color 'olive'</summary>
		public const string css_color_olive = "olive";
		/// <summary>CSS Color 'olive'</summary>
		public const int css_color_olive_h = 0x808000;
		/// <summary>CSS Color 'olivedrab'</summary>
		public const string css_color_olivedrab = "olivedrab";
		/// <summary>CSS Color 'olivedrab'</summary>
		public const int css_color_olivedrab_h = 0x6B8E23;
		/// <summary>CSS Color 'orange'</summary>
		public const string css_color_orange = "orange";
		/// <summary>CSS Color 'orange'</summary>
		public const int css_color_orange_h = 0xFFA500;
		/// <summary>CSS Color 'orangered'</summary>
		public const string css_color_orangered = "orangered";
		/// <summary>CSS Color 'orangered'</summary>
		public const int css_color_orangered_h = 0xFF4500;
		/// <summary>CSS Color 'orchid'</summary>
		public const string css_color_orchid = "orchid";
		/// <summary>CSS Color 'orchid'</summary>
		public const int css_color_orchid_h = 0xDA70D6;
		/// <summary>CSS Color 'palegoldenrod'</summary>
		public const string css_color_palegoldenrod = "palegoldenrod";
		/// <summary>CSS Color 'palegoldenrod'</summary>
		public const int css_color_palegoldenrod_h = 0xEEE8AA;
		/// <summary>CSS Color 'palegreen'</summary>
		public const string css_color_palegreen = "palegreen";
		/// <summary>CSS Color 'palegreen'</summary>
		public const int css_color_palegreen_h = 0x98FB98;
		/// <summary>CSS Color 'paleturquoise'</summary>
		public const string css_color_paleturquoise = "paleturquoise";
		/// <summary>CSS Color 'paleturquoise'</summary>
		public const int css_color_paleturquoise_h = 0xAFEEEE;
		/// <summary>CSS Color 'palevioletred'</summary>
		public const string css_color_palevioletred = "palevioletred";
		/// <summary>CSS Color 'palevioletred'</summary>
		public const int css_color_palevioletred_h = 0xDB7093;
		/// <summary>CSS Color 'papayawhip'</summary>
		public const string css_color_papayawhip = "papayawhip";
		/// <summary>CSS Color 'papayawhip'</summary>
		public const int css_color_papayawhip_h = 0xFFEFD5;
		/// <summary>CSS Color 'peachpuff'</summary>
		public const string css_color_peachpuff = "peachpuff";
		/// <summary>CSS Color 'peachpuff'</summary>
		public const int css_color_peachpuff_h = 0xFFDAB9;
		/// <summary>CSS Color 'peru'</summary>
		public const string css_color_peru = "peru";
		/// <summary>CSS Color 'peru'</summary>
		public const int css_color_peru_h = 0xCD853F;
		/// <summary>CSS Color 'pink'</summary>
		public const string css_color_pink = "pink";
		/// <summary>CSS Color 'pink'</summary>
		public const int css_color_pink_h = 0xFFC0CB;
		/// <summary>CSS Color 'plum'</summary>
		public const string css_color_plum = "plum";
		/// <summary>CSS Color 'plum'</summary>
		public const int css_color_plum_h = 0xDDA0DD;
		/// <summary>CSS Color 'powderblue'</summary>
		public const string css_color_powderblue = "powderblue";
		/// <summary>CSS Color 'powderblue'</summary>
		public const int css_color_powderblue_h = 0xB0E0E6;
		/// <summary>CSS Color 'purple'</summary>
		public const string css_color_purple = "purple";
		/// <summary>CSS Color 'purple'</summary>
		public const int css_color_purple_h = 0x800080;
		/// <summary>CSS Color 'red'</summary>
		public const string css_color_red = "red";
		/// <summary>CSS Color 'red'</summary>
		public const int css_color_red_h = 0xFF0000;
		/// <summary>CSS Color 'rosybrown'</summary>
		public const string css_color_rosybrown = "rosybrown";
		/// <summary>CSS Color 'rosybrown'</summary>
		public const int css_color_rosybrown_h = 0xBC8F8F;
		/// <summary>CSS Color 'royalblue'</summary>
		public const string css_color_royalblue = "royalblue";
		/// <summary>CSS Color 'royalblue'</summary>
		public const int css_color_royalblue_h = 0x4169E1;
		/// <summary>CSS Color 'saddlebrown'</summary>
		public const string css_color_saddlebrown = "saddlebrown";
		/// <summary>CSS Color 'saddlebrown'</summary>
		public const int css_color_saddlebrown_h = 0x8B4513;
		/// <summary>CSS Color 'salmon'</summary>
		public const string css_color_salmon = "salmon";
		/// <summary>CSS Color 'salmon'</summary>
		public const int css_color_salmon_h = 0xFA8072;
		/// <summary>CSS Color 'sandybrown'</summary>
		public const string css_color_sandybrown = "sandybrown";
		/// <summary>CSS Color 'sandybrown'</summary>
		public const int css_color_sandybrown_h = 0xF4A460;
		/// <summary>CSS Color 'seagreen'</summary>
		public const string css_color_seagreen = "seagreen";
		/// <summary>CSS Color 'seagreen'</summary>
		public const int css_color_seagreen_h = 0x2E8B57;
		/// <summary>CSS Color 'seashell'</summary>
		public const string css_color_seashell = "seashell";
		/// <summary>CSS Color 'seashell'</summary>
		public const int css_color_seashell_h = 0xFFF5EE;
		/// <summary>CSS Color 'sienna'</summary>
		public const string css_color_sienna = "sienna";
		/// <summary>CSS Color 'sienna'</summary>
		public const int css_color_sienna_h = 0xA0522D;
		/// <summary>CSS Color 'silver'</summary>
		public const string css_color_silver = "silver";
		/// <summary>CSS Color 'silver'</summary>
		public const int css_color_silver_h = 0xC0C0C0;
		/// <summary>CSS Color 'skyblue'</summary>
		public const string css_color_skyblue = "skyblue";
		/// <summary>CSS Color 'skyblue'</summary>
		public const int css_color_skyblue_h = 0x87CEEB;
		/// <summary>CSS Color 'slateblue'</summary>
		public const string css_color_slateblue = "slateblue";
		/// <summary>CSS Color 'slateblue'</summary>
		public const int css_color_slateblue_h = 0x6A5ACD;
		/// <summary>CSS Color 'slategray'</summary>
		public const string css_color_slategray = "slategray";
		/// <summary>CSS Color 'slategray'</summary>
		public const int css_color_slategray_h = 0x708090;
		/// <summary>CSS Color 'slategrey'</summary>
		public const string css_color_slategrey = "slategrey";
		/// <summary>CSS Color 'slategrey'</summary>
		public const int css_color_slategrey_h = 0x708090;
		/// <summary>CSS Color 'snow'</summary>
		public const string css_color_snow = "snow";
		/// <summary>CSS Color 'snow'</summary>
		public const int css_color_snow_h = 0xFFFAFA;
		/// <summary>CSS Color 'springgreen'</summary>
		public const string css_color_springgreen = "springgreen";
		/// <summary>CSS Color 'springgreen'</summary>
		public const int css_color_springgreen_h = 0x00FF7F;
		/// <summary>CSS Color 'steelblue'</summary>
		public const string css_color_steelblue = "steelblue";
		/// <summary>CSS Color 'steelblue'</summary>
		public const int css_color_steelblue_h = 0x4682B4;
		/// <summary>CSS Color 'tan'</summary>
		public const string css_color_tan = "tan";
		/// <summary>CSS Color 'tan'</summary>
		public const int css_color_tan_h = 0xD2B48C;
		/// <summary>CSS Color 'teal'</summary>
		public const string css_color_teal = "teal";
		/// <summary>CSS Color 'teal'</summary>
		public const int css_color_teal_h = 0x008080;
		/// <summary>CSS Color 'thistle'</summary>
		public const string css_color_thistle = "thistle";
		/// <summary>CSS Color 'thistle'</summary>
		public const int css_color_thistle_h = 0xD8BFD8;
		/// <summary>CSS Color 'tomato'</summary>
		public const string css_color_tomato = "tomato";
		/// <summary>CSS Color 'tomato'</summary>
		public const int css_color_tomato_h = 0xFF6347;
		/// <summary>CSS Color 'turquoise'</summary>
		public const string css_color_turquoise = "turquoise";
		/// <summary>CSS Color 'turquoise'</summary>
		public const int css_color_turquoise_h = 0x40E0D0;
		/// <summary>CSS Color 'violet'</summary>
		public const string css_color_violet = "violet";
		/// <summary>CSS Color 'violet'</summary>
		public const int css_color_violet_h = 0xEE82EE;
		/// <summary>CSS Color 'wheat'</summary>
		public const string css_color_wheat = "wheat";
		/// <summary>CSS Color 'wheat'</summary>
		public const int css_color_wheat_h = 0xF5DEB3;
		/// <summary>CSS Color 'white'</summary>
		public const string css_color_white = "white";
		/// <summary>CSS Color 'white'</summary>
		public const int css_color_white_h = 0xFFFFFF;
		/// <summary>CSS Color 'whitesmoke'</summary>
		public const string css_color_whitesmoke = "whitesmoke";
		/// <summary>CSS Color 'whitesmoke'</summary>
		public const int css_color_whitesmoke_h = 0xF5F5F5;
		/// <summary>CSS Color 'yellow'</summary>
		public const string css_color_yellow = "yellow";
		/// <summary>CSS Color 'yellow'</summary>
		public const int css_color_yellow_h = 0xFFFF00;
		/// <summary>CSS Color 'yellowgreen'</summary>
		public const string css_color_yellowgreen = "yellowgreen";
		/// <summary>CSS Color 'yellowgreen'</summary>
		public const int css_color_yellowgreen_h = 0x9ACD32;
		#endregion
	
	}
}
