title: Attribute-Insight-Collection
meta: encoding=UTF8

# CSS Dictionary Resources

>I certainly wasn't about to go and type out each of the 'Insight' resources, so...
This file points out how the AttributeInsightCollection items were generated of CSS documentation resources found on the internet.

>* At this point a significant point of interest would be a scheme to provide values concurrent of [CSS Values and Units Module Level 3](http://www.w3.org/TR/css3-values/).
>* Once again, the aim is to remain as simple as possible.

[**TOC**]

* [Provided Table Columns](#providedtablecolumns)
* [w3.org](#w3.orgreferencedpropertytables) (referenced property tables)
* [Regular Expressions](#regularexpressions)
* [`AttributeInsight` and `AttributeInsightCollection`](#attributeinsightandattributeinsightcollection)
    * [`AttributeInsight` (class)](#attributeinsightclass)
    * [`AttributeInsightCollection` (class)](#attributeinsightcollectionclass)

## Files

The following files were copy/pasted from the respective working drafts or public references on 2012-04-26, using Mozilla Firefox which provided a tab-delimited section of content.

* css3-animations.text
* css3-background.text
* css21props.text

> It woud perhaps prove more sufficiant to use some remoting or Client-Http methods from C# in conjunction with some sort of parser to strip more information out of the content in the future.
This works for now however.

## W3.ORG (Referenced Property Tables)

* [CSS 2.1: Property Index](http://www.w3.org/TR/CSS21/propidx.html)
* [CSS Animations](http://www.w3.org/TR/css3-animations/#property-index)
* [CSS Backgrounds and Borders Module Level 3](http://www.w3.org/TR/css3-background/#property-index)

## Provided Table Columns

1. Name
2. Values
3. Initial Value
4. Applies To (Default: All)
5. Inherited?
6. Percentages (Default: N/A)
7. Media groups

## Regular Expressions

From that point they were encapsulated via conversion from a regular expression replace:

regular expression:

	^([^\t])\t([^\t])\t([^\t])\t([^\t])\t([^\t])\t([^\t])\t([^\t])$

regular expression-replace:

	this.Add(@"\0",@"\1",@"\2",@"\3",@"\4",@"\5",@"\6");

or perhaps an alternative:

	this.Add(@"$0",@"$1",@"$2",@"$3",@"$4",@"$5",@"$6");

## `AttributeInsight` and `AttributeInsightCollection`

`AttributeInsight` is the name of our encapsulating class.  `AttributeInsightCollection` encapsulates `Collection<AttributeInsight>`.
Each result is added to a `Collection<T>` where `T` is our type containing [the columns](#providedtablecolumns) mentioned above.
Some of the content is repeated (with different values such the property-index table: "CSS Backgrounds and Borders Module Level 3".
Each 'node' is filtered as it is added into our collection (I've termed AttributeInsight)

### `AttributeInsight` (class)

<pre style="brush: cs;">
	/// <summary>
	/// Attribute Insight values derive from CSS Documentation Properties tables.
	/// </summary>
	public class AttributeInsight
	{
		public string Name { get; set; }
		public string Values { get; set; }
		public string InitialValue { get; set; }
		public string AppliesTo { get; set; }
		public string Inherited { get; set; }
		public string Percentages { get; set; }
		public string MediaGroups { get; set; }

		public AttributeInsight(string qname, string values, string initialValue, string appliesTo, string inherited, string percentages, string mediaGroups)
		{
			this.Name = qname;
			this.Values = values;
			this.InitialValue = initialValue;
			this.AppliesTo = appliesTo;
			this.Inherited = inherited;
			this.Percentages = percentages;
			this.MediaGroups = mediaGroups;
		}
	}
</pre>

### `AttributeInsightCollection` (class)

> Note the filtering within the `Add(string,string,string,string,string,string,string)` method will probably be elaborated to filter or enable some sort of recognition on value-types.

> Perhaps this system may be thrown away all-together in the prospect of implementing a more thorough reference materials.

<pre style="brush: csharp;">
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
		
		public AttributeInsightCollection()
		{
			...
			this.Add(...);
			...
		}
	}
</pre>