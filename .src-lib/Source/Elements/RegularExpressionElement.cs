/**
 * oIo * 2/18/2011 4:32 AM
 **/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

#endregion
namespace Generator.Elements
{
	/// <summary>
	/// Stores a hierarchical set of regular expressions.
	/// <para>See <see cref="IRegularExpressionElement" /> for more info.</para>
	/// <para>Note: Not implemented.</para>
	/// </summary>
	/// <remarks>
	/// I suppose that it would be worthy to point out that this is not in any way implement
	/// and also (along with IRegularExpressionElement), the impelementation here
	/// neglects Colors, Parent expression an additional concepts.
	/// <para>A prospective implementation would take these things into account.</para>
	/// <para>Implementations would derive from the regular-expression element to 
	/// assign colors and such.</para>
	/// </remarks>
	public class RegularExpressionElement
	{
		internal const bool def_ignroe_state = false;
		internal const string express = @"($\((?<env>([A-Za-z0-9]+))\))|(?<env>%\w[a-zA-Z0-9]+)|((\($(?<env>[A-Za-z0-9]+)\))";
		internal const string default_output = @"(?<{0}>{1})";
		bool isIgnored;
		public bool IsIgnored {
			get { return isIgnored; }
			set { isIgnored = value; }
		}
		string name;
		public string Name {
			get { return name; }
			set { name = value; }
		}
		string alias;
		public string Alias {
			get { return alias; }
			set { alias = value; }
		}
		string group;
		public string Group {
			get { return group; }
			set { group = value; }
		}
		string _value;
		public string Value {
			get { return _value; }
			set { _value = value; }
		}
//		string expression;
//		public string Expression {
//			get { return expression; }
//			set { expression = value; }
//		}
		
		[XmlIgnore,Browsable(false)] public string Expression
		{
			get
			{
				if (isIgnored) return string.Empty;
				string vv = Value;
				if (InnerList.Count>0)
				{
					foreach (RegularExpressionElement rx in InnerList)
					{
						if (rx.isIgnored) continue;
						if (rx.ToString()==string.Empty) {}
						else vv += "|"+rx.ToString();
					}
				}
				return vv.TrimStart('|');
			}
		}
		public object parent;
		
		
		[XmlIgnoreAttribute]
		List<IRegularExpressionElement> innerList;
		[XmlIgnoreAttribute]
		public List<IRegularExpressionElement> InnerList { get { return innerList; }  set { innerList=value; } }
		
		/// <summary/>
		[XmlIgnore,Browsable(false)] internal DICT<string,RegularExpressionElement> collection;
		/// <summary/>
		[XmlIgnore,Browsable(false)] public DICT<string,RegularExpressionElement> Collection
		{
			get
			{
				if (collection==null) collection = new DICT<string, RegularExpressionElement>();
				foreach (RegularExpressionElement item in InnerList)
				{
					collection.Add(item.Alias,item);
				}
				return collection;
			}
		}
		#region ' ToString '
		/// <summary/>
		public override string ToString() {
			string sam = Value;
			if (Alias!=null&Alias!=string.Empty) sam = string.Format(default_output,Alias,sam);
			return sam;
		}
		#endregion
	}
	
	/// <summary>
	/// Holy crap I had actually implemented this guy!
	/// </summary>
	/// <remarks>
	/// This implementation is actually kind of a dinosaur first implemented in
	/// a test to try and colourize Scintilla/SciteNET by usage of regular expressions
	/// which was part of a project that I had named efx (environment framework).
	/// <para>
	/// The <see cref="RegularExpressionElement" /> class seems to implement a dictionary
	/// items that are catered to a readable list and is hierarchical in nature.
	/// </para>
	/// <para>
	/// As I remember, specific efx.elements housed a mechanism which allowed for 
	/// a specified set of ELEMENT.Child entities via a reflection <see cref="Type" />
	/// provided in an array ow allowed types.
	/// </para>
	/// </remarks>
	public interface IRegularExpressionElement
	{
		/*
		[XmlIgnore,Browsable(false)] public string Expression
		{
			get
			{
				if (IsIgnored) return string.Empty;
				string vv = Value;
				if (InnerList.Count>0)
				{
					foreach (RegEx rx in InnerList)
					{
						if (rx.IsIgnored) continue;
						if (rx.ToString()==string.Empty) {}
						else vv += "|"+rx.ToString();
					}
				}
				return vv.TrimStart('|');
			}
			
			• in the above example, we need a ToString() override which
			  seemingly returns our expression.
		
  			• I belive that this expression seeks through it's
		  	  children for additional queries.
		
		const string express = @"($\((?<env>([A-Za-z0-9]+))\))|(?<env>%\w[a-zA-Z0-9]+)|((\($(?<env>[A-Za-z0-9]+)\))";
		*/
//		internal const bool def_ignroe_state = false;
		string Name {get;set;}
		string Alias {get;set;}
		string Group {get;set;}
		//string Value {get;set;} this is on the private impl end
		string Expression {get;set;}
		// Parenting Object
//		string Parent {get;set;}
		// Background/Foreground
//		string BackColor {get;set;}
//		string ForeColor {get;set;}
		// this list type should be general, like a hash table
		// rexlist bob; bob[i] = 'get { return collection[i]; }'
		List<IRegularExpressionElement> InnerList {get;set;}
		DICT<string,IRegularExpressionElement> Collection {get;set;}
		string Replace(string input, string replace);
//		return System.Text.RegularExpressions.Regex.Replace(input,express,replace,Text.REGEX.regex_defaults);
		MatchCollection Matches(string input, string pattern);
//		return System.Text.RegularExpressions.Regex.Matches(input,pattern,Text.REGEX.regex_defaults);
		// • MenuItems
		// • ValueFiltered (we don't know what this is)
		// • Converters for Setting and Getting string values from others.
//		#region ' ExpressionCollection '
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] internal DICT<string,RegEx> collection;
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] public DICT<string,RegEx> Collection
//		{
//			get
//			{
//				if (collection==null) collection = new DICT<string, RegEx>();
//				foreach (RegEx item in InnerList)
//				{
//					collection.Add(item.Alias,item);
//				}
//				return collection;
//			}
//		}
//		#endregion
//		#region ' ToString '
//		/// <summary/>
//		public override string ToString() {
//			string sam = Value;
//			if (Alias!=null&Alias!=string.Empty) sam = string.Format(default_output,Alias,sam);
//			return sam;
//		}
//		#endregion
	}
	
	#region Impulse
//	/// <summary/>
//	[Serializable(),] public class RegEx : RegEx_TheLastOneAtLeast
//	{
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] public string Expression
//		{
//			get
//			{
//				if (IsIgnored) return string.Empty;
//				string vv = Value;
//				if (InnerList.Count>0)
//				{
//					foreach (RegEx rx in InnerList)
//					{
//						if (rx.IsIgnored) continue;
//						if (rx.ToString()==string.Empty) {}
//						else vv += "|"+rx.ToString();
//					}
//				}
//				return vv.TrimStart('|');
//			}
//		}
//	}
////	
//	/// <summary/>
//	[Serializable(),] public class RegEx_TheLastOneAtLeast : RegExCeed
//	{
//		internal const bool def_ignroe_state = false;
//		internal const string express = @"($\((?<env>([A-Za-z0-9]+))\))|(?<env>%\w[a-zA-Z0-9]+)|((\($(?<env>[A-Za-z0-9]+)\))";
//
//		/// <summary/>
//		public MatchCollection Query(string input)
//		{
//			return System.Text.RegularExpressions.Regex.Matches(input,express,Text.REGEX.regex_defaults);
//		}
//		/// <summary/>
//		public string Replace(string input,string replace)
//		{
//			return System.Text.RegularExpressions.Regex.Replace(input,express,replace,Text.REGEX.regex_defaults);
//		}
//		/// <summary/>
//		public MatchCollection CompileExpression(string input,string pattern)
//		{
//			return System.Text.RegularExpressions.Regex.Matches(input,pattern,Text.REGEX.regex_defaults);
//		}
//
//		/// <summary/>
//		[XmlIgnore,Category("Expression")] public bool IsIgnored { get { return S2Bool(this["Ignore"].ToString()); } set { this["Ignore"]=value; } }
//		/// <summary/>
//		[XmlAttribute,Browsable(false)] public string Ignore {
//			get { return (IsIgnored==false)?null:this["Ignore"].ToString(); } set { IsIgnored = S2Bool(value); }
//		}
//
//		bool S2Bool(string input) { return bool.Parse(input); }
//		/// <summary/>
//		public RegEx_TheLastOneAtLeast() : this(new object[]{"BackColor","ForeColor","Parent","Group","Evaluated","Ignore"},Color.Transparent,Color.Black,null,string.Empty,string.Empty,def_ignroe_state) {}
//		/// <summary/>
//		public RegEx_TheLastOneAtLeast(RegEx parent) : this(new object[]{"BackColor","ForeColor","Parent","Group","Evaluated","Ignore"},Color.Transparent,Color.Black,null,string.Empty,string.Empty,def_ignroe_state) {}
//		/// <summary/>
//		public RegEx_TheLastOneAtLeast(Hashtable htable) : base(htable) { }
//		/// <summary/>
//		public RegEx_TheLastOneAtLeast(object[] names, params object[] values) : base(names,values) {  }
//		/// <summary/>
//		public RegEx_TheLastOneAtLeast(object K, object V) : base(K,V) {  }
//	}
//	/// <summary/>
//	[Serializable(),] public class RegExCeed : ConstRegularExpression, IRegX
//	{
//		/// <summary/>
//		[XmlElement,Browsable(false)] virtual public string ValueFiltered
//		{
//			get
//			{
//				return Value;
//			}
//		}
//		#region ' MenuItems '
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] public override ToolStripItem MenuItem
//		{
//			get
//			{
//				ToolStripMenuItem tsmi = new ToolStripMenuItem(RealName);
//				tsmi.Tag = this;
//				tsmi.Image = famfam_silky.tag_blue;
//				tsmi.Click += PlugIns.SciPlugin.Main.RegExDlg.eRexQuery;
//				tsmi.DropDownItems.AddRange(MenuItemCollection);
//				return tsmi;
//			}
//		}
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] virtual public ToolStripItem[] MenuItemCollection
//		{
//			get
//			{
//				List<ToolStripItem> list = new List<ToolStripItem>();
//				foreach (RegEx rx in InnerList) list.Add(rx.MenuItem);
//				return list.ToArray();
//			}
//		}
//		#endregion
//		#region ' ExpressionCollection '
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] internal DICT<string,RegEx> collection;
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] public DICT<string,RegEx> Collection
//		{
//			get
//			{
//				if (collection==null) collection = new DICT<string, RegEx>();
//				foreach (RegEx item in InnerList)
//				{
//					collection.Add(item.Alias,item);
//				}
//				return collection;
//			}
//		}
//		#endregion
//		/// <summary/>
//		public RegExCeed() : this(new object[]{"BackColor","ForeColor","Parent","Group","Evaluated"},Color.Transparent,Color.Black,null,string.Empty,string.Empty) {}
//		/// <summary/>
//		public RegExCeed(RegEx parent) : this(new object[]{"BackColor","ForeColor","Parent","Group","Evaluated"},Color.Transparent,Color.Black,parent,string.Empty,string.Empty) {}
//		/// <summary/>
//		public RegExCeed(Hashtable htable) : base(htable) { }
//		/// <summary/>
//		public RegExCeed(object[] names, params object[] values) : base(names,values) {  }
//		/// <summary/>
//		public RegExCeed(object K, object V) : base(K,V) {  }
//	}
//	public class ConstRegularExpression : ConstSimpleRegularExpression
//	{
//		#region ' SubItems '
//		/// <summary/>
//		[XmlIgnore] public List<RegEx> InnerList = new List<RegEx>();
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] public IRegX[] SubItems { get { return (IRegX[])InnerList.ToArray(); } }
//		/// <summary/>
//		[XmlElement(typeof(RegEx[]),IsNullable=true)] public RegEx[] TreeEx { get { return InnerList.ToArray(); } set { if(value!=null) InnerList = new List<RegEx>(value); } }
//		#endregion
//		/// <summary/>
//		public ConstRegularExpression() : base(new object[]{"BackColor","ForeColor","Parent","Group","Evaluated"},Color.Transparent,Color.Black,null,string.Empty,string.Empty) {}
//		/// <summary/>
//		public ConstRegularExpression(RegEx parent) : this(new object[]{"BackColor","ForeColor","Parent","Group","Evaluated"},Color.Transparent,Color.Black,parent,string.Empty,string.Empty) {}
//		/// <summary/>
//		public ConstRegularExpression(Hashtable htable) : base(htable) { }
//		/// <summary/>
//		public ConstRegularExpression(object[] names, params object[] values) : base(names,values) {  }
//		/// <summary/>
//		public ConstRegularExpression(object K, object V) : base(K,V) { }
//		#region ' ToTreeNode '
//		/// <summary/>
//		public override System.Windows.Forms.TreeNode CreateTreeNode()
//		{
//			TreeNode tn = base.CreateTreeNode();
//			tn.Text = (Name==string.Empty)?Alias:Name;
//			tn.ToolTipText = string.Format("{0}: {1}",InnerList.Count,ToString());
//			//tn.ContextMenuStrip.Items.Add(ControlUtil.TSItem());
//			//tn.ContextMenuStrip.Items.Add(ControlUtil.TSItem("hymmmm..."));
//			foreach (RegEx rx in InnerList) tn.Nodes.Add(rx.CreateTreeNode());
//			return tn;
//		}
//		#endregion
//		#region ' ExpressionCollection '
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] internal DICT<string,RegEx> collection;
//		/// <summary/>
//		[XmlIgnore,Browsable(false)] public DICT<string,RegEx> Collection
//		{
//			get
//			{
//				if (collection==null) collection = new DICT<string, RegEx>();
//				foreach (RegEx item in InnerList)
//				{
//					collection.Add(item.Alias,item);
//				}
//				return collection;
//			}
//		}
//		#endregion
//		#region ' ToString '
//		/// <summary/>
//		public override string ToString() {
//			string sam = Value;
//			if (Alias!=null&Alias!=string.Empty) sam = string.Format(default_output,Alias,sam);
//			return sam;
//		}
//		#endregion
//	}
//	/// <summary/>
//	public class ConstSimpleRegularExpression : ConstantColorValue
//	{
//		internal const string default_output = @"(?<{0}>{1})";
//
//		#region ' Parent '
//		/// <summary/>
//		[XmlAttribute,Browsable(false)]
//		public string Parent { get { return (this["Parent"]==null)?null:(string)this["Parent"]; } set { this["Parent"]=(string)value; } }
//		#endregion
//		#region ' Group '
//		/// <summary/>
//		[XmlAttribute,Description("maybe you can think of something to put here?"),Category("Expression")]
//		public string Group { get { return (this["Group"]==null)?null:(string)this["Group"]; } set { this["Group"]=(string)value; } }
//		#endregion
//		#region ' Value (editor:UIRegExLooker) '
//		/// <summary/>
//		[XmlAttribute,EditorAttribute(typeof(UIRegExLooker),typeof(UITypeEditor)),Category("Expression")]
//		public override string Value { get { return base.Value; } set { base.Value = value; } }
//		#endregion
//		#region ' Alias '
//		/// <summary/>
//		[XmlAttribute,Category("Expression")]
//		public override string Alias { get { return base.Alias; } set { base.Alias = value; } }
//		#endregion
//
//		/// <summary/>
//		public ConstSimpleRegularExpression() : this(new object[]{"BackColor","ForeColor","Parent","Group"},Color.Transparent,Color.Black,null,string.Empty,string.Empty) {}
//		/// <summary/>
//		public ConstSimpleRegularExpression(RegEx parent) : this(new object[]{"BackColor","ForeColor","Parent","Group"},Color.Transparent,Color.Black,parent,string.Empty) {}
//		/// <summary/>
//		public ConstSimpleRegularExpression(Hashtable htable) : base(htable) { }
//		/// <summary/>
//		public ConstSimpleRegularExpression(object[] names, params object[] values) : base(names,values) {  }
//		/// <summary/>
//		public ConstSimpleRegularExpression(object K, object V) : base(K,V) { }
//
//	}
#endregion
}
