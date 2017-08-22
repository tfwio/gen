// Copyright (c) 2009 Daniel Grunwald
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using Generator.Core.Parser;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace AvalonEdit.Sample
{
	/// <summary>
	/// Implements AvalonEdit ICompletionData interface to provide the entries in the completion drop down.
	/// </summary>
	public class MyCompletionData : ICompletionData
	{
		public const string UriImageGray		= "pack://application:,,,/System.Cor3.Stock;component/images/boxgray.png";
		public const string UriImageboxG		= "pack://application:,,,/System.Cor3.Stock;component/images/boxG.png";
		public const string UriImageboxB0		= "pack://application:,,,/System.Cor3.Stock;component/images/boxB0.png";
		public const string UriImageboxB012		= "pack://application:,,,/System.Cor3.Stock;component/images/boxB012.png";
		public const string UriImageboxGold		= "pack://application:,,,/System.Cor3.Stock;component/images/boxGold.png";
		public const string UriImageboxM		= "pack://application:,,,/System.Cor3.Stock;component/images/boxM.png";
		public const string UriImageboxOrange	= "pack://application:,,,/System.Cor3.Stock;component/images/boxOrange.png";
		public const string UriImageboxR		= "pack://application:,,,/System.Cor3.Stock;component/images/boxR.png";
		public const string UriImageFieldLighter= "pack://application:,,,/System.Cor3.Stock;component/images/field-lighter.png";
		
		string imageuri = UriImageboxM;
		public string ImageUri {
			get {
				return imageuri;
			}
			set {
				imageuri=value;
				image = new BitmapImage(new Uri(imageuri,UriKind.RelativeOrAbsolute)); 
			}
		}
		
		
		static public string UriImageFromNsType(NsTypes t){
			if (t== NsTypes.AdapterTypes) return UriImageboxG;
			else if (t== NsTypes.DatabaseTypes) return UriImageboxB012;
			else if (t== NsTypes.FieldTypes) return UriImageFieldLighter;
			else if (t== NsTypes.Global) return UriImageboxM;
			else if (t== NsTypes.TableTypes) return UriImageboxOrange;
			else  return UriImageGray;
		}
		
		public MyCompletionData(string text, string imgUri)
		{
			this.Text = text;
			this.ImageUri = imgUri;
		}
		public MyCompletionData(string text, NsTypes imgUri)
		{
			this.Text = text;
			this.ImageUri = UriImageFromNsType(imgUri);
		}
//		public const string UriImage = "/images/.png";
//		public const string UriImage = "/images/.png";
		
		ImageSource image = null;
		public ImageSource Image { get { return image; } set { image=value; } }

		public string Text { get; private set; }
		
		// Use this property if you want to show a fancy UIElement in the drop down list.
		public object Content {
			get { return this.Text; }
		}
		
		public object Description {
			get { return "Description for " + this.Text; }
		}
		
		public double Priority { get { return 0; } }
		
		public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{
			SimpleSegment sgm = new SimpleSegment(completionSegment.Offset,completionSegment.Length);
			sgm.Offset-=1; sgm.Length++;
			textArea.Document.Replace(sgm, this.Text);
		}
	}

}
