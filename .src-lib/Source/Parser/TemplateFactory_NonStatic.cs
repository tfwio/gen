using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using GenConfig = Generator.Export.Intrinsic.IDbConfiguration4;
using Generator.Elements;

using Generator.Core.Markup;
namespace Generator.Parser
{
  static public class TemplateFactory_NonStatic
  {
    
    #region Check For Error
    internal static bool CheckForError( DatabaseElement db, TableElement table, bool showMessageBox=false, bool ignoreException=true)
    {
      bool hasError = false;
      if (hasError = (db==null || table==null))
      {
        if (showMessageBox) System.Windows.Forms.MessageBox.Show(
          Gen.Strings.MsgDatabaseOrTableNullError_Title,
          Gen.Strings.MsgDatabaseOrTableNullError_Message
         );
        if (!ignoreException) throw new ArgumentException (Gen.Strings.MsgDatabaseOrTableNullError_Exception);
      }
      return hasError;
    }
    internal static bool CheckForError(TableElement table, bool showMessageBox=false, bool ignoreException=true)
    {
      bool hasError = false;
      if (hasError = (table==null))
      {
        if (showMessageBox) System.Windows.Forms.MessageBox.Show(
          Gen.Strings.MsgDatabaseOrTableNullError_Title,
          Gen.Strings.MsgDatabaseOrTableNullError_Message
         );
        if (!ignoreException) throw new ArgumentException (Gen.Strings.MsgDatabaseOrTableNullError_Exception);
      }
      return hasError;
    }
    #endregion
    
    #region Convert
    
    /// <summary>#1 Starting Point
    /// 
    /// 
    /// The selection must have at the least a table and selected template.
    /// Converts a selection (table or view) provided by IDbConfiguration4.
    /// 
    /// - prepares parsing
    /// 
    /// - performs initial layer of parsing.
    /// </summary>
    /// <param name="config">IDbConfiguration4; (selection)</param>
    /// <returns>parsed string result.</returns>
    static public string Generate(GenConfig config)
    {
      string output = string.Empty;
      
      var view = TemplateModel.LoadSelection(config);
      
      if (view.T != null)
      {
        output = Gen_Pass2(view);
        
        List<QuickMatch> list; // holds a list of template-tag matches.
        
        while (0 != (list = TemplateReferenceUtil.GetReferences(output)).Count)
          output = WritePart(view, list[0], output);
      }
      
      #region IF View IS SELECTION
      else if (config.SelectedView!=null)
      {
        view  = TemplateModel.LoadView(config);
        output = Gen_Pass2(view);
        view.T.View = null;
        
        // check for errors
        if (view.DB==null || view.T==null) { System.Windows.Forms.MessageBox.Show("Error finding database or table for view-link","Exiting generation..."); return "Error finding database or table for view."; }
        
        List<QuickMatch> list;
        while (0 != (list = TemplateReferenceUtil.GetReferences( output )).Count)
        {
          var m1 = list[0]; // m1.Value = the name of our Template, right?
          
          Logger.LogY( "Template?" , "QuickMatch[0].Value={0}, QuickMatch[0].Name={1}", m1.Value, m1.Name );
          
          if (!m1.HasParams) { Logger.LogC( "TemplateFactory.Write ERROR" , "No Params" ); continue; }
          
          if (!m1.HasMultipleParams)
          {
            #region Single Table Reference
            
            view.TT = config.Templates[m1.Params[0]];
            if (view.TT==null) { Logger.Warn("TemplateFactory.Write ERROR","Tag: ‘{0}’ value.", m1.Name); continue; } // return tableOut;
            
            // reset the current view?
            
            view.T.View = config.SelectedView;
            string newOut = Gen_Pass2( view );
            
            output = output.Replace(m1.FullString,newOut);
            
            view.T.View = null;
            
            Logger.LogM("TemplateFactory.Write","{0}", m1.Params[0]);
            
            #endregion
          }
          else if (m1.Name=="Directory")
          {
            #region Directory

            if (System.IO.Directory.Exists(m1.Value))
            {
              var listf = new List<string>();
              foreach (string dir in System.IO.Directory.GetDirectories(m1.Value))
              {
                listf.Add(dir);
              }
              output = output.Replace(m1.FullString,string.Join(",",listf.ToArray()));
            }
            
            #endregion
          }
          else
          {
            #region Main Parser Section
            
            Logger.LogC("TemplateFactory.Write", "Match0.Value = “{0}”",m1.Value);
            Logger.LogC("TemplateFactory.Write", ".Name = “{0}”",m1.Name);
            Logger.LogC("TemplateFactory.Write", "{0}", m1.Params[0]);
            
            string newOut = string.Empty;
            
            for (int i = 0, match0ParamsLength = m1.Params.Length; i < match0ParamsLength; i++) {
              var param = m1.Params[i];
              
              Logger.LogG("TemplateFactory.Write", "table: “{0}”", m1.Params[i]);
              
              view.SetTemplate(m1.Params[0]);
              view.T.View = config.SelectedView; // but why?
              
              newOut += string.Format("{0}",Gen_Pass2( view ));
              
              view.T.View = null;
            }
            
            output = output.Replace(m1.FullString,newOut);
            #endregion
          }
        }
      }
      #endregion
      
      return output;
    }

    static string WritePart(TemplateModel view, QuickMatch match, string generatedIn)
    {
      string generatedOut = generatedIn; //  QuickMatch match0 = list[0]; // Logger.LogY("TemplateFactory.WritePart","{0}", match.Value);// this is "TemplateName,TableName"
      if (!match.HasParams) { Logger.Warn("TemplateFactory.WritePart","ERROR: No Params"); return generatedIn; }
      
      if (!match.HasMultipleParams)
      {
        view.SetTemplate(match.Params[0]);
        if (view.TT==null) { Logger.Warn( "TemplateFactory.WritePart ERROR", "Tag: ‘{0}’ value.", match.Name ); return generatedIn; }
        generatedOut = generatedIn.Replace( match.FullString, Gen_Pass2( view ) ); // replace the template tag with the parsed content.
      }
      // the Directory Element is not parsed.
      else if (match.Name=="Directory")
      {
        if (System.IO.Directory.Exists(match.Value))
        {
          var listf = new List<string>();
          foreach (string dir in System.IO.Directory.GetDirectories(match.Value)) listf.Add(dir);
          generatedOut = generatedIn.Replace(match.FullString,string.Join(",",listf.ToArray()));
        }
      }
      // ¡MAIN PARSER LOOP!
      else
      {
        string newOut = string.Empty;
        for(int i=1; i < match.Params.Length; i++) // match.Params[i] = table-name; notice I starts at one
        {
          view.SetTemplate(match.Params[0]);
          view.SetView(match.Params[i]);
          newOut += Gen_Pass2(view);
        }
        generatedOut = generatedIn.Replace( match.FullString, newOut );
      }
      return generatedOut;
    }
    
    /// <summary>Parser Pass #2:
    /// <para>
    /// This is the parser's initialization point.
    /// </para>
    /// <para>Here, we process the given template given a particular table.</para>
    /// <para>The parser process repeats using this method for each requested table/template
    /// provided by the given template until there are no templates left to parse.</para>
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    static public string Gen_Pass2(TemplateModel view)
    {
      if (view.T==null) return view.TemplateBody; // UNDONE: Log Error
      bool noPrimaryKey  = view.TemplateBody.Contains(StrKeys.FieldValuesNK);
      string generated  = view.T.ReplaceValues( view.TemplateBody );
      
      Logger.Error(ConsoleColor.Yellow,"TF.Generate" , "T: {0}, TT: {{{1}, {2}}}", view.T.Name, view.TT.Group, view.TT.Alias);
      
      List<string>   paramStrings = GetParamStrings( view, noPrimaryKey );
      generated = ReplaceFieldValues( generated, paramStrings.ToArray() );
      paramStrings.Clear();
      paramStrings = null;
      
      return generated;
    }
    
    static string ReplaceFieldValues(string input, params string[] values)
    {
      string fieldOut = string.Join(string.Empty,values); // not comma-delimited
      string cFieldOut = string.Join(",",values);         //     comma-delimited
      return input
        .ReplaceP(StrKeys.FieldValues     , fieldOut)
        .ReplaceP(StrKeys.FieldValuesNK   , fieldOut)
        .ReplaceP(StrKeys.FieldValuesCdf  , cFieldOut)
        .ReplaceP(StrKeys.FieldValuesNKCdf, cFieldOut)
        ;
    }
    
    static List<string> GetParamStrings(TemplateModel view, bool noPrimaryKey)
    {
      var result_pList = new List<string>();
      string curr_FieldName = null;
      
      // this is the primary date conversion or replacement portion of this method.
      // ------------------------------------------------------------
      if (view.T.View==null && view.T.Link==null)
      {
        view.PrepareReformat();
        for (int i = 0, elmTableFieldsCount = view.T.Fields.Count; i < elmTableFieldsCount; i++)
        {
          curr_FieldName = GetParam(view, noPrimaryKey, i);
          if (!string.IsNullOrEmpty(curr_FieldName)) result_pList.Add(curr_FieldName);
        }
        view.TemplateContentReformat = null;
      }
      // ------------------------------------------------------------
      // we're dealing with a DataViewElement -- not sure if this is working so focus more up there...
      // ------------------------------------------------------------
      else if (view.T.View!=null)
        // here, we iterate on either of the following:
        // 1.  FieldElement, tbl.Fields
        // 2.  DataViewLink, tableElement.View.LinkItems
        //     2.1 FieldElement, tbl.Fields
      {
        view.SetView();
        view.PrepareReplaceValues();
        if (CheckForError(view.T)) { Logger.Warn("TemplateFactory.GetParamStrings","couldn't find table named \"{0}\"", view.T.Name); } // this semantic really kind of sucks.
        
        for (int i = 0, tblFieldsCount = view.T.Fields.Count; i < tblFieldsCount; i++)
        {
          FieldElement field = view.T.Fields[i];
          field.View = view.T.View;
          
          curr_FieldName = GetParam(view, noPrimaryKey, i);
          
          
          if (view.T.View.HasField(view.T, field, true) && !string.IsNullOrEmpty(curr_FieldName)) result_pList.Add(curr_FieldName);
          field.View = null;
        }
        
        foreach (DataViewLink link in view.T.View.LinkItems)
        {
          view.SetView(link);
          view.PrepareReplaceValues();
          if (CheckForError(view.T)) { Logger.Warn("GetParamStrings","Table \"{0}\" wasn't found"); } // how about exiting?
          Logger.LogG("--->","Found table: {0}", view.T.Name);
          
          for (int i = 0, tblFieldsCount = view.T.Fields.Count; i < tblFieldsCount; i++) {
            FieldElement field = view.T.Fields[i];
            field.View = view.T.View;
            field.Link = link;
            
            curr_FieldName = GetParam(view, noPrimaryKey, i);
            
            bool hasField = link.HasField(view.T, field, true);
            if (hasField && !string.IsNullOrEmpty(curr_FieldName))
              result_pList.Add(curr_FieldName);
            field.View = null;
            field.Link = null;
          }
        }
        view.TemplateContentReformat = null;
      }
      return result_pList;
    }
    
    /// <summary>
    /// field template "DataName" is used.
    /// 
    /// if no primary key was asked and elmTable.PK==tplField.DataName
    /// </summary>
    /// <param name="view"></param>
    /// <param name="noPrimaryKey"></param>
    /// <param name="index">the field's index</param>
    /// <returns></returns>
    static string GetParam(TemplateModel view, bool noPrimaryKey, int index)
    {
      bool   isPK  = view.T.PrimaryKey==view.T.Fields[index].DataName; // Check if this is the primary-key
      return (noPrimaryKey & isPK) ?
        null :
        view.T.Fields[index].Replace(
          string.Copy(view.TemplateContentReformat)
          .ReplaceP(StrKeys.FieldIndex, index)
          .ReplaceP(StrKeys.IsPrimaryKey,isPK)
          .ReplaceP(StrKeys.PrimaryKey, view.T.PrimaryKey) // FIXME:  ?? string.Empty
         );
    }
    
    #endregion

    public const string fmt_field = "$({0})";
    
    static readonly Regex regex_fieldTemplateTag = new Regex(Gen.Strings.Regex_Field_Template_Tag,RegexOptions.Multiline);
    
    static readonly Dictionary<NsTypes,string> AC001 = new Dictionary<NsTypes,string>(){
      { NsTypes.Global, Gen.Strings.Factory_AcErratum },
      { NsTypes.TableTypes, string.Concat(
        Gen.Strings.Factory_AcTable,";",
        Gen.Strings.Factory_AcPrime) },
      { NsTypes.AdapterTypes,
        Gen.Strings.Factory_AcAdapt },
      { NsTypes.DatabaseTypes,
        Gen.Strings.Factory_AcData },
      { NsTypes.FieldTypes, string.Concat(
        Gen.Strings.Factory_AcField,";",
        Gen.Strings.Factory_AcTypes,";",
        Gen.Strings.Factory_AcPkType) },
    };
    static readonly Dictionary<NsTypes,string> AC002 = new Dictionary<NsTypes,string>(){
      { NsTypes.Global,Gen.Strings.Factory_AcErratum },
      { NsTypes.TableTypes, string.Concat(
        Gen.Strings.Factory_AcTable,";",
        Gen.Strings.Factory_AcTypes,";",
        Gen.Strings.Factory_AcPrime) },
      { NsTypes.AdapterTypes,
        Gen.Strings.Factory_AcAdapt },
    };
    
    static public readonly Dictionary<NsTypes,string> Ac01 = AC001;
    static public readonly Dictionary<NsTypes,string> Ac02 = AC002;

    static public string[] Group1(NsTypes groupn) { return Ac01[groupn].Split(';'); }
    static public string[] Group2(NsTypes groupn) { return Ac02[groupn].Split(';'); }

  }

}


