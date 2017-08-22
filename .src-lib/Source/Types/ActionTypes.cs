/* oIo * 2/5/2011 – 10:00 PM */
using System;
namespace Generator.Elements.Types
{
	public enum ActionTypes
	{
		// Code-Dom Convertable
		@Class, @Module, @Interface, @Type, @Enum,
		// General Windows.Forms Tactics
		@Method, @Property, @Event, @EventHandler,
		// non-sense
		@RoutedEvent, @RoutedEventHandler
	}
}
