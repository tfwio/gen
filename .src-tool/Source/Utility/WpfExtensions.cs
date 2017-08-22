/* oio : 12/07/2013 10:02 */
using System;
using System.Windows;
using System.Windows.Controls;

namespace GeneratorTool
{
	static class DragDropExtensions
	{
		static public void ApplyDragDropCb(this TextBox textBox, DragEventHandler dragEnter, DragEventHandler drop)
		{
			textBox.AllowDrop = true;
			textBox.PreviewDragOver += dragEnter;
			textBox.Drop += drop;
		}
	}
}
