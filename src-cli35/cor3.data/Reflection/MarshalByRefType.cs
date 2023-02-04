#region User/License
// Copyright (c) 2005-2013 tfwroble
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */
using System;

using Global=System.Cor3.last_addon;

namespace System.Cor3.Reflection
{
	// Because this class is derived from MarshalByRefObject, a proxy
	// to a MarshalByRefType object can be returned across an AppDomain
	// boundary.
	public class MarshalByRefType : MarshalByRefObject
	{
		//  Call this method via a proxy.
		public void SomeMethod(string callingDomainName)
		{
			// Get this AppDomain's settings and display some of them.
			AppDomainSetup ads = AppDomain.CurrentDomain.SetupInformation;
			Global.statY(
				"AppName={0}, AppBase={1}, ConfigFile={2}",
				ads.ApplicationName,
				ads.ApplicationBase,
				ads.ConfigurationFile
			);
			// Display the name of the calling AppDomain and the name
			// of the second domain.
			// NOTE: The application's thread has transitioned between
			// AppDomains.
			Global.statY(
				"Calling from '{0}' to '{1}'.",
				callingDomainName, System.Threading
				.Thread.GetDomain().FriendlyName
			);
		}
	}
}
