/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Linq;
namespace System.Cor3.Parsers.CascadingStyleSheets
{
	public class AttributeInsightCollection : Collection<AttributeInsight>
	{
		static public AttributeInsightCollection Instance { get { return _instance; }
		} readonly static AttributeInsightCollection _instance = new AttributeInsightCollection();
		
		public bool ContainsKey(string Key)
		{
			IEnumerable<AttributeInsight> returned = this.Where(ai=>ai.Name==Key);
			if (returned.Count()==0) return false;
			return true;
		}
		
		/// <summary>
		/// Get the attribute provided by name.  EG: 'background-color'.
		/// If the key isn't found, returns Null.
		/// </summary>
		public AttributeInsight this[string Key]
		{
			get
			{
				IEnumerable<AttributeInsight> returned = this.Where(ai=>ai.Name==Key);
				if (!this.ContainsKey(Key)) return null;
				return returned.First();
			}
		}

		#region Add
				/// <summary>
		/// Adds the given element by key if the key is not allready present.
		/// </summary>
		/// <param name="name">attribute name.</param>
		/// <param name="values">Value spec.</param>
		/// <param name="ivalue">Initial value.</param>
		/// <param name="appliesto">Applies to</param>
		/// <param name="inherited">Inherited Value</param>
		/// <param name="percentages">Percentages.</param>
		/// <param name="groups">Visual or Audio groups.</param>
		void Add(string name, string values, string ivalue, string appliesto, string inherited, string percentages, string groups)
		{
			if (ContainsKey(name)) return;
			string newname = name.Trim();
			string[] names = name.Split(' ');
			foreach (string n in names)
				this.Add(
					new AttributeInsight(
						n.Trim('\'').Trim(),
						values.Trim(),
						ivalue.Trim(),
						appliesto.Trim(),
						inherited.Trim(),
						percentages.Trim(),
						groups.Trim()));
		}
		/// <summary>
		/// Adds the given element by key if the key is not allready present.
		/// </summary>
		/// <param name="overwrite">.</param>
		/// <param name="name">If set to true, the item is removed if found and replaced.</param>
		/// <param name="values">Value spec.</param>
		/// <param name="ivalue">Initial value.</param>
		/// <param name="appliesto">Applies to</param>
		/// <param name="inherited">Inherited Value</param>
		/// <param name="percentages">Percentages.</param>
		/// <param name="groups">Visual or Audio groups.</param>
		void Add(bool overwrite, string name, string values, string ivalue, string appliesto, string inherited, string percentages, string groups)
		{
			if (ContainsKey(name) && overwrite) this.Remove(this[name]);
			Add(name,values,ivalue,appliesto,inherited,percentages,groups);
		}

		#endregion

		public AttributeInsightCollection()
		{
			AddCss2Elements();
			AddBorderElements();
			AddAnimationElements();
			AddBoxElements();
		}

		void AddCss2Elements()
		{
			this.Add(@"'azimuth' ",@"<angle> | [[ left-side | far-left | left | center-left | center | center-right | right | far-right | right-side ] || behind ] | leftwards | rightwards | inherit ",@"center ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'background-attachment' ",@"scroll | fixed | inherit ",@"scroll ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'background-color' ",@"<color> | transparent | inherit ",@"transparent ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'background-image' ",@"<uri> | none | inherit ",@"none ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'background-position' ",@"[ [ <percentage> | <length> | left | center | right ] [ <percentage> | <length> | top | center | bottom ]? ] | [ [ left | center | right ] || [ top | center | bottom ] ] | inherit ",@"0% 0% ",@"  ",@"no ",@"refer to the size of the box itself ",@"visual");
			this.Add(@"'background-repeat' ",@"repeat | repeat-x | repeat-y | no-repeat | inherit ",@"repeat ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'background' ",@"['background-color' || 'background-image' || 'background-repeat' || 'background-attachment' || 'background-position'] | inherit ",@"see individual properties ",@"  ",@"no ",@"allowed on 'background-position' ",@"visual");
			this.Add(@"'border-collapse' ",@"collapse | separate | inherit ",@"separate ",@"'table' and 'inline-table' elements ",@"yes ",@"  ",@"visual");
			this.Add(@"'border-color' ",@"[ <color> | transparent ]{1,4} | inherit ",@"see individual properties ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'border-spacing' ",@"<length> <length>? | inherit ",@"0 ",@"'table' and 'inline-table' elements  ",@"yes ",@"  ",@"visual");
			this.Add(@"'border-style' ",@"<border-style>{1,4} | inherit ",@"see individual properties ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'border-top' 'border-right' 'border-bottom' 'border-left' ",@"[ <border-width> || <border-style> || 'border-top-color' ] | inherit ",@"see individual properties ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'border-top-color' 'border-right-color' 'border-bottom-color' 'border-left-color' ",@"<color> | transparent | inherit ",@"the value of the 'color' property ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'border-top-style' 'border-right-style' 'border-bottom-style' 'border-left-style' ",@"<border-style> | inherit ",@"none ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'border-top-width' 'border-right-width' 'border-bottom-width' 'border-left-width' ",@"<border-width> | inherit ",@"medium ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'border-width' ",@"<border-width>{1,4} | inherit ",@"see individual properties ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'border' ",@"[ <border-width> || <border-style> || 'border-top-color' ] | inherit ",@"see individual properties ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'bottom' ",@"<length> | <percentage> | auto | inherit ",@"auto ",@"positioned elements ",@"no ",@"refer to height of containing block ",@"visual");
			this.Add(@"'caption-side' ",@"top | bottom | inherit ",@"top ",@"'table-caption' elements ",@"yes ",@"  ",@"visual");
			this.Add(@"'clear' ",@"none | left | right | both | inherit ",@"none ",@"block-level elements ",@"no ",@"  ",@"visual");
			this.Add(@"'clip' ",@"<shape> | auto | inherit ",@"auto ",@"absolutely positioned elements ",@"no ",@"  ",@"visual");
			this.Add(@"'color' ",@"<color> | inherit ",@"depends on user agent ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'content' ",@"normal | none | [ <string> | <uri> | <counter> | attr(<identifier>) | open-quote | close-quote | no-open-quote | no-close-quote ]+ | inherit ",@"normal ",@":before and :after pseudo-elements ",@"no ",@"  ",@"all");
			this.Add(@"'counter-increment' ",@"[ <identifier> <integer>? ]+ | none | inherit ",@"none ",@"  ",@"no ",@"  ",@"all");
			this.Add(@"'counter-reset' ",@"[ <identifier> <integer>? ]+ | none | inherit ",@"none ",@"  ",@"no ",@"  ",@"all");
			this.Add(@"'cue-after' ",@"<uri> | none | inherit ",@"none ",@"  ",@"no ",@"  ",@"aural");
			this.Add(@"'cue-before' ",@"<uri> | none | inherit ",@"none ",@"  ",@"no ",@"  ",@"aural");
			this.Add(@"'cue' ",@"[ 'cue-before' || 'cue-after' ] | inherit ",@"see individual properties ",@"  ",@"no ",@"  ",@"aural");
			this.Add(@"'cursor' ",@"[ [<uri> ,]* [ auto | crosshair | default | pointer | move | e-resize | ne-resize | nw-resize | n-resize | se-resize | sw-resize | s-resize | w-resize | text | wait | help | progress ] ] | inherit ",@"auto ",@"  ",@"yes ",@"  ",@"visual, interactive");
			this.Add(@"'direction' ",@"ltr | rtl | inherit ",@"ltr ",@"all elements, but see prose ",@"yes ",@"  ",@"visual");
			this.Add(@"'display' ",@"inline | block | list-item | inline-block | table | inline-table | table-row-group | table-header-group | table-footer-group | table-row | table-column-group | table-column | table-cell | table-caption | none | inherit ",@"inline ",@"  ",@"no ",@"  ",@"all");
			this.Add(@"'elevation' ",@"<angle> | below | level | above | higher | lower | inherit ",@"level ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'empty-cells' ",@"show | hide | inherit ",@"show ",@"'table-cell' elements ",@"yes ",@"  ",@"visual");
			this.Add(@"'float' ",@"left | right | none | inherit ",@"none ",@"all, but see 9.7 ",@"no ",@"  ",@"visual");
			this.Add(@"'font-family' ",@"[[ <family-name> | <generic-family> ] [, <family-name>| <generic-family>]* ] | inherit ",@"depends on user agent ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'font-size' ",@"<absolute-size> | <relative-size> | <length> | <percentage> | inherit ",@"medium ",@"  ",@"yes ",@"refer to inherited font size ",@"visual");
			this.Add(@"'font-style' ",@"normal | italic | oblique | inherit ",@"normal ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'font-variant' ",@"normal | small-caps | inherit ",@"normal ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'font-weight' ",@"normal | bold | bolder | lighter | 100 | 200 | 300 | 400 | 500 | 600 | 700 | 800 | 900 | inherit ",@"normal ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'font' ",@"[ [ 'font-style' || 'font-variant' || 'font-weight' ]? 'font-size' [ / 'line-height' ]? 'font-family' ] | caption | icon | menu | message-box | small-caption | status-bar | inherit ",@"see individual properties ",@"  ",@"yes ",@"see individual properties ",@"visual");
			this.Add(@"'height' ",@"<length> | <percentage> | auto | inherit ",@"auto ",@"all elements but non-replaced inline elements, table columns, and column groups ",@"no ",@"see prose ",@"visual");
			this.Add(@"'left' ",@"<length> | <percentage> | auto | inherit ",@"auto ",@"positioned elements ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'letter-spacing' ",@"normal | <length> | inherit ",@"normal ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'line-height' ",@"normal | <number> | <length> | <percentage> | inherit ",@"normal ",@"  ",@"yes ",@"refer to the font size of the element itself ",@"visual");
			this.Add(@"'list-style-image' ",@"<uri> | none | inherit ",@"none ",@"elements with 'display: list-item' ",@"yes ",@"  ",@"visual");
			this.Add(@"'list-style-position' ",@"inside | outside | inherit ",@"outside ",@"elements with 'display: list-item' ",@"yes ",@"  ",@"visual");
			this.Add(@"'list-style-type' ",@"disc | circle | square | decimal | decimal-leading-zero | lower-roman | upper-roman | lower-greek | lower-latin | upper-latin | armenian | georgian | lower-alpha | upper-alpha | none | inherit ",@"disc ",@"elements with 'display: list-item' ",@"yes ",@"  ",@"visual");
			this.Add(@"'list-style' ",@"[ 'list-style-type' || 'list-style-position' || 'list-style-image' ] | inherit ",@"see individual properties ",@"elements with 'display: list-item' ",@"yes ",@"  ",@"visual");
			this.Add(@"'margin-right' 'margin-left' ",@"<margin-width> | inherit ",@"0 ",@"all elements except elements with table display types other than table-caption, table and inline-table ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'margin-top' 'margin-bottom' ",@"<margin-width> | inherit ",@"0 ",@"all elements except elements with table display types other than table-caption, table and inline-table ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'margin' ",@"<margin-width>{1,4} | inherit ",@"see individual properties ",@"all elements except elements with table display types other than table-caption, table and inline-table ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'max-height' ",@"<length> | <percentage> | none | inherit ",@"none ",@"all elements but non-replaced inline elements, table columns, and column groups ",@"no ",@"see prose ",@"visual");
			this.Add(@"'max-width' ",@"<length> | <percentage> | none | inherit ",@"none ",@"all elements but non-replaced inline elements, table rows, and row groups ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'min-height' ",@"<length> | <percentage> | inherit ",@"0 ",@"all elements but non-replaced inline elements, table columns, and column groups ",@"no ",@"see prose ",@"visual");
			this.Add(@"'min-width' ",@"<length> | <percentage> | inherit ",@"0 ",@"all elements but non-replaced inline elements, table rows, and row groups ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'orphans' ",@"<integer> | inherit ",@"2 ",@"block container elements ",@"yes ",@"  ",@"visual, paged");
			this.Add(@"'outline-color' ",@"<color> | invert | inherit ",@"invert ",@"  ",@"no ",@"  ",@"visual, interactive");
			this.Add(@"'outline-style' ",@"<border-style> | inherit ",@"none ",@"  ",@"no ",@"  ",@"visual, interactive");
			this.Add(@"'outline-width' ",@"<border-width> | inherit ",@"medium ",@"  ",@"no ",@"  ",@"visual, interactive");
			this.Add(@"'outline' ",@"[ 'outline-color' || 'outline-style' || 'outline-width' ] | inherit ",@"see individual properties ",@"  ",@"no ",@"  ",@"visual, interactive");
			this.Add(@"'overflow' ",@"visible | hidden | scroll | auto | inherit ",@"visible ",@"block containers ",@"no ",@"  ",@"visual");
			this.Add(@"'padding-top' 'padding-right' 'padding-bottom' 'padding-left' ",@"<padding-width> | inherit ",@"0 ",@"all elements except table-row-group, table-header-group, table-footer-group, table-row, table-column-group and table-column ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'padding' ",@"<padding-width>{1,4} | inherit ",@"see individual properties ",@"all elements except table-row-group, table-header-group, table-footer-group, table-row, table-column-group and table-column ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'page-break-after' ",@"auto | always | avoid | left | right | inherit ",@"auto ",@"block-level elements (but see text) ",@"no ",@"  ",@"visual, paged");
			this.Add(@"'page-break-before' ",@"auto | always | avoid | left | right | inherit ",@"auto ",@"block-level elements (but see text) ",@"no ",@"  ",@"visual, paged");
			this.Add(@"'page-break-inside' ",@"avoid | auto | inherit ",@"auto ",@"block-level elements (but see text) ",@"no ",@"  ",@"visual, paged");
			this.Add(@"'pause-after' ",@"<time> | <percentage> | inherit ",@"0 ",@"  ",@"no ",@"see prose ",@"aural");
			this.Add(@"'pause-before' ",@"<time> | <percentage> | inherit ",@"0 ",@"  ",@"no ",@"see prose ",@"aural");
			this.Add(@"'pause' ",@"[ [<time> | <percentage>]{1,2} ] | inherit ",@"see individual properties ",@"  ",@"no ",@"see descriptions of 'pause-before' and 'pause-after' ",@"aural");
			this.Add(@"'pitch-range' ",@"<number> | inherit ",@"50 ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'pitch' ",@"<frequency> | x-low | low | medium | high | x-high | inherit ",@"medium ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'play-during' ",@"<uri> [ mix || repeat ]? | auto | none | inherit ",@"auto ",@"  ",@"no ",@"  ",@"aural");
			this.Add(@"'position' ",@"static | relative | absolute | fixed | inherit ",@"static ",@"  ",@"no ",@"  ",@"visual");
			this.Add(@"'quotes' ",@"[<string> <string>]+ | none | inherit ",@"depends on user agent ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'richness' ",@"<number> | inherit ",@"50 ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'right' ",@"<length> | <percentage> | auto | inherit ",@"auto ",@"positioned elements ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'speak-header' ",@"once | always | inherit ",@"once ",@"elements that have table header information ",@"yes ",@"  ",@"aural");
			this.Add(@"'speak-numeral' ",@"digits | continuous | inherit ",@"continuous ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'speak-punctuation' ",@"code | none | inherit ",@"none ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'speak' ",@"normal | none | spell-out | inherit ",@"normal ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'speech-rate' ",@"<number> | x-slow | slow | medium | fast | x-fast | faster | slower | inherit ",@"medium ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'stress' ",@"<number> | inherit ",@"50 ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'table-layout' ",@"auto | fixed | inherit ",@"auto ",@"'table' and 'inline-table' elements ",@"no ",@"  ",@"visual");
			this.Add(@"'text-align' ",@"left | right | center | justify | inherit ",@"a nameless value that acts as 'left' if 'direction' is 'ltr', 'right' if 'direction' is 'rtl' ",@"block containers ",@"yes ",@"  ",@"visual");
			this.Add(@"'text-decoration' ",@"none | [ underline || overline || line-through || blink ] | inherit ",@"none ",@"  ",@"no (see prose) ",@"  ",@"visual");
			this.Add(@"'text-indent' ",@"<length> | <percentage> | inherit ",@"0 ",@"block containers ",@"yes ",@"refer to width of containing block ",@"visual");
			this.Add(@"'text-transform' ",@"capitalize | uppercase | lowercase | none | inherit ",@"none ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'top' ",@"<length> | <percentage> | auto | inherit ",@"auto ",@"positioned elements ",@"no ",@"refer to height of containing block ",@"visual");
			this.Add(@"'unicode-bidi' ",@"normal | embed | bidi-override | inherit ",@"normal ",@"all elements, but see prose ",@"no ",@"  ",@"visual");
			this.Add(@"'vertical-align' ",@"baseline | sub | super | top | text-top | middle | bottom | text-bottom | <percentage> | <length> | inherit ",@"baseline ",@"inline-level and 'table-cell' elements ",@"no ",@"refer to the 'line-height' of the element itself ",@"visual");
			this.Add(@"'visibility' ",@"visible | hidden | collapse | inherit ",@"visible ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'voice-family' ",@"[[<specific-voice> | <generic-voice> ],]* [<specific-voice> | <generic-voice> ] | inherit ",@"depends on user agent ",@"  ",@"yes ",@"  ",@"aural");
			this.Add(@"'volume' ",@"<number> | <percentage> | silent | x-soft | soft | medium | loud | x-loud | inherit ",@"medium ",@"  ",@"yes ",@"refer to inherited value ",@"aural");
			this.Add(@"'white-space' ",@"normal | pre | nowrap | pre-wrap | pre-line | inherit ",@"normal ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'widows' ",@"<integer> | inherit ",@"2 ",@"block container elements ",@"yes ",@"  ",@"visual, paged");
			this.Add(@"'width' ",@"<length> | <percentage> | auto | inherit ",@"auto ",@"all elements but non-replaced inline elements, table rows, and row groups ",@"no ",@"refer to width of containing block ",@"visual");
			this.Add(@"'word-spacing' ",@"normal | <length> | inherit ",@"normal ",@"  ",@"yes ",@"  ",@"visual");
			this.Add(@"'z-index' ",@"auto | <integer> | inherit ",@"auto ",@"positioned elements ",@"no ",@"  ",@"visual");
		}
		void AddBorderElements()
		{
			// css3-border
			this.Add(@"background",@"[ <bg-layer> , ]* <final-bg-layer>",@"see individual properties",@"all elements",@"no",@"see individual properties",@"visual");
			this.Add(@"background-attachment",@"<attachment> [ , <attachment> ]*",@"scroll",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"background-clip",@"<box> [ , <box> ]*",@"border-box",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"background-color",@"<color>",@"transparent",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"background-image",@"<bg-image> [ , <bg-image> ]*",@"none",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"background-origin",@"<box> [ , <box> ]*",@"padding-box",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"background-position",@"<position> [ , <position> ]*",@"0% 0%",@"all elements",@"no",@"refer to size of background positioning area minus size of background image; see text",@"visual");
			this.Add(@"background-repeat",@"<repeat-style> [ , <repeat-style> ]*",@"repeat",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"background-size",@"<bg-size> [ , <bg-size> ]*",@"auto",@"all elements",@"no",@"see text",@"visual");
			this.Add(@"border",@"<border-width> || <border-style> || <color>",@"See individual properties",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"border-color",@"<color>{1,4}",@"(see individual properties)",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"border-image",@"<‘border-image-source’> || <‘border-image-slice’> [ / <‘border-image-width’> | / <‘border-image-width’>? / <‘border-image-outset’> ]? || <‘border-image-repeat’>",@"See individual properties",@"See individual properties",@"no",@"N/A",@"visual");
			this.Add(@"border-image-outset",@"[ <length> | <number> ]{1,4}",@"0",@"All elements, except internal table elements when ‘border-collapse’ is ‘collapse’",@"no",@"N/A",@"visual");
			this.Add(@"border-image-repeat",@"[ stretch | repeat | round | space ]{1,2}",@"stretch",@"All elements, except internal table elements when ‘border-collapse’ is ‘collapse’",@"no",@"N/A",@"visual");
			this.Add(@"border-image-slice",@"[<number> | <percentage>]{1,4} && fill?",@"100%",@"All elements, except internal table elements when ‘border-collapse’ is ‘collapse’",@"no",@"refer to size of the border image",@"visual");
			this.Add(@"border-image-source",@"none | <image>",@"none",@"All elements, except internal table elements when ‘border-collapse’ is ‘collapse’",@"no",@"N/A",@"visual");
			this.Add(@"border-image-width",@"[ <length> | <percentage> | <number> | auto ]{1,4}",@"1",@"All elements, except table elements when ‘border-collapse’ is ‘collapse’",@"no",@"Relative to width/height of the border image area",@"visual");
			this.Add(@"border-radius",@"[ <length> | <percentage> ]{1,4} [ / [ <length> | <percentage> ]{1,4} ]?",@"see individual properties",@"all elements (but see prose)",@"no",@"Refer to corresponding dimension of the border box.",@"visual");
			this.Add(@"border-style",@"<border-style>{1,4}",@"(see individual properties)",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"border-top, border-right, border-bottom, border-left",@"<border-width> || <border-style> || <color>",@"See individual properties",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"border-top-color , border-right-color, border-bottom-color, border-left-color",@"<color>",@"currentColor",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"border-top-left-radius, border-top-right-radius, border-bottom-right-radius, border-bottom-left-radius",@"[ <length> | <percentage> ]{1,2}",@"0",@"all elements (but see prose)",@"no",@"Refer to corresponding dimension of the border box.",@"visual");
			this.Add(@"border-top-style, border-right-style, border-bottom-style, border-left-style",@"<border-style>",@"none",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"border-top-width, border-right-width, border-bottom-width, border-left-width",@"<border-width>",@"medium",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"border-width",@"<border-width>{1,4}",@"(see individual properties)",@"all elements",@"no",@"see individual properties",@"visual");
			this.Add(@"box-decoration-break",@"slice | clone",@"slice",@"all elements",@"no",@"N/A",@"visual");
			this.Add(@"box-shadow",@"none | <shadow> [ , <shadow> ]*",@"none",@"all elements",@"no",@"N/A",@"visual");
		}
		void AddAnimationElements()
		{
			// css3-animation
			this.Add(@"animation",@"[<animation-name> || <animation-duration> || <animation-timing-function> || <animation-delay> || <animation-iteration-count> || <animation-direction> || <animation-fill-mode>] [, [<animation-name> || <animation-duration> || <animation-timing-function> || <animation-delay> || <animation-iteration-count> || <animation-direction> || <animation-fill-mode>] ]*",@"see individual properties",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-delay",@"<time> [, <time>]*",@"0s",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-direction",@"[ normal | reverse | alternate | alternate-reverse ] [, [ normal | reverse | alternate | alternate-reverse ] ]*",@"normal",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-duration",@"<time> [, <time>]*",@"0s",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-fill-mode",@"[ none | forwards | backwards | both ] [, [ none | forwards | backwards | both ] ]*",@"none",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-iteration-count",@"[ infinite | <number> ] [, [ infinite | <number> ] ]*",@"1",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-name",@"[ none | IDENT ] [, [ none | IDENT ] ]*",@"none",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-play-state",@"[ running | paused ] [, [ running | paused ] ]*",@"running",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
			this.Add(@"animation-timing-function",@"[ ease | linear | ease-in | ease-out | ease-in-out | step-start | step-end | steps(<number>[, [ start | end ] ]?) | cubic-bezier(<number>, <number>, <number>, <number>) ] [, [ ease | linear | ease-in | ease-out | ease-in-out | step-start | step-end | steps(<number>[, [ start | end ] ]?) | cubic-bezier(<number>, <number>, <number>, <number>)] ]*",@"ease",@"all elements, :before and :after pseudo elements",@"no",@"N/A",@"visual");
		}
		void AddBoxElements()
		{
			// css3-box
			this.Add(@"clear ",@"none | left | right | both ",@"none ",@"block-level elements ",@"no ",@"N/A ",@"visual");
			this.Add(true,@"display ",@"inline | block | inline-block | list-item | run-in | compact | table | inline-table | table-row-group | table-header-group | table-footer-group | table-row | table-column-group | table-column | table-cell | table-caption | ruby | ruby-base | ruby-text | ruby-base-group | ruby-text-group | <template>| none ",@"inline ",@"all elements ",@"no ",@"N/A ",@"visual (‘none’ applies to all media)");
			this.Add(@"float ",@"left | right | none | <page-floats> ",@"none ",@"all, but see 9.7 ",@"no ",@"N/A ",@"visual");
			this.Add(@"height ",@"<length> | <percentage> | auto ",@"auto ",@"all elements but non-replaced inline elements, table columns, and column groups ",@"no ",@"see prose ",@"visual");
			this.Add(@"margin ",@"[ <length> | <percentage> | auto ]{1,4} ",@"(see individual properties) ",@"see text ",@"no ",@"width* of containing block ",@"visual");
			this.Add(@"margin-top , margin-right, margin-bottom, margin-left ",@"<length> | <percentage> | auto ",@"0 ",@"see text ",@"no ",@"width* of containing block ",@"visual");
			this.Add(@"marquee-direction ",@"forward | reverse ",@"reverse ",@"same as ‘overflow’ ",@"yes ",@"N/A ",@"visual");
			this.Add(@"marquee-loop ",@"<non-negative-integer> | infinite ",@"1 ",@"same as ‘overflow’ ",@"no ",@"N/A ",@"visual");
			this.Add(@"marquee-speed ",@"slow | normal | fast ",@"normal ",@"same as ‘overflow’ ",@"no ",@"N/A ",@"visual");
			this.Add(@"marquee-style ",@"scroll | slide | alternate ",@"scroll ",@"same as ‘overflow’ ",@"no ",@"N/A ",@"visual");
			this.Add(@"max-width, max-height ",@"<length> | <percentage> | none ",@"none ",@"all elements but non-replaced inline elements, table rows, and row groups ",@"no ",@"refer to width, resp. height of containing block ",@"visual");
			this.Add(@"min-width, min-height ",@"<length> | <percentage> | inherit ",@"0 ",@"all elements but non-replaced inline elements, table rows, and row groups ",@"no ",@"refer to width, resp. height of containing block ",@"visual");
			this.Add(@"overflow ",@"[ visible | hidden | scroll | auto | no-display | no-content ]{1,2} ",@"see individual properties ",@"non-replaced block-level elements and non-replaced ‘inline-block’ elements ",@"no ",@"N/A ",@"visual");
			this.Add(@"overflow-style ",@"auto | [scrollbar | panner | move | marquee] [, [scrollbar | panner | move | marquee]]* ",@"auto ",@"same as ‘overflow’ ",@"yes ",@"N/A ",@"visual");
			this.Add(@"overflow-x, overflow-y, ",@"visible | hidden | scroll | auto | no-display | no-content ",@"visible ",@"non-replaced block-level elements and non-replaced ‘inline-block’ elements ",@"no ",@"N/A ",@"visual");
			this.Add(@"padding ",@"[ <length> | <percentage> ]{1,4} ",@"(see individual properties) ",@"all elements ",@"no ",@"width* of containing block ",@"visual");
			this.Add(@"padding-top , padding-right, padding-bottom, padding-left ",@"[ <length> | <percentage> ] ",@"0 ",@"all elements ",@"no ",@"width* of containing block ",@"visual");
			this.Add(@"rotation ",@"<angle> ",@"0 ",@"block-level elements, inline-table and inline-block ",@"no ",@"N/A ",@"visual");
			this.Add(@"rotation-point ",@"<bg-position> ",@"50% 50% ",@"block-level elements ",@"no ",@"Width and height of border box ",@"visual");
			this.Add(@"visibility ",@"visible | hidden | collapse ",@"visible ",@"all elements ",@"yes ",@"N/A ",@"visual");
			this.Add(@"width ",@"<length> | <percentage> | auto ",@"auto ",@"all elements but non-replaced inline elements, table rows, and row groups ",@"no ",@"refer to width of containing block ",@"visual ");

		}
	}
}
