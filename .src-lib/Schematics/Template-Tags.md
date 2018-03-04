[28591e4be-System.FieldStringExtension.cs]: https://tfw.io/dot.io/gen.lib/src/commit/28591e4be068c7023cca5dd8f71ade2cdca14eed/.src-lib/cor3.data/Extensions/System.FieldStringExtension.cs
[master-System.FieldStringExtension.cs]: https://tfw.io/dot.io/gen.lib/commits/branch/master/.src-lib/cor3.data/Extensions/System.FieldStringExtension.cs
[TableElement.cs]:https://tfw.io/dot.io/gen.lib/src/commit/3392ab774300ceff3c1b6bfbde4b6b20157c935c/.src-lib/Source/Elements/TableElement.cs#L332-L440
# Table and Field Template Tags

> DOCUMENT STATUS: DRAFT
> 
> This page is a work in progress and may be behind current development phase.

From the source-code perspective, we have a static class (StrKeys) which presents const string 'keys' of each FieldElement value...  
https://tfw.io/dot.io/gen.lib/src/commit/3392ab774300ceff3c1b6bfbde4b6b20157c935c/.src-lib/Source/Parser/StrKeys.cs

## Generator Data-Configuration ➞ Template

There are two parts to each template: Table-Level and Field-Level.  The tags available to the Table-level are present within the field-level, and not the other way around within XML (*.xtpl).

The most important construct of this tool is the TableTemplate semantic which allows for `transclusion` of other templates as applied to your defined data-schematic.

## TableTemplate tag

```
$(TableTemplate,my.template.name[, FixedTable|$(TableName)][, TableName])
```

- Parameters
    - (1) Template-Name
    - (2) (optional) Table-Name
    - Any number of tables may be specified, or (rather) by supplying the `$(TableName)` tag rather then a fixed name such as `"my-tablename"`, you can enable the template to support multiple assertions.
    - as a rule of thumb, you should not use spaces or non-ascii characters within data-table or data-table-field names (of course).

The following example has the first parameter specifying the template being used.  Each tag following specifies the table being applied to the template.

```
$(TableTemplate: myTemplate, table1, table2, table3)
```

When the above template syntax is used, it may be applied to a template such as the one below:

```
// why doesn't this make any sense?
// please ignore this.  I think I should have put a actual template here.
$(TableTemplate: $(TableName))
```

When the template (output) is being generated, a database-name becomes the current context, and is applied to a given template:

```
// this is another unwritten template (please ignore it)
$(TableTemplate)
```

[28591e4be-System.FieldStringExtension.cs]  
[master-System.FieldStringExtension.cs]

todo: provide a link to `.src-lib/Schematics`

----

*don't trust the following link ;)*

The rest of the tags can be found in [FieldElement.cs](https://github.com/tfwio/cor3-gen/blob/master/source/GenaratorLib/Source/Elements/FieldElement.cs#L196-L396)#L196-L396

# Table of Tags

> note that some of this may be categorized improperly at the moment.
> 
> Some of the comma-delimited tags are (likely) missing
> Missing (special elements): `FieldValuesNKCdf`, `FieldValuesCdf`, `FieldValuesNK`, `FieldValues`

For the majority of tags, you can likely understand the meaning of the tag by the UI's data-table input page:

![](https://tfw.io/dot.io/gen.lib/raw/commit/447bfde4a9921773f95f45029c164787d155bc00/.src-lib/Schematics/data-field-input.png)

You can think of nearly every tag you can input into the above UI as a simple Dictionary entry with exception to how a few of the fields are going to be handled.

Thinking in to this project's scope, one can intuit that we simply need to convert fields pertaining to:

- **Native Type** — corresponds to the syntax-specific target programming language which is default to CSharp.  Its painful to do with the UI, but you can type anything you want in the Native Type field.
- **Data Type** — This corresponds to the target database technology and the types that pertain to that.
- **Default Value** can generally be used like just about any other field (except the numerical, boolean or read-only ones) and is just a hard-coded dictionary value that can be used in any way you choose.

Where `DefaultValue` will pertain to a give target usage.  Otherwise it would probably be wise to author into this application an appropriate `DefaultValue` field which would generally depend upon the target syntax language.  
This was written for primary use in CSharp and has been (made) useful for ActionScript (which is very-much similar to Java syntax) and has also been used to produce Javascript coherance of Database schematics.

## A note on Nullable types

DOTNET has its native types which this application tends to rely on for its default types.
Native types are generally structs (in c/c++) and include string, decimal, double, int (and variants
Int8, SInt8, Int16...), float (and variants).
Most languages will generally conform to these types or can easily utilize (wrap or create) new types
representing the above NativeTypes common to CSharp or DOTNET.
Recently we've added some tags to support Golang.

If you don't understand DOTNET's nullable types, any type that is **NOT NativeType** (base on a class or Object)
can be assigned a value of `null`.  Marking a type 'nullable' is mostly common to working with relational databases
and can go beyond the above rule by explicitly marking a given type (reference) such as `int? a = null`.  In DOTNET,
the `int? a = null` will create a Nullable Type and apply the value `DbNull.Value` to it (for null) and also allow
you to access a property such as `a.IsNullable` (bool).

The Golang types are very much similar to C/C++ native types in that we may use pointers, references and de-references.
The pointer references or Golang related tags may change in the future, however for now can be used
and pertain to fields marked `IsNullable` producing a native `*FieldName` pointer reference on the
tag for `$(NativeNullTypeGo)` as an example.  To access just a pointer in this case or address-of operator,
the same rules apply (has to be marked `IsNullable`) to `PKNativeNullValueGoAddress`, `PKNativeNullValueGoPointer`,
`NativeNullTypeGoAddress` and `NativeNullTypeGoPointer`.

> **Note that the Golang specific tags have not been mapped to all integer and floating-point native types.  We're just
> using them at this point for the mentioned int, int32, int64, float, float32 and float64 types.**


## Global Tags

| TAG | DESCRIPTION-1 | DESCRIPTION-2 |
| --- | ------------- | ------------- |
|Date|Current DateTime|Provides the date in a short format: `{yy/MM/dd}`|
|Time|Current DateTime|Provides time in a short format: `{hh:mm tt}`|
|DateTime|Current DateTime|Provides the full DateTime in the format: `{yy/MM/dd hh:mm tt} or maybe {yyMMdd-hhmmss}`|

**NOTE**

> See [TemplateFactory.cs](https://github.com/tfwio/cor3-gen/blob/90c5a763a2610c8e7c46b56be52c26c683c33520/source/GenaratorLib/Source/Parser/ParserHelper.cs#L50-L53) for the source of these.
> 
> **FieldIndex** — (integer) zero-inclusive field-index value; EG: Field number.
> 
> **IsPrimary** — `true` if the field is the primary key, otherwise `false`.
> 
> **PrimaryKey** — primary-key name

## TableElement Tags

Note that the generator expects a defined PrimaryKey and will crash the application if no PrimaryKey is defined! (at least I have a vague reccolection of this being the case)

This are typically the most important tags for this app.

| Tag                      | Detail |
| ------------------------ | ------ |
|`FieldValues`             |This tag is what steps into the FieldElement-Template iterations.  The Field-Template is applied once for each field in the active (or selected) database-table.|
|`FieldValues,Cdf`         |This is the same as the above, however each parsed field-template is comma-delimited.|
|`FieldValuesNK`           |This is the same as the field template, however it skips the **PrimaryKey** if present (and likely crashes if there is no primary key defined.|
|`FieldValuesNK,Cdf`       |Comma delimited field-element-template generation skipping the primary key value.|

## Special Field-Tags

| Tag                      | Detail |
| ------------------------ | ------ |
|`FieldIndex`              |Prints the (zero-inclusive) integer index of the table-field.|
|`IsPrimaryKey`            |Prints `true` or `false`.|

<!-- |``||| -->

## Primary data descriptions

| Tag                      | Detail |
| ------------------------ | ------ |
| `DataName`	             |Corresponds to the database->table->field entity. This is required.	|
| `dataname`	             |A lowercase version of `DataName`|
| `DataNameX`              |This no longer exists.  Use `DataNameQ` in stead.|
| `DataNameQ`	             |Same as DataName, however converts value 'id', 'Id' or 'ID' to 'xid'. This is particularly useful for scenarios where 'id' (keyword) is reserved for other uses and the field-name might conflict---so we would know to rather use 'xid'.	|
| `DataNameC`		           |Capitolized DataName|
| `CleanName,Nodash`		   ||
| `FriendlyName`		       ||
| `CleanName`		           ||
| `FriendlyNameC`          |Capitalize|

**FieldElement DataAlias values.** for data-views and links.

| Tag                      | Detail |
| ------------------------ | ------ |
| `DataAlias`		           |Same as supplied DataAlias value.|
| `dataalias`		           |To lowercase.|
| `DataAliasC`		         |Capitolized.|
| `CleanAlias,Nodash`		   |Remove under-scores and dashes, convert to 'camelCase'.|
| `FriendlyAlias`		       |Same as `CleanAlias,Nodash`?|
| `CleanAlias`		         |Replaces `-` Dash with `_` Underscore.|
| `FriendlyAliasC`		     |Same as FriendlyAlias, however 'Capitolized'.|

**Data Type (Data type descriptions)**

| Tag                      | Detail |
| ------------------------ | ------ |
| `DataType`		           ||
| `datatype`		           ||
| `DataTypeNative`		     ||
| `DataTypeNativeF`		     ||
| `datatypenative`		     ||

**Data-related (not properly categorized at the moment**

| Tag                      | Detail |
| ------------------------ | ------ |
| `Native`                 |Not sure at the moment if this was intended to be used at public level, but its in there.|
| `NativeNullType`         ||
| `NativeNullTypeGoAddress`||
| `NativeNullTypeGoPointer`||
| `NativeNullTypeGo`       |Provides pointer value for native types `int`, `int32` and `int64` as well as floats.|
| `NativeNullValue`        ||
| `DataTypeNullable`       ||
| `FlashDataType`          |Provide ActionScript3 'native type'.|

### Field-level meta-data tags 1

| Tag                      | Detail |
| ------------------------ | ------ |
| `FormatString`           ||
| `MaxLMAX`		             ||
| `nmax`		               ||
| `smax`		               ||
| `MaxLength`		           ||
| `CodeBlock`		           ||
| `BlockAction`		         ||
| `FormType`		           ||
| `FormTypeClean`		       ||
| `formtype`		           ||
| `IsString`		           ||
| `IsBool`		             ||
| `IsNum`		               ||
| `IsNullable`             ||
| `SqlFormat`              ||
| `Format`                 ||
| `fmax`                   ||
| `max`                    ||
| `UseFormat`              ||
| `Description`            ||
| `DefaultValue`           ||

### Un-used Misc Field-level Tag(s)

| Tag                      | Detail |
| ------------------------ | ------ |
| `IsArray`		             ||

## Table-Level Tags

All tags are defined in [TableElement.cs].  
All tags are enclosed in the same tag-syntax as the others `$([tagname])`.

### Primary Key tags

> note that some of this may be categorized improperly at the moment.

These tags are specific to TableElement or in other words can be used in table-level
and field-level templates.

| Tag                      | Detail |
| ------------------------ | ------ |
|`TableType`               ||
|`tabletype`               ||
|`PK`                      ||
|`pk`                      ||
|`PrimaryKey`              ||
|`PrimaryKeyCleanC`        ||
|`primarykey`              ||
|`PKDataName`              ||
|`PKDataType`              ||
|`PKDataTypeNative`        ||
|`PKNativeIsNullable`      | Tells if the primary key is marked as nullable.|
|`PKNativeNullType`        ||
|`PKNativeNullValue`       ||
|`PKNativeNullValueGo`     ||
|`PKNativeNullValueGoAddress` | If primary-key element IsNullable (PKNativeIsNullable), will return the address-of operator `&`|
|`PKNativeNullValueGoPointer` | If primary-key element IsNullable (PKNativeIsNullable), will return the pointer operator `*`|
|`PKDescription`           ||
|`PKDataNameC`             ||
|`PKCleanName`             ||
|`PKCleanName,Nodash`      ||
|`PKFriendlyName`          ||
|`PKFriendlyNameC`         ||

### Additional Table-level Tags

| Tag                      | Detail |
| ------------------------ | ------ |
|`Link`                    ||
|`View`                    ||
|`Name`                    ||
|`Table`                   ||
|`TableAlias`              ||
|`_TableAlias`             ||
|`tablealias`              ||
|`TableAliasC`             ||
|`FriendlyTableAlias`      ||
|`TableAliasClean`         ||
|`tablealiasclean`         ||
|`TableAliasCClean`        ||
|`TABLEALIASCLEAN`         ||
|`TableAliasCName`         ||
|`TableAliasCNameC`        ||
|`TableName`               ||
|`tablename`               ||
|`TableNameC`              ||
|`TableNameClean`          ||
|`tablenameclean`          ||
|`TableNameCClean`         ||
|`TableCleanName`          ||
|`TableCleanNameC`         ||

### Native .Net OLE or Database Relation

| Tag                      | Detail |
| ------------------------ | ------ |
|`AdapterNs`               ||
|`AdapterT`                ||
|`AdapterNsT`              ||
|`CommandNs`               ||
|`CommandT`                ||
|`CommandNsT`              ||
|`ConnectionNs`            ||
|`ConnectionT`             ||
|`ConnectionNsT`           ||
|`ParameterT`              ||
|`ReaderNs`                ||
|`ReaderT`                 ||
|`ReaderNsT`               ||
