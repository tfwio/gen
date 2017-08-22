/*
 * 

 * oio * 04/22/2012 * 14:16
 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace System.Cor3.Parsers.Tools
{

	public class SimpleTemplateConverter : INotifyPropertyChanged
	{
		
		#region Properties
		
		/// <summary>
		/// The Path which is used to start looking for files with the extensions provided by 'Extensions'
		/// </summary>
		
		public string PathStart {
			get { return pathStart; }
			set {
				pathStart = value;
				OnPropertyChanged("PathStart");
			}
		} string pathStart;
		
		/// <summary>
		/// The File-Extensions to search for.
		/// <para>Extensions are comma delimited and contain a period with wildcard characters such as: '*.tpl'</para>
		/// </summary>
		
		public string Extensions {
			get { return extensions; }
			set { extensions = value; OnPropertyChanged("Extensions"); }
		} string extensions;
		
		string[] ExtensionsArray { get; set; }
		
		#region Ignored
		/// <summary>
		/// Determines weather or not to use OutputPath
		/// </summary>
		public bool OutputPathIsSameAsInput { get; set; }
		#endregion
		/// <summary>
		/// If (bool) SameAsInput (always true), the file written is
		/// always contained in the same directory as the template file.
		/// </summary>
		public string OutputPath { get;set; }
		
		bool recurseDirectories;
		
		public bool RecurseDirectories {
			get { return recurseDirectories; }
			set { recurseDirectories = value; }
		}
		
		List<SimpleTemplateItem> items;
		
		public List<SimpleTemplateItem> Items {
			get { return items; }
			set { items = value; OnPropertyChanged("Items"); }
		}
		
		public System.Text.Encoding FileEncoding = Encoding.UTF8;
		
		string textToReplace;
		
		public string TextToReplace {
			get { return textToReplace; }
			set { textToReplace = value; OnPropertyChanged("TextToReplace"); }
		}
		string textReplacement;
		
		public string TextReplacement {
			get { return textReplacement; }
			set { textReplacement = value; OnPropertyChanged("TextReplacement"); }
		}
		
		
		#endregion
		
		#region Methods
		/// <summary>
		/// Refreshes 'ExtensionsArray' based on the string 'Extensions'.
		/// </summary>
		/// <returns>ExtensionsArray</returns>
		string[] RefreshExtensions()
		{
			string[] exts = Extensions.Split(',');
			int i = 0; for ( ; i < exts.Length; i++ )
			{
				exts[i] = exts[i].Trim();
			}
			return ExtensionsArray = exts;
		}
		#endregion
		
		#region Actions
		void ActionFindItems()
		{
			this.OnFindStarted(null);
			System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(this.PathStart);
			this.RefreshExtensions();
			this.Items = new List<SimpleTemplateItem>();
			foreach (string ext in ExtensionsArray)
				foreach (FileInfo file in directory.GetFiles(ext, this.RecurseDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
					Items.Add(new SimpleTemplateItem(file.FullName));
		}
		#endregion
		
		public event EventHandler FindStarted;
		protected virtual void OnFindStarted(EventArgs e)
		{
			if (this.FindStarted != null) {
				this.FindStarted(this, e);
			}
		}
		
		public event EventHandler ReplaceStarted;
		protected virtual void OnReplaceStarted(EventArgs e)
		{
			if (this.ReplaceStarted != null) {
				this.ReplaceStarted(this, e);
			}
		}
		
		#region Event: CountChanged, OnCountChanged
		public event EventHandler<ItemCountChanged> CountChanged;
		protected virtual void OnCountChanged(int max, int value) { OnCountChanged(new ItemCountChanged(max,value)); }
		protected virtual void OnCountChanged(ItemCountChanged e) { if (CountChanged != null) { CountChanged(this, e); } }
		#endregion
		
		public void FindItems()
		{
			this.ActionFindItems();
		}
		public void ClearItems()
		{
			this.items.Clear();
		}

		/// <summary>
		/// Starts writing files!
		/// <para>with a Yield per execution.</para>
		/// </summary>
		public void WriteReplacementsAction()
		{
			if ( (Items==null) || Items.Count==0) return;
			int i = 0, j = Items.Count;
			foreach (SimpleTemplateItem item in Items)
			{
				lock (item)
				{
					string outputtext = item.Replace(TextToReplace, TextReplacement, FileEncoding, false);
					File.WriteAllText(item.FileOutput,outputtext,FileEncoding);
				}
				this.OnCountChanged(i,j);
//				System.Threading.Thread.Yield();
			}
		}

		public SimpleTemplateConverter()
		{
			this.OutputPath = null;
			this.RecurseDirectories = true;
			this.OutputPathIsSameAsInput = true;
		}
		public SimpleTemplateConverter(string startpath, string extensions) : this()
		{
			this.PathStart = startpath;
			this.Extensions = extensions;
		}
		
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, e);
			}
		}
	}
}
