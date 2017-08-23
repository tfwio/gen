/* oio : 12/27/2013 2:03 AM */
using System;
using System.Diagnostics;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;

namespace GeneratorTool.Views
{
	/// <summary>
	/// Interaction logic for MuiModern.xaml
	/// </summary>
	public partial class MuiModern : ModernWindow
	{
		public ModernFrame Frame { get;set; }
		// #a6cd5f
		public MuiModern()
		{
			//FontFamily="/assets/typo/#Roboto 900"
			InitializeComponent();
			LocalMuiSettings.Assert();
//			this.GetValue(DependencyObject
			//System.Diagnostics.Debug.WriteLine("We have: {0}",this.);
			mainLinks.Links.Add( new Link(){ DisplayName="GENERATOR", Source = new Uri("/generator",UriKind.RelativeOrAbsolute) });
			mainLinks.Links.Add( new Link(){ DisplayName="TEMPLATE-WRITER", Source = new Uri("/writerTemplate",UriKind.RelativeOrAbsolute) });
			mainLinks.Links.Add( new Link(){ DisplayName="SQLite Tool", Source = new Uri("/sqlTool",UriKind.RelativeOrAbsolute) });
//			mainLinks.Links.Add( new Link(){ DisplayName="TEMPLATE", Source = new Uri("/2",UriKind.RelativeOrAbsolute), });
//			mainLinks.Links.Add( new Link(){ DisplayName="3", Source = new Uri("/3",UriKind.RelativeOrAbsolute), });
//			mainLinks.Links.Add( new Link(){ DisplayName="4", Source = new Uri("/4",UriKind.RelativeOrAbsolute), });
			
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Frame = GetTemplateChild("ContentFrame") as ModernFrame;
			Debug.WriteLine("FRAME: {0}",Frame);
			
//			Frame.Navigating += (sender,e) => { if (e.Frame. };
		}
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
		}
	}
}