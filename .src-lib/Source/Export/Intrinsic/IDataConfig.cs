/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */
using System;
using Generator.Elements;
using Generator.Core.Markup;
using SqlDbType = System.Data.SqlDbType;

/*
 * This is designed to be imported into another project as an include.
 */
namespace Generator.Export.Intrinsic
{
	public interface IDataConfig
	{
		DatabaseCollection Databases { get; set; }
		DatabaseElement Database { get; set; }
		TemplateCollection Templates { get;set; }
		TableElement Table { get;set; }
		TableTemplate Template { get;set; }
		bool HasTemplate(string alias);
	}
}
