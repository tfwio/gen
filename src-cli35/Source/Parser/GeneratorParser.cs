using System;
using System.ComponentModel;

namespace Generator.Parser
{
	public class GeneratorParser : BackgroundWorker
	{
		public string ParserResult { get;set; }
		public Generator.Export.Intrinsic.IDbConfiguration4 Configuration { get;set; }
		public Action<string> ResultAction { get;set; }
		
		public void Prepare(Generator.Export.Intrinsic.IDbConfiguration4 configuration)
		{
			Configuration = configuration;
		}
		
		protected override void OnDoWork(DoWorkEventArgs e)
		{
			base.OnDoWork(e);
			ParserResult = TemplateFactory.Generate(Configuration);
		}
		
		protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
		{
			base.OnRunWorkerCompleted(e);
			if (ResultAction!=null) ResultAction.Invoke(ParserResult);
			this.Configuration = null;
			this.ParserResult = null;
			GC.Collect();
		}
		
	}
}
