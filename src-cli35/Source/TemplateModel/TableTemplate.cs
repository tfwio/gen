﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using Generator.Elements;
using Generator.Elements.Types;

namespace Generator.Core.Markup
{
	/// <summary>
	/// TableTemplates are transferable to and from DataRowView
	/// and DataRow nodes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Template items are ENTITY elements that can be converted to and from
	/// DataRow and DataRowView elements.  The decision to do this is rooted
	/// in how easy it is to work with Binding and DataSet in a WinForms UI.
	/// </para>
	/// <para>
	/// SQLite popped into the paradigm quite a while later and though it hasn't
	/// been implemented here, there is a project or two that uses a nearly parallel
	/// infrastructure for other things: MVC DataTable to HTML transitions, Blog
	/// listings, etc.
	/// </para>
	/// <para>
	/// I don't know why I put this here, this is another template system
	/// for a respective mvc model generated by this template system:
	/// <example><code>
	/// 	CREATE TABLE templates (
	/// 		id		INTEGER PRIMARY KEY AUTOINCREMENT,
	/// 		admin		INTEGER DEFAULT 0,
	/// 		title		VARCHAR DEFAULT NULL,
	/// 		container	VARCHAR DEFAULT NULL,
	/// 		row			VARCHAR DEFAULT NULL,
	/// 		note		VARCHAR DEFAULT NULL,
	/// 		table		VARCHAR DEFAULT NULL,
	/// 		fields		VARCHAR DEFAULT NULL
	/// 	);
	/// </code></example>
	/// </para>
	/// 
	/// <example>
	/// <para>
	/// The following example is a SQLite Query that generates tables that
	/// can contain the Database Configuration and TemplateCollection elements.
	/// </para>
	/// <para>
	/// Note that we've not implemented reading and writing templates to/from
	/// SQLite with an exception to exporting a SQLite3 Database containing
	/// the templates from a provided templated file.
	/// </para>
	/// <code>
	/// # generator-templates
	/// CREATE TABLE "generator-templates" (
	/// 	id INTEGER PRIMARY KEY AUTOINCREMENT, pid INTEGER,
	/// 	'crd'				DATETIME DEFAULT NULL,
	/// 	'mod'				DATETIME DEFAULT NULL,
	/// 	summary				LONGVARCHAR DEFAULT NULL,
	/// 	mmd					LONGVARCHAR DEFAULT NULL,
	/// 	'Name'				VARCHAR DEFAULT NULL,
	/// 	'Alias'				VARCHAR DEFAULT NULL,
	/// 	'Group'				VARCHAR DEFAULT NULL,
	/// 	'SyntaxLanguage'	VARCHAR DEFAULT NULL,
	/// 	'Tags'				VARCHAR DEFAULT NULL,
	/// 	'ItemsTemplate'		VARCHAR DEFAULT NULL,
	/// 	'ElementTemplate'	VARCHAR DEFAULT NULL,
	/// 	'file-project'		LONGVARCHAR DEFAULT NULL,
	/// 	'file-type'			LONGVARCHAR DEFAULT NULL,
	/// 	'file-name'			LONGVARCHAR DEFAULT NULL
	/// );
	/// 
	/// /* templates*/
	/// CREATE TABLE "templates" (
	/// 	id			INTEGER PRIMARY KEY AUTOINCREMENT,
	/// 	admin		INTEGER DEFAULT 0,
	/// 	title		VARCHAR DEFAULT NULL,
	/// 	container	VARCHAR DEFAULT NULL,
	/// 	[grouphead]	VARCHAR DEFAULT NULL,
	/// 	[groupfoot]	VARCHAR DEFAULT NULL,
	/// 	[head]		VARCHAR DEFAULT NULL,
	/// 	[foot]		VARCHAR DEFAULT NULL,
	/// 	'row'		VARCHAR DEFAULT NULL,
	/// 	'note'		VARCHAR DEFAULT NULL,
	/// 	'table'		VARCHAR DEFAULT NULL,
	/// 	'fields'	VARCHAR DEFAULT NULL
	/// );
	/// 
	/// /*gen-data-db*/
	/// CREATE TABLE "gen-data-db" (
	/// 	id					INTEGER PRIMARY KEY AUTOINCREMENT,
	/// 	pid					INTEGER,
	/// 	'crd'				DATETIME DEFAULT NULL,
	/// 	'mod'				DATETIME DEFAULT NULL,
	/// 	summary				LONGVARCHAR DEFAULT NULL,
	/// 	mmd					LONGVARCHAR DEFAULT NULL,
	/// 	'Name'				VARCHAR DEFAULT NULL,
	/// 	'PrimaryKey'		VARCHAR DEFAULT NULL,
	/// 	'ConnectionType'	VARCHAR DEFAULT NULL
	/// );
	/// </code></example>
	/// <para>
	/// Note: There are some System.Windows.Forms based methods in
	/// here that should be moved.
	/// </para>
	/// </remarks>
	public sealed class TableTemplate : MarkupTemplate<TableElement>
	{
		#region Props
		string className,syntaxLanguage;
		/// <summary>
		/// The name that would be used (if code is designates as a class object construct)
		/// </summary>
		[XmlAttribute,Category(groupAssembly)]
		public string ClassName { get { return className; } set { className = value; } }

		/// <summary>
		/// This can be one of a few things.
		/// <para>
		/// In the future, it is expected that currently we're supporting
		/// .Net NativeTypes through the NativeType enumeration provided
		/// in the System library.
		/// </para>
		/// <para>
		/// -- additional language support has been added to Adobe Flex Native Types
		/// <para>(TypeCode)</para>
		/// <para>(System.Cor3.Data.Map.Types.NativeTypes)</para>
		/// <para>(System.Cor3.Data.Map.Types.FlashNativeTypes)</para>
		/// </para>
		/// </summary>
		[XmlAttribute, Category(groupTemplate)] public string SyntaxLanguage { get { return syntaxLanguage; } set { syntaxLanguage = value; } }
		
		#endregion

		// .ctor
		public TableTemplate() : this(new TableElement())
		{
		}
		public TableTemplate(DataRow row) : this(new TableElement())
		{
			FromRowValues(row);
		}
		public TableTemplate(TableElement value) : base(value)
		{
		}

		#region DataRow Implementation (Template From DataRowView)
		protected override void FromRowValues(DataRowView row)
		{
			FromRowValues(row.Row);
		}
		protected override void FromRowValues(DataRow row)
		{
			base.FromRowValues(row);
			this.SyntaxLanguage =  RowValue<string>(row,"SyntaxLanguage");
			this.ClassName = RowValue<string>(row,"ClassName");
			this.Tags = RowValue<string>(row,"Tags");
			this.ItemsTemplate =  RowValue<string>(row,res.itmTpl);
			this.ElementTemplate =  RowValue<string>(row,res.elmTpl);
		}
		protected override void ToRow(DataRowView row)
		{
			base.ToRow(row);
			row["ClassName"] = this.ClassName;
			row[res.itmTpl] = this.ItemsTemplate;
			row["Tags"] = this.Tags;
			row["SyntaxLanguage"] = this.SyntaxLanguage;
			row[res.elmTpl] = this.ElementTemplate;
			row[res.itmTpl] = this.ItemsTemplate;
		}
		#endregion

		#region CheckInput

		/// <summary>
		/// Provide a list of references for this template.
		/// </summary>
		/// <remarks>
		/// This method is known to be used in the TemplateReferenceUtil class,
		/// for counting the number of matches in a specific template.
		/// </remarks>
		/// <returns>a List of Tags (and File/Directory tag) matches.</returns>
		public List<QuickMatch> GetReferences()
		{
			Logger.LogY("table-template", "Checking template: ‘{0}’", this.Alias);
			List<QuickMatch> matches = new List<QuickMatch>();
			// note that table template contains field templates and their contained references.
			// UNDONE: Itentify TableTemplateCdf
			MatchCollection mc = TemplateReferenceUtil.ListTagsAndFiles(this.ElementTemplate);
			if (mc==null) return matches;
			foreach (Match match in mc)
			{
				matches.Add(
					new QuickMatch(
						match.Groups[0].Value,
						match.Groups[1].Value,
						match.Groups[2].Value)
				);
			}
			return matches;
		}
		#endregion

	}
}
