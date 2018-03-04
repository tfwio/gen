/* oOo * 11/14/2007 : 10:22 PM */
using System;
#if !NCORE
using DialogResult = System.Windows.Forms.DialogResult;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
#endif
namespace System
{
	static class ErrorMessage
	{
#if NCORE
    static public void Show(string msgTitle, string msgLog)
    {
      throw new Exception(msgTitle + "\n" + msgLog);
    }
    static public void Show(string msgLog)
    {
      throw new Exception(msgLog);
    }
    static public void ShowLog(string msgTitle, string msgLog)
    {
      Logger.LogG(msgTitle, msgLog);
    }
#else
    static public void Show(string msgTitle, string msgLog)
    {
      MessageBox.Show(msgTitle, msgLog);
    }
    static public void Show(string msgLog)
    {
      MessageBox.Show(msgLog);
    }
    static public void ShowLog(string msgTitle, string msgLog)
    {
      MessageBox.Show(msgTitle, msgLog);
    }
#endif
	}
}


